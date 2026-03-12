using BehaviourTree;
using UnityEngine;
using KaijuSolutions.Agents.Exercises.CTF;
using KaijuSolutions.Agents;
public class PatrolToNode : Node
{
    private TrooperController controller;
    private Trooper trooper;
    private KaijuAgent trooper_agent;

    public PatrolToNode(TrooperController controller)
    {
        this.controller = controller;
        trooper = controller.GetComponent<Trooper>();
        trooper_agent = controller.GetComponent<KaijuAgent>();
    }

    public override NodeState Evaluate()
    {
        //get our patrol target data
        object data = GetData("patrolTarget");

        if (data == null)
        {
            return NodeState.FAILURE;

        }

        //pathfollow to target
        Vector3 target = (Vector3)data;
        trooper_agent.PathFollow(target);

        //If we are within distince return success, else keep running
        if (Vector3.Distance(trooper_agent.transform.position, target) < 0.1f)
        {

            return NodeState.SUCCESS;

        }

            

        return NodeState.RUNNING;
    }
}