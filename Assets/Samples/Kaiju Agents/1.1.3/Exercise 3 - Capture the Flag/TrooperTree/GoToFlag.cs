using BehaviourTree;
using UnityEngine;
using KaijuSolutions.Agents.Exercises.CTF;
using KaijuSolutions.Agents;
public class GoToFlag : Node
{
    private TrooperController controller;
    private Trooper trooper;
    private KaijuAgent trooper_agent;

    public GoToFlag(TrooperController controller)
    {
        this.controller = controller;
        trooper = controller.GetComponent<Trooper>();
        trooper_agent = controller.GetComponent<KaijuAgent>();
    }

    public override NodeState Evaluate()
    {
        //Go to flag
        Vector3 targetPosition;
        targetPosition = trooper.EnemyFlag.transform.position;

        //If flag is grabbed, move to flag at an offset
        if (trooper.EnemyFlag.IsGrabbed)
        {
            Vector3 directionAway = (targetPosition - trooper_agent.transform.position).normalized;

            // Large offset away from the agent if the flag is grabbed
            float offsetDistance = 20f; // adjust as needed
            targetPosition = targetPosition + directionAway * offsetDistance;
        }


        trooper_agent.PathFollow(targetPosition);

        //If we are within distince return success, else keep running
        if (Vector3.Distance(controller.transform.position, targetPosition) < 1.2f)
        {
            return NodeState.SUCCESS;

        }



        return NodeState.RUNNING;
    }
}


