# Unity Laptop Screen Setup Guide
## "Don't Feed the Monkeys" Style 2D Game on 3D Laptop Screen

### 1. SCENE SETUP

#### Main Scene (your 3D bedroom scene)
- Keep your existing 3D bedroom scene with desk and laptop model

#### Game Scene (Mario scene)
**CRITICAL: This is likely where the problem is**
- Your Mario scene MUST be added to Build Settings
- Go to **File > Build Settings**
- Click **Add Open Scenes** while your Mario scene is open
- OR drag the Mario scene file from Project window to Build Settings
- **Verify the scene name matches exactly** - if your scene file is called "Mario.unity", the scene name should be "Mario"

### 2. GAMEOBJECT HIERARCHY SETUP

#### In your main 3D scene, create this hierarchy:

```
Main Camera (your existing 3D camera)
├── DeskGameController (Empty GameObject)
├── Canvas (for UI)
│   └── InstructionText (TextMeshPro - UI)
├── DeskPosition (Empty GameObject - positioned at desk view)
└── RoomStartPosition (Empty GameObject - positioned for room exploration)
```

### 3. COMPONENT ASSIGNMENTS

#### DeskGameController GameObject:
**Components to add:**
- `DeskGameController` script
- `Character Controller` (for room movement)

**DeskGameController Script Fields:**
```
Desk Camera: [Drag your Main Camera here]
Normal FOV: 60
Zoomed FOV: 30
Zoom Speed: 2

Laptop Screen Texture: [Create new RenderTexture - see step 4]
Texture Width: 1920
Texture Height: 1080
Laptop Screen Material: [Your laptop screen material]
Material Texture Property: "_MainTex"

Game Scene Name: "Mario" (MUST match your scene name exactly)

Instruction Canvas: [Drag your Canvas here]
Instruction Text: [Drag your TextMeshPro component here]

Desk Position: [Drag DeskPosition GameObject here]
Room Start Position: [Drag RoomStartPosition GameObject here]
```

### 4. RENDER TEXTURE SETUP

#### Create RenderTexture:
1. Right-click in Project window
2. **Create > Render Texture**
3. Name it "LaptopScreenTexture"
4. Set Size: **1920 x 1080**
5. Set Depth Buffer: **16 bit**
6. Drag this to the "Laptop Screen Texture" field in DeskGameController

### 5. LAPTOP SCREEN MATERIAL SETUP

#### Method 1: Using existing material
1. Find your laptop screen material
2. In the Inspector, change the **Albedo** texture to your new RenderTexture
3. Drag this material to "Laptop Screen Material" field

#### Method 2: Create new material with custom shader
1. Create new Material: **Create > Material**
2. Name it "LaptopScreenMaterial"
3. Change Shader to **Custom/LaptopScreen** (if you have the custom shader)
4. OR use **Standard** shader and set:
   - **Albedo**: Your RenderTexture
   - **Emission**: Your RenderTexture (for screen glow)
   - **Emission** intensity: 0.5-1.0

### 6. MARIO SCENE REQUIREMENTS

#### Your Mario scene MUST have:
1. **Exactly one Camera** (this is critical)
2. Camera should be **orthographic** (2D games use orthographic cameras)
3. Camera **Audio Listener can be present** (script will disable it)

#### To verify Mario scene structure:
1. Open your Mario scene
2. Check the Hierarchy - you should see a Camera GameObject
3. Select the Camera, verify it's set to **Projection: Orthographic**
4. Note the exact scene name in the Project window

### 7. BUILD SETTINGS VERIFICATION

**This is the most likely cause of your issue:**

1. Go to **File > Build Settings**
2. Verify your Mario scene is listed
3. **Scene name must match exactly** what you put in "Game Scene Name" field
4. If scene shows as "Mario" in build settings, use "Mario" in the script
5. If scene shows as "SuperMario" or something else, update the script accordingly

### 8. UI SETUP

#### Canvas Setup:
- **Canvas Render Mode**: Screen Space - Overlay
- **Canvas Scaler**: Scale With Screen Size
- **Reference Resolution**: 1920x1080

#### TextMeshPro Setup:
- **MUST use TextMeshPro - Text (UI)**, not regular Text component
- Position in bottom-left or wherever you prefer
- Set font size appropriately (e.g., 24)

### 9. POSITION SETUP

#### DeskPosition GameObject:
1. Position this where you want the camera when at the desk
2. Usually directly in front of the laptop, focused on the screen
3. Rotation should be looking at the laptop

#### RoomStartPosition GameObject:
1. Position this where you want to start when exploring the room
2. Usually a few steps back from the desk
3. Rotation can be looking at the desk initially

### 10. TROUBLESHOOTING CHECKLIST

If "Scene is loaded" debug message doesn't appear:

**Check these in order:**
1. ✅ Mario scene is in Build Settings
2. ✅ Scene name in Build Settings matches "Game Scene Name" field exactly
3. ✅ Mario scene has a Camera component
4. ✅ DeskGameController script is attached to a GameObject
5. ✅ All required fields in DeskGameController are assigned
6. ✅ Press 'F' key while in Play mode and at the desk

**If Mario scene loads but appears on main screen instead of laptop:**
1. ✅ RenderTexture is created and assigned
2. ✅ Laptop screen material uses the RenderTexture
3. ✅ Laptop 3D model has the correct material applied
4. ✅ Mario scene camera is orthographic (not perspective)

### 11. TESTING PROCEDURE

1. **Play the scene**
2. **Press 'F'** - you should see debug logs:
   - "Loading game scene: Mario"
   - "Build setting scene 0: 'YourSceneName'"
   - "Setting up game camera..."
   - "Scene is loaded" ← **This should appear**
   - "Found camera: 'Main Camera'" (or whatever your camera is named)
   - "Target texture set"

3. **If successful**: Mario game should appear on laptop screen only
4. **Press 'Tab'** to switch to room exploration mode
5. **Press 'Tab'** again to return to desk

### 12. COMMON ISSUES & SOLUTIONS

**Problem**: "Scene is loaded" doesn't appear
**Solution**: Scene not in Build Settings or name mismatch

**Problem**: Game appears on main screen instead of laptop
**Solution**: Check RenderTexture assignment and material setup

**Problem**: Can't move in room mode
**Solution**: Add Character Controller component to DeskGameController GameObject

**Problem**: UI doesn't show
**Solution**: Use TextMeshPro - Text (UI), not regular Text component

**Problem**: Black laptop screen
**Solution**: Check material is using RenderTexture, check emission settings

Let me know which specific step is failing and I can help debug further!
