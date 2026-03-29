using System.Collections.Generic;
using System.Linq;
using UnityEngine;

///<summary>
///控制器管理器
///</summary>
public class ControllerManager
{
    private Dictionary<int, BaseController> _modules; //存储控制器的字典

    public ControllerManager()
    {
        _modules = new();
    }

    public void InitAllModules()
    {
        foreach (BaseController controller in _modules.Values)
        {
            controller.Init();
        }
    }

    public void Register(ControllerType controllerType, BaseController ctl)
    {
        Register((int)controllerType, ctl);
    } //注册控制器

    public void Register(int controllerKey, BaseController ctl)
    {
        if (!_modules.ContainsKey(controllerKey))
        {
            _modules.Add(controllerKey, ctl);
        }
    } //注册控制器

    public void UnRegister(int controllerKey)
    {
        if (_modules.ContainsKey(controllerKey))
        {
            _modules.Remove(controllerKey);
        }
    } // 移除控制器

    public void Clear()
    {
        _modules.Clear();
    }

    public void ClearAllModules()
    {
        foreach (var module in _modules.Values)
        {
            module.Destroy();
        }
        Clear();
    } //清除所有控制器

    public void ApplyFunc(int controllerKey, string eventName, params object[] args)
    {
        if (_modules.ContainsKey(controllerKey))
        {
            _modules[controllerKey].ApplyFunc(eventName, args);
        }
    } //通过控制器管理器触发指定控制器事件

    public BaseModel GetControllerModel(int controllerKey)
    {
        if (_modules.ContainsKey(controllerKey))
        {
            return _modules[controllerKey].GetModel();
        }
        return null;
    } //获取指定控制器数据对象
}
