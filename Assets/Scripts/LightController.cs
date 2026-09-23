using UnityEngine;
using UnityEngine.InputSystem;

public class LightController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Light light;
    public float cycleSpeed = 0.5f;
    private float hue = 0f;
    public InputActionReference lightChangeButton;
    void Start()
    {
        lightChangeButton.action.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if (lightChangeButton.action.IsPressed())
        {
            hue = Mathf.Repeat(hue + cycleSpeed * Time.deltaTime, 1f);
            light.color = Color.HSVToRGB(hue, 1f, 1f);
        }
    }
}
