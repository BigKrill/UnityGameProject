using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ShopPurchases : MonoBehaviour
{
    [SerializeField] public int _sizePrice;
    [SerializeField] public int _speedPrice;
    
    [SerializeField] private int _sizeIncrease;
    [SerializeField] private int _speedIncrease;

    [SerializeField] private Button sizeButton;
    [SerializeField] private Button speedButton;

    [SerializeField] private TextMeshProUGUI _sizeButtonText;
    [SerializeField] private TextMeshProUGUI _speedButtonText;

    [SerializeField] public GameObject _shopUI;
    [SerializeField] private VariableDisplay _variableDisplay;
    [SerializeField] public GameObject _player;

    private Inventory inventory;
    private ThirdPersonMovement movement;

    private void Start()
    {
        inventory = _player.GetComponent<Inventory>();
        movement = _player.GetComponent<ThirdPersonMovement>();
        _sizeButtonText.text = "Price: " + _sizePrice + "$";
        _speedButtonText.text = "Price: " + _speedPrice + "$";
    }

    public void IncreaseSize()
    {
        if(inventory.money >= _sizePrice)
        {
            inventory.money -= _sizePrice;
            _sizePrice += 1;
            inventory.inventorySize += _sizeIncrease;
            _variableDisplay.inventoryUpdate(inventory);

            _sizeButtonText.text = "Price: " + _sizePrice + "$";
        }
        
        UpdateButtons();
    }

    public void IncreaseSpeed()
    {
        if(inventory.money >= _speedPrice)
        {
            inventory.money -= _speedPrice;
            _speedPrice += 1;
            movement.speed += _speedIncrease;
            _variableDisplay.inventoryUpdate(inventory);

            _speedButtonText.text = "Price: " + _speedPrice + "$";
        }

        UpdateButtons();
    }

    public void UpdateButtons()
    {
        if(inventory.money < _speedPrice)
        {
            speedButton.interactable = false;
        }
        else
        {
            speedButton.interactable = true;
        }

        if(inventory.money < _sizePrice)
        {
            sizeButton.interactable = false;
        }
        else
        {
            sizeButton.interactable = true;
        }
    }
}