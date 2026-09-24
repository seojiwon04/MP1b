using EscapeRoom.Inventory;
using Unity.VectorGraphics;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToFinalRoom : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("teleport hit by " + other.name);
        if (other.GetComponentInParent<XROrigin>() != null)
        {
            SceneManager.LoadSceneAsync("WinScene", LoadSceneMode.Single);
        }
    }
}
