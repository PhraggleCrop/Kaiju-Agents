using BehaviourTree;
using KaijuSolutions.Agents.Exercises.CTF;

public class TeamFlagGrabbed : Node
{
    private Trooper trooper;

    public TeamFlagGrabbed(TrooperController controller)
    {
        trooper = controller.GetComponent<Trooper>();
    }

    public override NodeState Evaluate()
    {
        //Get our team's flag
        Flag teamFlag = trooper.TeamFlag;

        if (teamFlag == null)
        {
            // No flag exists? treat as not grabbed
            return NodeState.FAILURE;
        }

        // If the flag has a holder, it has been grabbed
        return teamFlag.IsGrabbed ? NodeState.SUCCESS : NodeState.FAILURE;
    }
}