using UnityEngine;

public class Easer : MonoBehaviour
{
    public GameObject movingObject;
    private Transform body;
    private Vector3 velocity;
    public float rate;
    public float goal;
    public float damping;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        body = movingObject.GetComponent<Transform>();
	velocity = new Vector3(0f, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
       body.position += velocity * Time.deltaTime;
       velocity += new Vector3(0f, rate * (goal - body.position.y) - damping * velocity.y, 0f) * Time.deltaTime; 
    }
}
