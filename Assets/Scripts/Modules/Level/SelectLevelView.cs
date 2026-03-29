using UnityEngine;
using UnityEngine.UI;

//选择关卡界面
public class SelectLevelView : BaseView
{
    protected override void OnAwake()
    {
        Find<Button>("close").onClick.AddListener(OnCloseBtn);
        Find<Button>("level/fightBtn").onClick.AddListener(OnFightBtn);

        HideLevelDescription();
    }

    private void OnCloseBtn()
    {
        GameApp.ViewManager.Close(ViewID);

        LoadModel loadModel = new()
        {
            sceneName = "GameScene",
            callback = delegate()
            {
                //打开开始界面
                Controller.ApplyControllerFunc(ControllerType.GameUI, Defines.OpenBeginView);
            },
        };

        Controller.ApplyControllerFunc(ControllerType.Load, Defines.LoadScene, loadModel);
    }

    public void ShowLevelDescription()
    {
        Find("level").SetActive(true);
        LevelData curr = Controller.GetModel<LevelModel>().curr;
        Find<Text>("level/name/txt").text = curr.name;
        Find<Text>("level/des/txt").text = curr.description;
    }

    public void HideLevelDescription()
    {
        Find("level").SetActive(false);
    }

    //切换到战斗场景
    private void OnFightBtn()
    {
        //关闭当前界面
        GameApp.ViewManager.Close(ViewID);
        GameApp.CameraManager.ResetPosition();

        LoadModel loadModel = new()
        {
            sceneName = Controller.GetModel<LevelModel>().curr.sceneName, //跳转到当前选择的关卡
            callback = delegate()
            {
                //加载成功后显示战斗界面等
                Controller.ApplyControllerFunc(ControllerType.Fight, Defines.BeginFight);
                GameApp.SoundManager.PlayBGM("fightbgm");
            },
        };

        Controller.ApplyControllerFunc(ControllerType.Load, Defines.LoadScene, loadModel);
    }
}
