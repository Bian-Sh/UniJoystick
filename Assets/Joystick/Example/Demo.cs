using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using zFrame.UI;

namespace zFrame.Example
{
    public class Demo : MonoBehaviour
    {
        public Joystick joystick;
        Text text;
        void Start()
        {
            text = GetComponent<Text>();
            joystick.OnValueChanged.AddListener(v=> text.text = string.Format("Horizontal ：{0} \nVertical：{1}",v.x,v.y));
        }
    }
}
