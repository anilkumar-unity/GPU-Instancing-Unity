# GPU-Instancing-Unity

A Unity project providing scripts to easily integrate GPU instancing for rendering multiple instances of objects efficiently.

## Overview

GPU Instancing is a powerful rendering technique that allows you to draw multiple instances of the same mesh with a single draw call, significantly improving performance when rendering many similar objects.

This repository contains two essential scripts to help you implement GPU instancing in your Unity projects:

1. **GPUInstancer.cs** - Manages GPU instancing for multiple objects programmatically
2. **InstancedObject.cs** - Component-based approach for individual GameObjects

## Scripts

### 1. GPUInstancer.cs

A manager script that handles GPU instancing for rendering thousands of instances of the same mesh.

**Features:**
- Spawn multiple instances programmatically
- Configurable spawn area
- Random position, rotation, and scale
- Per-instance color variation
- Batch rendering (handles Unity's 1023 instance per batch limitation)
- Runtime regeneration of instances
- Gizmo visualization of spawn area

**Usage:**

1. Create an empty GameObject in your scene
2. Attach the `GPUInstancer` component
3. Assign a mesh and material (ensure material has "Enable GPU Instancing" checked)
4. Configure settings:
   - Instance Count
   - Spawn Area
   - Scale range
   - Random rotation option
5. Press Play to see instances rendered

**Inspector Parameters:**
```
- instanceMesh: The mesh to be instanced
- instanceMaterial: Material with GPU instancing enabled
- instanceCount: Number of instances to create
- spawnArea: Size of the spawning area (Vector3)
- minScale/maxScale: Scale variation range
- randomRotation: Enable random rotation
```

**Public Methods:**
```csharp
// Regenerate all instances
RegenerateInstances()

// Update instance count at runtime
UpdateInstanceCount(int newCount)
```

### 2. InstancedObject.cs

A component script to attach to individual GameObjects for GPU instancing support.

**Features:**
- Automatic GPU instancing setup
- Per-instance color tinting
- MaterialPropertyBlock support for per-instance properties
- Runtime color modification
- Custom property support
- Material validation warnings

**Usage:**

1. Attach `InstancedObject` component to any GameObject with a MeshRenderer
2. Ensure the material has "Enable GPU Instancing" checked in the Inspector
3. Configure instance color and other settings
4. The script automatically sets up instancing on Start

**Inspector Parameters:**
```
- enableInstancing: Toggle GPU instancing on/off
- instanceColor: Color tint for this instance
- usePropertyBlock: Use MaterialPropertyBlock for per-instance properties
```

**Public Methods:**
```csharp
// Change instance color at runtime
SetInstanceColor(Color color)

// Enable/disable instancing
SetInstancingEnabled(bool enabled)

// Set custom float property
SetCustomProperty(string propertyName, float value)

// Set custom color property
SetCustomColorProperty(string propertyName, Color value)
```

## Setup Requirements

### Material Setup
For GPU instancing to work, your materials must have GPU instancing enabled:

1. Select your material in the Project window
2. In the Inspector, check **"Enable GPU Instancing"** checkbox
3. This option is available for most built-in shaders and custom shaders with instancing support

### Shader Support
If you're using custom shaders, ensure they support GPU instancing by including:

```hlsl
#pragma multi_compile_instancing

// In vertex shader
UNITY_SETUP_INSTANCE_ID(v);

// To access per-instance properties
UNITY_DEFINE_INSTANCED_PROP(float4, _Color)
```

## Example Scenarios

### Scenario 1: Rendering a Forest
Use `GPUInstancer` to render thousands of trees:
```csharp
// Attach to empty GameObject
// Assign tree mesh and material
// Set instanceCount = 5000
// Set spawnArea = (200, 0, 200)
```

### Scenario 2: Individual Object Variations
Use `InstancedObject` for objects that need individual control:
```csharp
// Attach to each tree GameObject
// Set different instanceColor for each
// Modify at runtime: GetComponent<InstancedObject>().SetInstanceColor(Color.red);
```

### Scenario 3: Runtime Instancing
Create instances dynamically:
```csharp
GPUInstancer instancer = GetComponent<GPUInstancer>();
instancer.UpdateInstanceCount(10000);
instancer.RegenerateInstances();
```

## Performance Tips

1. **Batch Size**: Unity limits 1023 instances per draw call. GPUInstancer handles this automatically.
2. **Material Sharing**: All instances must share the same material for GPU instancing to work.
3. **LOD**: Combine with Unity's LOD system for better performance at distance.
4. **Occlusion Culling**: Enable occlusion culling to avoid rendering hidden instances.
5. **Shadow Casting**: Consider disabling shadow casting for distant instances.

## Troubleshooting

**Issue: Instances not rendering**
- Ensure material has "Enable GPU Instancing" enabled
- Check that mesh and material are assigned
- Verify shader supports instancing

**Issue: Performance not improving**
- Confirm instances share the same material
- Check Graphics API supports instancing (DX11+, Metal, Vulkan)
- Verify profiler shows reduced draw calls

**Issue: Colors not varying**
- Ensure shader supports per-instance properties
- Check MaterialPropertyBlock is enabled
- Verify shader has _Color property with UNITY_DEFINE_INSTANCED_PROP

## Compatibility

- Unity 2019.4 or later
- Graphics APIs: DirectX 11+, Metal, Vulkan, OpenGL ES 3.0+
- All render pipelines (Built-in, URP, HDRP with compatible shaders)

## License

This project is open source and available for use in your Unity projects.

## Contributing

Feel free to submit issues and enhancement requests!

## Author

Anil Kumar