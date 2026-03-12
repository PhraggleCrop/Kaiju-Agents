using UnityEngine;
using BehaviourTree;
using KaijuSolutions.Agents;

public class TaskWander : Node
{
    private KaijuAgent agent;
    private Vector3 wanderTarget;
    private float changeTimer;
    private float changeInterval = 2f;

    public TaskWander(KaijuAgent agent)
    {
        this.agent = agent;
        PickNewTarget();
    }

    //Evaluate function override
    public override NodeState Evaluate()
    {
        if (agent == null)
            return NodeState.FAILURE;
        //Incrementer time to change seeking pposition
        changeTimer += Time.deltaTime;

        //Check distance
        float distance = Vector3.Distance(agent.transform.position, wanderTarget);

        //If close enough, pick new target immediately
        if (distance < 0.5f)
        {
            PickNewTarget();
        }

        //Raycast in front and check for Walls
        Ray ray = new Ray(agent.transform.position, agent.transform.forward);
        RaycastHit hit;
        //If we detect something and it is a wall, trigger finding a random target so we avoid walls
        if (Physics.Raycast(ray, out hit, 5f))
        {
            if (hit.collider.gameObject.name.StartsWith("Wall"))
            {
                PickNewTarget();
            }
        }

        //Change if not reached and time is up
        if (changeTimer >= changeInterval)
        {
            PickNewTarget();
        }

        //Seek towards the target position
        agent.Seek(wanderTarget);

        //Return our state as running
        state = NodeState.RUNNING;
        return state;
    }

    private void PickNewTarget()
    {

        changeTimer = 0f;
        //Pick random direction
        Vector2 random = Random.insideUnitCircle.normalized;

        //Project by distance
        wanderTarget=agent.transform.position+ new Vector3(random.x, 0f,random.y) * 5f;
    }
}