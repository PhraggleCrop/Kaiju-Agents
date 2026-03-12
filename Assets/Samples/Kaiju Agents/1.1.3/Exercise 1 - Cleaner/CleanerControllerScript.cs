using UnityEngine;
using KaijuSolutions.Agents.Actuators;
using KaijuSolutions.Agents.Extensions;
using KaijuSolutions.Agents.Movement;
using KaijuSolutions.Agents.Sensors;

namespace KaijuSolutions.Agents.Exercises.Cleaner
{
    public class CleanerControllerScript : MonoBehaviour
    {

        private KaijuTransformAgent agent;
        private FloorCleanActuator cleanActuator;
        private Quaternion targetRotation;

        public string state = "WANDER";
        public GameObject prefab;
        private KaijuEverythingVisionSensor kes;

        //Get componenets to use like the agent we move around the cleaning actuator, and the everything sensor
        void Awake()
        {
            agent = GetComponent<KaijuTransformAgent>();
            cleanActuator = GetComponent<FloorCleanActuator>();
            kes = GetComponent<KaijuEverythingVisionSensor>();
        }

        // Update is called once per frame
        void Update()
        {
            //Set a short t for transform shortcult
            Transform t = transform;

            //Switch state machine for managing the behaviour stuff
            switch (state)
            {
                //WANDER
                //In wander we move around randomly and bump against walls like a roomba
                //If we see a dirty tile we switch to moving to clean it
                case "WANDER":
                    //Detect walls.
                    //If we see a wall in front of us...
                    if (Physics.Raycast(t.position, t.forward, out RaycastHit wallHit, 2f))
                    {
                        //Switch state to rotate
                        state = "ROTATE";
                        //Set targetrotation for ROTATE state to go to
                        Vector3 newForward = wallHit.normal;
                        //Add randomn angle offset
                        float randomAngle = Random.Range(-60f,60f);
                        newForward = Quaternion.AngleAxis(randomAngle, Vector3.up) * newForward;
                        //New rotation to rotate towards in our rotation state
                        targetRotation = Quaternion.LookRotation(newForward, Vector3.up);

                        break;
                    }


                    //Check if our vision sees a dirty tile
                    foreach (Transform o in kes.Observed)
                    {
                        //Get Floor component of the tile
                        Floor floor = o.GetComponent<Floor>();

                        if (floor != null && floor.Dirty)
                        {
                            //If it is dirty we set a target position vector 
                            Vector3 targetPos = o.transform.position;
                            targetPos.y = agent.transform.position.y;
                            //Call agent to pursue/go towards the target position and switch to seek.
                            agent.Pursue(targetPos);

                            state = "SEEK";
                            Debug.Log("SEEK: " + o.name);
                            break;
                        }
                    }

                    //Move forwards
                    t.position += t.forward * 8f * Time.deltaTime;
                    break;


                //SEEK
                //State where we let agent move, and ignore seeing any other dirty tiles
                case "SEEK":
                    //Check if we are above dirtystuff
                    //If there is stuff we sit and wait for actuator to clean it
                    if (Physics.Raycast(t.position, Vector3.down, out RaycastHit floorHit, 2f))
                    {
                        //Our raycast checks floor component and if it is dirty
                        var targetFloor = floorHit.collider.GetComponent<Floor>();
                        Debug.Log(targetFloor.name);
                        if (targetFloor != null)
                        {

                            if (targetFloor.Dirty == true)
                            {
                                Debug.Log("CLEAN");
                                //Switch to cleaning state
                                state = "CLEANING";
                                break;

                            }

                        }

                    }
                    
                    break;

                //ROTATE
                //After bumping into a wall, we rotate the our rotation towards the target rotation
                case "ROTATE":
                    t.rotation = Quaternion.RotateTowards(t.rotation,targetRotation,180f * Time.deltaTime);
                    //Once we reach within a threshold of 1 we set ourselves back to WANDER
                    if (Quaternion.Angle(t.rotation, targetRotation) < 1f)
                    {
                        state = "WANDER";
                    }

                    break;

                //CLEANING
                //STATE WHERE WE CLEAN
                case "CLEANING":

                    //SHOOT RAYCAST FOR DIRTY FLOORS
                    if (Physics.Raycast(t.position, Vector3.down, out RaycastHit dirtyCheck, 3f))
                    {

                        Floor targetFloor = dirtyCheck.collider.GetComponent<Floor>();
                        Debug.Log((targetFloor).ToString());
                        if (targetFloor != null)
                        {
                            Debug.Log("DirtyStatus: " + (targetFloor.Dirty).ToString());
                            //If floor isn't dirty our actuator must have cleaned it
                            if (targetFloor.Dirty == false)
                            {
                                //GameObject newInstance = Instantiate(prefab, t.position, Quaternion.Euler(0f, t.eulerAngles.y + Random.Range(-20f, 20f), 0f));
                                state = "WANDER";

                                Debug.Log("CLEANED");

                                break;

                            }

                            //Run actuator, make it start cleaning if its not
                            if (cleanActuator.targetFloor == null)
                            {
                                cleanActuator.Begin();
                            }

                        }

                    }
                        break;
            }


        }





    }

}
