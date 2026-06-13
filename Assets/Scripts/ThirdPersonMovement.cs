using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;
    public GameObject Player;

    public float speed = 10f;
    public float sprintMult = 1.5f;

    private float _gravity = -9.81f;
    [SerializeField] private float gravityMultiplier = 3.0f;
    [SerializeField] private float jumpPower;
    public int _numberOfJumps;
    [SerializeField] public int maxNumberOfJumps = 2;

    float radius;
    Vector3 pos;
    public bool isGrounded;
    [SerializeField] private LayerMask groundedCheck;
    
    public float _velocity;
    private Vector3 _direction;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        var capsulecollider = Player.GetComponent<CapsuleCollider>();
        radius = capsulecollider.radius * 1f;
    }    
    
    void Update()
    {
        applyGravity();
        applyMovement();
    }

    private void applyMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        
        pos = transform.position + Vector3.down*(radius*1.5f);
        isGrounded = Physics.CheckSphere(pos, radius, groundedCheck);

        controller.Move(_direction * Time.deltaTime);

        if(direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;

            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            
            if (Input.GetKey(KeyCode.LeftShift))
            {
                controller.Move(moveDir.normalized * (speed * sprintMult) * Time.deltaTime);
            }
            else
            {
                controller.Move(moveDir.normalized * speed * Time.deltaTime);
            }
        }
    }

    public void applyGravity()
    {
        if (isGrounded && _velocity < 0.0f)
        {
            _velocity = -1.0f;
        }
        else
        {
            _velocity += _gravity * gravityMultiplier * Time.deltaTime;
        }
        
        
        _direction.y = _velocity;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (!isGrounded && _numberOfJumps >= maxNumberOfJumps) return;
        if (_numberOfJumps >= 0) StartCoroutine(WaitForLanding());

        _numberOfJumps++;
        _velocity += jumpPower;
    }

    private IEnumerator WaitForLanding()
    {
        yield return new WaitUntil(() => !isGrounded);
        yield return new WaitUntil(() => isGrounded);

        _numberOfJumps = 0;
    }

    public bool IsGrounded() => controller.isGrounded;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(pos, radius);
    }
}
