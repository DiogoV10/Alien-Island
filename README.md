# Alien Island

WASD - Move <br>
Space - Jump <br>
Ctrl - Sprint <br>
Left/Right Mouse Button - Attack <br>
"1" & "2" - Switch Weapon <br>
"3" & "4" - Ultimate ability <br>
"5" - Skill <br>

---
## Artificial Intelligence

### A*

### State Machine
We implemented a state machine that handles the enemy AI. The states are: Wander, Chase, Attack, Search, Hit and Dead.<br>
The enemies are always searching for the player, even if they are wandering. <br>
When they chase, they use the navigation system from unity. If the player is at a certain range, the enemy attacks.<br>
If the player runs away, some enemies go to the previous seen location and search around for a bit.<br>
The hit and dead states are self-explanatory.<br>
It's not very complex but it works as intended.

