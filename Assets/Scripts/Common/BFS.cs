using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//广度优先搜索
public class BFS
{
    //搜索点类
    public class Point
    {
        public int rowIndex;
        public int colIndex;
        public Point father;

        public Point(int row, int col)
        {
            this.rowIndex = row;
            this.colIndex = col;
        }

        public Point(int row, int col, Point father)
        {
            this.rowIndex = row;
            this.colIndex = col;
            this.father = father;
        }
    }

    public int rowCount; //行数
    public int colCount; //列数

    //存储查找到的点的字典 (key: 点的行列拼接的字符串, value: 搜索点)
    public Dictionary<string, Point> finds;

    public BFS(int row, int col)
    {
        this.rowCount = row;
        this.colCount = col;
        finds = new();
    }

    ///<summary>
    ///搜索可行走区域
    ///</summary>
    public List<Point> Search(int row, int col, int step)
    {
        //定义搜索集合
        List<Point> searchs = new();
        //开始点
        Point start = new(row, col);
        //将开始点存储到搜索集合
        searchs.Add(start);
        //开始点默认开始已经找到 存储到已找到字典中
        finds.Add($"{row}_{col}", start);

        //遍历步数 相当于可搜索次数
        for (int i = 0; i < step; i++)
        {
            //定义一个临时的集合 用于存储目前找到的满足条件的点
            List<Point> temp = new();
            //遍历搜索集合
            for (int j = 0; j < searchs.Count; j++)
            {
                Point curr = searchs[j];
                //查找当前点四周的点
                FindAroundPoints(curr, temp);
            }
            if (temp.Count == 0)
            { //周围一个点都没有 相当于死路
                break;
            }
            //搜索的集合要清空
            searchs.Clear();
            //将临时集合的点添加到搜索集合
            searchs.AddRange(temp);
        }

        //将查找到的点转换成集合 返回
        return finds.Values.ToList();
    }

    public void FindAroundPoints(Point curr, List<Point> temp)
    {
        //上
        if (curr.rowIndex - 1 >= 0)
        {
            AddFinds(curr.rowIndex - 1, curr.colIndex, curr, temp);
        }
        //下
        if (curr.rowIndex + 1 < rowCount)
        {
            AddFinds(curr.rowIndex + 1, curr.colIndex, curr, temp);
        }
        //左
        if (curr.colIndex - 1 >= 0)
        {
            AddFinds(curr.rowIndex, curr.colIndex - 1, curr, temp);
        }
        //右
        if (curr.colIndex + 1 < colCount)
        {
            AddFinds(curr.rowIndex, curr.colIndex + 1, curr, temp);
        }
    }

    //添加到查找到的字典
    public void AddFinds(int row, int col, Point father, List<Point> temp)
    {
        //不在查找的节点 且 对应地图格子的类型不是障碍物 即可加入查找字典
        if (
            !finds.ContainsKey($"{row}_{col}")
            && GameApp.MapManager.GetBlockType(row, col) != BlockType.Obstacle
        )
        {
            Point p = new(row, col, father);
            finds.Add($"{row}_{col}", p);
            //添加到临时集合 用于下次继续查找
            temp.Add(p);
        }
    }

    //寻找可移动的点 离终点最近的点的路径集合
    public List<Point> FindMinPath(ModelBase model, int step, int endRowIndex, int endColIndex)
    {
        List<Point> results = Search(model.rowIndex, model.colIndex, step); //获得能到达的点集
        if (results.Count == 0)
        {
            return null;
        }
        Point minPoint = results[0];
        int min_dist =
            Mathf.Abs(minPoint.rowIndex - endRowIndex) + Mathf.Abs(minPoint.colIndex - endColIndex);
        foreach (var item in results)
        {
            int dist =
                Mathf.Abs(item.rowIndex - endRowIndex) + Mathf.Abs(item.colIndex - endColIndex);
            if (dist < min_dist)
            {
                min_dist = dist;
                minPoint = item;
            }
        }
        List<Point> path = new();
        Point curr = minPoint.father;
        path.Add(minPoint);

        while (curr != null)
        {
            path.Add(curr);
            curr = curr.father;
        }
        path.Reverse();
        return path;
    }
}
