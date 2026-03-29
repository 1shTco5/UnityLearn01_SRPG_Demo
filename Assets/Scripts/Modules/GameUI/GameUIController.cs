using UnityEngine;

///<summary>
///处理通用UI的控制器 (设置面板 提示面板 开始面板等在这个控制器注册)
///</summary>
public class GameUIController : BaseController
{
    public GameUIController()
        : base()
    {
        //注册视图
        GameApp.ViewManager.Register(
            ViewType.BeginView,
            new ViewInfo()
            {
                prefabName = "BeginView",
                controller = this,
                parentTf = GameApp.ViewManager.canvasTf,
                sortingOrder = 1,
            }
        );

        GameApp.ViewManager.Register(
            ViewType.SettingsView,
            new ViewInfo()
            {
                prefabName = "SettingsView",
                controller = this,
                parentTf = GameApp.ViewManager.canvasTf,
                sortingOrder = 2,
            }
        );

        GameApp.ViewManager.Register(
            ViewType.MessageView,
            new ViewInfo()
            {
                prefabName = "MessageView",
                controller = this,
                parentTf = GameApp.ViewManager.canvasTf,
                sortingOrder = 999,
            }
        );

        InitModuleEvent(); //初始化模板事件
        InitGlobalEvent(); //初始化全局事件
    }

    public override void InitModuleEvent()
    {
        RegisterFunc(Defines.OpenBeginView, OpenBeginView); //注册打开开始面板事件
        RegisterFunc(Defines.OpenSettingsView, OpenSettingsView);
        RegisterFunc(Defines.OpenMessageView, OpenMessageView);
    }

    private void OpenBeginView(object[] args)
    {
        GameApp.ViewManager.Open(ViewType.BeginView, args);
    }

    private void OpenSettingsView(object[] args)
    {
        GameApp.ViewManager.Open(ViewType.SettingsView, args);
    }

    private void OpenMessageView(object[] args)
    {
        GameApp.ViewManager.Open(ViewType.MessageView, args);
    }
}
