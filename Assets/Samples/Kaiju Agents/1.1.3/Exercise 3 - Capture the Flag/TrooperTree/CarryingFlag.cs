using BehaviourTree;
using KaijuSolutions.Agents.Exercises.CTF;
using KaijuSolutions.Agents;

public class CarryingFlag : Node
{
    private Trooper trooper;

    public CarryingFlag(TrooperController controller)
    {
        trooper = controller.GetComponent<Trooper>();
    }

    public override NodeState Evaluate()
    {
        if (trooper._flag != null)
        {
            return NodeState.SUCCESS;
        }

        return NodeState.FAILURE;
    }
}