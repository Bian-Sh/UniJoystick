namespace zFrame.UI
{
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.Events;
    public class Joystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        public float maxRadius = 100; //Handle 移动最大半径
        [SerializeField, EnumFlags]
        Direction activatedAxis = (Direction)(-1); //选择激活的轴向
        [SerializeField]
        bool showDirection = true;
        public JoystickEvent OnValueChanged = new JoystickEvent(); //Update驱动的事件
        public bool IsDraging { get { return fingerId != int.MinValue; } } //可供外部断言当前摇杆状态

        private RectTransform backGround, handle, direction; //摇杆背景、摇杆手柄、方向指引
        private Vector2 joystickValue = Vector2.zero;
        private Vector3 backGroundOriginLocalPostion;
        private int fingerId = int.MinValue; //当前触发摇杆的 pointerId ，预设一个永远无法企及的值
        [System.Serializable]
        public class JoystickEvent : UnityEvent<Vector2> { }
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
            backGroundOriginLocalPostion = backGround.localPosition;
        }

        void Update()
        {
            if (!IsDraging) return;//仅当摇杆拖拽时驱动事件
            joystickValue.x = handle.anchoredPosition.x / maxRadius;
            joystickValue.y = handle.anchoredPosition.y / maxRadius;
            OnValueChanged.Invoke(joystickValue);
        }

        // 摇杆被触发，初始化摇杆
        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            if (IsDraging) return;
            fingerId = eventData.pointerId;
            Vector3 backGroundPos = new Vector3() // As it is too long for trinocular operation so I create Vector3 like this.
            {
                x = eventData.position.x,
                y = eventData.position.y,
                z = (null == eventData.pressEventCamera) ? backGround.position.z :
                 eventData.pressEventCamera.WorldToScreenPoint(backGround.position).z //无奈，这个坐标转换不得不做啊,就算来来回回的折腾。
            };
            backGround.position = (null == eventData.pressEventCamera) ? backGroundPos : eventData.pressEventCamera.ScreenToWorldPoint(backGroundPos);
        }

        // 当鼠标拖拽时
        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            if (fingerId != eventData.pointerId) return;
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

        // 更新指向器的朝向
        private void UpdateDirectionArrow(Vector2 position)
        {
            if (showDirection && position.magnitude != 0)
            {
                direction.gameObject.SetActive(true);
                direction.localEulerAngles = new Vector3(0, 0, Vector2.Angle(Vector2.right, position) * (position.y > 0 ? 1 : -1));
            }
        }

        // 当鼠标停止拖拽时,重置数据
        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            if (fingerId != eventData.pointerId) return;
            fingerId = int.MinValue;
            direction.gameObject.SetActive(false);
            backGround.localPosition = backGroundOriginLocalPostion;
            handle.localPosition = Vector3.zero;
        }
    }
}
