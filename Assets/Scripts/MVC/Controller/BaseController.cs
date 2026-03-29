using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

///<summary>
///Controller基类
///</summary>
public class BaseController
{
    private Dictionary<string, UnityAction<object[]>> message; //事件字典

    protected BaseModel model; //模板数据

    public BaseController()
    {
        message = new();
    }

    public virtual void Init() { } //注册后调用的初始化函数 (要所有控制器初始化后执行)

    public virtual void OnLoadView(IBaseView view) { } //加载视图

    public virtual void OpenView(IBaseView view) { } //打开视图

    public virtual void CloseView(IBaseView view) { } //关闭视图

    public void RegisterFunc(string eventName, UnityAction<object[]> callback)
    {
        if (message.ContainsKey(eventName))
        {
            message[eventName] += callback;
        }
        else
        {
            message.Add(eventName, callback);
        }
    } //注册模板事件

    public void UnRegisterFunc(string eventName)
    {
        if (message.ContainsKey(eventName))
        {
            message.Remove(eventName);
        }
    } //移除模板事件

    public void ApplyFunc(string eventName, params object[] args)
    {
        if (message.ContainsKey(eventName))
        {
            message[eventName]?.Invoke(args);
        }
        else
        {
            Debug.Log("Error: " + eventName);
        }
    } //触发本模块事件

    public void ApplyControllerFunc(
        ControllerType controllerType,
        string eventName,
        params object[] args
    )
    {
        GameApp.ControllerManager.ApplyFunc((int)controllerType, eventName, args);
    } //触发其他模板事件

    public void ApplyControllerFunc(int controllerKey, string eventName, params object[] args)
    {
        GameApp.ControllerManager.ApplyFunc(controllerKey, eventName, args);
    } //触发其他模板事件

    public void SetModel(BaseModel model)
    {
        this.model = model;
        this.model.controller = this;
    } //设置模板数据

    public BaseModel GetModel()
    {
        return this.model;
    } //获取模板数据

    public T GetModel<T>()
        where T : BaseModel
    {
        return model as T;
    }

    public BaseModel GetControllerModel(int controllerKey)
    {
        return GameApp.ControllerManager.GetControllerModel(controllerKey);
    } //获取其他控制器

    public virtual void Destroy()
    {
        RemoveModuleEvent();
        RemoveGlobalEvent();
    } //销毁控制器

    public virtual void InitModuleEvent() { } //初始化模板事件

    public virtual void RemoveModuleEvent() { } //移除模板事件

    public virtual void InitGlobalEvent() { } //初始化全局事件

    public virtual void RemoveGlobalEvent() { } //移除全局事件
}
