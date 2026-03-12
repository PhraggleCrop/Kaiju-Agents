using BehaviourTree;
using KaijuSolutions.Agents;
using UnityEngine;
using KaijuSolutions.Agents.Exercises.Microbes;

public class CheckForEnergy : Node
{
    private Microbe agent;
    private EnergyVisionSensor sensor;

    //Uses reference to microbe agent for the energyvisionsensor
    public CheckForEnergy(Microbe agent)
    {
        this.agent = agent;
        sensor = agent.GetComponent<EnergyVisionSensor>();
    }
    //Evaluate function uses sensor to see if it can find any energy
    public override NodeState Evaluate()
    {
        if (sensor == null || sensor.ObservedCount == 0)
            return NodeState.FAILURE;

        //Select closest energy element for priority.
        //Base distance is infinity so the things we see are closer 
        EnergyPickup closest = null;
        float closestDist = Mathf.Infinity;

        //FOr each energy we see in the sensor...
        foreach (EnergyPickup energy in sensor.Observed)
        {
            if (energy == null) continue;

            //If distance is smaller than closest than we set it as what we found
            float dist = Vector3.Distance(agent.transform.position, energy.transform.position);
            if (dist < closestDist)
            {
                closest = energy;
                closestDist = dist;
            }
        }

        //If closest isnt null we found an element, set our data dictionary to have an energy target
        //Return success
        //Because in our tree it is the first element in a sequence, it allows us to extend into the task_move_to_energy
        if (closest != null)
        {
            parent.SetData("energyTarget", closest);
            return NodeState.SUCCESS;
        }

        return NodeState.FAILURE;
    }
}