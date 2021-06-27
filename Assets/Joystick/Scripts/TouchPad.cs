using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static InputExtension;

public class TouchPad : MonoBehaviour
{
    public TouchPadEvent OnTouchPadValueChanged = new TouchPadEvent();
    [System.Serializable]
    public class TouchPadEvent : UnityEvent<Vector2> { }

    //记录的那些不在UI上触发的点 ，matian
    private List<Touch> fingerIds = new List<Touch>();

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            foreach (var item in Input.touches)
            {
                int index = fingerIds.FindIndex(touch => touch.fingerId == item.fingerId);
                if (index == -1)
                {
                    //2. 收集有效 Touch ，其特点为：刚按下 + 没打到UI上 + 在屏幕右侧
                    if (item.phase == TouchPhase.Began && item.position.x > Screen.width * 0.5f && !item.IsRaycastUI())
                    {
                        fingerIds.Add(item);
                    }
                }
                else
                {
                    if (item.phase == TouchPhase.Ended)
                    {
                        fingerIds.RemoveAt(index); //3. 如果此Touch 已失效（unity 会回传 phase = end 的 touch），则剔除之
                    }
                    else
                    {
                        fingerIds[index] = item; //4. 由于Touch是  非引用类型的临时变量，所以要主动更新之
                    }
                }
            }

            //5. 有效Touch 处于 move 则可以驱动事件了：
            foreach (var item in fingerIds)
            {
                if (item.phase == TouchPhase.Moved)
                {
                    OnTouchPadValueChanged.Invoke(item.deltaPosition);
                }
            }
        }

#if UNITY_EDITOR
        // 仅供编辑器测试，如果不从 TouchPad 移除，由于存在 mouse simulate 功能，会紊乱 Touch.IsRaycastUI 检测逻辑
        //For editor testing only. If it is not removed from the TouchPad, the detection logic of Touch.IsRaycastUI will be disrupted due to the mouse simulate function
        var h = Input.GetAxis("Mouse X");
        var v = Input.GetAxis("Mouse Y");
        OnTouchPadValueChanged.Invoke(new Vector2(h, v));
#endif
    }
}
