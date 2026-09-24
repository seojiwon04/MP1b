using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SettingWindows : MonoBehaviour
{
    public InputActionReference toggleAction;
    public GameObject panel;
    public bool IsOpen => panel && panel.activeSelf;

    private InputAction boundAction;


    private void Awake() 
    { 
        panel.SetActive(false); 
    }

    private void OnEnable()
    {
        boundAction = toggleAction.action;
        boundAction.performed += OnToggle;
        boundAction.Enable();
    }
    private void OnDisable()
    {
        if (boundAction != null)
        {
            boundAction.performed -= OnToggle;
            boundAction.Disable();
        }
        boundAction = null;
        Close();
    }
    private void OnToggle(InputAction.CallbackContext context) => Toggle();

    public void Toggle()
    {
        if (IsOpen) 
        { 
            Close(); 
            return; 
        }
        panel.SetActive(true);
    }
    public void Close()
    {
        panel.SetActive(false);
    }

    public void Reset()
    {
        Close();
        SceneManager.LoadSceneAsync("MainRoom", LoadSceneMode.Single);
    }
}
