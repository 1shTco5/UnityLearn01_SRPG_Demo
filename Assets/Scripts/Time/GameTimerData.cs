using UnityEngine;
using UnityEngine.Events;

public class GameTimerData
{
    private float time; //计时时长
    private UnityAction callback; //到达时长回调

    public GameTimerData(float timer, UnityAction callback)
    {
        this.time = timer;
        this.callback = callback;
    }

    public bool OnUpdate(float dt)
    {
        time -= dt;
        if (time <= 0)
        {
            callback?.Invoke();
            return true;
        }
        return false;
    }
}
