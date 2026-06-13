using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;


public class Purchasables : MonoBehaviour
{
    [SerializeField] public GameObject _player;
    [SerializeField] public GameObject shop;

    private Shop _shop = null;
    private Inventory inventory = null;
    private ThirdPersonMovement movement = null;

    [SerializeField] public int buttonPrice = 5;
    [SerializeField] public int buttonPriceIncrease = 1;
    [SerializeField] public TextMeshProUGUI buttonText;

    public void Start()
    {
        _shop = shop.GetComponent<Shop>();
    }

    public void getInfo()
    {
        inventory = _player.GetComponent<Inventory>();
        movement = _player.GetComponent<ThirdPersonMovement>();
    }
    
    public void IncreaseSize()
    {
        getInfo();

        if(inventory.money >= buttonPrice)
        {   
            inventory.money -= buttonPrice;
            buttonPrice += buttonPriceIncrease;
            buttonPriceIncrease++;
            inventory.inventorySize += 1;
        }
        
        _shop.UpdateButtons();
    }

    public void IncreaseSpeed()
    {
        getInfo();
        
        if(inventory.money >= buttonPrice)
        {
            inventory.money -= buttonPrice;
            buttonPrice += buttonPriceIncrease;
            buttonPriceIncrease++;
            movement.speed += 1;
        }

        _shop.UpdateButtons();
    }

}