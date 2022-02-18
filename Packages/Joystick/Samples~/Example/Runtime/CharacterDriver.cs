// Copyright (c) Bian Shanghai
// https://github.com/Bian-Sh/UniJoystick
// Licensed under the MIT license. See the LICENSE.md file in the project root for more information.
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class CharacterDriver : MonoBehaviour
{
    #region Character Move /角色移动
    CharacterController controller;
    public float speed = 5;
    #endregion

    #region View Rotate/视角旋转
    [Header("Character Left Right Rotate Setting:")]
    public float rotateSensitivity = 100;    //方向灵敏度  
    public Slider rotateSensitivitySlider;

    [Header("Camera Up Down Rotate Setting:")]
    public Camera m_Camera;
    public float viewSensitivity = 100;    //上下最大视角条件灵敏度  
    public float upLimite = -45;  // 上仰角度限制
    public float dnLimite = 60;   //  俯视角度限制
    public Slider viewSensitivitySlider;
    #endregion

    private void OnEnable()
    {
        controller = GetComponent<CharacterController>();

        viewSensitivitySlider.value = viewSensitivity;
        viewSensitivitySlider.onValueChanged.AddListener(v=>viewSensitivity=v);

        rotateSensitivitySlider.value = rotateSensitivity;
        rotateSensitivitySlider.onValueChanged.AddListener(v => rotateSensitivity = v);
    }

    /// <summary>
    /// Drive the character to move 
    /// 驱动角色移动
    /// </summary>
    /// <param name="v"></param>
    public void Move(Vector2 v)
    {
        Vector3 direction = transform.TransformDirection(new Vector3(v.x, 0, v.y));
        controller.SimpleMove(direction * speed);
    }

    /// <summary>
    ///  Rotate character and  view 
    ///  旋转角色极其上下视角
    /// </summary>
    /// <param name="v"></param>
    public void Rotate(Vector2 v) 
    {
        if (v.x != 0)
        {
            transform.Rotate(Vector3.up * v.x * rotateSensitivity * Time.deltaTime, Space.Self);
        }
        if (v.y != 0)
        {
            var current = m_Camera.transform.localEulerAngles.x;
            if (current >= 180) current -= 360;
            if ((current > -45 || v.y < 0) && (current < 60 || v.y > 0))
            {
                m_Camera.transform.Rotate(Vector3.right * -v.y * viewSensitivity * Time.deltaTime, Space.Self);
            }
        }
    }

    private void Update()
    {
        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");
        if (v!=0||h!=0)
        {
            Move(new Vector2(h,v));
        }
    }
}
