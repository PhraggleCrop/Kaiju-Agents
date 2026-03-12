using BehaviourTree;
using KaijuSolutions.Agents;
using UnityEngine;
using KaijuSolutions.Agents.Exercises.Microbes;

public class CheckForPredator : Node
{
    private Microbe agent;
    private MicrobeVisionSensor sensor;

    public CheckForPredator(Microbe agent)
    {
        this.agent = agent;
        sensor = agent.GetComponent<MicrobeVisionSensor>();
    }

    public override NodeState Evaluate()
    {
        //Check if we already have a predator stored
        //Since fleeing rotates our vision cone around, if we are fleeing someone already and turned out, keep them in memory
        //And compare their distance even if theyre not in vision
        Microbe existingTarget = (Microbe)GetData("predatorTarget");

        //If they exist
        if (existingTarget != null && existingTarget.gameObject.activeInHierarchy)
        {
            float existingDistance = Vector3.Distance(
                agent.transform.position,
                existingTarget.transform.position
            );

            //If predator still close, keep fleeing
            if (existingDistance < 6f)
            {
                parent.SetData("predatorTarget", existingTarget);
                return NodeState.SUCCESS;
            }
            else
            {
                //FOrget it if its out of range
                ClearData("predatorTarget");
            }
        }

        //If we have no pre existant predator chaisng us, we check our vision
        Microbe bestTarget = null;
        float closestDistance = Mathf.Infinity;

        foreach (Microbe other in sensor.Observed)
        {
            if (other == null)
                continue;

            if (!other.Eatable(agent))
                continue;

            if (agent.Energy > other.Energy)
                continue;

            float distance = Vector3.Distance(
                agent.transform.position,
                other.transform.position
            );

            if (distance < closestDistance)
            {
                closestDistance = distance;
                bestTarget = other;
            }
        }

        if (bestTarget == null)
            return NodeState.FAILURE;

        parent.SetData("predatorTarget", bestTarget);
        return NodeState.SUCCESS;
    }
}