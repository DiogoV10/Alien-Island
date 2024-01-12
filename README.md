# Alien Island

WASD - Move
Space - Jump
Ctrl - Sprint
Left/Right Mouse Button - Attack
"1" & "2" - Switch Weapon
"3" & "4" - Ultimate ability
"5" - Skill

## Artificial Intelligence

### A*

### State Machine
We implemented a state machine that handles the enemy AI. The states are: Wander, Chase, Attack, Search, Hit and Dead.
The enemies are always searching for the player, even if they are wandering. 
When they chase, they use the navigation system from unity. If the player is at a certain range, the enemy attacks.
If the player runs away, some enemies go to the previous seen location and search around for a bit.
The hit and dead states are self-explanatory.
It's not very complex but it works as intended.
