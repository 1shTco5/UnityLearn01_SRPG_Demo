using UnityEngine;

///<summary>
///常量类
///</summary>
public class Defines : MonoBehaviour
{
    //控制器相关的事件字符串
    public static readonly string OpenBeginView = "OpenBeginView";
    public static readonly string OpenSettingsView = "OpenSettingsView";
    public static readonly string OpenMessageView = "OpenMessageView";
    public static readonly string LoadScene = "LoadScene";
    public static readonly string OpenSelectLevel = "OpenSelectLevel";
    public static readonly string BeginFight = "BeginFight";

    //全局事件相关
    public static readonly string ShowLevelDescriptionEvent = "ShowLevelDescriptionEvent";
    public static readonly string HideLevelDescriptionEvent = "HideLevelDescriptionEvent";

    public static readonly string OnSelectEvent = "OnSelectEvent"; //选中事件
    public static readonly string OnUnSelectEvent = "OnUnSelectEvent"; //未选中事件

    //option
    public static readonly string OnAttackEvent = "OnAttackEvent";
    public static readonly string OnIdleEvent = "OnIdleEvent";
    public static readonly string OnCancelEvent = "OnCancelEvent";
    public static readonly string OnRemoveHeroToSceneEvent = "OnRemoveHeroToSceneEvent";
}
