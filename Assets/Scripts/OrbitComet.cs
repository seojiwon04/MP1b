using UnityEngine;
using Unity.Mathematics;

public class OrbitComet : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public double3 position;
    public double3 attractorPosition = new double3(0, 0, 0);
    private double3 velocity = new double3(1, 0, 0);
    private double ax = 1.0; // acceleration in x direction
    private double ay = 1.0; // acceleration in y direction
    private double az = 1.0; // acceleration in z direction
    void Start()
    {
        position = new double3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
    }

    // Update is called once per frame
    void Update()
    {
        const double gravity = 1.4;
        double distance = math.sqrt(math.pow(position.x - attractorPosition.x, 2) + math.pow(position.y - attractorPosition.y, 2) + math.pow(position.z - attractorPosition.z, 2));
        ax = -gravity * (position.x - attractorPosition.x) / math.pow(distance, 3);
        ay = -gravity * (position.y - attractorPosition.y) / math.pow(distance, 3);
        az = -gravity * (position.z - attractorPosition.z) / math.pow(distance, 3);

        velocity.x = velocity.x + ax * Time.deltaTime;
        velocity.y = velocity.y + ay * Time.deltaTime;
        velocity.z = velocity.z + az * Time.deltaTime;

        position.x = position.x + velocity.x * Time.deltaTime;
        position.y = position.y + velocity.y * Time.deltaTime;
        position.z = position.z + velocity.z * Time.deltaTime;

        transform.localPosition = (float3)position;
    }
}