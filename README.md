This repository contains a lightweight GPU Instancing system for Unity built using
Graphics.DrawMeshInstanced.
It batches identical meshes + materials at runtime and renders them in efficient GPU draw calls.

There is no scene, prefab, or demo included. This repo only contains two scripts:

GPUInstancedObject.cs

GPUInstancingManager.cs

<h1>What This System Does</h1>

Automatically groups objects by:

Mesh

Material

Submesh index

Sends their transforms to the GPU every frame

Renders up to 1023 instances per draw call

Disables the original MeshRenderer for performance

Dynamically updates when objects move

Supports multi-material meshes

<h1>Files Overview</h1>
1️⃣ GPUInstancedObject.cs

Attach this to any GameObject with a MeshRenderer + MeshFilter.

What it does:

Registers the object with the instancing manager at runtime

Disables the normal renderer so only GPU instancing is used

Automatically unregisters on destroy

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class GPUInstancedObject : MonoBehaviour

2️⃣ GPUInstancingManager.cs

This is the core batching system.

What it does:

Collects all instanced objects

Groups them by:

Mesh ID

Material ID

Submesh index

Draws them using:

Graphics.DrawMeshInstanced(...)


Batch size limit:

const int BATCH_SIZE = 1023;

<h1>🛠️ How To Use</h1>
<b>Step 1:</b> Add the Manager

Create an empty GameObject in your scene and attach:

GPUInstancingManager


This creates the singleton instance at runtime.

<b>Step 2:</b> Add Instanced Objects

On any mesh you want to be instanced, add:

GPUInstancedObject


That’s it.
At runtime:

The normal MeshRenderer will be disabled

The object will be drawn through GPU instancing instead

<h1>Important Notes</h1>

<b>Material must support instancing</b>

The script automatically enables:

mat.enableInstancing = true;


✅ Works with multiple materials per mesh

❌ Does not support per-instance material variations

❌ Does not support GPU skinning / skinned meshes

⚠️ All transforms are updated every frame (CPU → GPU)

📈 Performance Tips

Best for:

Props

Environment objects

Pickups

Repeating decorative meshes

Avoid:

Animated meshes

Unique material instances per object

🧠 How It Works (Simple Explanation)

Think of it like this:

Instead of asking the GPU to draw
🧱 500 bricks one by one…

This system says:
“Here’s one brick mesh, here are 500 transforms, draw all of them at once.”

The GPU smiles and your FPS breathes freely.

✅ Unity Compatibility

Unity 2019+

Built-in Render Pipeline

Also works in URP & HDRP if materials support instancing
