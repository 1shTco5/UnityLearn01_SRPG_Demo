using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MessageInfo
{
    public string text;
    public UnityAction okCallback;
    public UnityAction noCallback;
}

public class MessageView : BaseView
{
    private MessageInfo info;

    protected override void OnAwake()
    {
        Find<Button>("okBtn").onClick.AddListener(OnOkBtn);
        Find<Button>("noBtn").onClick.AddListener(OnNoBtn);
    }

    public override void Open(params object[] args)
    {
        info = args[0] as MessageInfo;
        Find<Text>("content/txt").text = info.text;
    }

    private void OnOkBtn()
    {
        info.okCallback?.Invoke();
    }

    private void OnNoBtn()
    {
        info.noCallback?.Invoke();
        GameApp.ViewManager.Close(ViewID);
    }
}
