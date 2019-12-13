using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using NodeCanvas.Framework;

public class CriminalBehavior : MonoBehaviour {

    public Sprite sprite0;
    public Sprite sprite1;
    public Sprite sprite2;
    public Sprite sprite3;
    public GameObject cells;
    public Move move;
    SteeringArrive arrive;
    public Button attack_icon;
    public bool captured = false;
    public bool action = false;
    public NavMeshPath path;
    public SteeringFollowPath follow_path;
    public Cell cell;
    AssignCell assign;
    public GameObject c_agent;
    public Animator anim;
    public bool to_cell = false;
    public LevelLoop level;
    public float timer;
    public bool free = false;
    public bool countdown = false;
    public bool toExit = false;
    public bool escape = false;
    public bool night = false;
    public bool detected = false;
    public bool assigned = false;
    HealthBar popul;
    public float popularityloss;
    float auxm;
    float attack_timer;

    // Use this for initialization
    void Awake()
    {
        toExit = false;
        to_cell = true;
        countdown = false;
        assigned = false;
        level = GameObject.Find("Level").GetComponent<LevelLoop>();
        move = GetComponent<Move>();
        arrive = GetComponent<SteeringArrive>();
        action = false;
        follow_path = gameObject.GetComponent<SteeringFollowPath>();
        follow_path.path = new NavMeshPath();
        cells = GameObject.Find("Cells");
        anim = GetComponent<Animator>();
        popul = GameObject.Find("Healthbar").GetComponent<HealthBar>();
        move.move = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Exit(GameObject agent)
    {
        anim.SetBool("sitting", false);
        anim.SetBool("moving", true);
        anim.SetBool("running", false);
        move.move = true;
        toExit = true;
        setAgent(agent);
    }

    public void SpawnAgent()
    {
        c_agent = Instantiate(level.policemen_prebab[Random.Range(0, 2)], new Vector3(0, 0, 0), Quaternion.identity);
        c_agent.GetComponent<PoliceBehaviour>().to_cell = true;
        c_agent.GetComponent<Move>().target = gameObject;
        c_agent.transform.position = transform.position + Vector3.back;
        c_agent.gameObject.layer = 0;
        c_agent.GetComponent<SteeringPursue>().enabled = true;
        c_agent.GetComponent<SteeringObstacleAvoidance>().enabled = true;
        c_agent.GetComponent<SteeringCollisionAvoidance>().enabled = true;
        c_agent.GetComponent<SteeringVelocityMatching>().enabled = true;
        c_agent.GetComponent<GraphOwner>().enabled = true;
        level.policemen.Add(c_agent);
    }

    public void setAgent(GameObject agent)
    {
        captured = true;
        c_agent = agent;
        c_agent.GetComponent<Move>().target = gameObject;
        c_agent.gameObject.layer = 0;
        c_agent.GetComponent<SteeringPursue>().enabled = true;
        c_agent.GetComponent<SteeringObstacleAvoidance>().enabled = true;
        c_agent.GetComponent<SteeringCollisionAvoidance>().enabled = true;
        c_agent.GetComponent<SteeringVelocityMatching>().enabled = true;
    }

    //Assign each criminal a point to stand on the cell
    public void AssignCell()
    {
        assign = cells.GetComponent<AssignCell>();
        for (int i = 0; i < assign.cells.Count; ++i)
        {
            Cell c_cell = assign.cells[i].gameObject.GetComponent<Cell>();
            if (c_cell.isAvailable() && c_cell.gameObject.active)
            {
                cell = c_cell.setAgent(gameObject);
                break;
            }
        }

        if (cell != null)
        {
            to_cell = true;
            escape = false;
            toExit = false;
            move.target = cell.getPoint().gameObject;
            follow_path.calcPath(cell.getPoint());
            timer = 60;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == GameObject.Find("Exit").GetComponent<Collider>())
        {
            auxm = popul.CurrentValue;
            auxm = auxm - popularityloss;
            popul.SetBar((int)auxm);
            Destroy(gameObject);
        }
        else if (other == GameObject.Find("Entrance").GetComponent<Collider>())
        {
            gameObject.GetComponent<SteeringCollisionAvoidance>().enabled = false;
            //gameObject.GetComponent<SteeringSeparation>().enabled = false;
        }
        else if (move.target != null)
        {
            if (other == move.target.gameObject.GetComponent<Collider>())
            {
                if (c_agent != null)
                {
                    c_agent.gameObject.layer = 8;
                    c_agent.GetComponent<Move>().target = null;
                    c_agent.GetComponent<SteeringPursue>().enabled = false;
                    c_agent.GetComponent<SteeringObstacleAvoidance>().enabled = true;
                    c_agent.GetComponent<SteeringCollisionAvoidance>().enabled = true;
                    c_agent.GetComponent<SteeringVelocityMatching>().enabled = false;
                    c_agent.GetComponent<PoliceBehaviour>().to_cell = false;
                    c_agent.GetComponent<PoliceBehaviour>().detected = false;
                    c_agent = null;
                }
            }
        }
    }

    public void ReleaseAgent()
    {
        c_agent.gameObject.layer = 8;
        c_agent.GetComponent<Move>().target = null;
        c_agent.GetComponent<SteeringPursue>().enabled = false;
        c_agent.GetComponent<SteeringObstacleAvoidance>().enabled = true;
        c_agent.GetComponent<SteeringCollisionAvoidance>().enabled = true;
        c_agent.GetComponent<SteeringVelocityMatching>().enabled = false;
        c_agent.GetComponent<PoliceBehaviour>().to_cell = false;
        c_agent.GetComponent<PoliceBehaviour>().detected = false;
        c_agent = null;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == GameObject.Find("Entrance").GetComponent<Collider>())
        {
            gameObject.GetComponent<SteeringCollisionAvoidance>().enabled = true;
        }
    }

    private void OnDestroy()
    {
        cell = null;
    }
    

    //Set up night mode

    public void Night()
    {
        captured = false;
        night = true;
        if(timer <= 0)
            escape = true;
    }


    public void Day()
    {
        night = false;
        escape = false;
        captured = false;
    }

    public void AttackTarget(int message)
    {
        Debug.Log("Received Damage");
        
        //anim.SetBool("attack", false);
        //to_cell = true;
    }

    public void StartAttack()
    {
        attack_timer = Time.time;
        attack_icon.gameObject.active = true;
        StartCoroutine("Attack");
    }

    IEnumerator Attack()
    {
        while (Time.time - attack_timer <= 2)
        {
            if (Time.time - attack_timer >= 1.5 || action)
            {
                attack_icon.GetComponent<Image>().sprite = sprite3;
                action = true;
            }
            else if (Time.time - attack_timer >= 1)
            {
                attack_icon.GetComponent<Image>().sprite = sprite1;

                if (captured)
                {
                    attack_icon.GetComponent<Image>().sprite = sprite2;
                    anim.SetBool("attack", false);
                    to_cell = true;
                    yield return new WaitForSeconds(1);
                }

            }
            else if (Time.time - attack_timer >= 0)
            {
                attack_icon.GetComponent<Image>().sprite = sprite0;
            }
            yield return null;
        }
        attack_icon.gameObject.SetActive(false);
    }

    public void ButtonAttack()
    {
        if(Time.time - attack_timer >= 1 && Time.time - attack_timer < 1.5 && !action)
            captured = true;
        else
            action = true;
    }
    
}