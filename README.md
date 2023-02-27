# ElectricalContactResistance

_Top-down action-shooter game with electricity puzzles on every level_

<img src="https://bitbucket.org/electricalcontactresistance/electricalcontactresistance.bitbucket.io/raw/59d29a51660e1cc8241f06210699f619ad5efb80/Documentation/Images/TitleImage.png" alt="ECR" width="640" height="360px"/>

[![GitHub release (latest by date)](https://img.shields.io/github/v/release/rmolotov/ElectricalContactResistance?include_prereleases)](https://github.com/rmolotov/ElectricalContactResistance/releases)
![GitHub Release Date](https://img.shields.io/github/release-date-pre/rmolotov/electricalcontactresistance)
![GitHub contributors](https://img.shields.io/github/contributors/rmolotov/ElectricalContactResistance?include_prereleases)
![GitHub last commit](https://img.shields.io/github/last-commit/rmolotov/ElectricalContactResistance?include_prereleases)

[![Author's linked-in profile](https://img.shields.io/badge/LinkedIn-Roman%20Molotov-informational)](https://linkedin.com/in/roman-molotov)

### Core pillars:
Use defeated enemies for assembly or fix electric circuit and tune it's parameters to solve the puzzle.
There are lot of electric parts and modules which help you to build you own ~~hero~~ kill switch and beat them all.
Piles of enemies and many tangled circuits are waiting you!

<details>
<summary>Screenshots</summary>

<img src="https://bitbucket.org/electricalcontactresistance/electricalcontactresistance.bitbucket.io/raw/59d29a51660e1cc8241f06210699f619ad5efb80/Documentation/Images/MainMenu.png" alt="ECR" width="640" height="360px"/>
<img src="https://bitbucket.org/electricalcontactresistance/electricalcontactresistance.bitbucket.io/raw/59d29a51660e1cc8241f06210699f619ad5efb80/Documentation/Images/SettingsWindow.png" alt="ECR" width="640" height="360px"/>
<img src="https://bitbucket.org/electricalcontactresistance/electricalcontactresistance.bitbucket.io/raw/59d29a51660e1cc8241f06210699f619ad5efb80/Documentation/Images/ShopWindow.png" alt="ECR" width="640" height="360px"/>
<img src="https://bitbucket.org/electricalcontactresistance/electricalcontactresistance.bitbucket.io/raw/59d29a51660e1cc8241f06210699f619ad5efb80/Documentation/Images/Gameplay.png" alt="ECR" width="640" height="360px"/>

</details>

### Tech details:

Project structure contains three sections:
* **Infrastructure** (providers and services),
* **Meta** (main menu, settings and shop windows),
* **(Core) Gameplay** (player controls, enemy logic, spawners and etc.).

and 3rd-party **libraries** and **plugins** such as:
* **Zenject**
* **RSG Promises** 
* **DOTween** used for procedural animations in UI 
* **TextMeshPro**
* Debug tools and editor extensions: **SRDebugger** and **Odin Inspector**
* **UI soft mask** by [mob-sakai](https://github.com/mob-sakai/SoftMaskForUGUI)
* **Nice Vibrations** by [More Mountains](https://assetstore.unity.com/publishers/10305) - TODO

Project includes level editor based on custom EditorWindow and Odin Inspector. This window available via `Tools/ECR/Board Editor` menu. Requires `StageEditor` scene opening.
<details>
<summary>Screenshot</summary>

<img src="https://bitbucket.org/electricalcontactresistance/electricalcontactresistance.bitbucket.io/raw/59d29a51660e1cc8241f06210699f619ad5efb80/Documentation/Images/StageEditor.png" alt="ECR" width="762px" height="486px"/>

</details>

Some services in projects have local and remote implementations. So, I'm using **UGS** for:
* Static Data (**Unity Remote Config**) - DONE
* Save/Load players' prefs and progress (**Unity Cloud Save**) - TODO
* Purchases and inventory management (**Economy**) - TODO

There are couple of last **Unity features** used in project:
* **Addressables** for content management and reduce app-size - DONE
* **Input System** implements actions-based handling off user's input and interactions - DONE
* **Cinemachine** and **Timeline** for camera control and cut-scenes - TODO
* **Shader graph** and **Render features** in **URP** for toon-stylized rendering and post FX - DONE

Project have a ~~micro~~ **services architecture** which implements a SRP and DI-framework provides LCP and DIP effects.
I'm using **factories** for gameplay-characters and UI-elements, so all dependencies inject in ctor-s or special `Construct`
methods via Installers or factories only (according to Zenject's best practices). I've created some **assembly definitions**
for libs' folders and project's submodules for sake of faster recompilation of sources.

There is **Game State Machine** which controls main phases of game: Boot -> Load Config -> Meta -> Gameplay.
It's provide more control and flexibility in game life-cycle.
GSM enter in `BootstrapState` on `Initialize` method (GSM derives from `IInitializable`) which indicate **entry point** for entire project. 