using UnityEngine;
using UnityEngine.UI;

///<summary>
///开始界面
///</summary>
public class BeginView : BaseView
{
    protected override void OnAwake()
    {
        Find<Button>("BtnBegin").onClick.AddListener(OnBeginBtn);
        Find<Button>("BtnSettings").onClick.AddListener(OnSettingsBtn);
        Find<Button>("BtnExit").onClick.AddListener(OnExitBtn);
    }

    private void OnBeginBtn()
    {
        //关闭开始界面
        GameApp.ViewManager.Close(ViewID);

        LoadModel loadModel = new()
        {
            sceneName = "MapScene",
            callback = delegate()
            {
                //打开选择关卡界面
                Controller.ApplyControllerFunc(ControllerType.Level, Defines.OpenSelectLevel);
                GameApp.SoundManager.PlayBGM("mapbgm");
            },
        };

        Controller.ApplyControllerFunc(ControllerType.Load, Defines.LoadScene, loadModel);
    }

    private void OnSettingsBtn()
    {
        ApplyFunc(Defines.OpenSettingsView);
    }

    private void OnExitBtn()
    {
        Controller.ApplyControllerFunc(
            ControllerType.GameUI,
            Defines.OpenMessageView,
            new MessageInfo()
            {
                text = "确认退出游戏?",
                okCallback = delegate()
                {
                    Application.Quit();
                },
            }
        );
    }
}
