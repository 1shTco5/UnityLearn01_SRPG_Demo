using System;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///技能帮助类
///</summary>
public static class SkillHelper
{
    //目标是否在技能范围内
    public static bool IsModelInSkillArea(this ISkill skill, ModelBase target)
    {
        ModelBase curr = (ModelBase)skill;
        if (curr.GetDist(target) <= skill.skill.atkRange)
        {
            return true;
        }
        return false;
    }

    public static bool IsModelInSkillArea(this ISkill skill, Vector2Int cellPos)
    {
        int row = cellPos.x,
            col = cellPos.y;
        ModelBase curr = (ModelBase)skill;
        if (curr.GetDist(row, col) <= skill.skill.atkRange)
        {
            return true;
        }
        return false;
    }

    //获得技能作用的目标
    public static List<ModelBase> GetTarget(this ISkill skill)
    {
        //0:以鼠标指向的目标为目标
        //1:在攻击范围内的所有目标
        //2:在攻击范围内的英雄的目标
        switch (skill.skill.target)
        {
            case 0:
                return GetTarget_0(skill);
            case 1:
                return GetTarget_1(skill);
            case 2:
                return GetTarget_2(skill);
        }

        return null;
    }

    //0:以鼠标指向的目标为目标
    public static List<ModelBase> GetTarget_0(ISkill skill)
    {
        List<ModelBase> rets = new();
        Collider2D c = Tools.ScreenPointToRay2D(Camera.main, Input.mousePosition);
        if (c != null)
        {
            ModelBase target = c.GetComponent<ModelBase>();
            if (target != null)
            {
                //技能的目标类型 跟 技能指向的目标类型要跟配置表一致
                if (skill.skill.targetType == target.type && skill.IsModelInSkillArea(target))
                {
                    rets.Add(target);
                }
            }
        }
        return rets;
    }

    //1:在攻击范围内的所有目标
    public static List<ModelBase> GetTarget_1(ISkill skill)
    {
        List<ModelBase> rets = new();
        foreach (ModelBase target in GameApp.FightManager.heros)
        {
            //找到在技能范围内的目标
            if (skill.IsModelInSkillArea(target))
            {
                rets.Add(target);
            }
        }
        foreach (ModelBase target in GameApp.FightManager.enemies)
        {
            //找到在技能范围内的目标
            if (skill.IsModelInSkillArea(target))
            {
                rets.Add(target);
            }
        }

        return rets;
    }

    //2:在攻击范围内的英雄的目标
    public static List<ModelBase> GetTarget_2(ISkill skill)
    {
        List<ModelBase> rets = new();
        foreach (ModelBase target in GameApp.FightManager.heros)
        {
            //找到在技能范围内的目标
            if (skill.IsModelInSkillArea(target))
            {
                rets.Add(target);
            }
        }

        return rets;
    }
}
