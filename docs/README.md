# Police-Station-Simulator
In this GOD style game, you are a new police captain and your duty is to lead this police station to maximum efficency. You have the objective to control the situation of your police station and avoid criminals to escape from the justice. You and your officers are the ones that have to set justice in the city!

# Authors
<table style="width:100%; border: none;">
  <tr>
    <td><img src="https://raw.githubusercontent.com/knela96/Police-Station-Simulator/master/docs/Eric.png?token=AGOACYL54SMMVALD23QOTLK576KPG" alt="Eric" />
	    <br>
	  <ul>
		  <li><a href="https://github.com/knela96">Github</a></li>
		  <li><a href="https://www.linkedin.com/in/eric-canela-sol/">LinkedIn</a></li>
	    </ul>
    </td>
    <td>
	    <img src="https://raw.githubusercontent.com/knela96/Police-Station-Simulator/master/docs/Ferran.png?token=AGOACYITIRRHFADQJSNPI52576LFG" alt="Ferran" />
	    <br>
	  <ul>
		  <li><a href="https://github.com/FBarnes99">Github</a></li>
		  <li><a href="https://www.linkedin.com/in/ferran-barnes-garcia-a76bb7181/">LinkedIn</a></li>
	    </ul>
	</td>
  </tr>
</table>

# Mechanics:
As the police captain your job is to assign different workers to differents desks to start investigate some cases. Also you will have to provide new cells to capture more criminals. 
At night as a captain you will be able to stun the criminals that are escaping for a period of time, just to help your officers and avoid the escape.

# Different npc's roles:

## Citizens
<table style="width:100%">
  <tr>
    <td><img src="https://raw.githubusercontent.com/knela96/Police-Station-Simulator/master/Wiki_Assets/Icons/Citizen2.PNG?token=AGOACYLA3X6IFPIKGJTJ4R257KZEK" alt="Citizen" height="200" width="auto"></td>
    <td>
<ul>
<li>Citizen Will Go to the Reception</li>
<li>If other Citizen is currently on the Reception he will go and Wait on the Waiting Room</li>
<li>When it is his turn it will go to the Reception</li>
<li>When he has finished giving the task to the Police he will leave the Building</li>
</ul>
</td>
  </tr>
</table>

### Behaviour Tree
![](https://raw.githubusercontent.com/knela96/Police-Station-Simulator/master/Wiki_Assets/Citizen_BT.png?token=AGOACYK62TBX5U3SHDJUETK57KY34)

## Police
<table style="width:100%">
  <tr>
    <td><img src="https://raw.githubusercontent.com/knela96/Police-Station-Simulator/master/Wiki_Assets/Icons/Police.PNG?token=AGOACYM4N3VFADSCUVGU6GK57KZEU" alt="Policeman" width="180" width="auto"></td>
    <td>
<ul>
<li>Will go to the Desk to start Investigating a case</li>
<li>When it has finished, it will check if there is any criminal to liberate</li>
<li>If not, it will exit the building to chase the criminal</li>
<li>If the policeman has arrested a Criminal he will escort him to a Cell</li>
<li>At night if he doesn't need to patrol, will go Home</li>
<li>While Patrolling if he finds a criminal escaping, he will fight him 
(Also if it is the morning and a criminal was escaping the night before)</li>
<li>If the attack is a success, will escort the criminal to the Cell</li>
<li>If he was patrolling, the next morning it will go Home to sleep</li>
</ul>
</td>
  </tr>
</table>

### Behaviour Tree
![](https://raw.githubusercontent.com/knela96/Police-Station-Simulator/master/Wiki_Assets/Police_BT.png?token=AGOACYNSQ2MKY4KDRH5SX4K57KZBG)

## Criminals
<table style="width:100%">
  <tr>
    <td><img src="https://raw.githubusercontent.com/knela96/Police-Station-Simulator/master/Wiki_Assets/Icons/Criminal.PNG?token=AGOACYIOFQVVT4RKILHJRM257KZEQ" alt="Criminal" height="200" width="auto"></td>
    <td>
<ul>
<li>If the Criminal is arrested, it will be escorted to the Cell</li>
<li>He will wait in the cell for an ammount of time</li>
<li>If it is the Morning, he will wait until a police liberates and escort him to the Exit</li>
<li>If it is Night and has Waited enough time, he will escape the cell</li>
<li>If he is detected he will try to attack the police</li>
<li>He will return to the Cell if he doesn't succeeds on the attack</li>
</ul>
</td>
  </tr>
</table>

### Behaviour Tree
![](https://raw.githubusercontent.com/knela96/Police-Station-Simulator/master/Wiki_Assets/Criminal_BT.png?token=AGOACYMUYJGGBOT63BXR5F257KY7S)

# Different Police station Rooms:
![](https://raw.githubusercontent.com/knela96/Police-Station-Simulator/master/Wiki_Assets/Criminal_BT.png?token=AGOACYMUYJGGBOT63BXR5F257KY7S)
- Lobby: Where the citizen make the crime notifications, also where the door to the street is located.

![](https://raw.githubusercontent.com/knela96/Police-Station-Simulator/master/Wiki_Assets/Criminal_BT.png?token=AGOACYMUYJGGBOT63BXR5F257KY7S)
- Containment Cell: Where the criminals are guarded until sent to jail.

![](https://raw.githubusercontent.com/knela96/Police-Station-Simulator/master/Wiki_Assets/Criminal_BT.png?token=AGOACYMUYJGGBOT63BXR5F257KY7S)
- Office: Where the policemen spent most of the time.

# Resources:

 - Money
 - Satisfaction
 - Police availability

 
 

**You can check its Behaviour Trees here:** [Entities Behaviour Trees](https://github.com/knela96/Police-Station-Simulator/wiki/Entities-Behaviours)

# Gameloop:

## DAY (2 minutes)
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


## NIGHT (2 minutes)

- Citizen leave the station
- Policemen have to be assigned to random emergency cases
- 2 Policemen in the police station light up their torchlights and make rounds around keeping watch for scaped criminals
- Criminals scape. If the number of policemen are enough to capture the criminal after a short period of time the crimnal will go back to the cell, if the criminal arrives at the door and opens it (Opening the door lasts a short period of time) it will scape.



