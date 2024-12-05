# MonoZelda
Monogame implementation of OG Zelda by team Pixel Pushers

## Controls
Players can use a compatible gamepad or a keyboard to play. The keybindings are below.

### Keyboard
W or UP -> Move up in game / Up in menus  
A or LEFT -> Move left in game / Left in menus  
S or DOWN -> Move down in game / Down in menus  
D or RIGHT -> Move right in game / Right in menus  
F -> Use equipped item  
G -> Toggle hitbox gizmos (for development purposes)  
Z or N -> Main attack  
M -> Mute game sound  
Q -> Exit game  
ENTER -> Start / execute menu selection  
R -> Reset game to start menu  
I or ESCAPE -> Toggle inventory  
F5 -> Quick Save in regular mode  
F9 -> Quick load in regular mode  

### Gamepad
DPAD UP -> Up in menus  
DPAD LEFT -> Left in menus  
DPAD DOWN -> Down in menus  
DPAD RIGHT -> Right in menus  
LEFT ANALOG -> Move Link in all directions  
RIGHT TRIGGER -> Toggle hitbox gizmos (for development purposes)  
LEFT TRIGGER -> Mute game sound  
X -> Use equipped item  
A -> Main attack  
LEFT STICK DOWN -> Exit game  
START -> Start / execute menu selection  
RIGHT STICK DOWN -> Reset game to start menu  
Y -> Toggle inventory  
LEFT BUMPER -> Quick Save in regular mode  
RIGHT BUMPER -> Quick load in regular mode  


# Special Notes
- For pushable blocks, keep pushing into the block for a few seconds for functionality 
to work.
- Dungeon rooms are downloaded from Google sheets using an HTTP request. You must have an active internet connection to run the game. Please allow a few moments for the room to load in some cases.
- Some of us had issues building the exe with the shaders we implemented in Sprint 5. If playtesters run into this, please download the build from github. Our team can also sit down to talk through any issues that may arise when building as a result of shaders. Not all computers function the same in this regard.
- The input device is selected when you start the game. If you have a controller plugged in or connected in any way to the device, MonoGame will pick this up and use a gamepad. If you do not have a controller connected, MonoGame will default to keyboard controls.

## Developers
- Josh S
- Calvin L
- Carson Miller
- Joshua Klasmeier
- Om Kurkure
- Robbie G

# Code Metrics
A weekly collection of code metrics and analysis can be found in `CodeMetrics.xlsx`.

# Sprint Reflections
All Sprint review write ups can be found in `SprintReflections.md`.

# Sprint 5

## Sprint 5 Burndown
A complete burndown chart of start and end times for all Sprint 5 functionality can be found at
https://github.com/orgs/Pixel-Pushers-1/projects/1/views/4 

## Sprint 5 Pull Requests
The pull requests at the link below demonstrate our teams code review and maintainability for Sprint 5.
https://github.com/Pixel-Pushers-1/MonoZelda/pulls?page=1&q=is%3Apr+is%3Aclosed+closed%3A%3E2024-11-13

## New Features
The following features have been implemented this sprint.

### Quick Save / Quick Load
You can save at any time in the normal game mode using `F5`. You can load at any time in the normal game mode using `F9`.

### Lighting
Many room feature dynamic light sources that can cast shadows. Link always illuminates the area around him. The Link light extends further when the blue candle item is equiped. When using the fireball, it will also cast light through that specific room until the fireball collides with something.

### Gamepad
Players can now choose to play with a compatible gamepad or with the keyboard. All controls are available in both modes. Additional guidance for this is available in the Special Notes section.

### Infinite Game Mode
Players can choose to play an infinite mode that moves Link through a series of rooms. Link needs to kill all enemies in the room before moving to the next. This sequence goes on infinitely or until Link dies.

### RPG Elements
Link can now level up! There is an xp bar at the top along with level number that is updated when link picks up orbs from killing enemies. 

**Every Level**: 
- Recover Health
- Gain Defense(reduces damage)
  
**Every 5 Levels**:

- Link does more damage
- Sword color changes
- Cloak color changes


# Sprint 4

## Sprint 4 Pull Requests
The pull requests at the link below demonstrate our teams code review and maintainability for Sprint 4.
https://github.com/Pixel-Pushers-1/MonoZelda/pulls?q=is%3Apr+is%3Aclosed

## Sprint 4 Burndown
A complete burndown chart of start and end times for all Sprint 4 functionality can be found at
https://github.com/orgs/Pixel-Pushers-1/projects/1/views/4 

# Sprint 3

## Sprint 3 Pull Requests
The pull requests at the link below demonstrate our teams code review and maintainability for Sprint 3.
https://github.com/Pixel-Pushers-1/MonoZelda/pulls?q=is%3Apr+milestone%3ASprint3

Code review and maintainability is not limited to the Pull Requests. We have daily design discussions within our method of comunication (Discord). We would be happy to share those conversations if needed to show our dedication in this regard. We understand the formality of conversation in Discord is lacking, but we believe that it is the best way to share opinions and reflect on them.

## Sprint 3 Burndown
A complete burndown chart of start and end times for all Sprint 3 functionality can be found at
https://github.com/orgs/Pixel-Pushers-1/projects/1/views/4 

# Sprint 2

## Sprint 2 Pull Requests
The pull requests at the link below demonstrate our teams code review and maintainability for Sprint 3.
https://github.com/Pixel-Pushers-1/MonoZelda/pulls?q=is%3Apr+milestone%3ASprint2+

## Sprint 2 Burndown
A complete burndown chart of start and end times for all Sprint 2 functionality can be found at
https://github.com/orgs/Pixel-Pushers-1/projects/1/views/4
