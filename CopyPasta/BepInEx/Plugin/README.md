# Angry British Bug – Plugin (startup message)

This folder builds **AngryBritishBug.dll**, which shows the in-game message *"The bugs are angry and British!"* when the game starts (once per session).

## Building

1. **Set your Lethal Company path** in `AngryBritishBug.csproj` (`LethalCompanyDir`), or build with:
   ```bat
   dotnet build -c Release /p:LethalCompanyDir="C:\Path\To\Lethal Company"
   ```

2. **Output:** The DLL is written to `..\plugins\`. Add it to the `plugins` folder when creating your Thunderstore package so the startup message works for users who install from Thunderstore.

## Requirements

- .NET SDK
- Lethal Company install path for game assemblies
