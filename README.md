# UniJoystick
这是一个代码极简但功能相对完善的基于UGUI的摇杆（Joystick）组件。

![](DocAssets/Interface.png)

# Summary
1. 支持设置摇杆半径范围
2. 支持挂载指示器（指示器已经剥离成独立可选挂件）
3. 支持配置哪一个轴向使能。
4. 支持一键将摇杆设置为动态底座摇杆或者静态底座摇杆
5. 事件驱动，使用UnityEvent 使得事件可以在面板上挂载
6. 本仓库配备了3个摇杆控制解决方案（Demo，仅供参考）：
    * 第一人称 Charactor 控制
    * 第三人称 Charactor 控制
    * 常规Transform.Translate控制
7. 适配 Canvas 的 三种 RenderMode。
8. 几乎每句代码都热情洋溢充满注释。

# How to use?

1. 拖入 Joystick 预制体到 Canvas 中（按需更换 UI 素材）。
2. 再关注摇杆数据的地方获取摇杆实例并注册 OnValueChanged 事件
3. 点击运行即可。

> 示例代码
```
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

![](DocAssets/CanvasRenderMode.gif)

> 指向器挂载即可使用，不想用销毁即可（有些同学反映指向器用不到，所以剥离并作为可选挂件提供给大家）。

![](DocAssets/DirectionArrow.gif)

> 第一人称解决方案示例

![](DocAssets/FirstPersonSolution.gif)

> 第三人称解决方案示例

![](DocAssets/ThirdPersonSolution.gif)

# 结语

因为使用 IpointerXXXHandler 这套事件接口，淡化了 Touch 和 Input API ，所以：

1. 这个摇杆理论上是支持各种各样的多点触控设备的
2. 支持同屏多个摇杆实例且互不干扰。

ps ：在 Microsoft's Surface Pro 测试OK。

> My Blog

[[Unity 3d] 使用UGUI做一个类似王者荣耀的摇杆 - 简书]( https://www.jianshu.com/p/2b2cdccafef4)
