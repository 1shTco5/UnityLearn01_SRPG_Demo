using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//A星寻路类
public class AStar
{
    private int rowCount;
    private int colCount;
    private List<Point> open; //open表
    private Dictionary<string, Point> close; //close表 已经查找过的路径点会存到这个表里
    private Point start;
    private Point end;

    public AStar(int rowCount, int colCount)
    {
        this.rowCount = rowCount;
        this.colCount = colCount;
        open = new();
        close = new();
    }

    //找到open表的路径点
    public Point IsInOpen(int row, int col)
    {
        for (int i = 0; i < open.Count; i++)
        {
            if (open[i].rowIndex == row && open[i].colIndex == col)
            {
                return open[i];
            }
        }
        return null;
    }

    //某个点是否已经存在close表中
    public bool IsInClose(int row, int col)
    {
        return close.ContainsKey($"{row}_{col}");
    }

    /* A星思路:
     * 1.将起点添加到open表
     * 2.查找open表中f值最小的路径点 (1.排序 2.优先队列)
     * 3.将找到的最小f值的点从open表中移除, 并添加到close表
     * 4.将当前路径点周围的点添加到open表中
     * 5.判断终点是否在open表, 如果不在 从步骤2继续执行逻辑
     */
    public bool FindPath(Point start, Point end, UnityAction<List<Point>> findCallback)
    {
        this.start = start;
        this.end = end;
        open = new();
        close = new();

        //1.将起点添加到open表
        open.Add(start);
        while (true)
        {
            //2.从open表中获取f值最小的点
            Point curr = GetMinFPoint();
            if (curr == null)
            { //死路
                return false;
            }
            //3.1.从open表中移除
            open.Remove(curr);
            //3.2.添加到close表
            close.Add($"{curr.rowIndex}_{curr.colIndex}", curr);
            //4.将当前路径点周围的点添加到open表中
            AddAroundToOpen(curr);
            //5.判断终点是否在open表中
            Point eP = IsInOpen(end.rowIndex, end.colIndex);
            if (eP != null)
            {
                //找到路径了
                findCallback(GetPath(eP));
                return true;
            }

            //将open表排序 按照F值从小到大排序
            open.Sort(OpenSort);
        }
    }

    public List<Point> GetPath(Point p)
    {
        List<Point> path = new();
        path.Add(p);
        Point father = p.father;
        while (father != null)
        {
            path.Add(father);
            father = father.father;
        }
        path.Reverse();
        return path;
    }

    public int OpenSort(Point a, Point b)
    {
        return a.F - b.F;
    }

    public void AddAroundToOpen(Point curr)
    {
        //上
        if (curr.rowIndex - 1 >= 0)
        {
            AddToOpen(curr.rowIndex - 1, curr.colIndex, curr);
        }
        //下
        if (curr.rowIndex + 1 < rowCount)
        {
            AddToOpen(curr.rowIndex + 1, curr.colIndex, curr);
        }
        //左
        if (curr.colIndex - 1 >= 0)
        {
            AddToOpen(curr.rowIndex, curr.colIndex - 1, curr);
        }
        //右
        if (curr.colIndex + 1 < colCount)
        {
            AddToOpen(curr.rowIndex, curr.colIndex + 1, curr);
        }
    }

    public void AddToOpen(int row, int col, Point father)
    {
        //不在open表 不在close表 不是障碍物
        if (
            !IsInClose(row, col)
            && IsInOpen(row, col) == null
            && GameApp.MapManager.GetBlockType(row, col) != BlockType.Obstacle
        )
        {
            Point p = new(row, col, father);
            p.G = p.GetG();
            p.H = p.GetH(end);
            p.F = p.G + p.H;
            open.Add(p);
        }
    }

    public Point GetMinFPoint()
    {
        if (open.Count == 0)
        {
            return null;
        }
        return open[0]; //open表会排序
    }

    public class Point
    {
        public int rowIndex;
        public int colIndex;
        public int G; //当前节点到起点的距离
        public int H; //当前节点到终点的距离
        public int F; //F = G + H
        public Point father; //当前点的父节点

        public Point(int row, int col)
        {
            rowIndex = row;
            colIndex = col;
            father = null;
        }

        public Point(int row, int col, Point father)
        {
            rowIndex = row;
            colIndex = col;
            this.father = father;
        }

        //计算G
        public int GetG()
        {
            int g = 0;
            Point father = this.father;
            while (father != null)
            {
                g++;
                father = father.father;
            }
            return g;
        }

        //计算H
        public int GetH(Point end)
        {
            return Mathf.Abs(rowIndex - end.rowIndex) + Mathf.Abs(colIndex - end.colIndex);
        }
    }
}
