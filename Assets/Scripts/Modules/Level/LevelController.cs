using UnityEngine;

//关卡控制器
public class LevelController : BaseController
{
    public LevelController() : base()
    {
        SetModel(new LevelModel());

        GameApp.ViewManager.Register(
            ViewType.SelectLevelView,
            new ViewInfo()
            {
                prefabName = "SelectLevelView",
                controller = this,
                parentTf = GameApp.ViewManager.canvasTf,
            }
        );

        InitModuleEvent();
        InitGlobalEvent();
    }

    public override void Init()
    {
        model.Init(); //初始化关卡数据
    }

    public override void InitModuleEvent()
    {
        RegisterFunc(Defines.OpenSelectLevel, OnOpenSelectLevelView);
    }

    public override void InitGlobalEvent()
    {
        GameApp.EventCenter.AddEvent(
            Defines.ShowLevelDescriptionEvent,
            OnShowLevelDescriptionCallback
        );
        GameApp.EventCenter.AddEvent(
            Defines.HideLevelDescriptionEvent,
            OnHideLevelDescriptionCallback
        );
    }

    public override void RemoveGlobalEvent()
    {
        GameApp.EventCenter.RemoveEvent(
            Defines.ShowLevelDescriptionEvent,
            OnShowLevelDescriptionCallback
        );
        GameApp.EventCenter.RemoveEvent(
            Defines.HideLevelDescriptionEvent,
            OnHideLevelDescriptionCallback
        );
    }

    private void OnShowLevelDescriptionCallback(object arg)
    {
        LevelModel levelModel = GetModel<LevelModel>();
        levelModel.curr = levelModel.GetLevel(int.Parse(arg.ToString()));

        GameApp
            .ViewManager.GetView<SelectLevelView>(ViewType.SelectLevelView)
            .ShowLevelDescription();
    }

    private void OnHideLevelDescriptionCallback(object arg)
    {
        GameApp
            .ViewManager.GetView<SelectLevelView>(ViewType.SelectLevelView)
            .HideLevelDescription();
    }

    private void OnOpenSelectLevelView(params object[] args)
    {
        GameApp.ViewManager.Open(ViewType.SelectLevelView, args);
    }
}
