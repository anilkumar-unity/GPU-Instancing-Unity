using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class GPUInstancedObject : MonoBehaviour
{
    private void Start()
    {
        var mf = GetComponent<MeshFilter>();
        var mr = GetComponent<MeshRenderer>();

        GPUInstancingManager.Instance.Register(
            transform,
            mf.sharedMesh,
            mr.sharedMaterials
        );

        mr.enabled = false;
    }

    private void OnDestroy()
    {
        if (GPUInstancingManager.Instance != null)
            GPUInstancingManager.Instance.Unregister(transform);
    }
}
