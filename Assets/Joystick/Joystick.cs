using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace zFrame.UI
{
    /// <summary>
    /// 简易而不简单的摇杆组件
    /// </summary>
    public class Joystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        public float maxRadius = 100; //Handle 移动最大半径
        [SerializeField, EnumFlags]
        private Direction activatedAxis = (Direction)(-1);
        [SerializeField]
        private bool showDirection = true;
        public JoystickEvent OnValueChanged = new JoystickEvent(); //事件
        public bool IsDraging { get; private set; }

        private RectTransform backGround, handle, direction; //摇杆背景、摇杆手柄、方向指引
        private Vector2 joysticValue = Vector2.zero;

        [System.Serializable] public class JoystickEvent : UnityEvent<Vector2> { }
        [System.Flags]
        public enum Direction
        {
            Horizontal = 1 << 0,
            Vertical = 1 << 1
        }

        private void Awake()
        {
            backGround = transform.Find("BackGround") as RectTransform;
            handle = transform.Find("BackGround/Handle") as RectTransform;
            direction = transform.Find("BackGround/Direction") as RectTransform;
            direction.gameObject.SetActive(false);
        }

        void Update()
        {
            if (IsDraging) //摇杆拖拽进行时驱动事件
            {
                joysticValue.x = handle.anchoredPosition.x / maxRadius;
                joysticValue.y = handle.anchoredPosition.y / maxRadius;
                OnValueChanged.Invoke(joysticValue);
            }
        }

        /// <summary>
        /// 显示摇杆底盘
        /// </summary>
        /// <param name="eventData"></param>
        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            Vector3 backGroundPos = new Vector3() // As it is too long for trinocular operation so I create Vector3 like this.
            {
                x = eventData.position.x,
                y = eventData.position.y,
                z = (null == eventData.pressEventCamera) ? backGround.position.z :
                eventData.pressEventCamera.WorldToScreenPoint(backGround.position).z //无奈，这个坐标转换不得不做啊,就算来来回回的折腾。
            };
            backGround.position = (null == eventData.pressEventCamera) ? backGroundPos : eventData.pressEventCamera.ScreenToWorldPoint(backGroundPos);
            IsDraging = true;
        }

        /// <summary>
        /// 当鼠标拖拽时
        /// </summary>
        /// <param name="eventData"></param>
        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            Vector2 backGroundPos = (null == eventData.pressEventCamera) ?
                backGround.position : eventData.pressEventCamera.WorldToScreenPoint(backGround.position);
            Vector2 direction = eventData.position - backGroundPos; //得到方位盘中心指向光标的向量
            float radius = Mathf.Clamp(Vector3.Magnitude(direction), 0, maxRadius); //获取并锁定向量的长度 以控制 Handle 半径
            Vector2 localPosition = new Vector2()
            {
                x = (0 != (activatedAxis & Direction.Horizontal)) ? (direction.normalized * radius).x : 0,
                y = (0 != (activatedAxis & Direction.Vertical)) ? (direction.normalized * radius).y : 0
            };
            handle.localPosition = localPosition; //更新 Handle 位置
            UpdateDirectionArrow(localPosition);
        }

        /// <summary>
        /// 更新指向器的朝向
        /// </summary>
        private void UpdateDirectionArrow(Vector2 position)
        {
            if (showDirection && position.magnitude != 0)
            {
                direction.gameObject.SetActive(true);
                direction.localEulerAngles = new Vector3(0, 0, Vector2.Angle(Vector2.right, position) * (position.y > 0 ? 1 : -1));
            }
        }

        /// <summary>
        /// 当鼠标停止拖拽时
        /// </summary>
        /// <param name="eventData"></param>
        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            direction.gameObject.SetActive(false);
            backGround.localPosition = Vector3.zero;
            handle.localPosition = Vector3.zero;
            IsDraging = false;
        }
    }
}
