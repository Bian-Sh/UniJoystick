// Copyright (c) Bian Shanghai
// https://github.com/Bian-Sh/UniJoystick
// Licensed under the MIT license. See the LICENSE.md file in the project root for more information.
namespace zFrame.UI
{
    using System;
    using UnityEngine;
    public class DirectionArrow : MonoBehaviour
    {
        private Joystick joystick;
        private void Start()
        {
            joystick = GetComponentInParent<Joystick>();
            if (null == joystick)
            {
                throw new InvalidOperationException("The directional arrow is an optional part of the joystick and it relies on the instance of the joystick!");
            }
            joystick.OnPointerUp.AddListener(OnPointerUp);
            joystick.OnValueChanged.AddListener(UpdateDirectionArrow);
            gameObject.SetActive(false);
        }

        void OnDestroy()
        {
            joystick.OnPointerUp.RemoveListener(OnPointerUp);
            joystick.OnValueChanged.RemoveListener(UpdateDirectionArrow);
        }
        // 更新指向器的朝向
        private void UpdateDirectionArrow(Vector2 position)
        {
            if (position.magnitude != 0)
            {
                if (!gameObject.activeSelf)
                {
                    gameObject.SetActive(true);
                }
                transform.localEulerAngles = new Vector3(0, 0, Vector2.Angle(Vector2.right, position) * (position.y > 0 ? 1 : -1));
            }
        }
        void OnPointerUp(Vector2 pos)
        {
            gameObject.SetActive(false);
        }
    }
}
