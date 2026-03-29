using System.Collections.Generic;
using UnityEngine;

//显示移动路径的指令
public class ShowPathCommand : BaseCommand
{
    private Collider2D prev; //鼠标之前检测到的2D碰撞盒
    private Collider2D curr; //鼠标当前检测到的2D碰撞盒
    private AStar astar;
    private AStar.Point start;
    private AStar.Point end;
    List<AStar.Point> prevPath; //之前检测到的路径集合 用来清空
    List<AStar.Point> currPath;

    public ShowPathCommand(ModelBase model)
        : base(model)
    {
        start = new(model.rowIndex, model.colIndex);
        astar = new(GameApp.MapManager.rowCount, GameApp.MapManager.colCount);
        prevPath = new();
    }

    public override bool Update(float dt)
    {
        //点击鼠标 确定移动的位置
        if (Input.GetMouseButtonDown(0))
        {
            if (prevPath.Count != 0 && this.model.step >= prevPath.Count - 1)
            {
                GameApp.CommandManager.AddCommand(new MoveCommand(this.model, prevPath)); //移动
            }
            else
            {
                GameApp.EventCenter.BroadcastEvent(Defines.OnUnSelectEvent); //执行未选中

                //不移动 直接显示操作选项
                GameApp.ViewManager.Open(
                    ViewType.SelectOptionView,
                    this.model.data["Event"],
                    (Vector2)this.model.transform.position
                );
            }

            return true;
        }
        curr = Tools.ScreenPointToRay2D(Camera.main, Input.mousePosition); //检测当前鼠标位置是否有2D碰撞盒
        if (curr != null)
        {
            //如果之前检测到的碰撞体和当前不一致 则进行路径检测
            if (prev != curr)
            {
                prev = curr;
                Block b = curr.GetComponent<Block>();

                if (b != null)
                {
                    //检测到Block脚本的物体 进行寻路
                    end = new(b.rowIndex, b.colIndex);
                    astar.FindPath(start, end, UpdatePath);
                }
                else
                {
                    //没检测到 将之前的路径清除
                    for (int i = 0; i < prevPath.Count; i++)
                    {
                        GameApp
                            .MapManager.mapArr[prevPath[i].rowIndex, prevPath[i].colIndex]
                            .SetDirSp(null, Color.white);
                    }
                    prevPath.Clear();
                }
            }
        }

        return false;
    }

    private void UpdatePath(List<AStar.Point> path)
    {
        //如果之前已经有路径了 需要先清除
        if (prevPath.Count != 0)
        {
            for (int i = 0; i < prevPath.Count; i++)
            {
                GameApp
                    .MapManager.mapArr[prevPath[i].rowIndex, prevPath[i].colIndex]
                    .SetDirSp(null, Color.white);
            }
            prevPath.Clear();
        }

        if (path.Count >= 2 && model.step >= path.Count - 1)
        {
            for (int i = 0; i < path.Count; i++)
            {
                BlockDirection dir = BlockDirection.down;

                if (i == 0)
                {
                    dir = GameApp.MapManager.GetDirection1(path[i], path[i + 1]);
                }
                else if (i == path.Count - 1)
                {
                    dir = GameApp.MapManager.GetDirection2(path[i], path[i - 1]);
                }
                else
                {
                    dir = GameApp.MapManager.GetDirection3(path[i - 1], path[i], path[i + 1]);
                }

                GameApp.MapManager.SetBlockDir(
                    path[i].rowIndex,
                    path[i].colIndex,
                    dir,
                    Color.yellow
                );
            }
        }
        prevPath = path;
    }
}
