using BehaviourTree;
using KaijuSolutions.Agents;
using UnityEngine;
using KaijuSolutions.Agents.Exercises.CTF;
public class CheckReload : Node
{
    private TrooperController controller;

    public CheckReload(TrooperController controller)
    {
        this.controller = controller;
    }


    public override NodeState Evaluate()
    {
        if (controller.strategy == "RELOAD")
        {

            return NodeState.SUCCESS;
        }

        return NodeState.FAILURE;
    }
}