using UnityEngine;
using UnityEngine.UI;

public class LossView : BaseView
{
    protected override void OnStart()
    {
        base.OnStart();

        Find<Button>("okBtn").onClick.AddListener(OnOkBtn);
    }

    private void OnOkBtn()
    {
        //卸载战斗中的资源
        GameApp.FightManager.Clear();
        GameApp.ViewManager.CloseAll();

        //切换场景
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
}
