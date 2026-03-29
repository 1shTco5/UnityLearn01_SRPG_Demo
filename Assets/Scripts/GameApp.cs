using UnityEngine;

///<summary>
///统一初始化游戏中的管理器
///</summary>
public class GameApp : Singleton<GameApp>
{
    public static SoundManager SoundManager;
    public static ControllerManager ControllerManager;
    public static ViewManager ViewManager;
    public static ConfigManager ConfigManager;
    public static CameraManager CameraManager;
    public static EventCenter EventCenter;
    public static TimeManager TimeManager;
    public static FightManager FightManager;
    public static MapManager MapManager;
    public static GameDataManager GameDataManager;
    public static UserInputManager UserInputManager;
    public static CommandManager CommandManager;
    public static SkillManager SkillManager;

    public override void Init()
    {
        SoundManager = new();
        ControllerManager = new();
        ViewManager = new();
        ConfigManager = new();
        CameraManager = new();
        EventCenter = new();
        TimeManager = new();
        FightManager = new();
        MapManager = new();
        GameDataManager = new();
        UserInputManager = new();
        CommandManager = new();
        SkillManager = new();
    }

    public override void Update(float dt)
    {
        UserInputManager.Update();
        TimeManager.OnUpdate(dt);
        FightManager.Update(dt);
        CommandManager.Update(dt);
        SkillManager.Update(dt);
    }
}
