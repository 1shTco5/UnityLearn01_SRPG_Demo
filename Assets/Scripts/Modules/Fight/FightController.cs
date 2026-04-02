using UnityEngine;

//战斗控制器 (战斗相关的界面 事件等)
public class FightController : BaseController
{
    public FightController()
        : base()
    {
        SetModel(new FightModel(this)); //设置战斗数据

        GameApp.ViewManager.Register(
            ViewType.FightSelCharView,
            new ViewInfo()
            {
                prefabName = "FightSelCharView",
                controller = this,
                parentTf = GameApp.ViewManager.canvasTf,
                sortingOrder = 1,
            }
        );
        GameApp.ViewManager.Register(
            ViewType.DragHeroView,
            new ViewInfo()
            {
                prefabName = "DragHeroView",
                controller = this,
                parentTf = GameApp.ViewManager.worldCanvasTf, //设置到世界画布
                sortingOrder = 2,
            }
        );
        GameApp.ViewManager.Register(
            ViewType.TipView,
            new ViewInfo()
            {
                prefabName = "TipView",
                controller = this,
                parentTf = GameApp.ViewManager.canvasTf,
                sortingOrder = 2,
            }
        );
        GameApp.ViewManager.Register(
            ViewType.HeroDesView,
            new ViewInfo()
            {
                prefabName = "HeroDesView",
                controller = this,
                parentTf = GameApp.ViewManager.canvasTf,
                sortingOrder = 2,
            }
        );
        GameApp.ViewManager.Register(
            ViewType.EnemyDesView,
            new ViewInfo()
            {
                prefabName = "EnemyDesView",
                controller = this,
                parentTf = GameApp.ViewManager.canvasTf,
                sortingOrder = 2,
            }
        );
        GameApp.ViewManager.Register(
            ViewType.SelectOptionView,
            new ViewInfo()
            {
                prefabName = "SelectOptionView",
                controller = this,
                parentTf = GameApp.ViewManager.canvasTf,
                sortingOrder = 2,
            }
        );
        GameApp.ViewManager.Register(
            ViewType.FightOptionDesView,
            new ViewInfo()
            {
                prefabName = "FightOptionDesView",
                controller = this,
                parentTf = GameApp.ViewManager.canvasTf,
                sortingOrder = 3,
            }
        );
        GameApp.ViewManager.Register(
            ViewType.WinView,
            new ViewInfo()
            {
                prefabName = "WinView",
                controller = this,
                parentTf = GameApp.ViewManager.canvasTf,
                sortingOrder = 3,
            }
        );
        GameApp.ViewManager.Register(
            ViewType.LossView,
            new ViewInfo()
            {
                prefabName = "LossView",
                controller = this,
                parentTf = GameApp.ViewManager.canvasTf,
                sortingOrder = 3,
            }
        );

        InitModuleEvent();
    }

    public override void Init()
    {
        model.Init();
    }

    public override void InitModuleEvent()
    {
        RegisterFunc(Defines.BeginFight, OnBeginFightCallback);
    }

    private void OnBeginFightCallback(params object[] args)
    {
        //进入战斗
        GameApp.FightManager.ChangeState(GameState.Enter);

        // GameApp.ViewManager.Open(ViewType.FightView);
        GameApp.ViewManager.Open(ViewType.FightSelCharView);
    }
}
