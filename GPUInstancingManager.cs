using System.Collections.Generic;
using UnityEngine;

public class GPUInstancingManager : MonoBehaviour
{
    public static GPUInstancingManager Instance;

    const int BATCH_SIZE = 1023;

    class Group
    {
        public Mesh mesh;
        public Material material;
        public int subMesh;
        public List<Transform> transforms = new List<Transform>();
    }

    Dictionary<string, Group> groups = new Dictionary<string, Group>();

    void Awake()
    {
        Instance = this;
    }

    public void Register(Transform t, Mesh mesh, Material[] materials)
    {
        for (int i = 0; i < materials.Length; i++)
        {
            Material mat = materials[i];
            mat.enableInstancing = true;
            string key = mesh.GetInstanceID() + "_" + mat.GetInstanceID() + "_" + i;

            if (!groups.TryGetValue(key, out Group g))
            {
                g = new Group
                {
                    mesh = mesh,
                    material = mat,
                    subMesh = i
                };
                groups.Add(key, g);
            }

            g.transforms.Add(t);
        }
    }

    public void Unregister(Transform t)
    {
        foreach (var g in groups.Values)
            g.transforms.Remove(t);
    }

    void Update()
    {
        foreach (var g in groups.Values)
        {
            int total = g.transforms.Count;

            for (int start = 0; start < total; start += BATCH_SIZE)
            {
                int count = Mathf.Min(BATCH_SIZE, total - start);
                Matrix4x4[] matrices = new Matrix4x4[count];

                for (int i = 0; i < count; i++)
                    matrices[i] = g.transforms[start + i].localToWorldMatrix;

                Graphics.DrawMeshInstanced(
                    g.mesh,
                    g.subMesh,
                    g.material,
                    matrices
                );
            }
        }
    }
}
