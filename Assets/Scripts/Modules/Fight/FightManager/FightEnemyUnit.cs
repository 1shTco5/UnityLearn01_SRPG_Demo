using UnityEngine;

///<summary>
///敌人回合
///</summary>
public class FightEnemyUnit : FightUnitBase
{
    public override void Init()
    {
        base.Init();

        GameApp.FightManager.ResetHeros();
        GameApp.ViewManager.Open(ViewType.TipView, "敌方回合");

        GameApp.CommandManager.AddCommand(new WaitCommand(1.25f));

        //敌人AI (移动 使用技能等)
        foreach (var enemy in GameApp.FightManager.enemies)
        {
            GameApp.CommandManager.AddCommand(new WaitCommand(0.25f)); //等下
            GameApp.CommandManager.AddCommand(new AIMoveCommand(enemy)); //移动
            GameApp.CommandManager.AddCommand(new WaitCommand(0.25f)); //等下
            GameApp.CommandManager.AddCommand(new SkillCommand(enemy)); //使用技能
            GameApp.CommandManager.AddCommand(new WaitCommand(0.25f)); //等下
        }

        //等待一段时间 切换回玩家回合
        GameApp.CommandManager.AddCommand(
            new WaitCommand(
                0.25f,
                delegate()
                {
                    GameApp.FightManager.ChangeState(GameState.Player);
                }
            )
        );
    }
}
