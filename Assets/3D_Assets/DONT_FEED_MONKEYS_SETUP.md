# "Don't Feed the Monkeys" Style Setup Instructions

This setup creates the exact experience you described - sitting at a desk, seeing the 3D environment, with the ability to load and play a 2D game on the laptop screen.

## Unity Scene Setup

### 1. **3D Scene Hierarchy (SampleScene.unity):**

```
Scene Hierarchy:
├── DeskGameManager (Empty GameObject)
│   └── DeskGameController (Script Component)
│
├── Main Camera (GameObject) - positioned at desk level
│   ├── Camera Component (renders 3D scene)
│   └── Audio Listener Component
│
├── Canvas (UI Canvas - Screen Space Overlay)
│   └── InstructionText (UI Text Component)
│
├── 3D Environment
│   ├── Desk (3D Model)
│   ├── Laptop (3D Model)
│   │   ├── Laptop Body (uses regular material)
│   │   └── Laptop Screen (uses LaptopScreenMaterial)
│   ├── Chair
│   ├── Room Objects
│   └── Lighting
```

### 2. **Camera Positioning:**
- **Position the Main Camera** like you're sitting at the desk
- **Example position**: (0, 1.2, -1.5) - adjust based on your desk height
- **Example rotation**: (15, 0, 0) - looking slightly down at the desk
- **Field of View**: 60 (will zoom to 30 when focusing on laptop)

### 3. **UI Setup:**
1. **Create Canvas** (Screen Space - Overlay)
2. **Add Text component** as child of Canvas
3. **Position text** at bottom of screen or wherever you prefer
4. **Set text color** to white or yellow for visibility

### 4. **Laptop 3D Model Setup:**
- **Laptop body**: Regular material
- **Laptop screen**: Apply the LaptopScreenMaterial
- **Make sure screen is a separate mesh/material** from the body

## Component Configuration

### **DeskGameController Settings:**

```
Camera Setup:
├── Desk Camera: Drag Main Camera here
├── Normal FOV: 60
├── Zoomed FOV: 30
└── Zoom Speed: 2

Laptop Screen Setup:
├── Laptop Screen Texture: Leave empty (auto-created)
├── Texture Width: 1920
├── Texture Height: 1080
├── Laptop Screen Material: Drag your laptop screen material
└── Material Texture Property: "_MainTex"

2D Game Settings:
└── Game Scene Name: "1-1"

UI:
├── Instruction Canvas: Drag your Canvas
├── Instruction Text: Drag your Text component
├── Play Game Key: F
└── Exit Game Key: Escape

Game Controls:
├── Zoom In Key: E (zoom camera to laptop)
├── Zoom Out Key: Q (zoom camera back)
├── Game Zoom Speed: 2
├── Min Game Zoom: 0.5
└── Max Game Zoom: 3
```

## Controls

| Key | Action |
|-----|--------|
| **F** | Load/Start 2D game on laptop screen (desk mode only) |
| **Escape** | Exit/Unload 2D game (desk mode only) |
| **Tab** | Toggle between desk mode and room exploration |
| **E** | Zoom camera in (focus on laptop - desk mode only) |
| **Q** | Zoom camera out (normal desk view - desk mode only) |
| **+/-** | Zoom the 2D game itself (desk mode only) |
| **WASD** | Move around (room exploration mode only) |
| **Mouse** | Look around (room exploration mode only) |

## The Enhanced Experience Flow

### **1. Game Start (Desk Mode):**
- Camera positioned at desk level
- Can see entire desk area and surroundings
- UI shows "Press [F] to play game | Press [Tab] to explore room"
- Laptop screen is blank/off
- **Cursor is free** for UI interaction

### **2. Press F (Play Game):**
- 2D Mario game loads onto laptop screen only
- 3D scene remains visible around the laptop
- UI changes to show game controls and mode options
- You can see both the game and the desk environment

### **3. Press Tab (Switch to Room Mode):**
- **Game automatically unloads** (can't play while exploring)
- Camera smoothly transitions to room exploration
- **WASD movement + mouse look** enabled
- **Cursor locks** for first-person controls
- UI shows room navigation instructions

### **4. Room Exploration Mode:**
- **Free movement** around the 3D bedroom
- **First-person controls** (WASD + mouse)
- Can explore the entire room
- **No game interaction** available in this mode

### **5. Press Tab Again (Return to Desk):**
- Camera smoothly transitions back to desk position
- Returns to desk mode for game interaction
- **Cursor unlocks** for UI interaction
- Can load and play the 2D game again

## Key Features
- **Dual Mode System**: Desk mode (static, game interaction) vs Room mode (free movement)
- **Smooth Transitions**: Camera smoothly moves between modes
- **Automatic Game Management**: Game unloads when switching modes
- **Context-Sensitive Controls**: Different controls for each mode
- **Realistic Experience**: Like having a computer in a real room

This creates the exact "sitting at a computer desk" experience you described!

## Troubleshooting
- **Game appears fullscreen**: Check that 2D camera gets targetTexture assigned
- **Can't see game**: Verify laptop screen material is applied correctly  
- **UI not showing**: Make sure Canvas is Screen Space - Overlay
- **Camera doesn't zoom**: Check FOV values and camera reference
