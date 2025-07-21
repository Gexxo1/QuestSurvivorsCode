# Quest Survivors - Game Scripts

Welcome! This repository contains the C# scripts for my game **'Quest Survivors'**, a top-down 2D roguelite I developed in Unity.

### [➡️ Play the game on itch.io! ⬅️](https://gexxo1.itch.io/questsurvivors)

This was the first major game I created, and it represents a huge learning milestone for me. While the code isn't perfect, I'm proud of the result and happy to share my journey.

> **Note**: This repository contains **only the `Scripts` folder** from the Unity project. Assets like graphics, audio, and scenes are not included.

---

## 🎓 Bachelor's Thesis Project

This game wasn't just a passion project; it also served as the case study for my Bachelor's thesis in Computer Science.

*   **Thesis Title:** "An In-Depth Analysis of Programming Aspects in 2D Video Game Development with Unity"
*   **Author:** Giuliano Spata
*   **University:** University of Catania (Department of Mathematics and Computer Science)

While the project's goal was to apply and analyze programming solutions like the **Observer, Object Pooling, and Singleton patterns**, I also poured my creativity into it to make a genuinely fun and complete game. The result is a blend of academic requirements and my personal vision for a fast-paced roguelite.

---

## About the Game

'Quest Survivors' is a 2D top-down survivor roguelite where you fight hordes of enemies, level up with powerful abilities, and survive till the final boss.
It is inspired by games like 'Vampire Survivors' or 'Enter the Gungeon'.
The game was built using **Unity 2D (version 2021.3.16f1)**.

## Scripts Structure

Here's a brief overview of the main script folders:

*   `Ability`: All scripts related to player abilities and their logic.
*   `Actor`: The class hierarchy for all "living" entities in the game (e.g., Player, Enemies).
*   `Collectables`: Scripts for items that can be picked up, like power-ups or potions.
*   `DataPersistence`: Handles saving and loading game data.
*   `Enum`: Contains various public enums used for utility across the project.
*   `Interfaces`: C# interfaces implemented by various classes to enforce a common structure.
*   `Managers`: Singleton managers like `MenuManager`, `PowerupManager`, and `WaveManager`.
*   `Mechanics_VisualEffects`: Reusable components for abilities or power-ups, focused on mechanics or visual feedback.
*   `Misc`: Miscellaneous utility scripts that don't fit in other categories.
*   `NeoStats`: An experimental stats system, currently not implemented.
*   `Optimization`: Scripts used for performance testing or features like Object Pooling.
*   `PowerUps`: Contains the class hierarchy for all types of power-ups.
*   `ScriptableObjects`: `ScriptableObject` assets used to store data for stats, abilities, etc.
*   `Structs`: Custom public structs used for utility.
*   `UI`: Scripts that manage User Interface elements.
*   `Utility`: Static utility classes and methods for both programming and editor purposes.
*   `Weapons`: All weapon types used by the Player and their corresponding projectiles.
*   `Z_Unused`: Discarded scripts that are not used in the final game.
*   `GameManager.cs`: The core of the game. It's a Singleton that acts as the central hub for all other managers. (Located in the root `Scripts` folder).

## License

This project is licensed under the MIT License. See the `LICENSE` file for details.