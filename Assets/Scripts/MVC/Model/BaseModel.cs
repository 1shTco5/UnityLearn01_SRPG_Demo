using UnityEngine;

///<summary>
///Model基类
///</summary>
public class BaseModel
{
    public BaseController controller;

    public BaseModel(BaseController ctl)
    {
        this.controller = ctl;
    }

    public BaseModel() { }

    public virtual void Init() { }
}
