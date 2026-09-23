using UnityEngine;

public class StarVelocity : MonoBehaviour
{
    public Vector3 velocity;

    void Start()
    {

    }

    void Update()
    {
        transform.position += velocity * Time.deltaTime;
    }
}