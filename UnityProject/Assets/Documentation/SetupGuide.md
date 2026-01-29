# Setup Guide (Unity)

Ce guide décrit comment connecter le squelette de scripts dans Unity.

## 1) Créer la scène
- `Assets/Scenes/Main.unity`
- Ajouter un Canvas (Screen Space - Overlay)
- Ajouter un empty GameObject `Systems`

## 2) Ajouter GameState
- `Systems` > Add Component > `GameState`

## 3) Ajouter GameFlow
- `Systems` > Add Component > `GameFlow`
- Assigner le `GameState` dans l'Inspector

## 4) Ajouter NoteSpawner
- `Systems` > Add Component > `NoteSpawner`
- Assigner `GameState`
- Assigner `ScoreManager`
- Créer un `RectTransform` (ex: `NotesContainer`) et l'assigner
- Assigner les prefabs de notes

## 5) Ajouter ScoreManager
- `Systems` > Add Component > `ScoreManager`
- Assigner `GameState`
- Assigner `UltimateManager` et `PerformanceMeter`

## 6) Ajouter UltimateManager
- `Systems` > Add Component > `UltimateManager`
- Assigner `GameState`

## 7) Ajouter PerformanceMeter
- `Systems` > Add Component > `PerformanceMeter`
- Assigner `GameState`

## 8) Ajouter InputManager
- `Systems` > Add Component > `InputManager`
- Assigner `GameFlow`, `ScoreManager`, `UltimateManager`

## 9) HUD
- Créer des Text + Image
- Ajouter `HudController` sur un GameObject `HUD`
- Assigner `GameState` et références UI

## 10) Lancer
- Play
- Utiliser A/P/Espace + E pour ultimate
