using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using KaijuSolutions.Agents;
using KaijuSolutions.Agents.Exercises.Microbes;

//We set up the whole tree here and custom structure

public class MicrobeTree : BehaviourTree.Tree
{
    private Microbe agent;

    //Constructor sets our reference to our microbe agent
    public MicrobeTree(Microbe agent)
    {
        this.agent = agent;
        Start(); // initialize the tree
    }

    //Sets up the tree custom structor
    protected override Node SetupTree()
    {
        //Start tree at a selector
        //Selector goes through the children prioritizing top down
        //Stops when one is running successfully

        return new Selector(new List<Node>{
        
        //Avoiding predator
        new Sequence(new List<Node>{
            new CheckForPredator(agent),
            new TaskAvoidPredator(agent)
        }),
        //Energy seeking sequence
        new Sequence(new List<Node>{
            new CheckForEnergy(agent),
            new TaskMoveToEnergy(agent)
        }),

        //Hunting sequence/Running away
        new Sequence(new List<Node>{
            new CheckForPrey(agent),
            new TaskMoveToPrey(agent)
        }),

        //Mating branch
        new Sequence(new List<Node>{
            new CheckForMate(agent),
            new TaskMoveToMate(agent)
        }),

        //Wander branch
        new TaskWander(agent)
    });
    }



    public void Tick()
    {
        Update();
    }
}