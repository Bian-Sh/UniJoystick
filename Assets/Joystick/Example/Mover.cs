using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using zFrame.UI;

public class Mover : MonoBehaviour
{
    [SerializeField] Joystick joystick;
    public float speed = 5;
    void Start()
    {
        joystick.OnValueChanged.AddListener(v =>
        {
            if (v.magnitude != 0)
            {
                transform.Translate(v.x * speed, 0, v.y * speed, Space.World);
                transform.rotation = Quaternion.LookRotation(new Vector3(v.x, 0, v.y));
            }
        });
    }
}
