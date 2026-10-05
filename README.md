# 🌱 PvZRepatched

> A mod for *Plants vs. Zombies™: Replanted* that fixes some vanilla bugs.
>
> Also includes a configurable Unity logger.

## 📦 Download

- [GameBanana](https://gamebanana.com/mods/721276)

## 🧩 Requirements

* 🎮 A copy of **Plants vs. Zombies™: Replanted**.
* 🍉 **[MelonLoader](https://github.com/LavaGang/MelonLoader)** - required mod loader.

## ✨ Fixes

1. Random startup black screen
    > Sometimes the Steam API failed to initialize.

2. Dance mode
    > Restored the missing dance mode option. If you have unlocked it, you can now enable it from gameplay options.

3. Spike plant height
    > Removed the extra spike height on certain tiles (Windows Phone leftover?).

4. Versus plant selection
    > Fixed some seed packets being incorrectly greyed out.

5. Plants turning gray after loading a save
    > Fixed normal plants randomly turning into gray Imitater versions after saving and resuming.

6. Pumpkin damage stages
    > Pumpkins now show their first damage stage exactly when they can be repaired with First Aid.

7. Button responsiveness
    > Shovel, Tree Food and Zen Garden buttons now register as soon as you press them, instead of waiting for you to release.

## ⚙️ Extra features

1. Run in background
    > Allows the game to continue running even when the window is not in focus (configurable, enabled by default).

2. Unity logger
    > Redirects Unity engine logs to the MelonLoader console for easier debugging.

## 📝 Unity logger

Configurable with [BloomEngine](https://github.com/PalmForest0/BloomEngine) (v0.4.1-beta) (optional).

### Options

| Option                   | Type         | Default    | Description                                 |
|--------------------------|--------------|------------|---------------------------------------------|
| `UnityLogMode`           | UnityLogMode | `Filtered` | Controls Unity logging verbosity            |
| `LogMissingLocalization` | Boolean      | `false`    | Enables logging of missing localization IDs |

> [!NOTE]
> **`LogMissingLocalization`** - Most users should keep this disabled as the vanilla game produces many localization warnings.
>
> Only enable this if you are debugging issues related to localization.

### UnityLogMode Values

- **`Disabled`** - Disables all Unity logging
- **`Filtered`** - Filters out known noise and logs the rest
- **`All`** - Logs all Unity messages without filtering

## 🚀 How to use

1. **Install MelonLoader**

    > [Download here](https://github.com/LavaGang/MelonLoader/releases)

2. **Place the Mod DLL**

    > Copy `PvZRepatched.dll` file into the game’s `Mods` folder.
