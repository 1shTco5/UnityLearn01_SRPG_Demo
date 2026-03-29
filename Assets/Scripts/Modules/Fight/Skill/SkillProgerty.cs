using System.Collections.Generic;
using UnityEngine;

//技能属性
public class SkillProgerty
{
    public int id;
    public string name;
    public int atk;
    public int atkCount;
    public int atkRange;
    public int target;
    public int targetType;
    public string soundEffect;
    public string aniName;
    public float time; //技能的持续时长
    public float atkTime; //检测攻击的时间
    public string atkEffect;

    public SkillProgerty(int id)
    {
        Dictionary<string, string> info = GameApp
            .ConfigManager.GetConfigData("skill")
            .GetDataByID(id);

        id = int.Parse(info["Id"]);
        name = info["Name"];
        atk = int.Parse(info["Atk"]);
        atkCount = int.Parse(info["AtkCount"]);
        atkRange = int.Parse(info["Range"]);
        target = int.Parse(info["Target"]);
        targetType = int.Parse(info["TargetType"]);
        soundEffect = info["Sound"];
        aniName = info["AniName"];
        time = float.Parse(info["Time"]) * 0.001f; //毫秒 ms
        atkTime = float.Parse(info["AttackTime"]) * 0.001f;
        atkEffect = info["AttackEffect"];
    }
}
