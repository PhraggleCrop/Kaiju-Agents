using BehaviourTree;
using KaijuSolutions.Agents.Exercises.Microbes;
using UnityEngine;

public class CheckForMate : Node
{
    private readonly Microbe microbe;

    public CheckForMate(Microbe microbe)
    {
        this.microbe = microbe;
    }
    //Same logic code ans checking for energy and prey
    //If we see something, get it and prioritize closest
    public override NodeState Evaluate()
    {
        //If we are on cooldown fail
        if (microbe.OnCooldown)
            return NodeState.FAILURE;

        Microbe bestMate = null;
        float closestDistance = Mathf.Infinity;

        //For each microbe
        foreach (Microbe other in Microbe.All)
        {
            //If its not null and not ourselves
            if (other == null || other == microbe)
                continue;
            //If its compatible for mating
            if (!microbe.Compatible(other))
                continue;
            //If our cooldowns work
            if (other.OnCooldown)
                continue;
            //Get closest by distance
            float distance = Vector3.Distance(
                microbe.transform.position,
                other.transform.position
            );

            if (distance < closestDistance)
            {
                closestDistance = distance;
                bestMate = other;
            }
        }

        if (bestMate == null)
            return NodeState.FAILURE;

        //Set a mating target and succeed!
        parent.SetData("mateTarget", bestMate);
        return NodeState.SUCCESS;
    }
}