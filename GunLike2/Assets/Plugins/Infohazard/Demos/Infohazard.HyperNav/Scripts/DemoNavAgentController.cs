using System;
using System.Collections;
using System.Collections.Generic;
using Infohazard.HyperNav;
using UnityEngine;

namespace Infohazard.Demos {
    public class DemoNavAgentController : MonoBehaviour {
        [SerializeField] private NavAgent _regularAgent;
        [SerializeField] private SplineNavAgent _splineAgent;
        
        [SerializeField] private float _acceleration = 1;
        [SerializeField] private float _maxSpeed = 2;
        [SerializeField] private float _turnSpeed = 5;

        [SerializeField] private MeshRenderer _targetMesh;
        [SerializeField] private Material _pathMat;
        [SerializeField] private Material _noPathMat;

        private bool _splineEnabled = false;
        private Vector3 _velocity;

        public NavAgent CurrentAgent => _splineEnabled ? _splineAgent : _regularAgent;

        public void SetSplineEnabled(bool value) {
            bool hadDest = !CurrentAgent.Arrived;
            Vector3 destination = CurrentAgent.Destination;
            
            _splineEnabled = value;
            _regularAgent.enabled = !value;
            _splineAgent.enabled = value;
            
            _regularAgent.Stop(true);
            _splineAgent.Stop(true);

            if (hadDest) CurrentAgent.Destination = destination;
        }

        private void Start() {
            SetSplineEnabled(false);
            
            _regularAgent.PathFailed += Agent_PathFailed;
            _regularAgent.PathReady += Agent_PathReady;
            _splineAgent.PathFailed += Agent_PathFailed;
            _splineAgent.PathReady += Agent_PathReady;
        }

        private void Agent_PathReady() {
            _targetMesh.sharedMaterial = _pathMat;
        }

        private void Agent_PathFailed() {
            _targetMesh.sharedMaterial = _noPathMat;
        }

        public void SetDestination(Vector3 position) {
            CurrentAgent.Destination = position;
        }

        public void Stop() {
            CurrentAgent.Stop(true);
        }

        private void Update() {
            NavAgent agent = CurrentAgent;
            if (!agent.Arrived) {
                Vector3 desiredVel = agent.DesiredVelocity;
                agent.AccelerationEstimate = _acceleration;
                _velocity = Vector3.MoveTowards(_velocity, desiredVel * _maxSpeed, 
                                                _acceleration * Time.deltaTime);
                transform.position += _velocity * Time.deltaTime;

                if (desiredVel.sqrMagnitude > 0) {
                    transform.rotation = Quaternion.Slerp(transform.rotation,
                                                          Quaternion.LookRotation(desiredVel, Vector3.up),
                                                          _turnSpeed * Time.deltaTime);
                }
            }
        }
    }
}