using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.UI;
using Unity.VisualScripting;
using TMPro;
using System;
using System.Collections.Generic;


public class Shop : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt;
    [SerializeField] public GameObject _shopUI;
    [SerializeField] public CinemachineCamera _camera;

    [SerializeField] private VariableDisplay _variableDisplay;
    public GameObject _player;

    private Inventory inventory;
    
    public List<Button> buttons;
    
    public string InteractionPrompt => _prompt;

    private void Start()
    {
        inventory = _player.GetComponent<Inventory>();
        _variableDisplay.inventoryUpdate(inventory);

        _shopUI.SetActive(false);

        foreach(Button button in _shopUI.GetComponentsInChildren<Button>()){
            buttons.Add(button);
            // Debug.Log("button found");
        }
        UpdateButtons();
        // Debug.Log("start up done");
        
    }

    public bool Interact(Interactor interactor)
    {
        // Debug.Log("Opening Shop");
        foreach(Button button in buttons)
        {
            button.interactable = true;
        }

        UpdateButtons();
            

        if (_shopUI.activeSelf)
        {
            _shopUI.SetActive(false);
            _camera.GetComponent<CinemachineInputAxisController>().enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            _shopUI.SetActive(true);
            _camera.GetComponent<CinemachineInputAxisController>().enabled = false;
            Cursor.lockState = CursorLockMode.None;
        }
        
        return true;
    }

    public void UpdateButtons()
    {    
        foreach(Button button in buttons)
        {
            var _button = button.GetComponent<Purchasables>();
            
            _button.buttonText.text = "Price: " + _button.buttonPrice + "$"; 
            if(inventory.money < _button.buttonPrice) button.interactable = false;
        }

        _variableDisplay.inventoryUpdate(inventory);
    }
}

