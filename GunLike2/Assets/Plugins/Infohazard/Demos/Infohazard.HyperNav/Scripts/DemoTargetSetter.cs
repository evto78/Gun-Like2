using UnityEngine;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace Infohazard.Demos {
    public class DemoTargetSetter : MonoBehaviour {
        [SerializeField] private Transform _target;
        [SerializeField] private Camera _camera;
        [SerializeField] private DemoNavAgentController _agent;
        [SerializeField] private float _destOffset;
        
        private void Update() {
#if ENABLE_INPUT_SYSTEM
            bool mouseDown = Mouse.current.leftButton.wasPressedThisFrame;
            Vector2 mousePosition = Mouse.current.position.ReadValue();
#else
            bool mouseDown = Input.GetMouseButtonDown(0);
            Vector2 mousePosition = Input.mousePosition;
#endif

            if (mouseDown && mousePosition.x > Screen.width / 2.0f) {
                Ray ray = _camera.ScreenPointToRay(mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit)) {
                    _target.position = hit.point + hit.normal * _destOffset;
                    _agent.SetDestination(_target.position);
                }
            }
        }
    }
}