using UnityEngine;
using UnityEngine.UI;

public class FightOptionDesView : BaseView
{
    protected override void OnStart()
    {
        base.OnStart();
        Find<Button>("bg/turnBtn").onClick.AddListener(OnChangeEnemyTurnBtn);
        Find<Button>("bg/gameOverBtn").onClick.AddListener(OnGameOverBtn);
        Find<Button>("bg/cancelBtn").onClick.AddListener(OnCancelBtn);
    }

    //结束本局游戏
    private void OnGameOverBtn()
    {
        GameApp.FightManager.Clear();
        GameApp.ViewManager.CloseAll();

        LoadModel load = new()
        {
            sceneName = "MapScene",
            callback = delegate()
            {
                GameApp.SoundManager.PlayBGM("mapbgm");
                GameApp.ViewManager.Open(ViewType.SelectLevelView);
            },
        };

        Controller.ApplyControllerFunc(ControllerType.Load, Defines.LoadScene, load);
    }

    //回合结束 切换到敌人回合
    private void OnChangeEnemyTurnBtn()
    {
        GameApp.ViewManager.Close(ViewID);

        GameApp.FightManager.ChangeState(GameState.Enemy);
    }

    //取消
    private void OnCancelBtn()
    {
        GameApp.ViewManager.Close(ViewID);
    }
}
