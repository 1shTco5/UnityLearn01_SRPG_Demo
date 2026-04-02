using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//处理拖拽英雄图标脚本
public class HeroItem : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private Dictionary<string, string> data;

    void Start()
    {
        transform.Find("icon").GetComponent<Image>().SetIcon(data["Icon"]);
    }

    public void Init(Dictionary<string, string> data)
    {
        this.data = data;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        GameApp.ViewManager.Open(ViewType.DragHeroView, data["Icon"]);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GameApp.ViewManager.Close(ViewType.DragHeroView, data["Icon"]);
        //检测拖拽后的位置 是否有block脚本
        Tools.ScreenPointToRay2D(
            eventData.pressEventCamera,
            Input.mousePosition,
            delegate(Collider2D c)
            {
                if (c != null)
                {
                    Block b = c.GetComponent<Block>();
                    if (b != null)
                    {
                        //有block
                        // Destroy(gameObject); //删除图标
                        gameObject.SetActive(false);
                        //创建英雄物体
                        GameApp.FightManager.SpawnHero(b, data);
                    }
                }
            }
        );
    }

    public void OnDrag(PointerEventData eventData) { }
}
