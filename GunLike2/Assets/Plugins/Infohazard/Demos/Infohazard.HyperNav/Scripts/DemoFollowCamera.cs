using System;
using System.Collections;
using System.Collections.Generic;
using Infohazard.Core;
using UnityEngine;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace Infohazard.Demos {
    [ExecuteInEditMode]
    public class DemoFollowCamera : MonoBehaviour {
        [SerializeField] private Transform _follow;
        [SerializeField] private float _distance;
        [SerializeField] private float _sensitivityX;
        [SerializeField] private float _sensitivityY;
        [SerializeField] private Vector3 _angles;

        private void LateUpdate() {
            if (Application.isPlaying) {
#if ENABLE_INPUT_SYSTEM
                bool w = Keyboard.current.wKey.isPressed;
                bool a = Keyboard.current.aKey.isPressed;
                bool s = Keyboard.current.sKey.isPressed;
                bool d = Keyboard.current.dKey.isPressed;
#else
                bool w = Input.GetKey(KeyCode.W);
                bool a = Input.GetKey(KeyCode.A);
                bool s = Input.GetKey(KeyCode.S);
                bool d = Input.GetKey(KeyCode.D);
#endif

                float inputX = a ? -1 : d ? 1 : 0;
                float inputY = s ? -1 : w ? 1 : 0;

                _angles.x = MathUtility.ClampInnerAngle(_angles.x + inputY * _sensitivityY, -90, 90);
                _angles.y = MathUtility.NormalizeAngle(_angles.y + inputX * _sensitivityX);
            }
            
            transform.eulerAngles = _angles;
            transform.position = _follow.position - transform.forward * _distance;
        }
    }
}
