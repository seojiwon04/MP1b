using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

[DisallowMultipleComponent]
public class RayHighlight : MonoBehaviour
{
    [Range(1.01f, 1.15f)] public float outlineScale = 1.04f;
    protected virtual Color OutlineColor => Color.green;

    private XRBaseInteractable interactable;
    private Material outlineMaterial;
    private List<GameObject> outlines = new();
    private HashSet<IXRHoverInteractor> hovering = new();

    protected virtual void Awake()
    {
        interactable = GetComponent<XRBaseInteractable>();
        if (!interactable) { enabled = false; return; }

        var shader = Resources.Load<Shader>("SimpleRayOutline");

        outlineMaterial = new Material(shader);
        outlineMaterial.SetColor("_OutlineColor", OutlineColor);
        outlineMaterial.SetFloat("_OutlineScale", outlineScale);

        foreach (var source in GetComponentsInChildren<MeshFilter>())
        {
            var renderer = source.GetComponent<MeshRenderer>();
            if (!source.sharedMesh || !renderer || !renderer.enabled) 
                continue;
            var outline = new GameObject("Ray Outline");
            outline.transform.SetParent(source.transform, false);
            outline.layer = source.gameObject.layer;
            outline.AddComponent<MeshFilter>().sharedMesh = source.sharedMesh;

            var copy = outline.AddComponent<MeshRenderer>();
            var materials = new Material[source.sharedMesh.subMeshCount];
            for (int i = 0; i < materials.Length; i++)
                materials[i] = outlineMaterial;
            copy.sharedMaterials = materials;
            copy.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            copy.receiveShadows = false;

            var properties = new MaterialPropertyBlock();
            properties.SetVector("_Center", source.sharedMesh.bounds.center);
            copy.SetPropertyBlock(properties);
            outline.SetActive(false);
            outlines.Add(outline);
        }
    }

    protected virtual void OnEnable()
    {
        if (!interactable || !outlineMaterial) 
            return;
        interactable.hoverEntered.AddListener(OnHoverEntered);
        interactable.hoverExited.AddListener(OnHoverExited);
        foreach (var hand in interactable.interactorsHovering)
            if (IsRay(hand)) hovering.Add(hand);
        SetVisible(hovering.Count > 0);
    }

    private static bool IsRay(IXRHoverInteractor hand)
        => hand is XRRayInteractor || hand is NearFarInteractor;

    private void OnHoverEntered(HoverEnterEventArgs args)
    {
        if (!IsRay(args.interactorObject)) 
            return;
        hovering.Add(args.interactorObject);
        SetVisible(true);
    }

    private void OnHoverExited(HoverExitEventArgs args)
    {
        hovering.Remove(args.interactorObject);
        SetVisible(hovering.Count > 0);
    }

    private void SetVisible(bool visible)
    {
        foreach (var outline in outlines)
            if (outline) outline.SetActive(visible);
    }

    protected virtual void OnDisable()
    {
        if (interactable)
        {
            interactable.hoverEntered.RemoveListener(OnHoverEntered);
            interactable.hoverExited.RemoveListener(OnHoverExited);
        }
        hovering.Clear();
        SetVisible(false);
    }

    protected virtual void OnDestroy()
    {
        foreach (var outline in outlines)
            if (outline) Destroy(outline);
        if (outlineMaterial) Destroy(outlineMaterial);
    }
}

