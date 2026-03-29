using System.Collections.Generic;
using UnityEngine;

//移动指令
public class MoveCommand : BaseCommand
{
    private List<AStar.Point> path;

    private AStar.Point curr;
    private int pathIndex;

    //移动前的行列坐标
    private int prevRow;
    private int prevCol;

    public MoveCommand(ModelBase model)
        : base(model) { }

    public MoveCommand(ModelBase model, List<AStar.Point> path)
        : base(model)
    {
        this.path = path;
        pathIndex = 0;
    }

    public override void Do()
    {
        base.Do();
        this.prevRow = this.model.rowIndex;
        this.prevCol = this.model.colIndex;
        //设置当前所占格子为null
        GameApp.MapManager.ChangeBlockType(model.rowIndex, model.colIndex, BlockType.Null);
    }

    public override bool Update(float dt)
    {
        curr = this.path[pathIndex];
        if (this.model.Move(curr.rowIndex, curr.colIndex, dt * 5))
        {
            pathIndex++;
            if (pathIndex >= path.Count)
            {
                //到达目的地
                this.model.PlayAni("idle");

                GameApp.MapManager.ChangeBlockType(
                    this.model.rowIndex,
                    this.model.colIndex,
                    BlockType.Obstacle
                );

                //显示选项界面
                GameApp.ViewManager.Open(
                    ViewType.SelectOptionView,
                    this.model.data["Event"],
                    (Vector2)this.model.transform.position
                );

                return true;
            }
            this.model.rowIndex = path[pathIndex].rowIndex;
            this.model.colIndex = path[pathIndex].colIndex;
        }
        this.model.PlayAni("move");
        return false;
    }

    //撤销移动指令
    public override void UnDo()
    {
        base.UnDo();
        //回到移动前的位置
        Vector3 pos = GameApp.MapManager.GetBlockPos(prevRow, prevCol);
        pos.z = this.model.transform.position.z;
        this.model.transform.position = pos;
        GameApp.MapManager.ChangeBlockType(
            this.model.rowIndex,
            this.model.colIndex,
            BlockType.Null
        );
        this.model.rowIndex = prevRow;
        this.model.colIndex = prevCol;
        GameApp.MapManager.ChangeBlockType(
            this.model.rowIndex,
            this.model.colIndex,
            BlockType.Obstacle
        );
    }
}
