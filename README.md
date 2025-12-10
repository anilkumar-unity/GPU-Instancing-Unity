# GPU-Instancing-Unity

A small Unity project demonstrating GPU instancing to render many copies of a mesh efficiently using GPU draw calls. This repository contains example scenes, materials, and scripts to manage and render large numbers of instances with good performance.

## Table of contents
- [Overview](#overview)
- [Features](#features)
- [Requirements](#requirements)
- [Installation](#installation)
- [Usage](#usage)
- [Scripts (assumed/typical)](#scripts-assumedtypical)
- [Performance tips](#performance-tips)
- [Troubleshooting](#troubleshooting)
- [Contributing](#contributing)
- [License](#license)

## Overview
This project demonstrates how to use GPU instancing in Unity to render many identical or similar objects while minimizing CPU overhead and draw calls. The sample scenes show instance management, LOD, and basic culling approaches that keep frame rates high with large instance counts.

## Features
- Example scene(s) with thousands of instanced objects
- Simple manager to create and update instance transforms
- Instanced material/shaders configured for GPU instancing
- Basic frustum/camera-based culling for performance
- Configurable instance count, spacing, and speed

## Requirements
- Unity 2019.4+ (LTS) or Unity 2020/2021+ recommended
- Graphics API that supports GPU instancing (DX11, Vulkan, Metal)
- URP/HDRP — works with built-in render pipeline as well; ensure shaders support instancing

## Installation
1. Clone the repository:
   git clone https://github.com/anilkumar-unity/GPU-Instancing-Unity.git
2. Open the project in Unity (choose the correct Unity version for the project).
3. Allow Unity to import assets and compile scripts.

## Usage
- Open the sample scene (e.g., `Scenes/SampleInstancing.unity`).
- Select the `InstanceManager` GameObject in the Hierarchy (name may differ).
- Adjust parameters: instance count, spacing, mesh/prefab, material, and enable/disable culling or LOD.
- Press Play to run the scene and observe instanced rendering and performance.

Notes:
- Ensure the material used by the mesh has "Enable GPU Instancing" checked in the Inspector.
- If using a custom shader, add the appropriate instancing macros (e.g., UNITY_INSTANCING_CBUFFER_START/END and UNITY_INSTANCING_BUFFER).

## Scripts (assumed/typical)
I couldn't access the repository scripts in full, so below are typical scripts you may have or want to include. Provide actual script contents for precise README API docs.

- InstanceManager.cs
  - Responsibilities: create instance transforms (positions/rotations/scales), upload per-instance data to GPU, control instance count and density, and handle runtime parameter changes.
  - Typical public settings: Mesh mesh, Material material, int instanceCount, float spacing, bool enableCulling.

- GPUInstancerRenderer.cs (or InstanceRenderer.cs)
  - Responsibilities: call Graphics.DrawMeshInstanced / DrawMeshInstancedIndirect / DrawMeshInstancedProcedural depending on approach; manage per-batch arrays; support instancing property blocks.

- InstanceData.cs (or InstanceBuffer.cs)
  - Responsibilities: construct per-instance matrices or structured buffers for GPU use. Provide GPU buffer creation and disposal.

- ExampleSpawner.cs
  - Responsibilities: utility for placing instances in patterns (grid, random scatter) and demo behaviors (simple movement, wind).

If your repository uses different script names or has extra utilities (e.g., culling, LOD, indirect draw with compute buffers), please paste them and I will replace the above placeholders with accurate API and usage examples.

## Performance tips
- Use Graphics.DrawMeshInstancedIndirect with a compute buffer and a shader that reads instance data for the best performance at very high counts.
- Batch instances by material and mesh to minimize draw calls.
- Use GPU frustum culling or a compute-driven culling pipeline for large scenes.
- Avoid per-instance GameObjects—store transforms in native arrays or compute buffers instead.
- Keep per-instance data compact (e.g., matrix + small float parameters); large buffers reduce throughput.

## Troubleshooting
- Nothing renders:
  - Verify the Mesh and Material are assigned to the manager.
  - Ensure the Material has GPU Instancing enabled or the shader supports instancing.
- Low fps with many instances:
  - Switch to indirect instancing (DrawMeshInstancedIndirect) and offload work to compute shader if possible.
  - Reduce per-instance data size and instance count.
- VRAM spikes or crashes:
  - Check buffer sizes and release compute buffers when not used.

## Contributing
Contributions welcome. If you open issues or PRs, please:
- Describe the change or bug with steps to reproduce.
- Keep pull requests focused and include scene/snippet demonstrating the change.

## License
Specify a license (e.g. MIT) here. If you don't have one yet, consider adding one (MIT recommended for small demo projects).

## A note about this README
I attempted to read the repository's scripts to produce exact API docs and usage examples but the automated retrieval was limited. Once you provide the script files or paste the main script contents (for example InstanceManager.cs, GPUInstancerRenderer.cs, etc.), I will update this README to include:
- Exact public API (fields, properties, methods)
- Code snippets showing how to call the system from other scripts
- Concrete examples for Direct vs Indirect instancing usage
