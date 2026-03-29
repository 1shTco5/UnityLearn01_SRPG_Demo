using UnityEngine;

public enum BlockType
{
    Null, //普通格子
    Obstacle, //障碍物
}

//地图中的单元格子
public class Block : MonoBehaviour
{
    public int rowIndex;
    public int colIndex;
    public BlockType type;
    private SpriteRenderer selSprite; //选中的格子Sprite
    private SpriteRenderer gridSprite; //网格Sprite
    private SpriteRenderer dirSprite; //移动方向Sprite

    void Awake()
    {
        selSprite = transform.Find("select").GetComponent<SpriteRenderer>();
        gridSprite = transform.Find("grid").GetComponent<SpriteRenderer>();
        dirSprite = transform.Find("dir").GetComponent<SpriteRenderer>();

        GameApp.EventCenter.AddEvent(gameObject, Defines.OnSelectEvent, OnSelectCallback);
        GameApp.EventCenter.AddEvent(Defines.OnUnSelectEvent, UnOnSelectCallback);
    }

    void OnDestroy()
    {
        GameApp.EventCenter.RemoveObjAllEvent(gameObject);
        GameApp.EventCenter.RemoveEvent(Defines.OnUnSelectEvent, UnOnSelectCallback);
    }

    public void ShowStepGrid(Color color)
    {
        gridSprite.enabled = true;
        gridSprite.color = color;
    }

    public void HideStepGrid()
    {
        gridSprite.enabled = false;
    }

    private void OnSelectCallback(object arg)
    {
        GameApp.EventCenter.BroadcastEvent(Defines.OnUnSelectEvent);
        if (!GameApp.CommandManager.IsRunningCommand)
        {
            GameApp.ViewManager.Open(ViewType.FightOptionDesView);
        }
    }

    private void UnOnSelectCallback(object arg)
    {
        dirSprite.sprite = null;
        GameApp.ViewManager.Close(ViewType.FightOptionDesView);
    }

    private void OnMouseEnter()
    {
        selSprite.enabled = true;
    }

    private void OnMouseExit()
    {
        selSprite.enabled = false;
    }

    //设置箭头方向图片和颜色
    public void SetDirSp(Sprite sp, Color color)
    {
        dirSprite.sprite = sp;
        dirSprite.color = color;
    }
}
