using UnityEngine;
using System.Collections;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private MeshCollider meshCollider;
    private static bool isStarted;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetState()
    {
        isStarted = false;
    }
    
    void Awake()
    {
        if (isStarted)
        {
            meshCollider.enabled = false;
            gameObject.SetActive(false);
        }
    }

    public void StartGame()
    {
        isStarted = true;
        meshCollider.enabled = false;
        gameObject.SetActive(false);
        // StartCoroutine(TurnOfCollider());
    }

    private IEnumerator TurnOfCollider()
    {
        isStarted = true;
        meshCollider.enabled = false;

        yield return null;

        gameObject.SetActive(false);
    }
}