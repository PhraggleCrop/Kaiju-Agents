using BehaviourTree;
using UnityEngine;
using KaijuSolutions.Agents.Exercises.CTF;
using KaijuSolutions.Agents;

public class ReturnToBase : Node
{
    private TrooperController controller;
    private Trooper trooper;
    private KaijuAgent trooper_agent;

    public ReturnToBase(TrooperController controller)
    {
        this.controller = controller;
        trooper = controller.GetComponent<Trooper>();
        trooper_agent = controller.GetComponent<KaijuAgent>();
    }

    public override NodeState Evaluate()
    {

        // Grab the flag reference directly from the trooper
        Flag carriedFlag = trooper._flag;

        if (carriedFlag == null)
        {
            // Not carrying a flag, so nothing to do
            return NodeState.FAILURE;
        }

        // Determine the trooper's own base
        Vector3 basePosition = trooper.TeamOne ? Flag.TeamOneBase3 : Flag.TeamTwoBase3;

        // Move toward the base
        trooper_agent.PathFollow(basePosition);

        // Check if we're close enough to capture/return the flag
        if (Vector3.Distance(controller.transform.position, basePosition) < CaptureTheFlagManager.CaptureDistance)
        {
            carriedFlag.Return();  // Return flag to base
            return NodeState.SUCCESS;
        }

        return NodeState.RUNNING;
    }
}