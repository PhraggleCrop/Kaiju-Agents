using BehaviourTree;
using UnityEngine;
using KaijuSolutions.Agents.Exercises.CTF;
using KaijuSolutions.Agents;

public class HealRetreat : Node
{
    private TrooperController controller;
    private Trooper trooper;
    private KaijuAgent trooper_agent;


    private HealthPickup targetHealth;

    public HealRetreat(TrooperController controller)
    {
        this.controller = controller;
        trooper = controller.GetComponent<Trooper>();
        trooper_agent = controller.GetComponent<KaijuAgent>();
    }

    public override NodeState Evaluate()
    {
        //If we already picked one, keep going to it
        float closestDist = Mathf.Infinity;

        foreach (var hp in HealthPickup.All)
        {

            //Check if pickup is oncooldown
            if (hp.OnCooldown)
            {
                continue;
            }
            float dist = Vector3.Distance(
                trooper_agent.transform.position,
                hp.transform.position
            );

            if (dist < closestDist)
                {
                closestDist = dist;
                targetHealth = hp;
            }
        }

        if (targetHealth == null)
            return NodeState.FAILURE;
        

        //Move toward health pickup
        trooper_agent.PathFollow(targetHealth.transform.position);

        float distance = Vector3.Distance(
            trooper_agent.transform.position,
            targetHealth.transform.position
        );

        //If close enough, pickup will trigger automatically
        if (distance < 0.15f)
        {
            targetHealth = null;
            return NodeState.SUCCESS;
        }

        return NodeState.RUNNING;
    }
}