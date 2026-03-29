using UnityEngine;
using UnityEngine.EventSystems;

///<summary>
///用户输入管理器 (键鼠操作等)
///</summary>
public class UserInputManager
{
    public UserInputManager() { }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                //点击到UI上了
            }
            else
            {
                Tools.ScreenPointToRay2D(
                    Camera.main,
                    Input.mousePosition,
                    delegate(Collider2D c)
                    {
                        if (c != null)
                        {
                            //检测到有碰撞体的物体
                            GameApp.EventCenter.BroadcastEvent(c.gameObject, Defines.OnSelectEvent);
                        }
                        else
                        {
                            //执行为选中
                            GameApp.EventCenter.BroadcastEvent(Defines.OnUnSelectEvent);
                        }
                    }
                );
            }
        }
    }
}
