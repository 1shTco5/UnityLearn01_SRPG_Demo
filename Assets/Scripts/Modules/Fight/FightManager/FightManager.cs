using System.Collections.Generic;
using UnityEngine;

//战斗中的状态枚举
public enum GameState
{
    Idle,
    Enter,
    Player,
    Enemy,
    GameOver,
}

///<summary>
///战斗管理器 (用于管理战斗相关的实体(敌人 英雄 地图 格子))
///</summary>
public class FightManager
{
    public GameState state = GameState.Idle;

    private FightUnitBase curr; //当前所处的战斗单元

    public List<Hero> heros;

    public List<Enemy> enemies;

    public int round; //回合数

    public FightUnitBase Curr
    {
        get { return curr; }
    }

    public FightManager()
    {
        heros = new();
        enemies = new();
        ChangeState(GameState.Idle);
    }

    public void Update(float dt)
    {
        if (curr != null && curr.Update(dt))
        {
            //to do
        }
        else
        {
            curr = null;
        }
    }

    //切换战斗状态
    public void ChangeState(GameState state)
    {
        FightUnitBase _curr = curr;
        this.state = state;
        switch (this.state)
        {
            case GameState.Idle:
                _curr = new FightIdle();
                break;
            case GameState.Enter:
                _curr = new FightEnter();
                break;
            case GameState.Player:
                _curr = new FightPlayerUnit();
                break;
            case GameState.Enemy:
                _curr = new FightEnemyUnit();
                break;
            case GameState.GameOver:
                _curr = new FightGameOverUnit();
                break;
        }
        _curr.Init();
    }

    //进入战斗 初始化一些信息 (敌人信息, 回合数等)
    public void EnterFight()
    {
        round = 1;
        enemies = new();
        heros = new();
        //将场景中的敌人脚本进行存储
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Enemy"); //给敌人添加Eneny标签
        for (int i = 0; i < objs.Length; i++)
        {
            Enemy enemy = objs[i].GetComponent<Enemy>();
            //将敌人位置设置为障碍物 表示位置被占了
            GameApp.MapManager.ChangeBlockType(enemy.rowIndex, enemy.colIndex, BlockType.Obstacle);
            enemies.Add(enemy);
        }
    }

    //生成英雄
    public void SpawnHero(Block b, Dictionary<string, string> data)
    {
        GameObject obj = GameObject.Instantiate(
            Resources.Load<GameObject>($"Model/{data["Model"]}")
        );
        obj.transform.position = new Vector3(b.transform.position.x, b.transform.position.y, -1);
        Hero hero = obj.AddComponent<Hero>();
        hero.Init(data, b);
        //这个位置被占据了 设置方块类型为障碍物
        b.type = BlockType.Obstacle;
        heros.Add(hero);
    }

    //移除敌人
    public void RemoveEnemy(Enemy enemy)
    {
        enemies.Remove(enemy);

        GameApp.MapManager.ChangeBlockType(enemy.rowIndex, enemy.colIndex, BlockType.Null);

        if (enemies.Count == 0)
        {
            ChangeState(GameState.GameOver);
        }
    }

    //移除英雄
    public void RemoveHero(Hero hero)
    {
        heros.Remove(hero);

        GameApp.MapManager.ChangeBlockType(hero.rowIndex, hero.colIndex, BlockType.Null);

        if (heros.Count == 0)
        {
            ChangeState(GameState.GameOver);
        }
    }

    //重置英雄
    public void ResetHeros()
    {
        for (int i = 0; i < heros.Count; i++)
        {
            heros[i].IsStop = false;
        }
    }

    //重置敌人
    public void ResetEnemies()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].IsStop = false;
        }
    }

    //获取最近的英雄
    public ModelBase GetMinDisHero(ModelBase model)
    {
        if (heros.Count == 0)
        {
            return null;
        }
        Hero hero = heros[0];
        float min_dist = hero.GetDist(model);
        for (int i = 1; i < heros.Count; i++)
        {
            float dist = heros[i].GetDist(model);
            if (dist < min_dist)
            {
                min_dist = dist;
                hero = heros[i];
            }
        }
        return hero;
    }

    //卸载资源
    public void Clear(){
        heros.Clear();
        enemies.Clear();
        GameApp.MapManager.Clear();
    }
}
