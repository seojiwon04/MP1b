using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class StartMenu : MonoBehaviour
{
    public InputActionReference start;
    public InputActionReference quit;
    public Behaviour[] gameplayComponents;

    private static bool isStarted;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetState()
    {
        isStarted = false;
    }
    
    void Awake()
    {
        if (isStarted)
            gameObject.SetActive(false);
    }

    void OnEnable()
    {
        if (isStarted) return;
        start.action.Enable();
        quit.action.Enable();
        start.action.performed += OnStartPerformed;
        quit.action.performed += OnQuitPerformed;

        for (int i = 0; i < gameplayComponents.Length; i++)
            gameplayComponents[i].enabled = false;
    }

    void OnDisable()
    {
        start.action.Disable();
        quit.action.Disable();
        start.action.performed -= OnStartPerformed;
        quit.action.performed -= OnQuitPerformed;

        for (int i = 0; i < gameplayComponents.Length; i++)
            gameplayComponents[i].enabled = true;
    }

    private void OnStartPerformed(InputAction.CallbackContext context)
    {
        StartCoroutine(CloseMenu());
    }

    private IEnumerator CloseMenu()
    {
        yield return null;
        while (start.action.IsPressed())
            yield return null;
        isStarted = true;
        gameObject.SetActive(false);
    }

    private void OnQuitPerformed(InputAction.CallbackContext context)
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
