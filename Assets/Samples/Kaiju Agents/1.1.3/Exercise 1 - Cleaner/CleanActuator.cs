using UnityEngine;
using KaijuSolutions.Agents.Actuators;
using KaijuSolutions.Agents.Exercises.Cleaner;
//DANIEL YUN
//110067280
public class FloorCleanActuator : KaijuActuator
{
    private float cleanDelay = 2.5f;

    private float timer;
    public Floor targetFloor;

    protected override KaijuActuatorState Run()
    {
        //CHECK IF THERES AN AGENT
        if (Agent == null)
            return KaijuActuatorState.Failed;
        Debug.Log("Cleaning");
        // If we haven't locked onto a floor yet, try to find one
        if (Physics.Raycast(Agent.transform.position,Vector3.down,out RaycastHit hit,2f))
        {
            if (hit.collider.GetComponent<Floor>() != targetFloor) {

                //If our raycast hits a floor continue, if not fail
                targetFloor = hit.collider.GetComponent<Floor>();

                if (targetFloor == null)
                    return KaijuActuatorState.Failed;
                //Set timer to zero to start cleaning
                timer = 0f;
            }
            
        }
        else
        {
            return KaijuActuatorState.Failed;
        }
        
        //Increase time with time as we wait
        timer += Time.deltaTime;
        //If timer is above the waiting time we clean and reset
        if (timer >= cleanDelay)
        {
            targetFloor.Clean();
            targetFloor = null;
            return KaijuActuatorState.Done;
        }

        return KaijuActuatorState.Executing;
    }
}