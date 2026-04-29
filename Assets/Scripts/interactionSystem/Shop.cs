using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.UI;
using Unity.VisualScripting;


public class Shop : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt;
    [SerializeField] public GameObject _shopUI;
    [SerializeField] public CinemachineCamera _camera;

    private ShopPurchases shop;
    
    public string InteractionPrompt => _prompt;

    private void Start()
    {
        _shopUI.SetActive(false);
        shop = _shopUI.GetComponent<ShopPurchases>();

    }

    public bool Interact(Interactor interactor)
    {
        Debug.Log("Opening Shop");
        shop.UpdateButtons();
        
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
}

