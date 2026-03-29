using UnityEngine;
using UnityEngine.UI;

//选项
public class OptionItem : MonoBehaviour
{
    private OptionData option;

    void Start()
    {
        GetComponent<Button>()
            .onClick.AddListener(
                delegate()
                {
                    GameApp.EventCenter.BroadcastTempEvent(option.eventName); //执行配置表中设置的Event事件
                    GameApp.ViewManager.Close(ViewType.SelectOptionView); //关闭选项界面
                }
            );

        transform.Find("txt").GetComponent<Text>().text = option.name;
    }

    public void Init(OptionData option)
    {
        this.option = option;
    }
}
