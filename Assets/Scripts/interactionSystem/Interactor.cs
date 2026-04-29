using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

public class Interactor : MonoBehaviour
{
    [SerializeField] private Transform _interactionPoint;
    [SerializeField] private float _interactionPointRadius = 0.5f;
    [SerializeField] private LayerMask _interactableMask;
    [SerializeField] private Shop _shop;
    [SerializeField] private interactionPromptUI _interactionPromptUI;
    
    // makes an array for objects to go into and limits so only 3 can show up in it at most
    private readonly Collider[] _colliders = new Collider[3];
    [SerializeField] private int _numFound;
    [SerializeField] private VariableDisplay _variableDisplay;
    [SerializeField] public CinemachineCamera _camera;



    public GameObject Player;
    private Inventory inventory;
    private IInteractable _interactable;

    void Start()
    {
        inventory = Player.GetComponent<Inventory>();
        _variableDisplay.inventoryUpdate(inventory);
        // _shopUI = _shop.GetComponent<>();
    }

    void Update()
    {
        // draws a sphere infront of the player character and checks if there is anything inside it
        // interactableMask limits what can be counted as inside the sphere so only objects with the "interactable" layer on the unity editor gets counted towards numFound
        _numFound = Physics.OverlapSphereNonAlloc(_interactionPoint.position, _interactionPointRadius, _colliders, _interactableMask);
        
        if(_numFound > 0)
        {
            // gets whatever script is connected to the interactable. distinquish between shop, trash and trash seller
            _interactable = _colliders[0].GetComponent<IInteractable>();

            if(_interactable != null)
            {
                // if a script is found it will display a pop up on the screen if there is text in the prompt field on the object script
                if(!_interactionPromptUI.IsDisplayed) _interactionPromptUI.SetUp(_interactable.InteractionPrompt);

                // if the e key was pressed interact with the object and update the inventory display on the screen so it accurately shows your stats
                if(Keyboard.current.eKey.wasPressedThisFrame) 
                {
                    _interactable.Interact(this);
                    _variableDisplay.inventoryUpdate(inventory);
                }
            }
        }
        else
        {
            // if there is nothing to be found then it just sets all ui to be closed and deactivated
            if (_interactable != null )_interactable = null;
            if (_interactionPromptUI.IsDisplayed) _interactionPromptUI.Close();
            if (_shop._shopUI.activeSelf)
            {
            _shop._shopUI.SetActive(false);
            _camera.GetComponent<CinemachineInputAxisController>().enabled = true;

            Cursor.lockState = CursorLockMode.Locked;
            }
                
        }
    }
    
    // draws the sphere infront of the player character in the editor for debugging
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_interactionPoint.position, _interactionPointRadius);
    }
}
