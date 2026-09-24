using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.XR.CoreUtils;
using UnityEngine.XR.Interaction.Toolkit.Inputs;
using UnityEditor.XR.LegacyInputHelpers;

[DefaultExecutionOrder(-10000)]
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
            // A duplicate must not disable the shared action assets used by the surviving rig.
            foreach (var manager in GetComponentsInChildren<InputActionManager>(true))
                manager.actionAssets = new System.Collections.Generic.List<UnityEngine.InputSystem.InputActionAsset>();
            gameObject.SetActive(false);
            Destroy(gameObject);
            return;
        }

        if (!xrOrigin) xrOrigin = GetComponentInChildren<XROrigin>(true);
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
        if (instance != this || mode != LoadSceneMode.Single) return;

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

        pendingSpawn = StartCoroutine(SpawnAfterLoad(destination));
    }

    private void CancelPendingSpawn()
    {
        if (pendingSpawn == null)
            return;

        StopCoroutine(pendingSpawn);
        pendingSpawn = null;
    }

    private IEnumerator SpawnAfterLoad(SpawnPoint destination)
    {
        yield return null;

        pendingSpawn = null;
        // Wait until the outgoing scene and duplicate player have completed OnDisable.
        foreach (var manager in GetComponentsInChildren<InputActionManager>(true))
            if (manager.isActiveAndEnabled) manager.EnableInput();

        if (!destination)
        {
            Debug.LogWarning("Scene has no active SpawnPoint; retaining the player position.", this);
            yield break;
        }
        if (!xrOrigin || !xrOrigin.Origin || !xrOrigin.Camera)
        {
            Debug.LogError("Persistent Player needs its XR Origin and Camera assigned.", this);
            yield break;
        }
        ResetPlayerToSpawn(destination.transform.position,
            Quaternion.Euler(0f, destination.transform.eulerAngles.y, 0f));
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
