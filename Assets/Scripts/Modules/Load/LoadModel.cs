using UnityEngine;
using UnityEngine.Events;

public class LoadModel : BaseModel
{
    public string sceneName; //加载场景名称
    public UnityAction callback; //加载完成后回调

    public LoadModel() { }
}
