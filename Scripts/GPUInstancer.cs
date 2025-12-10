using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// GPU Instancer - Manages GPU instancing for multiple objects
/// This script handles the rendering of multiple instances of the same mesh using GPU instancing
/// Attach this to a GameObject to manage instanced rendering
/// </summary>
public class GPUInstancer : MonoBehaviour
{
    [Header("Instance Settings")]
    [Tooltip("The mesh to be instanced")]
    public Mesh instanceMesh;
    
    [Tooltip("The material to be used (must have 'Enable GPU Instancing' checked)")]
    public Material instanceMaterial;
    
    [Tooltip("Number of instances to create")]
    public int instanceCount = 1000;
    
    [Header("Spawn Area")]
    [Tooltip("Size of the area where instances will spawn")]
    public Vector3 spawnArea = new Vector3(100f, 10f, 100f);
    
    [Header("Instance Properties")]
    [Tooltip("Minimum scale for instances")]
    public float minScale = 0.5f;
    
    [Tooltip("Maximum scale for instances")]
    public float maxScale = 2.0f;
    
    [Tooltip("Enable random rotation for instances")]
    public bool randomRotation = true;

    private List<Matrix4x4> matrices = new List<Matrix4x4>();
    private MaterialPropertyBlock propertyBlock;
    private Vector4[] colors;
    
    void Start()
    {
        if (instanceMesh == null || instanceMaterial == null)
        {
            Debug.LogError("GPUInstancer: Mesh or Material is not assigned!");
            return;
        }
        
        if (!instanceMaterial.enableInstancing)
        {
            Debug.LogWarning("GPUInstancer: Material does not have GPU Instancing enabled. Enable it in the material settings.");
        }
        
        InitializeInstances();
    }
    
    /// <summary>
    /// Initialize instance positions, rotations, and scales
    /// </summary>
    void InitializeInstances()
    {
        matrices.Clear();
        colors = new Vector4[instanceCount];
        propertyBlock = new MaterialPropertyBlock();
        
        for (int i = 0; i < instanceCount; i++)
        {
            // Random position within spawn area
            Vector3 position = new Vector3(
                Random.Range(-spawnArea.x / 2, spawnArea.x / 2),
                Random.Range(0, spawnArea.y),
                Random.Range(-spawnArea.z / 2, spawnArea.z / 2)
            );
            
            // Random rotation
            Quaternion rotation = randomRotation ? 
                Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)) : 
                Quaternion.identity;
            
            // Random scale
            float scale = Random.Range(minScale, maxScale);
            Vector3 scaleVec = new Vector3(scale, scale, scale);
            
            // Create transformation matrix
            matrices.Add(Matrix4x4.TRS(position, rotation, scaleVec));
            
            // Random color for variation
            colors[i] = new Vector4(Random.value, Random.value, Random.value, 1.0f);
        }
    }
    
    void Update()
    {
        if (instanceMesh == null || instanceMaterial == null || matrices.Count == 0)
            return;
        
        // Draw instances in batches (max 1023 per batch due to Unity limitation)
        int batchSize = 1023;
        for (int i = 0; i < matrices.Count; i += batchSize)
        {
            int count = Mathf.Min(batchSize, matrices.Count - i);
            List<Matrix4x4> batch = matrices.GetRange(i, count);
            
            // Set color property for variation (optional)
            Vector4[] batchColors = new Vector4[count];
            System.Array.Copy(colors, i, batchColors, 0, count);
            propertyBlock.SetVectorArray("_Color", batchColors);
            
            // Draw instanced mesh
            Graphics.DrawMeshInstanced(instanceMesh, 0, instanceMaterial, batch, propertyBlock);
        }
    }
    
    /// <summary>
    /// Regenerate instances at runtime
    /// </summary>
    public void RegenerateInstances()
    {
        InitializeInstances();
    }
    
    /// <summary>
    /// Update instance count at runtime
    /// </summary>
    public void UpdateInstanceCount(int newCount)
    {
        if (newCount > 0)
        {
            instanceCount = newCount;
            InitializeInstances();
        }
    }
    
    void OnDrawGizmosSelected()
    {
        // Visualize spawn area in editor
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, spawnArea);
    }
}
