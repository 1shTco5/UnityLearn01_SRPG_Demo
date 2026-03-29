using UnityEngine;
using UnityEngine.UI;

//显示角色信息面板
public class HeroDesView : BaseView
{
    public override void Open(params object[] args)
    {
        base.Open(args);

        Hero info = args[0] as Hero;
        Find<Image>("bg/icon").SetIcon(info.data["Icon"]);
        Find<Image>("bg/hp/fill").fillAmount = (float)info.currHp / (float)info.maxHp;
        Find<Text>("bg/hp/txt").text = $"{info.currHp}/{info.maxHp}";
        Find<Text>("bg/atkTxt/txt").text = $"{info.atk}";
        Find<Text>("bg/StepTxt/txt").text = $"{info.step}";
    }
}
