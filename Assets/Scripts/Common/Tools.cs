using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

///<summary>
///工具类
///</summary>
public static class Tools
{
    public static void SetIcon(this Image image, string res)
    {
        image.sprite = Resources.Load<Sprite>($"Icon/{res}");
    }

    //检测鼠标位置是否有2D碰撞物体
    public static void ScreenPointToRay2D(
        Camera camera,
        Vector2 mousePos,
        UnityAction<Collider2D> callback
    )
    {
        Vector3 worldPos = camera.ScreenToWorldPoint(mousePos);
        Collider2D collider = Physics2D.OverlapCircle(worldPos, 0.02f);
        callback?.Invoke(collider);
    }

    public static Collider2D ScreenPointToRay2D(Camera camera, Vector2 mousePos)
    {
        Vector3 worldPos = camera.ScreenToWorldPoint(mousePos);
        return Physics2D.OverlapCircle(worldPos, 0.02f);
    }
}
