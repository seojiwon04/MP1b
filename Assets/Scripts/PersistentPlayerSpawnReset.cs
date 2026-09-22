using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.XR.CoreUtils;

public class PersistentPlayerSpawnReset : MonoBehaviour
{
    public XROrigin xrOrigin;

    private static PersistentPlayerSpawnReset instance;
    private Coroutine pendingSpawn;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetStatics() => instance = null;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        if (instance == this)
            SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        CancelPendingSpawn();
    }

    private void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // if (mode != LoadSceneMode.Single)
        //     return;

        CancelPendingSpawn();
        SpawnPoint destination = null;
        foreach (GameObject root in scene.GetRootGameObjects())
        {
            foreach (SpawnPoint point in root.GetComponentsInChildren<SpawnPoint>())
            {
                if (!point.isActiveAndEnabled)
                    continue;

                destination = point;
            }
        }

        pendingSpawn = StartCoroutine(SpawnAfterLoad(
            destination.transform.position,
            Quaternion.Euler(0f, destination.transform.eulerAngles.y, 0f)));
    }

    private void CancelPendingSpawn()
    {
        if (pendingSpawn == null)
            return;

        StopCoroutine(pendingSpawn);
        pendingSpawn = null;
    }

    private IEnumerator SpawnAfterLoad(Vector3 position, Quaternion rotation)
    {
        yield return null;

        pendingSpawn = null;
        ResetPlayerToSpawn(position, rotation);
    }

    private void ResetPlayerToSpawn(Vector3 spawnPosition, Quaternion spawnRotation)
    {

        Transform origin = xrOrigin.Origin.transform;

        CharacterController[] controllers = GetComponentsInChildren<CharacterController>(true);
        bool[] wasEnabled = new bool[controllers.Length];
        for (int i = 0; i < controllers.Length; i++)
        {
            wasEnabled[i] = controllers[i].enabled;
            controllers[i].enabled = false;
        }

        try
        {
            transform.SetPositionAndRotation(spawnPosition, spawnRotation);

            origin.SetPositionAndRotation(spawnPosition, spawnRotation);

            Vector3 headForward = Vector3.ProjectOnPlane(
                xrOrigin.Camera.transform.forward, Vector3.up);
            if (headForward.sqrMagnitude > 0.0001f)
                xrOrigin.MatchOriginUpCameraForward(
                    Vector3.up, spawnRotation * Vector3.forward);

            float eyeHeight = Vector3.Dot(
                xrOrigin.Camera.transform.position - origin.position, Vector3.up);
            xrOrigin.MoveCameraToWorldLocation(
                spawnPosition + Vector3.up * eyeHeight);

            Physics.SyncTransforms();
        }
        finally
        {
            for (int i = 0; i < controllers.Length; i++)
                controllers[i].enabled = wasEnabled[i];
        }
    }
}