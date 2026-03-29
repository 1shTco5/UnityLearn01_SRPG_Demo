using System.Collections.Generic;
using UnityEngine;

public class SelectOptionView : BaseView
{
    private Dictionary<int, OptionItem> opItems;

    protected override void OnAwake()
    {
        base.OnAwake();
    }

    public override void InitData()
    {
        base.InitData();

        opItems = new();
        FightModel fightModel = Controller.GetModel<FightModel>();

        List<OptionData> options = fightModel.options;
        Transform tf = Find("bg/grid").transform;
        GameObject prefabObj = Find("bg/grid/item");

        for (int i = 0; i < options.Count; i++)
        {
            GameObject obj = GameObject.Instantiate(prefabObj, tf);
            obj.SetActive(false);
            OptionItem item = obj.AddComponent<OptionItem>();
            item.Init(options[i]);
            opItems.Add(options[i].id, item);
        }
    }

    public override void Open(params object[] args)
    {
        base.Open(args);

        //传入两个参数
        //一个是英雄的Event字符串 对应的选项id 需要切割字符串
        //第二个参数是角色位置
        //Event 1001-1002-1005
        string[] eventArr = args[0].ToString().Split("-");
        Find("bg/grid").transform.position = (Vector2)args[1];
        foreach (var item in opItems)
        {
            item.Value.gameObject.SetActive(false);
        }

        for (int i = 0; i < eventArr.Length; i++)
        {
            opItems[int.Parse(eventArr[i])].gameObject.SetActive(true);
        }
    }
}
