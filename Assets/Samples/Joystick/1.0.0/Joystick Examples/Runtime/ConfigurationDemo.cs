// Copyright (c) Bian Shanghai
// https://github.com/Bian-Sh/UniJoystick
// Licensed under the MIT license. See the LICENSE.md file in the project root for more information.
namespace zFrame.Example
{
    using UnityEngine;
    using UnityEngine.UI;
    using zFrame.UI;
    public class ConfigurationDemo : MonoBehaviour
    {
        public Joystick joystick;
        Text text;

#if UNITY_EDITOR
        [SerializeField,Multiline] string  message= "本例\n1. 可以观察到摇杆按下抬起事件的触发\n2. 可见摇杆数据的变化\n3. 通过键盘 R 键切换动态/静态摇杆\n4. 按下键盘 F 键显示/隐藏指向器";
        private void OnGUI()
        {
            GUILayout.Label(message);
        }
#endif

        void Start()
        {
#if UNITY_EDITOR
            joystick.OnPointerDown.AddListener(v => Debug.Log($"{nameof(ConfigurationDemo)}: 摇杆被按下！ "));
            joystick.OnPointerUp.AddListener(v => Debug.Log($"{nameof(ConfigurationDemo)}: 摇杆被释放！ "));
#endif

            text = GetComponent<Text>();
            joystick.OnValueChanged.AddListener(v => text.text = $"Horizontal ：{v.x} \nVertical：{v.y}");
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                joystick.IsDynamic = !joystick.IsDynamic;
                Debug.Log($"{nameof(ConfigurationDemo)}: {(joystick.IsDynamic ? "切换为动态摇杆" : "切换成静态摇杆")}");
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                joystick.ShowDirectionArrow = !joystick.ShowDirectionArrow;
                Debug.Log($"{nameof(ConfigurationDemo)}: {(joystick.ShowDirectionArrow ? "展示指向器" : "隐藏指向器")}");
            }
        }
    }
}
