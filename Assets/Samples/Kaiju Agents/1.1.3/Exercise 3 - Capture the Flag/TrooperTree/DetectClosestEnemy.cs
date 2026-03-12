using BehaviourTree;
using UnityEngine;
using KaijuSolutions.Agents.Exercises.CTF;
using KaijuSolutions.Agents;

public class DetectClosestEnemy : Node
{
    private TrooperController controller;

    public DetectClosestEnemy(TrooperController controller)
    {
        this.controller = controller;
    }

    public override NodeState Evaluate()
    {
        //If there are no enemies
        if (controller.visibleEnemies.Count == 0)
        {
            parent.SetData("Target",null); 
            return NodeState.FAILURE;
        }

        //Calculate closest enemy
        Transform closestEnemy = null;
        float closestDistanceSqr = float.MaxValue;

        foreach (var enemy in controller.visibleEnemies)
        {
            if (enemy == null) continue;

            float distSqr = (enemy.transform.position - controller.transform.position).sqrMagnitude;
            if (distSqr < closestDistanceSqr)
            {
                closestDistanceSqr = distSqr;
                closestEnemy = enemy.transform;
            }
        }
        //Set enemy
        if (closestEnemy != null)
        {

            parent.SetData("Target", closestEnemy);
            return NodeState.SUCCESS;
        }

        parent.SetData("Target", null);
        return NodeState.FAILURE;
    }
}