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
        _numFound = Physics.OverlapSphereNonAlloc(_interactionPoint.position, _interactionPointRadius, _colliders, _interactableMask);
        
        if(_numFound > 0)
        {
            _interactable = _colliders[0].GetComponent<IInteractable>();

            if(_interactable != null)
            {
                if(!_interactionPromptUI.IsDisplayed) _interactionPromptUI.SetUp(_interactable.InteractionPrompt);

                if(Keyboard.current.eKey.wasPressedThisFrame) 
                {
                    _interactable.Interact(this);
                    _variableDisplay.inventoryUpdate(inventory);
                }
            }
        }
        else
        {
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
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_interactionPoint.position, _interactionPointRadius);
    }
}
