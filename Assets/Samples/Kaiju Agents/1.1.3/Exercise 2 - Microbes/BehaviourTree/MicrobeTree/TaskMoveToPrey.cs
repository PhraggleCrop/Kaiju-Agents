using UnityEngine;
using BehaviourTree;
using KaijuSolutions.Agents.Exercises.Microbes;

public class TaskMoveToPrey : Node
{
    private readonly Microbe microbe;
    private float stoppingDistance = 1.2f;

    public TaskMoveToPrey(Microbe microbe)
    {
        this.microbe = microbe;
    }

    public override NodeState Evaluate()
    {
        //Get stored prey target
        Microbe target = (Microbe)GetData("preyTarget");

        // Check if it exists
        if (target == null || !target.gameObject.activeInHierarchy)
            return NodeState.FAILURE;

        //Get distance to it
        float distance = Vector3.Distance(
            microbe.transform.position,
            target.transform.position
        );

        // If we are in distance we succeeded, else we move closer
        if (distance <= stoppingDistance)
            return NodeState.SUCCESS;

        //Get vector calculation for the direction we need to move to
        Vector3 targetPos3 = target.transform.position;
        Vector2 targetPos2 = new(targetPos3.x, targetPos3.z);

        //Pursue towards target
        microbe.Agent.Pursue(targetPos2, distance: stoppingDistance);

        return NodeState.RUNNING;
    }
}