using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

///<summary>
///消息处理中心
///</summary>
public class EventCenter
{
    private Dictionary<string, UnityAction<object>> eventDict; //存储普通的消息字典
    private Dictionary<string, UnityAction<object>> tempEventDict; //存储临时的消息字典 实施后移除
    private Dictionary<object, Dictionary<string, UnityAction<object>>> objEventDict; //存储特定对象的消息字典

    public EventCenter()
    {
        eventDict = new();
        tempEventDict = new();
        objEventDict = new();
    }

    //添加事件
    public void AddEvent(string eventName, UnityAction<object> callback)
    {
        if (eventDict.ContainsKey(eventName))
        {
            eventDict[eventName] += callback;
        }
        else
        {
            eventDict.Add(eventName, callback);
        }
    }

    //移除事件
    public void RemoveEvent(string eventName, UnityAction<object> callback)
    {
        if (eventDict.ContainsKey(eventName))
        {
            eventDict[eventName] -= callback;
            if (eventDict[eventName] == null)
            {
                eventDict.Remove(eventName);
            }
        }
    }

    //执行事件
    public void BroadcastEvent(string eventName, object arg = null)
    {
        if (eventDict.ContainsKey(eventName))
        {
            eventDict[eventName]?.Invoke(arg);
        }
    }

    //添加对象事件
    public void AddEvent(object listener, string eventName, UnityAction<object> callback)
    {
        if (objEventDict.ContainsKey(listener))
        {
            if (objEventDict[listener].ContainsKey(eventName))
            {
                objEventDict[listener][eventName] += callback;
            }
            else
            {
                objEventDict[listener].Add(eventName, callback);
            }
        }
        else
        {
            Dictionary<string, UnityAction<object>> tempDict = new() { { eventName, callback } };
            objEventDict.Add(listener, tempDict);
        }
    }

    //移除对象事件
    public void RemoveEvent(object listener, string eventName, UnityAction<object> callback)
    {
        if (objEventDict.ContainsKey(listener))
        {
            if (objEventDict[listener].ContainsKey(eventName))
            {
                objEventDict[listener][eventName] -= callback;
                if (objEventDict[listener][eventName] == null)
                {
                    objEventDict[listener].Remove(eventName);
                    if (objEventDict[listener].Count == 0)
                    {
                        objEventDict.Remove(listener);
                    }
                }
            }
        }
    }

    //移除某个对象所有事件
    public void RemoveObjAllEvent(object listener)
    {
        if (objEventDict.ContainsKey(listener))
        {
            objEventDict.Remove(listener);
        }
    }

    //广播某个对象的某个事件
    public void BroadcastEvent(object listener, string eventName, object arg = null)
    {
        if (objEventDict.ContainsKey(listener))
        {
            if (objEventDict[listener].ContainsKey(eventName))
            {
                objEventDict[listener][eventName]?.Invoke(arg);
            }
        }
    }

    public void AddTempEvent(string eventName, UnityAction<object> callback)
    {
        if (tempEventDict.ContainsKey(eventName))
        {
            //添加的临时事件 要是覆盖的 不是累加
            tempEventDict[eventName] = callback;
        }
        else
        {
            tempEventDict.Add(eventName, callback);
        }
    }

    public void BroadcastTempEvent(string eventName, object arg = null)
    {
        if (tempEventDict.ContainsKey(eventName))
        {
            tempEventDict[eventName]?.Invoke(arg);
            tempEventDict[eventName] = null;
            tempEventDict.Remove(eventName);
        }
    }
}
