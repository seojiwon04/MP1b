using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectShoot : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject star;
    public ParticleSystem poof;
    public InputActionReference spawnButtonLeft;
    public InputActionReference spawnButtonRight;

    public Transform controllerLeft;
    public Transform controllerRight;
    public AudioClip poofSound;


    void OnSpawnLeft(InputAction.CallbackContext ctx)
    {
        GameObject starObj = Instantiate(star, controllerLeft.position, controllerLeft.rotation * Quaternion.Euler(-45f,0f,0f));
        starObj.GetComponent<StarVelocity>().velocity = controllerLeft.forward * 10f;
    
        Instantiate(poof, controllerLeft.position, controllerLeft.rotation);
        AudioSource.PlayClipAtPoint(poofSound, controllerLeft.position);
        Debug.Log("left button pressed");
    }

    void OnSpawnRight(InputAction.CallbackContext ctx)
    {
        GameObject starObj = Instantiate(star, controllerRight.position, controllerRight.rotation * Quaternion.Euler(45f,0f,0f));
        starObj.GetComponent<StarVelocity>().velocity = controllerRight.forward * 10f;
    
        Instantiate(poof, controllerRight.position, controllerRight.rotation);
        AudioSource.PlayClipAtPoint(poofSound, controllerRight.position);
        Debug.Log("left button pressed");
    }

    void Start()
    {
        spawnButtonLeft.action.Enable();
        spawnButtonLeft.action.performed += OnSpawnLeft;

        spawnButtonRight.action.Enable();
        spawnButtonRight.action.performed += OnSpawnRight;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
