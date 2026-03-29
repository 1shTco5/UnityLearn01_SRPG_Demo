using UnityEngine;

public interface ISkill
{
    SkillProgerty skill { get; set; }

    void ShowSkillArea();

    void HideSkillArea();
}
