using UnityEngine;

public class GlassesController : MonoBehaviour
{
    private bool wearingGlasses = false;

    void Start()
    {
        UpdateShader();
    }

    public void ToggleGlasses()
    {
        wearingGlasses = !wearingGlasses;
        UpdateShader();
    }

    void UpdateShader()
    {
        Shader.SetGlobalFloat(
            "_GlassesEnabled",
            wearingGlasses ? 1f : 0f
        );
    }

    // 在 Game 畫面建立測試按鈕
    void OnGUI()
    {
        string buttonText = wearingGlasses
            ? "Remove Glasses"
            : "Wear Glasses";

        if (GUI.Button(
            new Rect(20, 20, 180, 45),
            buttonText))
        {
            ToggleGlasses();
        }
    }
}
