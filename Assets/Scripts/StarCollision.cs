using UnityEngine;

public class StarCollision : MonoBehaviour
{
    public ParticleSystem poof; 
    public AudioClip poofSound;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Desk"))
            return;

        // read the desk's material color (works on Built-in and URP)
        Color deskColor = Color.white;
        Renderer render = other.GetComponent<Renderer>();
        Material mat = render.material;
        deskColor = mat.GetColor("_BaseColor");

        ParticleSystem burst = Instantiate(poof, transform.position, Quaternion.identity);
        var main = burst.main;
        main.startColor = deskColor;
        Destroy(burst.gameObject, 2f);

        AudioSource.PlayClipAtPoint(poofSound, transform.position);

        Destroy(gameObject);
    }
}