using System;
using System.Collections.Generic;
using UnityEngine;

public class ModelBase : MonoBehaviour
{
    public int id;
    public Dictionary<string, string> data; //数据表
    public int step; //行动力
    public int atk; //攻击力
    public int type; //类型
    public int maxHp; //最大血量
    public int currHp; //当前血量

    public int rowIndex;
    public int colIndex;
    public SpriteRenderer bodySprite; //图片渲染组件
    public GameObject stopObj; //停止行动的标记物体
    public Animator animator; //动画组件

    private bool isStop; //是否正在移动

    public bool IsStop
    {
        get { return isStop; }
        set
        {
            stopObj.SetActive(value);
            if (value == true)
            {
                // bodySprite.color = Color.gray;
            }
            else
            {
                bodySprite.color = Color.white;
            }
            isStop = value;
        }
    }

    void Awake()
    {
        bodySprite = transform.GetComponentInChildren<SpriteRenderer>();
        stopObj = transform.Find("stop").gameObject;
        animator = transform.GetComponentInChildren<Animator>();
    }

    protected virtual void Start()
    {
        AddEvents();
    }

    protected virtual void OnDestroy()
    {
        RemoveEvents();
    }

    protected virtual void AddEvents()
    {
        GameApp.EventCenter.AddEvent(gameObject, Defines.OnSelectEvent, OnSelectCallback);
        GameApp.EventCenter.AddEvent(Defines.OnUnSelectEvent, OnUnSelectCallback);
    }

    protected virtual void RemoveEvents()
    {
        GameApp.EventCenter.RemoveObjAllEvent(gameObject);
        GameApp.EventCenter.RemoveEvent(Defines.OnUnSelectEvent, OnUnSelectCallback);
    }

    //被选中回调
    protected virtual void OnSelectCallback(object arg)
    {
        GameApp.EventCenter.BroadcastEvent(Defines.OnUnSelectEvent);
        bodySprite.color = Color.green;

        GameApp.MapManager.ShowStepGrid(this, step);
    }

    //未选中回调
    protected virtual void OnUnSelectCallback(object arg)
    {
        bodySprite.color = Color.white;

        GameApp.MapManager.HideStepGrid(this, step);
    }

    //转向
    public void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    //移动到指定下标的格子
    public virtual bool Move(int row, int col, float dt)
    {
        Vector3 pos = GameApp.MapManager.GetBlockPos(row, col);

        pos.z = transform.position.z;

        bool flip = ((transform.position.x - pos.x) * transform.localScale.x) > 0;
        if (flip)
        {
            Flip();
        }

        //如果离目的地很近 返回true
        if (Vector3.Distance(transform.position, pos) <= 0.02f)
        {
            this.rowIndex = row;
            this.colIndex = col;
            transform.position = pos;
            return true;
        }

        transform.position = Vector3.MoveTowards(transform.position, pos, dt);
        return false;
    }

    //播放动画
    public void PlayAni(string aniName)
    {
        animator.Play(aniName);
    }

    //受伤
    public virtual void GetHit(ISkill skill) { }

    //播放特效
    public virtual void PlayEffect(string name)
    {
        GameObject obj = GameObject.Instantiate(Resources.Load<GameObject>($"Effect/{name}"));
        obj.transform.position = transform.position;
    }

    //计算两个model之间的距离 (曼哈顿距离)
    public float GetDist(ModelBase model)
    {
        return Mathf.Abs(rowIndex - model.rowIndex) + Mathf.Abs(colIndex - model.colIndex);
    }

    //播放音效 (攻击 受伤等)
    public void PlaySound(string name)
    {
        GameApp.SoundManager.PlaySE(name, transform.position);
    }

    //看向某个model
    public void LookAtModel(ModelBase model)
    {
        bool flip =
            ((model.transform.position.x - transform.position.x) * transform.localScale.x) < 0;
        if (flip)
        {
            Flip();
        }
    }
}
