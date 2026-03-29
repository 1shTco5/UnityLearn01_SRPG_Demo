using System.Collections.Generic;
using UnityEngine;

public class LevelData
{
    public int id;
    public string name;
    public string sceneName;
    public string description;
    public bool isFinished; //是否通关

    public LevelData(Dictionary<string, string> data)
    {
        id = int.Parse(data["Id"]);
        name = data["Name"];
        sceneName = data["SceneName"];
        description = data["Des"];
        isFinished = false;
    }
}

///关卡数据
public class LevelModel : BaseModel
{
    private ConfigData levelConfig;
    Dictionary<int, LevelData> levels; //关卡字典
    public LevelData curr; //当前关卡

    public LevelModel()
    {
        levels = new();
    }

    public override void Init()
    {
        levelConfig = GameApp.ConfigManager.GetConfigData("level");
        foreach (var item in levelConfig.GetLines())
        {
            LevelData l_data = new(item.Value);
            levels.Add(l_data.id, l_data);
        }
    }

    public LevelData GetLevel(int id)
    {
        return levels[id];
    }
}
