using System.Collections.Generic;
using UnityEngine;

public class OptionData
{
    public int id;
    public string eventName;
    public string name;
}

///<summary>
///战斗用的数据
///</summary>
public class FightModel : BaseModel
{
    public List<OptionData> options;
    public ConfigData optionConfig;

    public FightModel(BaseController ctl)
        : base(ctl)
    {
        options = new();
    }

    public override void Init()
    {
        optionConfig = GameApp.ConfigManager.GetConfigData("option");
        foreach (var item in optionConfig.GetLines())
        {
            OptionData option = new();
            option.id = int.Parse(item.Value["Id"]);
            option.name = item.Value["Name"];
            option.eventName = item.Value["EventName"];
            options.Add(option);
        }
    }
}
