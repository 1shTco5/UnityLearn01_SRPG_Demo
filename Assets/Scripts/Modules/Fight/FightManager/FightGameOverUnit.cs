using UnityEngine;

///<summary>
///战斗结束
///</summary>
public class FightGameOverUnit : FightUnitBase
{
    public override void Init()
    {
        base.Init();

        GameApp.CommandManager.Clear(); //清除所有指令

        if (GameApp.FightManager.heros.Count == 0)
        {
            GameApp.CommandManager.AddCommand(
                new WaitCommand(
                    1.25f,
                    delegate()
                    {
                        GameApp.ViewManager.Open(ViewType.LossView);
                    }
                )
            );
        }
        else if (GameApp.FightManager.enemies.Count == 0)
        {
            GameApp.CommandManager.AddCommand(
                new WaitCommand(
                    1.25f,
                    delegate()
                    {
                        GameApp.ViewManager.Open(ViewType.WinView);
                    }
                )
            );
        } else {

        }
    }

    public override bool Update(float dt)
    {
        return true;
    }
}
