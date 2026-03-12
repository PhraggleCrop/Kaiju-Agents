using UnityEngine;
using BehaviourTree;
using KaijuSolutions.Agents.Exercises.Microbes;

public class TaskAvoidPredator : Node
{
    private readonly Microbe microbe;
    private float stoppingDistance = 22f;

    public TaskAvoidPredator(Microbe microbe)
    {
        this.microbe = microbe;
    }

    public override NodeState Evaluate()
    {
        //Get stored prey target
        Microbe target = (Microbe)GetData("predatorTarget");

        // Check if it exists
        if (target == null || !target.gameObject.activeInHierarchy)
            return NodeState.FAILURE;

        //Get distance to it
        float distance = Vector3.Distance(
            microbe.transform.position,
            target.transform.position
        );

        //Get vector calculation for the direction we need to move away from
        Vector3 targetPos3 = target.transform.position;
        Vector2 targetPos2 = new(targetPos3.x, targetPos3.z);

        //Flee target
        microbe.Agent.Flee(targetPos2, distance: stoppingDistance);


        return NodeState.RUNNING;
    }
}