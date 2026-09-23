using UnityEngine;

public class RotateMoon : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * Time.deltaTime * 20f); // rotate moon around planet Y-axis
    }
}
