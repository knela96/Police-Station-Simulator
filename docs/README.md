# Police-Station-Simulator

<iframe width="560" height="315" src="https://www.youtube.com/embed/W4fsx69ntWM" frameborder="0" allow="accelerometer; autoplay; encrypted-media; gyroscope; picture-in-picture" allowfullscreen></iframe>

In this GOD style game, you are a new police captain and your duty is to lead this police station to maximum efficency. You have the objective to control the situation of your police station and avoid criminals to escape from the justice. You and your officers are the ones that have to set justice in the city!

# Download
You can download the game by clicking [here](https://github.com/knela96/Police-Station-Simulator/releases/download/Assignment3v.01/Police.Station.Simulator.zip). 
Extract it and before playing remember to read the README file.

# Authors
<table style="width:100%; border: none;">
  <tr>
    <td><img src="https://raw.githubusercontent.com/knela96/Police-Station-Simulator/master/docs/Eric.png?token=AGOACYOCUVZKNSV6CWZPRU26CCJ32" alt="Eric" />
	    <br>
	  <ul>
		  <li><a href="https://github.com/knela96">Github</a></li>
		  <li><a href="https://www.linkedin.com/in/eric-canela-sol/">LinkedIn</a></li>
	    </ul>
    </td>
    <td>
	    <img src="https://raw.githubusercontent.com/knela96/Police-Station-Simulator/master/docs/Ferran.png?token=AGOACYN7CWYAUVS6SGAW2U26CCJ5W" alt="Ferran" />
	    <br>
	  <ul>
		  <li><a href="https://github.com/FBarnes99">Github</a></li>
		  <li><a href="https://www.linkedin.com/in/ferran-barnes-garcia-a76bb7181/">LinkedIn</a></li>
	    </ul>
	</td>
  </tr>
</table>

# Mechanics:
- As the police captain your job is to assign different officers to the desks to start investigate some cases. 
- You will have to provide new cells to capture more criminals. 
- At night as a captain you will be able to stun, for a period of time, the criminals that are escaping just to help your officers and avoid the escape.

# Different npc's roles:

## Citizens
<table style="width:100%">
  <tr>
    <td><img src="https://raw.githubusercontent.com/knela96/Police-Station-Simulator/master/Wiki_Assets/Icons/Citizen1.PNG?token=AGOACYN5EECEMYN75VOPMYS6CCKU4" alt="Citizen" height="200" width="auto"></td>
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
![](https://raw.githubusercontent.com/knela96/Police-Station-Simulator/master/Wiki_Assets/Citizen_BT.png?token=AGOACYOCC7D6URGUDSRLIC26CCLFK)

## Citizens in the waiting roomn
<video src="C1.mp4" width="auto" height="300" controls preload></video>

## Police
<table style="width:100%">
  <tr>
    <td><img src="https://raw.githubusercontent.com/knela96/Police-Station-Simulator/master/Wiki_Assets/Icons/Police.PNG?token=AGOACYPOIQLB3T7WHQ4NPXK6CCKVG" alt="Policeman" width="180" width="auto"></td>
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
![]()

## Police arresting a criminal
<video src="P1.mp4" width="auto" height="300" controls preload></video>
## Police patroling at night
<video src="P2.mp4" width="auto" height="300" controls preload></video>

## Criminals
<table style="width:100%">
  <tr>
    <td><img src="https://raw.githubusercontent.com/knela96/Police-Station-Simulator/master/Wiki_Assets/Icons/Criminal.PNG?token=AGOACYK4TWPAPCEVVXQZNG26CCKVA" alt="Criminal" height="200" width="auto"></td>
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

## Criminal scaping
<video src="Cr1.mp4" width="auto" height="300" controls preload></video>

### Behaviour Tree
![](https://raw.githubusercontent.com/knela96/Police-Station-Simulator/master/Wiki_Assets/Criminal_BT.png?token=AGOACYIBUKNVOVT7NMX4JY26CCLFO)

# Different Police station Rooms:
![](https://github.com/knela96/Police-Station-Simulator/blob/master/docs/Lobby.png)
- Lobby: Where the citizen make the crime notifications, also where the door to the street is located.

![](https://raw.githubusercontent.com/knela96/Police-Station-Simulator/master/docs/Cells.png)
- Containment Cell: Where the criminals are guarded until sent to jail.

![](https://github.com/knela96/Police-Station-Simulator/blob/master/docs/Office.png)
- Office: Where the policemen investigate the different cases.

# Resources:
![](https://github.com/knela96/Police-Station-Simulator/blob/master/docs/money.png)

 - Money
 
![](https://github.com/knela96/Police-Station-Simulator/blob/master/docs/Popularity.PNG)
 
 - Satisfaction
 
![](https://github.com/knela96/Police-Station-Simulator/blob/master/docs/desks.PNG)
  
 - Police availability
 
![](https://github.com/knela96/Police-Station-Simulator/blob/master/docs/cell.PNG)
  
 - Cells availability

# Gameloop:

## Objective
![](https://github.com/knela96/Police-Station-Simulator/blob/master/docs/objective.PNG)

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




