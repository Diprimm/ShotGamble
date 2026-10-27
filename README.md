
# ShotGamble - FPS Prototype

A technical Unity prototype of a fast-paced retro FPS inspired by classic shooters like *Quake*. The core objective of this project was to architect a highly scalable, data-driven weapon system optimized for modularity, rapid iteration, and high-fidelity game feel.

This repository serves as a showcase of advanced **Gameplay Programming** and **Systems Architecture** in Unity.



[Demo Gameplay](https://github.com/user-attachments/assets/eb9ba1f9-6a91-4aae-a33a-4fa6436f2f0c)



---

## 🛠️ Systems Architecture & Technical Features

### 🔫 Monolithic & Data-Driven Weapon System
Instead of using messy inheritance hierarchies with multiple scripts for different gun behaviors, this framework implements a **single, highly customizable monolithic script/system** to handle all weapon logic. 

Every weapon in the game—from a fast-firing rifle to a multi-pellet shotgun—is instantiated and tuned directly within the Unity Inspector by tweaking serialized parameters:

*   **Firing Modes:** Seamlessly switch between **Hitscan** (raycasting for instant feedback) and **Projectiles** (instantiating actual physical physics-based objects traveling through time/space).
*   **Shotgun & Spread Logic:** Customizable pellet count, firing vectors, and random spread matrices per shot.
*   **Performance Metrics:** Exposed parameters for Fire Rate, Base Damage, Range, and Custom Projectile Velocity.

<img width="500" height="668" alt="inspector" src="https://github.com/user-attachments/assets/1702ea42-246f-49d8-8a1c-833fd4f697a1" />

### 💥 Advanced Juiciness & Game Feel
To replicate the visceral feedback of classic 90s shooters, the weapon system dynamically drives several visual and kinetic modules:
*   **Procedural Recoil:** Handles position and rotation spring forces per shot.
*   **Dynamic Camera Shake:** Intensity parameters mapped per weapon profile to amplify impact.

---

## ⚠️ Important Note Regarding 3D & Audio Assets

To comply with copyright guidelines, **several textures used in this prototype belong to third-party creators and are NOT included in this repository** (or are explicitly ignored via `.gitignore`). 

If you clone this project:
*   Wall Textures may be missing.
*   The underlying modular architecture and scripts will function seamlessly regardless of the visual assets used.

---

## 📝 License

This project is licensed under a custom Software and Source Code License Agreement. Commercial use and third-party asset distribution are strictly prohibited. See the [LICENSE](LICENSE) file for the full legal terms.
