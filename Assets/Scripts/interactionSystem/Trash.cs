using UnityEngine;
using System.Collections;
using System;


public class Trash : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt;
    [SerializeField] private int _trashSize = 1;
    [SerializeField] private ParticleSystem particles;

    public string InteractionPrompt => _prompt;
    
    public bool Interact(Interactor interactor)
    {
        var inventory = interactor.GetComponent<Inventory>();

        if (inventory == null)
        {
            Debug.Log("Inventory not found");
            return false;
        } 

        if(inventory.inventorySpace + _trashSize <= inventory.inventorySize){
            inventory.inventorySpace += _trashSize;
            Debug.Log("Picking up! "+ inventory.inventorySpace);

            particles.Play();

            gameObject.layer = 0;

            // gameObject.GetComponent<Collider>().isTrigger = true;
            gameObject.GetComponent<MeshRenderer>().enabled = false;

            // Destroy(gameObject, 1);
            Destroy(transform.parent.gameObject, 1);
            return true;
        }
        
        Debug.Log("Not enough space!");
        return false;
    }
}
