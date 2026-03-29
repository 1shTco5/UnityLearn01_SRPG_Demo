using UnityEngine;
using UnityEngine.Events;

///<summary>
///全局时间计时器管理器
///</summary>
public class TimeManager
{
    GameTimer timer;

    public TimeManager()
    {
        timer = new();
    }

    public void Register(float time, UnityAction callback)
    {
        timer.Register(time, callback);
    }

    public void OnUpdate(float dt)
    {
        timer.OnUpdate(dt);
    }
}
