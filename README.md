# Traffic Panic 3D for 64-bit Android

A port of **Traffic Panic 3D 1.5** (Neonplay, 2012) for modern Android phones. The original was
made with Unity 3.5 and only runs on 32-bit devices. This project rebuilds the same game as a
Unity 2019.4 project, which produces a 64-bit APK.

It runs fully offline, with no ads, no trackers and no real-money purchases.

## Features

- Runs on current phones (Android 5.0+, ARM64 and ARMv7).
- The whole game: every level, vehicle and building.
- No ads, no tracking and no permissions.
- "Unlock All" and "Insurance Fraud" are bought with in-game credits instead of real money.
- Online-only features (OpenFeint rankings and achievements, Facebook, Twitter, "More Games")
  stay on screen but do nothing.
- Installs alongside the original (`com.trafficpanic3d.port`).

## Building

1. Install **Unity 2019.4.40f1** with **Android Build Support** (Android SDK & NDK Tools,
   OpenJDK) from Unity Hub.
2. Open the `project` folder in Unity. The first import takes a few minutes.
3. Choose **Traffic Panic › Build APK** from the menu.

## Credits

- Traffic Panic 3D © Neonplay.
- Assets extracted with [AssetRipper](https://github.com/AssetRipper/AssetRipper).
