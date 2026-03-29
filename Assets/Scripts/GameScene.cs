using UnityEngine;

//继承Mono的脚本 需要挂载游戏物体 跳转场景后当前脚本物体不删除
public class GameScene : MonoBehaviour
{
    public Texture2D mouseTxt;
    private float dt;
    private static bool isLoaded = false;

    void Awake()
    {
        if (isLoaded)
        {
            Destroy(gameObject);
        }
        else
        {
            isLoaded = true;
            DontDestroyOnLoad(gameObject);
            GameApp.Instance.Init();
        }
    }

    void Start()
    {
        Cursor.SetCursor(mouseTxt, Vector2.zero, CursorMode.Auto);

        RegisterConfigs();
        GameApp.ConfigManager.LoadAllConfigs();

        GameApp.SoundManager.PlayBGM("login");

        RegisterModule(); //注册控制器
        InitModule();
    }

    void RegisterModule()
    {
        GameApp.ControllerManager.Register(ControllerType.GameUI, new GameUIController());
        GameApp.ControllerManager.Register(ControllerType.Game, new GameController());
        GameApp.ControllerManager.Register(ControllerType.Load, new LoadController());
        GameApp.ControllerManager.Register(ControllerType.Level, new LevelController());
        GameApp.ControllerManager.Register(ControllerType.Fight, new FightController());
    } //注册控制器

    void RegisterConfigs()
    {
        GameApp.ConfigManager.Register("enemy", new ConfigData("enemy"));
        GameApp.ConfigManager.Register("level", new ConfigData("level"));
        GameApp.ConfigManager.Register("option", new ConfigData("option"));
        GameApp.ConfigManager.Register("player", new ConfigData("player"));
        GameApp.ConfigManager.Register("role", new ConfigData("role"));
        GameApp.ConfigManager.Register("skill", new ConfigData("skill"));
    } //注册配置表

    void InitModule()
    {
        GameApp.ControllerManager.InitAllModules();
    } //初始化所有控制器

    void Update()
    {
        dt = Time.deltaTime;
        GameApp.Instance.Update(dt);
    }
}
