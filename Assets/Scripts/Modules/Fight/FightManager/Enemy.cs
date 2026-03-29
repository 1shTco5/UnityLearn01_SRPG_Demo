using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : ModelBase, ISkill
{
    public SkillProgerty skill { get; set; }

    private Slider sliderHp;

    protected override void Start()
    {
        base.Start();

        sliderHp = transform.Find("hp/bg").GetComponent<Slider>();

        data = GameApp.ConfigManager.GetConfigData("enemy").GetDataByID(id);

        this.type = int.Parse(this.data["Type"]);
        this.atk = int.Parse(this.data["Attack"]);
        this.step = int.Parse(this.data["Step"]);
        this.maxHp = int.Parse(this.data["Hp"]);
        this.currHp = maxHp;

        skill = new SkillProgerty(int.Parse(data["Skill"]));
    }

    protected override void OnSelectCallback(object arg)
    {
        if (GameApp.FightManager.state == GameState.Player)
        {
            if (GameApp.CommandManager.IsRunningCommand == true)
            {
                return;
            }
            base.OnSelectCallback(arg);

            GameApp.ViewManager.Open(ViewType.EnemyDesView, this);
        }
    }

    protected override void OnUnSelectCallback(object arg)
    {
        base.OnUnSelectCallback(arg);

        GameApp.ViewManager.Close(ViewType.EnemyDesView);
    }

    public void ShowSkillArea() { }

    public void HideSkillArea() { }

    //受伤
    public override void GetHit(ISkill skill)
    {
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
            GameApp.FightManager.RemoveEnemy(this);
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
