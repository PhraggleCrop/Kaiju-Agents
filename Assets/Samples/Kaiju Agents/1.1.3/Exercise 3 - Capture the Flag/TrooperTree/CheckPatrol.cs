using BehaviourTree;
using KaijuSolutions.Agents;
using UnityEngine;
using KaijuSolutions.Agents.Exercises.CTF;
public class CheckPatrol : Node
{
    private TrooperController controller;

    public CheckPatrol(TrooperController controller)
    {
        this.controller = controller;
    }


    public override NodeState Evaluate()
    {


        if (controller.strategy == "DEFEND")
        {

            return NodeState.SUCCESS;
        }

        return NodeState.FAILURE;
    }
}