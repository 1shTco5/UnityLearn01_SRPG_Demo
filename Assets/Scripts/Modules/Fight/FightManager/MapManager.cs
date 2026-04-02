using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

//格子显示方向枚举 枚举字符串跟资源图片路径一致
public enum BlockDirection
{
    none = -1,
    down,
    horizontal,
    left,
    left_down,
    left_up,
    right,
    right_down,
    right_up,
    up,
    vertical,
    max,
}

//地图管理器 存储地图网格信息
public class MapManager
{
    private Tilemap tilemap;

    public Block[,] mapArr;

    public int rowCount;
    public int colCount;

    public List<Sprite> dirSpArr; //存储箭头方向图片的容器

    //初始化地图信息
    public void Init()
    {
        dirSpArr = new();
        for (int i = 0; i < (int)BlockDirection.max; i++)
        {
            dirSpArr.Add(Resources.Load<Sprite>($"Icon/{(BlockDirection)i}"));
        }

        tilemap = GameObject.Find("Grid/ground").GetComponent<Tilemap>();

        //地图大小 可以在配置表中设置
        rowCount = 12;
        colCount = 20;

        mapArr = new Block[12, 20];

        List<Vector3Int> tempPosArr = new(); //临时记录瓦片地图中每个格子的位置

        //遍历瓦片地图
        foreach (var pos in tilemap.cellBounds.allPositionsWithin)
        {
            if (tilemap.HasTile(pos))
            {
                tempPosArr.Add(pos);
            }
        }

        //将一维数组的位置转换成二维数组的Block 进行存储
        GameObject prefabObj = Resources.Load<GameObject>("Model/block");
        for (int i = 0; i < tempPosArr.Count; i++)
        {
            int row = i / colCount;
            int col = i % colCount;

            Block b = GameObject.Instantiate(prefabObj).AddComponent<Block>();
            b.rowIndex = row;
            b.colIndex = col;
            b.transform.position = tilemap.CellToWorld(tempPosArr[i]) + new Vector3(0.5f, 0.5f, 0);
            mapArr[row, col] = b;
        }
    }

    public Vector2Int GetTilePos(Vector3 pos)
    {
        Vector3Int cellPos = tilemap.WorldToCell(pos);
        return (Vector2Int)cellPos;
    }

    public void ChangeBlockType(int row, int col, BlockType type)
    {
        mapArr[row, col].type = type;
    }

    public Vector3 GetBlockPos(int row, int col)
    {
        return mapArr[row, col].transform.position;
    }

    public BlockType GetBlockType(int row, int col)
    {
        return mapArr[row, col].type;
    }

    //显示可移动的区域
    public void ShowStepGrid(ModelBase model, int step)
    {
        BFS bfs = new(rowCount, colCount);
        List<BFS.Point> points = bfs.Search(model.rowIndex, model.colIndex, step);
        for (int i = 0; i < points.Count; i++)
        {
            mapArr[points[i].rowIndex, points[i].colIndex].ShowStepGrid(Color.white);
        }
    }

    //隐藏可移动的区域
    public void HideStepGrid(ModelBase model, int step)
    {
        BFS bfs = new(rowCount, colCount);
        List<BFS.Point> points = bfs.Search(model.rowIndex, model.colIndex, step);
        for (int i = 0; i < points.Count; i++)
        {
            mapArr[points[i].rowIndex, points[i].colIndex].HideStepGrid();
        }
    }

    //根据方向枚举 设置格子方向图标颜色
    public void SetBlockDir(int row, int col, BlockDirection dir, Color color)
    {
        mapArr[row, col].SetDirSp(dirSpArr[(int)dir], color);
    }

    //开始点 和 下一个点 计算方向
    public BlockDirection GetDirection1(AStar.Point start, AStar.Point next)
    {
        int row_offset = next.rowIndex - start.rowIndex;
        int col_offset = next.colIndex - start.colIndex;
        if (row_offset == 0)
        {
            return BlockDirection.horizontal;
        }
        if (col_offset == 0)
        {
            return BlockDirection.vertical;
        }
        return BlockDirection.none;
    }

    //终点 和 前一个点 计算方向
    public BlockDirection GetDirection2(AStar.Point end, AStar.Point prev)
    {
        int row_offset = end.rowIndex - prev.rowIndex;
        int col_offset = end.colIndex - prev.colIndex;
        if (row_offset == 0 && col_offset > 0)
        {
            return BlockDirection.right;
        }
        if (row_offset == 0 && col_offset < 0)
        {
            return BlockDirection.left;
        }
        if (row_offset > 0 && col_offset == 0)
        {
            return BlockDirection.up;
        }
        if (row_offset < 0 && col_offset == 0)
        {
            return BlockDirection.down;
        }
        return BlockDirection.none;
    }

    //前一个点 当前点 后一个点 计算方向
    public BlockDirection GetDirection3(AStar.Point prev, AStar.Point curr, AStar.Point next)
    {
        BlockDirection dir = BlockDirection.none;

        int row_offset_1 = prev.rowIndex - curr.rowIndex;
        int col_offset_1 = prev.colIndex - curr.colIndex;
        int row_offset_2 = next.rowIndex - curr.rowIndex;
        int col_offset_2 = next.colIndex - curr.colIndex;

        int sum_row_offset = row_offset_1 + row_offset_2;
        int sum_col_offset = col_offset_1 + col_offset_2;

        if (sum_row_offset == 1 && sum_col_offset == -1)
        {
            dir = BlockDirection.left_up;
        }
        else if (sum_row_offset == 1 && sum_col_offset == 1)
        {
            dir = BlockDirection.right_up;
        }
        else if (sum_row_offset == -1 && sum_col_offset == -1)
        {
            dir = BlockDirection.left_down;
        }
        else if (sum_row_offset == -1 && sum_col_offset == 1)
        {
            dir = BlockDirection.right_down;
        }
        else
        {
            if (row_offset_1 == 0)
            {
                dir = BlockDirection.horizontal;
            }
            else
            {
                dir = BlockDirection.vertical;
            }
        }

        return dir;
    }

    public void ShowAttachStep(ModelBase model, int atkStep, Color color)
    {
        int minRow = model.rowIndex - atkStep >= 0 ? model.rowIndex - atkStep : 0;
        int minCol = model.colIndex - atkStep >= 0 ? model.colIndex - atkStep : 0;
        int maxRow =
            model.rowIndex + atkStep >= rowCount - 1 ? rowCount - 1 : model.rowIndex + atkStep;
        int maxCol =
            model.colIndex + atkStep >= colCount - 1 ? colCount - 1 : model.colIndex + atkStep;

        for (int row = minRow; row <= maxRow; row++)
        {
            for (int col = minCol; col <= maxCol; col++)
            {
                if (Mathf.Abs(model.rowIndex - row) + Mathf.Abs(model.colIndex - col) <= atkStep)
                {
                    mapArr[row, col].ShowStepGrid(color);
                }
            }
        }
    }

    public void HideAttackStep(ModelBase model, int atkStep)
    {
        int minRow = model.rowIndex - atkStep >= 0 ? model.rowIndex - atkStep : 0;
        int minCol = model.colIndex - atkStep >= 0 ? model.colIndex - atkStep : 0;
        int maxRow =
            model.rowIndex + atkStep >= rowCount - 1 ? rowCount - 1 : model.rowIndex + atkStep;
        int maxCol =
            model.colIndex + atkStep >= colCount - 1 ? colCount - 1 : model.colIndex + atkStep;

        for (int row = minRow; row <= maxRow; row++)
        {
            for (int col = minCol; col <= maxCol; col++)
            {
                if (Mathf.Abs(model.rowIndex - row) + Mathf.Abs(model.colIndex - col) <= atkStep)
                {
                    mapArr[row, col].HideStepGrid();
                }
            }
        }
    }

    //清空
    public void Clear()
    {
        mapArr = null;
        dirSpArr.Clear();
    }
}
