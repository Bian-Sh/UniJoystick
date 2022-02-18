// Copyright (c) Bian Shanghai
// https://github.com/Bian-Sh/UniJoystick
// Licensed under the MIT license. See the LICENSE.md file in the project root for more information.
namespace zFrame.Example
{
    using UnityEngine;
    using zFrame.UI;
    public class TransformMoveSolution : MonoBehaviour
    {
        public Joystick joystick;
        public float speed = 5;
        void Start()
        {
            joystick.OnValueChanged.AddListener(v =>
            {
                if (v.magnitude != 0)
                {
                    transform.Translate(v.x * speed * Time.deltaTime, 0, v.y * speed * Time.deltaTime, Space.World);
                    transform.rotation = Quaternion.LookRotation(new Vector3(v.x, 0, v.y));
                }
            });
        }
    }
}
