using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadController : BaseController
{
    private AsyncOperation asyncOperation;

    public LoadController()
        : base()
    {
        GameApp.ViewManager.Register(
            ViewType.LoadView,
            new ViewInfo()
            {
                prefabName = "LoadView",
                controller = this,
                parentTf = GameApp.ViewManager.canvasTf,
            }
        );

        InitModuleEvent();
    }

    public override void InitModuleEvent()
    {
        RegisterFunc(Defines.LoadScene, LoadScene);
    }

    //加载场景回调
    private void LoadScene(params object[] args)
    {
        LoadModel loadModel = args[0] as LoadModel;

        SetModel(loadModel);

        //打开加载界面
        GameApp.ViewManager.Open(ViewType.LoadView);

        //加载场景
        asyncOperation = SceneManager.LoadSceneAsync(loadModel.sceneName);
        asyncOperation.completed += OnLoadEndCallback;
    }

    //加载完成后回调
    private void OnLoadEndCallback(AsyncOperation op)
    {
        asyncOperation.completed -= OnLoadEndCallback;

        //延迟一些
        GameApp.TimeManager.Register(
            1f,
            delegate()
            {
                GetModel<LoadModel>().callback?.Invoke(); //执行回调

                GameApp.ViewManager.Close(ViewType.LoadView); //关闭加载界面
            }
        );
    }
}
