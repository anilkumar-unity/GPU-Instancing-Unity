using UnityEngine;

/// <summary>
/// Instanced Object Component - Attach to GameObjects to enable GPU instancing
/// This script marks an object for GPU instancing and handles automatic setup
/// Works with MeshRenderer components
/// </summary>
[RequireComponent(typeof(MeshRenderer))]
public class InstancedObject : MonoBehaviour
{
    [Header("Instancing Settings")]
    [Tooltip("Enable GPU instancing for this object")]
    public bool enableInstancing = true;
    
    [Tooltip("Instance color tint (requires shader support)")]
    public Color instanceColor = Color.white;
    
    [Header("Material Property Block")]
    [Tooltip("Use MaterialPropertyBlock for per-instance properties")]
    public bool usePropertyBlock = true;
    
    private MaterialPropertyBlock propertyBlock;
    private MeshRenderer meshRenderer;
    
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        
        if (meshRenderer == null)
        {
            Debug.LogError("InstancedObject: MeshRenderer component not found!");
            return;
        }
        
        SetupInstancing();
    }
    
    /// <summary>
    /// Setup GPU instancing for this object
    /// </summary>
    void SetupInstancing()
    {
        if (!enableInstancing)
            return;
        
        // Check if material supports GPU instancing
        foreach (Material mat in meshRenderer.sharedMaterials)
        {
            if (mat != null && !mat.enableInstancing)
            {
                Debug.LogWarning($"InstancedObject: Material '{mat.name}' on {gameObject.name} does not have GPU Instancing enabled. Enable it in the material inspector.");
            }
        }
        
        // Setup property block for per-instance properties
        if (usePropertyBlock)
        {
            propertyBlock = new MaterialPropertyBlock();
            UpdatePropertyBlock();
        }
    }
    
    /// <summary>
    /// Update the MaterialPropertyBlock with instance-specific properties
    /// </summary>
    void UpdatePropertyBlock()
    {
        if (propertyBlock == null || meshRenderer == null)
            return;
        
        // Set color property
        propertyBlock.SetColor("_Color", instanceColor);
        
        // You can add more per-instance properties here
        // Example: propertyBlock.SetFloat("_Metallic", metallicValue);
        
        meshRenderer.SetPropertyBlock(propertyBlock);
    }
    
    /// <summary>
    /// Change instance color at runtime
    /// </summary>
    public void SetInstanceColor(Color color)
    {
        instanceColor = color;
        if (usePropertyBlock)
        {
            UpdatePropertyBlock();
        }
    }
    
    /// <summary>
    /// Enable or disable GPU instancing at runtime
    /// </summary>
    public void SetInstancingEnabled(bool enabled)
    {
        enableInstancing = enabled;
        if (enabled)
        {
            SetupInstancing();
        }
    }
    
    /// <summary>
    /// Set a custom property on the MaterialPropertyBlock
    /// </summary>
    public void SetCustomProperty(string propertyName, float value)
    {
        EnsurePropertyBlockExists();
        propertyBlock.SetFloat(propertyName, value);
        ApplyPropertyBlock();
    }
    
    /// <summary>
    /// Set a custom color property on the MaterialPropertyBlock
    /// </summary>
    public void SetCustomColorProperty(string propertyName, Color value)
    {
        EnsurePropertyBlockExists();
        propertyBlock.SetColor(propertyName, value);
        ApplyPropertyBlock();
    }
    
    /// <summary>
    /// Ensure MaterialPropertyBlock is initialized
    /// </summary>
    private void EnsurePropertyBlockExists()
    {
        if (propertyBlock == null)
        {
            propertyBlock = new MaterialPropertyBlock();
        }
    }
    
    /// <summary>
    /// Apply the MaterialPropertyBlock to the mesh renderer
    /// </summary>
    private void ApplyPropertyBlock()
    {
        if (meshRenderer != null && propertyBlock != null)
        {
            meshRenderer.SetPropertyBlock(propertyBlock);
        }
    }
    
    void OnValidate()
    {
        // Update property block when values change in editor
        if (Application.isPlaying && usePropertyBlock)
        {
            UpdatePropertyBlock();
        }
    }
}
