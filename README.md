# Police-Station-Simulator
In this GOD style game, you are a new police captain and your duty is to lead this police station to maximum efficency.

# Authors
Eric Canela & Ferran Barnes

# Website Link
https://knela96.github.io/Police-Station-Simulator/

# Github link
https://github.com/knela96/Police-Station-Simulator

# Wiki Link
https://github.com/knela96/Police-Station-Simulator/wiki

# License
MIT License

Copyright (c) 2019 Eric Canela & Ferran Barnes

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

# Controls
W,A,S,D: Camera movement
Left mouse click: Interact with the game
Right mouse click: Rotate camera

# Mechanics:
"UI graphics not final"
As the police captain your job is to assign different workers to different tasks and try to make the city as safe as possible.

# Different npc's roles:

- Citizen: Brings crime notifications to the police station
- Police: Works outside the police station and brings criminals to the containment cell.
- Police(Guard): Patrols the police station to ensure security
- Criminal: Tries to scape the containment cell.

# Different Police station Rooms:

- Lobby: Where the citizen make the crime notifications, also where the door to the street is located.
- Containment Cell: Where the criminals are guarded until sent to jail.
- Office: Where the policemen spent most of the time.

# Resources:

 - Money
 - Satisfaction
 - Police availability
 - Cell availability

 # Behaviour trees
 ## Citizen
 
- Citizen Will Go to the Reception
- If other Citizen is currently on the Reception he will go and Wait on the Waiting Room
- When it is his turn it will go to the Reception
- When he has finished giving the task to the Police he will leave the Building
 
 ## Police
 
- Will go to the Desk to start Investigating a case
- When it has finished, it will check if there is any criminal to liberate
- If not, it will exit the building to chase the criminal
- If the policeman has arrested a Criminal he will escort him to a Cell
- At night if he doesn't need to patrol, will go Home
- While Patrolling if he finds a criminal escaping, he will fight him (Also if it is the morning and a criminal was escaping the night before)
- If the attack is a success, will escort the criminal to the Cell
- If he was patrolling, the next morning it will go Home to sleep
- If a criminal is succesfully sent to jail the popularity will rise
 ## Criminals
 
- If the Criminal is arrested, it will be escorted to the Cell
- He will wait in the cell for an ammount of time
- If it is the Morning, he will wait until a police liberates and escort him to the Exit
- If it is Night and has Waited enough time, he will escape the cell, if the criminal escapes the popularity will drop
- If he is detected he will try to attack the police
- He will return to the Cell if he doesn't succeeds on the attack

**You can check its Behaviour Trees here:** [Entities Behaviour Trees](https://github.com/knela96/Police-Station-Simulator/wiki/Entities-Behaviours)

# Gameloop:

## Objective
Your objective is to set free 5 criminals without letting escape more than 2, if you accomplish the objective you will gain satisfaction and win the game, but is escapes more than 2 you will lose instantly.

## DAY (2 minutes)
### Citizens
- Citizen enters the police station
- Citizen gives you notification

### Police
- The policemen go to desk to investigate a case
	- Case can be harder needing more time to be completed.
- Policemen starts the case and becomes unavailable
- Policemen ends the case and returns to available police, the player gains money.
- If the policemen gets a criminal it goes to a cell

### Criminals
- After the criminals spent a period of time in the cell they are set free by a policemen


## NIGHT (2 minutes)

### Citizens
- Citizen leave the station

### Police
- Some Policemen will leave the station.
- 3 Policemen in the police station light up their torchlights and make rounds around keeping watch for scaped criminals

### Criminals
- Criminals scape. If the policemen have enough live to capture the criminal, the criminal will go back to the cell.
- If the criminal arrives at the door, he will escape and you will lose satisfaction.


