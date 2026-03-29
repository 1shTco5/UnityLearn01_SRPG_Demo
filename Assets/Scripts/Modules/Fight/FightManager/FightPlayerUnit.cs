using UnityEngine;

//玩家的回合
public class FightPlayerUnit : FightUnitBase
{
    public override void Init()
    {
        base.Init();

        GameApp.FightManager.ResetEnemies();
        GameApp.ViewManager.Open(ViewType.TipView, "玩家回合");
    }
}
