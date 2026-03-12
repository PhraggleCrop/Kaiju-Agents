using BehaviourTree;
using KaijuSolutions.Agents;
using UnityEngine;
using KaijuSolutions.Agents.Exercises.CTF;
public class HealCheck : Node
{
    private TrooperController controller;

    public HealCheck(TrooperController controller)
    {
        this.controller = controller;
    }


    public override NodeState Evaluate()
    {
        if (controller.strategy == "HEAL")
        {

            return NodeState.SUCCESS;
        }

        return NodeState.FAILURE;
    }
}