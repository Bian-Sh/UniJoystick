// Copyright (c) Bian Shanghai
// https://github.com/Bian-Sh/UniJoystick
// Licensed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace zFrame.Example
{
    using zFrame.UI;
    using UnityEngine;
    public class FirstPersonSolution : MonoBehaviour
    {
        [SerializeField] Joystick joystick;
        [SerializeField] Joystick joystick2;
        public float speed = 5;
        CharacterController controller;

        #region 视野旋转
        [SerializeField] Camera m_Camera;
        //方向灵敏度  
        public float rotateRange = 120;
        //上下最大视角(Y视角)  
        public float viewRange = 60F;
        float ry = 0, rx = 0;

        #endregion

        void Start()
        {
            controller = GetComponent<CharacterController>();

            joystick.OnValueChanged.AddListener(v =>
            {
                if (v.magnitude != 0)
                {
                    Vector3 direction = transform.TransformDirection(new Vector3(v.x, 0, v.y));
                    controller.SimpleMove(direction * speed);
                }
            });

            joystick2.OnPointerUp.AddListener(v =>
            {
                ry = transform.localEulerAngles.y;
                rx = m_Camera.transform.localEulerAngles.y;
            });
            joystick2.OnValueChanged.AddListener(v =>
            {
                if (v.magnitude != 0)
                {
                    float rotationy = ry + v.x * rotateRange;
                    float rotationx = Mathf.Clamp(rx + v.y * viewRange, -45, 60);
                    m_Camera.transform.localEulerAngles = new Vector3(-rotationx, 0, 0);
                    transform.localEulerAngles = new Vector3(0, rotationy, 0);
                }
            });
        }
    }
}
