# Police-Station-Simulator
In this GOD style game, you are a new police captain and your duty is to lead this police station to maximum efficency.

# Mechanics:

As the police captain your job is to assign different workers to different tasks and try to make the city as safe as possible.

# Different npc's roles:

- Citizen: Brings crime notifications to the police station
- Police: Works outside the police station and brings criminals to the containment cell.
	*-Police(Guard): Patrols the police station to ensure security
	*-Detective: It investigates the crimes brought by the citizens and sends the police.
- Criminal: Tries to scape the containment cell.

# Different Police station Rooms:

- Lobby: Where the citizen make the crime notifications, also where the door to the street is located.
- Containment Cell: Where the criminals are guarded until sent to jail.
- Office: Where the policemen spent most of the time.

# Resources:
 - Money
 - Police availability
 - Satisfaction
 
# Gameloop:
## DAY
- Citizen enters the police station
- Citizen gives you notification
- The player asigns a police to the case
	- Case can be harder needing more policemen or time to be completed, cases that involve criminal require more resources but give more results.
	- Cases have a timer to assign a policemen.
	
- Policemen starts the case and becomes unavailable
- Policemen ends the case and returns to available police, the player gains money and satisfaction.

- If the policemen gets a criminal(criminals have levels) it goes to a cell
	- The criminal can try to scape
- After the criminals spent a period of time in the cell they are moved outside the station by a policemen

- Every 24 hours the player will have to pay salaries for each police in the station.
- If the satisfaction reaches 0 or if you have negative money it will be game over.


## EMERGENCY STATUS

- Citizen try to refuge
- Policemen have to be assigned to random emergency cases
- Policemen in the police station has to keep check on the criminals
- Criminals scape. If the number of policemen are enough to capture the criminal after a short period of time the crimnal will go back to the cell, if the criminal arrives at the door and opens it (Opening the door lasts a short period of time) it will scape.



