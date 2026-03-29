using System.Collections.Generic;
using UnityEngine;

///<summary>
///命令管理器
///</summary>
public class CommandManager
{
    private Queue<BaseCommand> toDoQueue; //将要执行的命令队列

    private Stack<BaseCommand> unDoStack; //用于撤销命令的栈

    private BaseCommand curr; //当前正在执行的命令

    public CommandManager()
    {
        toDoQueue = new();
        unDoStack = new();
    }

    //是否正在执行命令
    public bool IsRunningCommand
    {
        get { return curr != null; }
    }

    public void AddCommand(BaseCommand cmd)
    {
        toDoQueue.Enqueue(cmd);
        unDoStack.Push(cmd);
    }

    //每帧执行
    public void Update(float dt)
    {
        if (curr == null)
        {
            if (toDoQueue.Count > 0)
            {
                curr = toDoQueue.Dequeue();
                curr.Do();
            }
        }
        else
        {
            if (curr.Update(dt) == true)
            {
                curr = null;
            }
        }
    }

    public void Clear()
    {
        toDoQueue.Clear();
        unDoStack.Clear();
        curr = null;
    }

    //撤销上一个指令
    public void UnDo()
    {
        if (unDoStack.Count > 0)
        {
            unDoStack.Pop().UnDo();
        }
    }
}
