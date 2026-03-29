using System.Collections.Generic;
using UnityEngine;

public class SoundManager
{
    private AudioSource bgmSource;

    //音频缓存字典
    private Dictionary<string, AudioClip> clips;

    private bool isMute; //是否静音

    public bool IsMute
    {
        get { return isMute; }
        set
        {
            isMute = value;
            if (isMute)
            {
                bgmSource.Pause();
            }
            else
            {
                bgmSource.Play();
            }
        }
    }

    private float bgmVolume;
    public float BGMVolume
    {
        get { return bgmVolume; }
        set
        {
            bgmVolume = value;
            bgmSource.volume = bgmVolume;
        }
    }

    private float seVolume;
    public float SEVolume
    {
        get { return seVolume; }
        set { seVolume = value; }
    }

    public SoundManager()
    {
        clips = new();
        bgmSource = GameObject.Find("Game").GetComponent<AudioSource>();
    }

    public void PlayBGM(string res)
    {
        if (res == null || isMute)
            return;
        if (!clips.ContainsKey(res))
        {
            AudioClip clip = Resources.Load<AudioClip>($"Sounds/{res}");
            clips.Add(res, clip);
        }
        bgmSource.clip = clips[res];
        bgmSource.Play();
    }

    public void PlaySE(string res, Vector3 pos)
    {
        if (res == null || isMute)
        {
            return;
        }
        if (!clips.ContainsKey(res))
        {
            AudioClip clip = Resources.Load<AudioClip>($"Sounds/{res}");
            clips.Add(res, clip);
        }
        AudioSource.PlayClipAtPoint(clips[res], pos);
    }
}
