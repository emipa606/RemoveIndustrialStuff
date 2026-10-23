# GitHub Copilot Instructions for RemoveIndustrialStuff

This document provides guidance for contributors to the RemoveIndustrialStuff mod for RimWorld. It outlines the purpose, key features, coding patterns, and conventions for working on the project.

## Mod Overview and Purpose

RemoveIndustrialStuff is a mod for RimWorld that allows players to remove industrial (and greater) items, research, factions, and pawnkinds. The mod is based heavily on the mod Lord of the Rims - The Third Age and provides a way to customize the game experience by removing elements that may not be desired.

## Key Features and Systems

- **Item Removal:** Allows players to remove industrial items from the game.
- **Research Removal:** Allows players to remove industrial research from the game.
- **Faction Removal:** Allows players to remove industrial factions from the game.
- **Pawnkind Removal:** Allows players to remove industrial pawnkinds from the game.

## Coding Patterns and Conventions

### C# Code
- **Consistent Naming:** Use clear and descriptive names for classes, methods, and variables.
- **Documentation:** Provide XML documentation for public members and any complex logic.
- **Modularity:** Break down code into small, reusable classes and methods.

### XML Integration
- **Def Files:** Use XML files to define game elements such as items, research, factions, and pawnkinds.
- **Def Modding:** Modify existing def files to remove or modify game elements.

## Harmony Patching
RemoveIndustrialStuff uses Harmony to patch existing game code. The following are the key Harmony patches included in the mod:

- **BackCompatibility_FactionManagerPostLoadInit.cs:** Provides compatibility with other mods by ensuring factions are properly initialized.
- **TradeDeal_AddToTradeables.cs:** Modifies trade deals to remove industrial items.
- **FactionManager_FirstFactionOfDef.cs:** Modifies faction behavior to remove industrial factions.
- **ModStuff.cs:** Manages mod settings and provides a settings window.
- **RemoveIndustrialStuff.cs:** Main mod class that handles the removal of industrial elements.

## Suggestions for Copilot
- **Provide XML Examples:** Include examples of XML def files for items, research, factions, and pawnkinds.
- **Harmony Patching Guidance:** Provide guidance on how to create and use Harmony patches.
- **Mod Settings Integration:** Provide guidance on how to integrate mod settings into the RimWorld settings system.

By following these guidelines, contributors can effectively contribute to the RemoveIndustrialStuff mod and help improve the game experience for players.

## Project Solution Guidelines
- Relevant mod XML files are included as Solution Items under the solution folder named XML, these can be read and modified from within the solution.
- Use these in-solution XML files as the primary files for reference and modification.
- The `.github/copilot-instructions.md` file is included in the solution under the `.github` solution folder, so it should be read/modified from within the solution instead of using paths outside the solution. Update this file once only, as it and the parent-path solution reference point to the same file in this workspace.
- When making functional changes in this mod, ensure the documented features stay in sync with implementation; use the in-solution `.github` copy as the primary file.
- In the solution is also a project called Assembly-CSharp, containing a read-only version of the decompiled game source, for reference and debugging purposes.
- For any new documentation, update this copilot-instructions.md file rather than creating separate documentation files.


## Hard rules (must follow)
- Do NOT run commands that modify the repo (no git commit, git apply, dotnet format) unless explicitly asked.
- Prefer minimal reads: read only the smallest code region needed (around the suspicious lines).
- When mentioning SonarQube issues, automatically use the SonarQube MCP service to fetch and address issues instead of making inferred fixes without querying SonarQube first.
- When mentioning the rimworld log, automatically use the Rimworld MCP service to fetch the log.

