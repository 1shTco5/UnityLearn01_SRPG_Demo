using UnityEngine;
using UnityEngine.UI;

public class SettingsView : BaseView
{
    protected override void OnAwake()
    {
        Find<Button>("bg/BtnClose").onClick.AddListener(OnCloseBtn);
        Find<Toggle>("bg/TogMute").onValueChanged.AddListener(OnIsMuteTog);
        Find<Slider>("bg/SliderBGM").onValueChanged.AddListener(OnBGMSlider);
        Find<Slider>("bg/SliderSE").onValueChanged.AddListener(OnSESlider);
    }

    private void OnCloseBtn()
    {
        GameApp.ViewManager.Close(ViewType.SettingsView);
    }

    private void OnIsMuteTog(bool isMute)
    {
        GameApp.SoundManager.IsMute = isMute;
    }

    private void OnBGMSlider(float value)
    {
        GameApp.SoundManager.BGMVolume = value;
    }

    private void OnSESlider(float value)
    {
        GameApp.SoundManager.SEVolume = value;
    }
}
