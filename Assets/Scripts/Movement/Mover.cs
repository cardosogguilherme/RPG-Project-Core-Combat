using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using RPG.Core;
using RPG.Saving;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] float maxSpeed = 6f;

        NavMeshAgent navMeshAgent;
        private Health health;

        private void Start()
        {
            // print("Mover started");
            navMeshAgent = GetComponent<NavMeshAgent>();
            health = GetComponent<Health>();
        }

        void Update()
        {
            navMeshAgent.enabled = !health.IsDead;

            UpdateAnimator();

            // Debug.DrawRay(lastRay.origin, lastRay.direction * 100);
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = GetComponent<NavMeshAgent>().velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);

            float speed = localVelocity.z;

            GetComponent<Animator>().SetFloat("forwardSpeed", speed);
        }

        public void MoveTo(Vector3 destination, float speedFraction)
        {
            // print("chamou o moveTo");
            navMeshAgent.destination = destination;
            navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
            navMeshAgent.isStopped = false;
        }

        public void Cancel()
        {
            // print($"Cancelling movement {gameObject.name}");
            navMeshAgent.isStopped = true;
        }

        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination, speedFraction);
        }

        public JToken CaptureAsJToken()
        {
            MoverSaveData data = new MoverSaveData();
            data.position = transform.position.ToToken();
            data.rotation = transform.eulerAngles.ToToken();
            // Dictionary<string, object> data = new Dictionary<string, object>();
            // data["position"] = transform.position.ToToken();
            // data["rotation"] = transform.eulerAngles.ToToken();

            return JToken.FromObject(data);
        }

        public void RestoreFromJToken(JToken state)
        {
            GetComponent<NavMeshAgent>().enabled = false;

            // transform.position = state["position"].ToVector3();
            // transform.eulerAngles = state["rotation"].ToVector3();
            MoverSaveData data = state.ToObject<MoverSaveData>();
            transform.position = data.position.ToVector3();
            transform.eulerAngles = data.rotation.ToVector3();

            GetComponent<NavMeshAgent>().enabled = true;
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        [Serializable]
        struct MoverSaveData
        {
            public JToken position;
            public JToken rotation;
        }
    }
}