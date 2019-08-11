using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using zFrame.UI;

public class CharacterControllerMove : MonoBehaviour
{
    [SerializeField] Joystick joystick;
    [SerializeField] Joystick joystick2;
    public float speed = 5;
    CharacterController controller;
    void Start()
    {
        controller = GetComponent<CharacterController>();

        joystick.OnValueChanged.AddListener(v =>
        {
            if (v.magnitude != 0)
            {
                Vector3 direction = transform.TransformDirection(new Vector3(v.x, 0, v.y));
                controller.Move(direction * speed * Time.deltaTime);
            }
        });
        joystick2.OnValueChanged.AddListener(v =>
        {
            if (v.magnitude != 0)
            {
                transform.rotation = Quaternion.LookRotation(new Vector3(v.x, 0, v.y));
            }
        });
    }
}
