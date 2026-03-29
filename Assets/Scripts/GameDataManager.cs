using System.Collections.Generic;
using UnityEngine;

//游戏数据管理器 (存储玩家基本游戏数据)
public class GameDataManager
{
    public List<int> heros; //英雄集合

    public int money;

    public GameDataManager()
    {
        heros = new List<int>();

        //英雄ID
        heros.Add(10001);
        heros.Add(10002);
        heros.Add(10003);
    }
}
