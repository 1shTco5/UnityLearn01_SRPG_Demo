using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FightSelCharView : BaseView
{
    protected override void OnAwake()
    {
        base.OnAwake();

        Find<Button>("bottom/startBtn").onClick.AddListener(OnBeginFightBtn);
    }

    //选完英雄点击开始战斗 进入玩家回合
    private void OnBeginFightBtn()
    {
        //如果一个英雄都没选 要提示玩家选择 (to do)
        if (GameApp.FightManager.heros.Count == 0)
        {
            Debug.Log("Please select at least ONE hero!");
        }
        else
        {
            GameApp.ViewManager.Close(ViewID); //关闭当前选择界面

            //切换到玩家回合
            GameApp.FightManager.ChangeState(GameState.Player);
        }
    }

    public override void Open(params object[] args)
    {
        GameObject prefabObj = Find("bottom/grid/item");
        Transform gridTf = Find("bottom/grid").transform;

        for (int i = 0; i < GameApp.GameDataManager.heros.Count; i++)
        {
            Dictionary<string, string> data = GameApp
                .ConfigManager.GetConfigData("player")
                .GetDataByID(GameApp.GameDataManager.heros[i]);

            GameObject obj = GameObject.Instantiate(prefabObj, gridTf);
            obj.SetActive(true);
            HeroItem item = obj.AddComponent<HeroItem>();

            item.Init(data);
        }
    }
}
