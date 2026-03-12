using System.Collections.Generic;
using UnityEngine;
using KaijuSolutions.Agents;
using KaijuSolutions.Agents.Exercises.CTF;
using BehaviourTree;

//We set up the whole tree here and custom structure

public class TrooperTree : BehaviourTree.Tree
{
    private TrooperController controller;

    //Constructor sets our reference to our microbe agent
    public TrooperTree(TrooperController controller)
    {
        this.controller = controller;
        Start(); //initialize the tree

        _root.SetData("strategy", "DEFEND");
    }

    //Sets up the tree custom structor
    protected override Node SetupTree()
    {
        //Each of our selectors check the strategy we use
        //Strategy is determined by our UtilityAI
        return new Selector(new List<Node>{

            



            //Retreat heal sequence
            new Sequence(new List<Node>{ 

                new HealCheck(controller),
                new HealRetreat(controller)
            
            }),

             //Retreat heal sequence
            new Sequence(new List<Node>{

                new CheckReload(controller),
                new ReloadRetreat(controller)

            }),



            //Attacking sequences
            new Sequence(new List<Node>{
                new CheckAttack(controller),
            
                    
                //Return to base with flag
                new Sequence(new List<Node>{
                    new CarryingFlag(controller),
                    new ReturnToBase(controller)
                }),


            }),


            //Sequence for trying to get flag
            new Sequence(new List<Node>{
                //Check if we are in attack strategy
                new CheckAttack(controller),

                new Selector(new List<Node>{ 
                    new Sequence(new List<Node>{
                        new DetectClosestEnemy(controller),
                        new AttackEnemy(controller),
                    }),
                    

                    //Attack sequence / Go to flag sequence
                    new Sequence(new List<Node>{
                        new NotCarryingFlag(controller),
                        new GoToFlag(controller)
                    }),



                })
            }),


            //Sequence for patroling
            new Sequence(new List<Node>{
                //Use a checkpatrol to make sure we are doing defend strategy
                new CheckPatrol(controller),
                //Selector runs our patrol logic
                new Selector(new List<Node>{

                    //DEFEND IF FLAG GRABBED
                    new Sequence(new List<Node>{
                        new TeamFlagGrabbed(controller),

                        new DefendFlag(controller),

                        new Sequence(new List<Node>{

                            new DetectClosestEnemy(controller),
                            new AttackEnemy(controller)


                        }),

                    }),


                    //Main sequence runs our patroling logic
                    new Sequence(new List<Node>{
                        new HasDestinationNode(),
                        new PatrolToNode(controller),
                        new ClearPatrolDestination()

                    }),
                    new PickRandomPickupNode(controller)
                })
            })

            
            
            



        });
    }

 


    public void Tick()
    {
        Update();
    }
}