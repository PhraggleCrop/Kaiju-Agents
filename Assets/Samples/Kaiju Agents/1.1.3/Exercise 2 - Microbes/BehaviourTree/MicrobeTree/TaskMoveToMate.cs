using BehaviourTree;
using KaijuSolutions.Agents.Exercises.Microbes;
using UnityEngine;

public class TaskMoveToMate : Node
{
    private readonly Microbe microbe;
    private float stoppingDistance = 1.2f;

    public TaskMoveToMate(Microbe microbe)
    {
        this.microbe = microbe;
    }

    //In our evalutate we move to 
    public override NodeState Evaluate()
    {
        //If we arent on cooldown
        Microbe target = (Microbe)GetData("mateTarget");

        if (target == null || target.OnCooldown)
            return NodeState.FAILURE;

        //Check if we are in distance range
        float distance = Vector3.Distance(
            microbe.transform.position,
            target.transform.position
        );

        if (distance <= stoppingDistance)
            return NodeState.SUCCESS; 

        //Pursue target if we arent in range
        microbe.Agent.Pursue(target.transform, stoppingDistance);

        return NodeState.RUNNING;
    }
}