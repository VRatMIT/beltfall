# Splash Notes:

This demo is adapted from a project started by previous club members.

### Requirements:
- Unity **2022.3 LTS** or later (this branch is on 2022.3.55f1)
- Glove controler
- Optional: Android build package if you want to build to a headset

### How to use with controler:
- Plug in controler ESP32, make sure red light turns on.
- Connect your computer to the ESP32's wifi
    - Name: Glove_#
    - Password: 12345678
- Run game

### Nerd stuff:
All the code relevant to the glove controller input is in:
- Assets/Singularity/Singularitymanager.cs: handles Wi-Fi connection to the ESP32
- Assets/Project/Asteroid Belt/Scripts/input_manager.cs: The **esp_message_received** function receives message from SingularityManager and extracts input information.
- Assets/Project/Asteroid Belt/Scripts/player_movement.cs: Takes input information from input_manager and moves the spaceship with it.

# 🌌 Beltfall: Tau Ceti

**A gritty multiplayer sci-fi shooter set in the asteroid belts of Tau Ceti.**  
Pick a faction. Fight for control. No heroes, only survivors.

---

## 🚀 Game Overview

> *Year 3458.* Humanity has long left the Solar System behind — but brought war with it.  
> In the distant **Tau Ceti** system, powerful factions clash in the asteroid belts, fighting for resources, power, and ideological dominance.  
> Players take on the role of fighter pilots, engaging in fast-paced multiplayer battles across rocky and icy asteroid fields.

---

## 🛠️ Development Setup

### Prerequisites

- Unity **2022.3 LTS** or later
- Git & Git LFS (for large asset handling)
- Optional: Visual Studio with Unity tools

### Clone the Repo

```bash
git clone https://github.com/your-org/tau-ceti-fractured-stars.git
cd tau-ceti-fractured-stars
