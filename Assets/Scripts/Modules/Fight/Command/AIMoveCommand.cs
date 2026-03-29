using System.Collections.Generic;
using UnityEngine;

///<summary>
///AI移动指令
///</summary>
public class AIMoveCommand : BaseCommand
{
    private Enemy enemy;
    private BFS bfs;
    private List<BFS.Point> path;
    private BFS.Point curr;
    private int pathIndex;
    private ModelBase target;

    public AIMoveCommand(Enemy enemy)
        : base(enemy)
    {
        this.enemy = enemy;
        bfs = new(GameApp.MapManager.rowCount, GameApp.MapManager.colCount);
        path = new();
    }

    public override void Do()
    {
        base.Do();
        target = GameApp.FightManager.GetMinDisHero(enemy); //获得最近的英雄
        if (target == null)
        {
            //没有目标了
            isFinish = true;
        }
        else
        {
            path = bfs.FindMinPath(enemy, enemy.step, target.rowIndex, target.colIndex);
            if (path.Count == 0)
            {
                //没路 可以随机一个点做移动
                isFinish = true;
            }
            else
            {
                //将当前敌人的位置设置成null
                GameApp.MapManager.ChangeBlockType(enemy.rowIndex, enemy.colIndex, BlockType.Null);
            }
        }
    }

    public override bool Update(float dt)
    {
        if (path.Count == 0)
        {
            return base.Update(dt);
        }
        else
        {
            curr = path[pathIndex];
            if (model.Move(curr.rowIndex, curr.colIndex, dt * 5))
            {
                pathIndex++;
                if (pathIndex >= path.Count)
                {
                    enemy.PlayAni("idle");
                    GameApp.MapManager.ChangeBlockType(
                        enemy.rowIndex,
                        enemy.colIndex,
                        BlockType.Obstacle
                    );
                    return true;
                }
            }
            enemy.PlayAni("move");
        }
        return false;
    }
}
