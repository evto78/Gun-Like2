using Infohazard.HyperNav;
using UnityEngine;

namespace Infohazard.Demos {
    public class DemoNavAgentControllerFloatingOrigin : MonoBehaviour {
        [SerializeField] private NavAgent _navAgent;
        [SerializeField] private Transform _destination1;
        [SerializeField] private Transform _destination2;
        [SerializeField] private Transform _destinationMarker;
        [SerializeField] private float _maxSpeed = 2;
        [SerializeField] private float _acceleration = 6;
        [SerializeField] private float _rotationSpeed = 5;
        [SerializeField] private float _floatingOriginLimit = 10;
        [SerializeField] private Transform _floatingOriginTransform;

        private Vector3 _velocity;
        private Transform _currentDest;

        private void Start() {
            _currentDest = _destination1;
            _velocity = Vector3.zero;
            UpdateDestination();
        }

        private void UpdateDestination() {
            _navAgent.Stop(true);
            _navAgent.Destination = _currentDest.position;
            _destinationMarker.position = _currentDest.position;
        }

        private void Update() {
            _navAgent.AvoidanceMaxSpeed = _maxSpeed;
            _velocity = Vector3.MoveTowards(_velocity, _navAgent.DesiredVelocity * _maxSpeed, 
                                            _acceleration * Time.deltaTime);
            
            if (_velocity.sqrMagnitude > 0.01f) {
                transform.rotation = Quaternion.Slerp(transform.rotation,
                                                      Quaternion.LookRotation(_velocity, Vector3.up),
                                                      Time.deltaTime * _rotationSpeed);
            }

            transform.position += _velocity * Time.deltaTime;

            if (_navAgent.Arrived) {
                if (_currentDest == _destination1) {
                    _currentDest = _destination2;
                } else {
                    _currentDest = _destination1;
                }
                UpdateDestination();
            }

            if (transform.position.sqrMagnitude > _floatingOriginLimit * _floatingOriginLimit) {
                _floatingOriginTransform.position -= transform.position;
                NavVolume.UpdateAllTransforms();
                UpdateDestination();
            }
        }
    }
}