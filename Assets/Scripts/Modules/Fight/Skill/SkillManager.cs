using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SkillManager
{
    private GameTimer timer; //计时器

    //skill: 使用的技能, targets: 技能的使用目标, callback: 回调
    private Queue<(ISkill skill, List<ModelBase> targets, UnityAction callback)> skills; //技能队列

    public SkillManager()
    {
        timer = new();
        skills = new();
    }

    //添加技能
    public void AddSkill(ISkill skill, List<ModelBase> targets = null, UnityAction callback = null)
    {
        skills.Enqueue((skill, targets, callback));
    }

    //使用技能
    public void UseSkill(ISkill skill, List<ModelBase> targets, UnityAction callback)
    {
        ModelBase curr = (ModelBase)skill;
        //看向一个目标
        if (targets.Count > 0)
        {
            curr.LookAtModel(targets[0]);
        }
        curr.PlaySound(skill.skill.soundEffect); //播放音效
        curr.PlayAni(skill.skill.aniName); //播放动画
        //延迟攻击
        timer.Register(
            skill.skill.atkTime,
            delegate()
            {
                //技能的最多作用个数
                int atkCount =
                    skill.skill.atkCount >= targets.Count ? targets.Count : skill.skill.atkCount;
                for (int i = 0; i < atkCount; i++)
                {
                    targets[i].GetHit(skill); //目标受伤
                }
            }
        );
        //技能的持续时长
        timer.Register(
            skill.skill.time,
            delegate()
            {
                //回到待机状态
                curr.PlayAni("idle");
                callback?.Invoke();
            }
        );
    }

    public void Update(float dt)
    {
        timer.OnUpdate(dt);
        if (timer.Count() == 0 && skills.Count > 0)
        {
            //下一个使用的技能
            var next = skills.Dequeue();
            if (next.targets != null)
            {
                UseSkill(next.skill, next.targets, next.callback);
            }
        }
    }

    //是否正在跑一个技能
    public bool IsRunningSkill()
    {
        if (timer.Count() == 0 && skills.Count == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    //清空技能
    public void Clear()
    {
        timer.Clear();
        skills.Clear();
    }
}
