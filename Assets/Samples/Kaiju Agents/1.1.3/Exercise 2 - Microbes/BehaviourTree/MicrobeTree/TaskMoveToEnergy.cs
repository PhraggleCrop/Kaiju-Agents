using UnityEngine;
using BehaviourTree;
using KaijuSolutions.Agents;
using KaijuSolutions.Agents.Exercises.Microbes;

public class TaskMoveToEnergy : Node
{
    private KaijuAgent agent;
    private float stoppingDistance = 1f;

    public TaskMoveToEnergy(KaijuAgent agent)
    {
        this.agent = agent;
    }

    public override NodeState Evaluate()
    {
        // get stored target in the data dictionary
        EnergyPickup target = (EnergyPickup)GetData("energyTarget");

        //If we cant find it or it doesnt exist than we abort
        if (target == null)
            return NodeState.FAILURE;

        //Get distance to target
        float distance = Vector3.Distance(
            agent.transform.position,
            target.transform.position
        );

        //We succeed if we get close enough to energy
        if (distance <= stoppingDistance)
            return NodeState.SUCCESS;

        //If we havent get, move closer
        agent.Seek(target.transform.position);

        return NodeState.RUNNING;
    }
}