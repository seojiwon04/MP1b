using UnityEngine;
using UnityEngine.InputSystem;

public class Teleport : MonoBehaviour
{
    public InputActionReference teleport;
    public ParticleSystem poof;
    public AudioClip poofSound;

    private double clicks = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnTeleport(InputAction.CallbackContext ctx) {
        clicks++;
        Debug.Log("teleport clicked");
        Vector3 location;
        if (clicks % 2 == 1) 
        {
            location =new Vector3(16f, 16f, 16f);
            transform.position = location;
            transform.rotation = Quaternion.Euler(0f, -133f, 0f);
            Instantiate(poof, location, Quaternion.identity);
            AudioSource.PlayClipAtPoint(poofSound, location);
        }
        else 
        {
            location = new Vector3(0f, 5f, 0f);
            transform.position = location;
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            Instantiate(poof, location, Quaternion.identity);
            AudioSource.PlayClipAtPoint(poofSound, location);
        }
    }
    
    void Start()
    {
        teleport.action.Enable();
        teleport.action.performed += OnTeleport;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
