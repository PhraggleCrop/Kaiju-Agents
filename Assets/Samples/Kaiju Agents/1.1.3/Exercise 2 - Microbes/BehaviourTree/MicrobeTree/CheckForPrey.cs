using BehaviourTree;
using KaijuSolutions.Agents;
using UnityEngine;
using KaijuSolutions.Agents.Exercises.Microbes;

public class CheckForPrey : Node
{
    private Microbe agent;
    private MicrobeVisionSensor sensor;

    public CheckForPrey(Microbe agent)
    {
        this.agent = agent;
        sensor = agent.GetComponent<MicrobeVisionSensor>();
    }

    public override NodeState Evaluate()
    {
        //Using same logic as energy checker
        //We get closest element possible, starting offf at infinity distnace
        Microbe bestTarget = null;
        float closestDistance =Mathf.Infinity;

        //Get observerd  things from microbe vision sensor
        foreach (Microbe other in sensor.Observed)
        {
            if (other == null)
                continue;

            //Check if its eatable
            if (!agent.Eatable(other))
                continue;

            //Check if we have more energy to win the exchange
            if (agent.Energy <= other.Energy)
                continue;
            //Get distance to the vector for comparison
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

        //Store the target in the data dictionary
        parent.SetData("preyTarget", bestTarget);
        return NodeState.SUCCESS;
    }
}