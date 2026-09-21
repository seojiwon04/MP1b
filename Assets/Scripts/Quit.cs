using UnityEngine;
using UnityEngine.InputSystem;
public class Quit : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    public InputActionReference quit;
    void Start()
    {
        quit.action.Enable();
        quit.action.performed += (ctx) =>
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
