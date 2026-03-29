using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Hero : ModelBase, ISkill
{
    public SkillProgerty skill { get; set; }

    private Slider sliderHp;

    public void Init(Dictionary<string, string> data, Block b)
    {
        sliderHp = transform.Find("hp/bg").GetComponent<Slider>();

        this.data = data;
        this.rowIndex = b.rowIndex;
        this.colIndex = b.colIndex;
        this.id = int.Parse(this.data["Id"]);
        this.type = int.Parse(this.data["Type"]);
        this.atk = int.Parse(this.data["Attack"]);
        this.step = int.Parse(this.data["Step"]);
        this.maxHp = int.Parse(this.data["Hp"]);
        this.currHp = maxHp;

        skill = new(int.Parse(data["Skill"]));
    }

    //选中时显示信息
    protected override void OnSelectCallback(object arg)
    {
        //玩家回合 才能选中角色
        if (GameApp.FightManager.state == GameState.Player)
        {
            //不能操作
            if (GameApp.CommandManager.IsRunningCommand)
            {
                return;
            }

            // GameApp.EventCenter.BroadcastEvent(Defines.OnUnSelectEvent);

            base.OnSelectCallback(arg);
            //添加显示路径指令
            GameApp.CommandManager.AddCommand(new ShowPathCommand(this));
            
            GameApp.ViewManager.Open(ViewType.HeroDesView, this);

            if (!IsStop)
            {
                //添加选项事件
                AddOptionEvent();
            }
        }
    }

    private void AddOptionEvent()
    {
        GameApp.EventCenter.AddTempEvent(Defines.OnAttackEvent, OnAttackCallback);
        GameApp.EventCenter.AddTempEvent(Defines.OnIdleEvent, OnIdleEvent);
        GameApp.EventCenter.AddTempEvent(Defines.OnCancelEvent, OnCancelEvent);
    }

    private void OnAttackCallback(object arg)
    {
        GameApp.CommandManager.AddCommand(new ShowSkillAreaCommand(this));
    }

    private void OnIdleEvent(object arg)
    {
        IsStop = true;
    }

    private void OnCancelEvent(object arg)
    {
        GameApp.CommandManager.UnDo();
    }

    //未选中时
    protected override void OnUnSelectCallback(object arg)
    {
        base.OnUnSelectCallback(arg);
        GameApp.ViewManager.Close(ViewType.HeroDesView);
    }

    //显示技能区域
    public void ShowSkillArea()
    {
        GameApp.MapManager.ShowAttachStep(this, skill.atkRange, Color.red);
    }

    //隐藏技能区域
    public void HideSkillArea()
    {
        GameApp.MapManager.HideAttackStep(this, skill.atkRange);
    }

    public override void GetHit(ISkill skill)
    {
        base.GetHit(skill);
        //播放受伤音效
        GameApp.SoundManager.PlaySE("hit", transform.position);
        //扣血
        currHp -= skill.skill.atk;
        //显示伤害数字
        GameApp.ViewManager.ShowHitNum($"-{skill.skill.atk}", Color.red, transform.position);
        //击中特效
        PlayEffect(skill.skill.atkEffect);

        //判断是否死亡
        if (currHp <= 0)
        {
            currHp = 0;
            PlayAni("die");
            Destroy(gameObject, 1.2f);
            //从敌人集合中移除
            GameApp.FightManager.RemoveHero(this);
        }

        StopAllCoroutines();
        StartCoroutine(ChangeColor());
        StartCoroutine(UpdateHpSlider());
    }

    private IEnumerator ChangeColor()
    {
        bodySprite.material.SetFloat("_FlashAmount", 1);
        yield return new WaitForSeconds(0.25f);
        bodySprite.material.SetFloat("_FlashAmount", 0);
    }

    private IEnumerator UpdateHpSlider()
    {
        sliderHp.gameObject.SetActive(true);
        sliderHp.DOValue((float)currHp / (float)maxHp, (float)0.25f);
        yield return new WaitForSeconds(0.75f);
        sliderHp.gameObject.SetActive(false);
    }
}
