using UnityEngine;
using UnityEngine.Events;

///<summary>
///等待指令
///</summary>
public class WaitCommand : BaseCommand
{
    private float time;
    private UnityAction callback;

    public WaitCommand(float time, UnityAction callback = null)
    {
        this.time = time;
        this.callback = callback;
    }

    public override bool Update(float dt)
    {
        this.time -= dt;
        if (this.time <= 0)
        {
            callback?.Invoke();
            return true;
        }
        return false;
    }
}
