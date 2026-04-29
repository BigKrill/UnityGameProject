using UnityEngine;

public class testScript : MonoBehaviour
{   
    // early script to test changing variables between scripts
    // this just changes the sprint mult

    public GameObject Player;
    private ThirdPersonMovement playerMovement;

    void Start()
    {
        playerMovement = Player.GetComponent<ThirdPersonMovement>();
    }


    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F)){
            playerMovement.sprintMult += 5f;
        }
    }
}
