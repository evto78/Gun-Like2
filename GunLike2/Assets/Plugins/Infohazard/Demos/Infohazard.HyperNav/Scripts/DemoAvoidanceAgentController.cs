using Infohazard.HyperNav;
using UnityEngine;

namespace Infohazard.Demos {
    public class DemoAvoidanceAgentController : MonoBehaviour {
        [SerializeField] private NavAgent _navAgent;
        [SerializeField] private Transform _destination;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private float _maxSpeed = 2;
        [SerializeField] private float _acceleration = 6;
        [SerializeField] private float _rotationSpeed = 5;

        private void Start() {
            _navAgent.Destination = _destination.position;
        }

        private void Update() {
            _navAgent.AvoidanceMaxSpeed = _maxSpeed;
            _rigidbody.velocity = Vector3.MoveTowards(_rigidbody.velocity, _navAgent.DesiredVelocity * _maxSpeed, 
                                                      _acceleration * Time.deltaTime);
            
            if (_rigidbody.velocity.sqrMagnitude > 0.01f) {
                transform.rotation = Quaternion.Slerp(transform.rotation,
                                                      Quaternion.LookRotation(_rigidbody.velocity, Vector3.up),
                                                      Time.deltaTime * _rotationSpeed);
            }
        }
    }
}