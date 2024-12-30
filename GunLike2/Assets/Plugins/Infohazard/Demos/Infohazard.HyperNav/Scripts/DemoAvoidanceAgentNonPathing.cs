using System.Collections;
using System.Collections.Generic;
using Infohazard.HyperNav;
using UnityEngine;

namespace Infohazard.Demos {
    public class DemoAvoidanceAgentNonPathing : MonoBehaviour {
        [SerializeField] private AvoidanceAgent _agent;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private float _acceleration;
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private MeshRenderer _renderer;
        [SerializeField] private float _acceptanceRadius = 0.5f;

        private Vector3 _dest;
        private bool _arrived;
        
        // Start is called before the first frame update
        private void Start() {
            _dest = -transform.position;
            _agent.InputVelocityFunc = GetDesiredVelocity;
        }

        // Update is called once per frame
        private void Update() {
            Vector3 movement = _agent.AvoidanceVelocity;
            _rigidbody.velocity =
                Vector3.MoveTowards(_rigidbody.velocity, movement, _acceleration * Time.deltaTime);

            if (movement.sqrMagnitude > 0.01f) {
                transform.rotation = Quaternion.Slerp(transform.rotation,
                                                      Quaternion.LookRotation(movement, Vector3.up),
                                                      _rotationSpeed * Time.deltaTime);
            }

            if (!_arrived && Vector3.SqrMagnitude(transform.position - _dest) < _acceptanceRadius) {
                _arrived = true;
            }
        }

        public void SetColor(Color color) {
            _renderer.materials[0].color = color;
        }

        private Vector3 GetDesiredVelocity() {
            return _arrived 
                ? Vector3.zero 
                : Vector3.ClampMagnitude(_dest - transform.position, _agent.MaxSpeed);
        }
    }
}