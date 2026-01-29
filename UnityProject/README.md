# Atomic Parcel - Unity Project Scaffold

Ce dossier contient un squelette de projet Unity prêt à ouvrir avec **Unity 2022.3 LTS**.
Le projet est volontairement minimal (pas de scènes binaires ni de meta) afin d'être
facile à importer et versionner sur Git.

## Pré-requis
- Unity Hub installé
- Unity 2022.3 LTS (ou version compatible)

## Ouverture dans Unity
1. Ouvrir Unity Hub
2. "Open" > sélectionner le dossier `UnityProject`
3. Unity va régénérer automatiquement les fichiers manquants (`Library`, `*.meta`, etc.)

## Structure
- `Assets/Scripts/Gameplay` : gameplay (notes, score, ultimate, etc.)
- `Assets/Scripts/Audio` : détection de beat + audio
- `Assets/Scripts/UI` : UI/HUD
- `Assets/Scenes` : scènes Unity (à créer depuis Unity)

## TODO dans Unity
- Créer une scène principale et la sauvegarder dans `Assets/Scenes/Main.unity`
- Importer les assets (modèles/sons) et configurer le Canvas HUD
- Assigner les scripts aux GameObjects correspondants

## Notes
Ce squelette vise à accélérer la migration. Les scripts sont des bases solides mais
nécessitent l'ajout d'assets et le wiring via l'Inspector.
