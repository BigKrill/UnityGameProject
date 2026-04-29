using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class VariableDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _inventorySpace;
    [SerializeField] private TextMeshProUGUI _inventorySize;
    [SerializeField] private TextMeshProUGUI _money;

    public void inventoryUpdate(Inventory inventory)
    {
        // displays the inventory variables on the screen
        _inventorySpace.text = inventory.inventorySpace.ToString();
        _inventorySize.text = inventory.inventorySize.ToString();
        _money.text = inventory.money.ToString()+ "$";
    }
}
