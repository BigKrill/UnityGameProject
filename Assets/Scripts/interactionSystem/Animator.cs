using NUnit.Framework;
using System.Collections;
using UnityEngine;


public class Animator : MonoBehaviour
{
    Animation anim;
    ThirdPersonMovement movement;

    public GameObject Player;
    private bool isFalling;
    private int numOfJumps = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animation>();
        movement = Player.GetComponent<ThirdPersonMovement>();

        anim["Armature_Walk"].speed = 2.0f;
        anim["Armature_JumpObjectReaction"].speed = 2.0f;
    }

    // Update is called once per frame
    void Update()
    {        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (numOfJumps < movement.maxNumberOfJumps)
            {
                anim.Stop();
                anim.Play("Armature_JumpObjectReaction");
                numOfJumps++;   
            }
        }
        

        
        if(movement.isGrounded && !anim.IsPlaying("Armature_JumpObjectReaction")){
            if (Input.GetAxisRaw("Vertical") != 0 || Input.GetAxisRaw("Horizontal") != 0 )
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    anim.Play("Armature_Run");
                }
                else
                {
                    anim.Play("Armature_Walk");
                }
            }
            else
            {
                if(!anim.IsPlaying("Armature_Land"))
                {
                    anim.Play("Armature_Wait");
                }
            }
        }
        else if (!movement.isGrounded)
        {
            if (!anim.IsPlaying("Armature_JumpObjectReaction") && isFalling == false)
            {
                anim.Play("Armature_Fall");
                isFalling = true;
            }
            
            StartCoroutine(WaitForLanding());
        }

        
    }

    private IEnumerator WaitForLanding()
    {
        yield return new WaitUntil(() => !movement.isGrounded);
        yield return new WaitUntil(() => movement.isGrounded);
        
        numOfJumps = 0;

        anim.Play("Armature_Land");
        isFalling = false;
    }
}
