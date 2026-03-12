using BehaviourTree;
using UnityEngine;
using KaijuSolutions.Agents;
using KaijuSolutions.Agents.Exercises.CTF;
using KaijuSolutions.Agents.Actuators;
public class AttackEnemy : Node
{
    private TrooperController controller;
    private Trooper trooper;
    private KaijuAgent agent;
    private BlasterActuator blaster;

    public AttackEnemy(TrooperController controller)
    {
        this.controller = controller;
        trooper = controller.GetComponent<Trooper>();
        agent = controller.GetComponent<KaijuAgent>();

        // Get the blaster from a child object
        blaster = controller.GetComponentInChildren<BlasterActuator>();
        
    }


    public override NodeState Evaluate()
    {
        Transform target = GetData("Target") as Transform;

        if (target == null)
        {
            return NodeState.FAILURE;
        }

        agent.Stop();
        // Look at target
        controller.GetComponent<Transform>().LookAt(target.position);

        // Fire
        var result = blaster.Fire();
        switch (result)
        {
            case KaijuActuatorState.Done:
                return NodeState.SUCCESS;
            case KaijuActuatorState.Executing:
                return NodeState.RUNNING;
            default:
                return NodeState.FAILURE;
        }
    }
}