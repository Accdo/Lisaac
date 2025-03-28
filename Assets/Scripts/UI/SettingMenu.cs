using UnityEngine;
using UnityEngine.UI;

public class SettingMenu : MonoBehaviour
{
    public GameObject SettingMenuWindow;

    // Sound Check Toggle
    public Toggle bgmCheckToggle;
    public Toggle soundeffectCheckToggle;

    // BGM 
    public AudioSource BgmAudioSource;

    //
    public Slider bgmSlider;
    public Slider soundeffectSlider;

    void Update()
    {
        BgmAudioSource.volume = bgmSlider.value;
        SoundManager.Instance.SetSoundVolume(soundeffectSlider.value);
    }

    public void OKButton()
    {
        //BgmAudioSource.volume = bgmSlider.value;
        // SoundManager.Instance.SetSoundVolume(soundeffectSlider.value);

        SettingMenuWindow.SetActive(false);
    }
    public void BgmCheckToggle()
    {
        BgmAudioSource.mute = !BgmAudioSource.mute;
    }
    public void SoundEffectCheckToggle()
    {
        SoundManager.Instance.SoundOnOff();
    }
}
