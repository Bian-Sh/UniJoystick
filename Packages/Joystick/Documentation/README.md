# UniJoystick

这是一个代码极简但功能相对完善的基于UGUI的摇杆（Joystick）组件。

![](./Images/joystick.png)

# Summary

1. 支持设置摇杆半径范围
2. 支持指示器
3. 支持配置哪一个轴向使能。
4. 支持一键将摇杆设置为动态底座摇杆或者静态底座摇杆
5. 事件驱动，使用UnityEvent 使得事件可以在面板上挂载
6. 本仓库配备了4个摇杆控制解决方案（Demo，仅供参考）：
   * 第一人称 Charactor 控制
   * 第一人称 character 控制，带TouchPad版本
   * 第三人称 Charactor 控制
   * 常规Transform.Translate控制
7. 适配 Canvas 的 三种 RenderMode。
8. 鼠标和Touch拖拽前需要检测是否打到 UI 组件，这里有[正确姿势](https://github.com/Bian-Sh/UniJoystick/blob/master/Assets/Joystick/Scripts/InputExtension.cs)

# Installation

Install via git URL

You can add `https://github.com/Bian-Sh/UniJoystick.git?path=Packages/Joystick` to Package Manager

If you want to set a target version, uses the tag so you can specify a version like #2.1.0. For example `https://github.com/Bian-Sh/UniJoystick.git?path=Packages/Joystick#1.0.0`

> 1. Requires a version of unity that supports path query parameter for git packages (Unity >= 2019.3.4f1, Unity >= 2020.1a21).
> 
> 2. There is a high chance of failure for users in China.
> 
> 3. Package development needs to be done in the Package folder，perhaps for avoid GUID conflicts。

# How to use?

![](./Images/addjoystick.gif)

1. Hierarchy 右键选择 “UI/Joystick” 生成摇杆（按需更换 UI 素材）。
2. 在关注摇杆数据的地方获取摇杆实例并注册 OnValueChanged 事件
3. 点击运行即可。

> 示例代码

```csharp
    public float speed = 5;
    [SerializeField] Joystick joystick;
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
```

# 功能演示

> 无论静态 or 动态摇杆，都支持 Canvas 的所有 RenderMode

![](./Images/CanvasRenderMode.gif)

> 指向器挂载即可使用，不想用销毁即可（有些同学反映指向器用不到，所以剥离并作为可选挂件提供给大家）。

![](./Images/DirectionArrow.gif)

> 第一人称解决方案示例

![](./Images/FirstPersonSolution.gif)

> 第三人称解决方案示例

![](./Images/ThirdPersonSolution.gif)

> 第一人称解决方案示例 (TouchPad版)

![](./Images/FirstPersonSolution-touchpad.gif)

视频中演示了如下内容：

1. 摇杆驱动人物移动
2. TouchPad 旋转视角
3. TouchPad 的灵敏度调节
4. TouchPad UI 检测，UI上开始的拖拽不触发旋转 ，UI的点击操作不会误触视野旋转功能
5. TouchPad 测试了响应多个手指的体验，还算正常

# 结语

因为使用 IpointerXXXHandler 这套事件接口，淡化了 Touch 和 Input API ，所以：

1. 这个摇杆理论上是支持各种各样的多点触控设备的
2. 支持同屏多个摇杆实例且互不干扰。

ps ：在 Microsoft's Surface Pro 测试OK。

> My Blog

[[Unity 3d] 使用UGUI做一个类似王者荣耀的摇杆 - 简书]( https://www.jianshu.com/p/2b2cdccafef4)
