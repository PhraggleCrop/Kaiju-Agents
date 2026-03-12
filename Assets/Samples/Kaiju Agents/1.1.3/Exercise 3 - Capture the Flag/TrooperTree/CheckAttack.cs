using BehaviourTree;
using KaijuSolutions.Agents;
using UnityEngine;
using KaijuSolutions.Agents.Exercises.CTF;
public class CheckAttack : Node
{
    private TrooperController controller;

    public CheckAttack(TrooperController controller)
    {
        this.controller = controller;
    }


    public override NodeState Evaluate()
    {
        if (controller.strategy == "ATTACK")
        {

            return NodeState.SUCCESS;
        }

        return NodeState.FAILURE;
    }
}