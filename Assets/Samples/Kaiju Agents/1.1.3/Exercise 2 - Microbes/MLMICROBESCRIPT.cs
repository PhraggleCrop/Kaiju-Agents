using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine;
using KaijuSolutions.Agents.Exercises.Microbes;

namespace Unity.MLAgents
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Microbe))]
    public class MLMICROBESCRIPT : Agent
    {
        private Rigidbody rb;
        private Microbe microbe;

        [Header("Sensors")]
        public MicrobeVisionSensor microbeSensor;
        public EnergyVisionSensor energySensor;

        [Header("Settings")]
        public int maxMicrobesObserved = 3;

        [Header("Movement")]
        public float moveForce = 10f;
        public float maxSpeed = 5f;

        private Microbe[] microbeCache;

        public override void Initialize()
        {
            rb = GetComponent<Rigidbody>();
            microbe = GetComponent<Microbe>();

            microbeCache = new Microbe[maxMicrobesObserved];
        }

        public override void OnEpisodeBegin()
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            transform.localPosition = new Vector3(
                Random.Range(-10f, 10f),
                0.5f,
                Random.Range(-10f, 10f)
            );

            microbe.Energy = 50f;
        }

        public override void CollectObservations(VectorSensor sensor)
        {
            // ===== SELF =====
            sensor.AddObservation(microbe.Energy / 100f);
            sensor.AddObservation(rb.linearVelocity.x / maxSpeed);
            sensor.AddObservation(rb.linearVelocity.z / maxSpeed);

            // ===== MULTI MICROBE VISION =====
            if (microbeSensor != null && microbeSensor.HasObserved)
            {
                int count = microbeSensor.SortDistance(microbeCache);

                for (int i = 0; i < maxMicrobesObserved; i++)
                {
                    if (i < count && microbeCache[i] != null)
                    {
                        var target = microbeCache[i];

                        Vector3 dir = (target.transform.position - transform.position).normalized;
                        float dist = Vector3.Distance(transform.position, target.transform.position) / microbeSensor.Distance;

                        sensor.AddObservation(dir.x);
                        sensor.AddObservation(dir.z);
                        sensor.AddObservation(target.Energy / 100f);
                        sensor.AddObservation(dist);

                        // Key learning signals
                        sensor.AddObservation(microbe.Energy > target.Energy ? 1f : 0f); // can eat
                        sensor.AddObservation(microbe.Compatible(target) ? 1f : 0f);     // can mate
                    }
                    else
                    {
                        // Empty slot
                        sensor.AddObservation(0f);
                        sensor.AddObservation(0f);
                        sensor.AddObservation(0f);
                        sensor.AddObservation(1f);
                        sensor.AddObservation(0f);
                        sensor.AddObservation(0f);
                    }
                }
            }
            else
            {
                // No microbes seen
                for (int i = 0; i < maxMicrobesObserved; i++)
                {
                    sensor.AddObservation(0f);
                    sensor.AddObservation(0f);
                    sensor.AddObservation(0f);
                    sensor.AddObservation(1f);
                    sensor.AddObservation(0f);
                    sensor.AddObservation(0f);
                }
            }

            // ===== ENERGY VISION =====
            if (energySensor != null && energySensor.HasObserved)
            {
                var food = energySensor.Nearest(out float dist, normalize: true);

                Vector3 dir = (food.transform.position - transform.position).normalized;

                sensor.AddObservation(dir.x);
                sensor.AddObservation(dir.z);
                sensor.AddObservation(dist);
            }
            else
            {
                sensor.AddObservation(0f);
                sensor.AddObservation(0f);
                sensor.AddObservation(1f);
            }
        }

        public override void OnActionReceived(ActionBuffers actions)
        {
            float moveX = actions.ContinuousActions[0];
            float moveZ = actions.ContinuousActions[1];

            Vector3 move = new Vector3(moveX, 0, moveZ);
            rb.AddForce(move * moveForce);

            if (rb.linearVelocity.magnitude > maxSpeed)
            {
                rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
            }

            AddReward(0.001f); 

            if (microbe.Energy < 10f)
            {
                AddReward(-0.002f);
            }

            if (microbe.Energy <= 0)
            {
                AddReward(-1f);
                EndEpisode();
            }
        }

        public override void Heuristic(in ActionBuffers actionsOut)
        {
            var actions = actionsOut.ContinuousActions;
            actions[0] = Input.GetAxis("Horizontal");
            actions[1] = Input.GetAxis("Vertical");
        }

        private void OnEnable()
        {
            if (microbe == null) return;

            microbe.OnEat += HandleEat;
            microbe.OnEaten += HandleEaten;
            microbe.OnMate += HandleMate;
        }

        private void OnDisable()
        {
            if (microbe == null) return;

            microbe.OnEat -= HandleEat;
            microbe.OnEaten -= HandleEaten;
            microbe.OnMate -= HandleMate;
        }

        private void HandleEat(Microbe other)
        {
            AddReward(0.5f);
        }

        private void HandleEaten(Microbe other)
        {
            AddReward(-1.0f);
            EndEpisode();
        }

        private void HandleMate(Microbe other)
        {
            AddReward(0.3f);
        }
    }
}