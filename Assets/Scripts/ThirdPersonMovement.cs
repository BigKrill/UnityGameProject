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
    
    // Update is called once per frame
    void Update()
    {
        applyGravity();
        applyMovement();
    }

    private void applyMovement()
    {
        // gets the controls for moving directions and puts them into a direction variable. ".normalized" makes it so that you move the same speed diagonally as you would in one direction
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        
        // puts a sphere below the player to check if they are touching the floor
        // it is better to use this rather than unity's built in grounded check
        pos = transform.position + Vector3.down*(radius*1.5f);
        isGrounded = Physics.CheckSphere(pos, radius, groundedCheck);

        controller.Move(_direction * Time.deltaTime);

        if(direction.magnitude >= 0.1f)
        {
            // calculates the angle to face based on what direction you are moving, then adds the camera angle to it so rotation is relative to the camera rather than the world
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;

            // uses a variable to smoothly move between angles rather than snapping
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0);

            // allows the character to move relative to the camera rather than the world
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            
            // if the left shift key is pressed changes the movement function to add a sprint multiplier
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
        // checks if the player is on the ground and sets the velocity to -1
        // this is to stop the velocity from increasing infinitely so fall speed stays the same at the beginning
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
        // checks if the space bar has been pressed or is being pressed to continue the function
        // can be used to make jump higher by holding the jump button like mario
        if (!context.started) return;

        // if the character is continued and still has jumps available then continue
        if (!isGrounded && _numberOfJumps >= maxNumberOfJumps) return;

        // when jumping the StartCoroutine waits for the player to be grounded again to reset jumps to 0 so that the player can continue to jump and double jump
        if (_numberOfJumps >= 0) StartCoroutine(WaitForLanding());

        // adds one to number of jumps and adds jump power to velocity to allow for the player to actually jump in the world
        _numberOfJumps++;
        _velocity += jumpPower;
    }

    private IEnumerator WaitForLanding()
    {
        yield return new WaitUntil(() => !isGrounded);
        yield return new WaitUntil(() => isGrounded);

        _numberOfJumps = 0;
    }

    // controller.isGrounded is a built in unity function to check if the character controller is touching the floor
    // this essentially puts that into a variable "IsGrounded()" which helps with readability
    public bool IsGrounded() => controller.isGrounded;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(pos, radius);
    }
}
