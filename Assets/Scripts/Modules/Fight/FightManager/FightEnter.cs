using UnityEngine;

//进入战斗需要处理的逻辑
public class FightEnter : FightUnitBase
{
    public override void Init() {
        GameApp.MapManager.Init();

        //进入战斗
        GameApp.FightManager.EnterFight();
    }
}
