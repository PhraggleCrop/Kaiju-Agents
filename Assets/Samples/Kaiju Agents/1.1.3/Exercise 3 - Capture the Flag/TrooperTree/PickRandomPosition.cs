using BehaviourTree;
using UnityEngine;
using KaijuSolutions.Agents.Exercises.CTF;
using KaijuSolutions.Agents;
using System.Collections.Generic;

//In this code we get random health pickups on our respective sides to patrol around
public class PickRandomPickupNode : Node
{
    private TrooperController controller;
    private Trooper trooper;
    private KaijuAgent trooper_agent;

    //Get controller and components we will use
    public PickRandomPickupNode(TrooperController controller)
    {
        this.controller = controller;
        trooper = controller.GetComponent<Trooper>();
        trooper_agent = controller.GetComponent<KaijuAgent>();
    }


    public override NodeState Evaluate()
    {
        if (GetData("patrolTarget") != null)
        {
            return NodeState.FAILURE;

        }

        //We will go through the health pickups and add valid ones into a list
        List<HealthPickup> valid = new List<HealthPickup>();

        foreach (var hp in HealthPickup.All)
        {
            //Valid nodes have our respective team in their name
            if (trooper.TeamOne && hp.name.StartsWith("Team One"))
                valid.Add(hp);
            else if (!trooper.TeamOne && hp.name.StartsWith("Team Two"))
                valid.Add(hp);
        }
        //If no valid we fail
        if (valid.Count == 0)
            return NodeState.FAILURE;

        //Else we choose a random node in a range
        HealthPickup chosen = valid[Random.Range(0, valid.Count)];

        //Offset so we don't stand directly on pickup
        Vector3 offset = new Vector3(Random.Range(-2f, 2f),0f,Random.Range(-2f, 2f));

        Vector3 targetPosition = chosen.transform.position + offset;

        //Set target in the data
        parent.SetData("patrolTarget", targetPosition);

        return NodeState.SUCCESS;
    }
}