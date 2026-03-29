using System.Collections.Generic;
using UnityEngine;

//管理游戏中所有配置表
public class ConfigManager
{
    private Dictionary<string, ConfigData> configs; //已经加载的配置表
    private Dictionary<string, ConfigData> toLoadList; //待加载的配置表

    public ConfigManager()
    {
        configs = new();
        toLoadList = new();
    }

    //注册待加载的配置表
    public void Register(string file, ConfigData config)
    {
        toLoadList[file] = config;
    }

    //加载所有配置表
    public void LoadAllConfigs()
    {
        foreach (var item in toLoadList)
        {
            TextAsset textAsset = item.Value.LoadFile();
            item.Value.Load(textAsset.text);
            configs.Add(item.Key, item.Value);
        }
        toLoadList.Clear();
    }

    public ConfigData GetConfigData(string file)
    {
        if (configs.ContainsKey(file))
        {
            return configs[file];
        }
        return null;
    }
}
