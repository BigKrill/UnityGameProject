using UnityEngine;
using System.Collections;
using System;


public class Trash : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt;
    [SerializeField] private int _trashSize = 1;
    [SerializeField] private ParticleSystem particleSystem;

    public string InteractionPrompt => _prompt;
    
    public bool Interact(Interactor interactor)
    {
        // stores the inventory script in a variable
        var inventory = interactor.GetComponent<Inventory>();

        // if there is no inventory for some reason then the script stops
        if (inventory == null)
        {
            Debug.Log("Inventory not found");
            return false;
        } 

        // if there is enough space in the inventory after picking up the object, allow the player to pick up the object
        if(inventory.inventorySpace + _trashSize <= inventory.inventorySize){
            // adds the size of the trash to the inventory
            inventory.inventorySpace += _trashSize;
            Debug.Log("Picking up! "+ inventory.inventorySpace);

            // plays the animation for the pickup
            particleSystem.Play();

            // sets the object layer to 0 so it is no longer interactable preventing multiple pickups
            gameObject.layer = 0;

            // changes certain components of the object to make it disappear but allow the particle animation to continue playing
            // gameObject.GetComponent<Collider>().isTrigger = true;
            gameObject.GetComponent<MeshRenderer>().enabled = false;

            // deletes the game object after 1 seconds (when the animation is finished) and returns true finishing the script as a success
            // Destroy(gameObject, 1);
            Destroy(transform.parent.gameObject, 1);
            return true;
        }
        
        Debug.Log("Not enough space!");
        return false;
    }
}
