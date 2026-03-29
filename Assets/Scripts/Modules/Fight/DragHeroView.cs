using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

///<summary>
///拖拽图标界面
///</summary>
public class DragHeroView : BaseView
{
    void Update()
    {
        //拖拽图标时跟随鼠标移动 显示的时候才进行移动
        if (!_canvas.enabled)
        {
            return;
        }

        //鼠标坐标转换成世界坐标
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mouseWorldPos;
    }

    public override void Open(params object[] args)
    {
        transform.GetComponent<Image>().SetIcon(args[0].ToString());
    }
}
