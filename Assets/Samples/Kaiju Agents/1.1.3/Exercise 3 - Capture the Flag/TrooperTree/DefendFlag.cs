using BehaviourTree;
using UnityEngine;
using KaijuSolutions.Agents.Exercises.CTF;
using KaijuSolutions.Agents;
public class DefendFlag : Node
{
    private TrooperController controller;
    private Trooper trooper;
    private KaijuAgent trooper_agent;

    public DefendFlag(TrooperController controller)
    {
        this.controller = controller;
        trooper = controller.GetComponent<Trooper>();
        trooper_agent = controller.GetComponent<KaijuAgent>();
    }

    public override NodeState Evaluate()
    {

        //Go to flag
        Flag teamFlag = trooper.TeamFlag;
        Vector3 targetPosition;
        targetPosition = teamFlag.transform.position;


        trooper_agent.PathFollow(targetPosition);

        //If we are within distince return success, else keep running
        if (Vector3.Distance(controller.transform.position, targetPosition) < 1.2f)
        {
            return NodeState.SUCCESS;

        }



        return NodeState.RUNNING;
    }
}


