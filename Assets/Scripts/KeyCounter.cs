using UnityEngine;
using TMPro;

public class KeyCounter : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int keysFound = 0; // number of keys found
    public TextMeshProUGUI keysFoundText;

    void Start()
    {
        keysFoundText.text = "Keys\n\n" + keysFound + "/9";
    }

    // Update is called once per frame
    void Update()
    {
        keysFoundText.text = "Keys\n\n" + keysFound + "/9";
    }
}
