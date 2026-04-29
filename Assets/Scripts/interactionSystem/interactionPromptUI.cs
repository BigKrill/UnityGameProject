using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class interactionPromptUI : MonoBehaviour
{
    private Camera _mainCam;
    [SerializeField] private GameObject _uiPanel;
    [SerializeField] private TextMeshProUGUI _promptText;

    private void Start()
    {
        // sets the main camera and ui panel to false so it doesnt show up when theres nothing to look at
        _mainCam = Camera.main;
        _uiPanel.SetActive(false);
    }

    private void LateUpdate()
    {
        // makes the ui panel look at the camera
        var rotation = _mainCam.transform.rotation;
        transform.LookAt(transform.position + rotation * Vector3.forward,
            rotation * Vector3.up);
    }

    public bool IsDisplayed = false;

    public void SetUp(string promptText)
    {
        // sets the ui panels prompt text to whatever is specified on the object being interacted with and sisplays it
        _promptText.text = promptText;
        if (promptText != ""){
            _uiPanel.SetActive(true);
            IsDisplayed = true;
        }
    }

    // turns the panel off
    public void Close()
    {
        _uiPanel.SetActive(false);
        IsDisplayed = false;
    }
}
