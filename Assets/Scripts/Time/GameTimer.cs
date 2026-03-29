using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameTimer
{
    private List<GameTimerData> timers; //存储计时器数据集合

    public GameTimer()
    {
        timers = new();
    }

    public void Register(float time, UnityAction callback)
    {
        timers.Add(new(time, callback));
    }

    public void OnUpdate(float dt)
    {
        //反向遍历
        for (int i = timers.Count - 1; i >= 0; i--)
        {
            if (timers[i].OnUpdate(dt))
            {
                timers.RemoveAt(i);
            }
        }
    }

    //打断计时器
    public void Clear()
    {
        timers.Clear();
    }

    public int Count()
    {
        return timers.Count;
    }
}
