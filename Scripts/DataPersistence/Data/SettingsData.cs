using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SettingsData
{
    public float bgmVolume;
    public float sfxVolume;
    public int resWidth; //chiamare metodi anche se gli attributi sono pubblici
    public int resHeight;
    public int resRefreshRate;
    public bool isFullscreen;
    public SettingsData() {
        bgmVolume = 0;
        sfxVolume = 1;
    }

    public override string ToString() {
        return "SettingsData: bgm volume[" + bgmVolume + "] sfx volume[" + sfxVolume + "] " 
            //+ "Resolution [ " + resolution + "] "
            + "Resolution[" + resWidth + "x" + resHeight + " " + resRefreshRate + "Hz] " 
            + "Fullscreen [" + isFullscreen + "]";
    }
    
    public Resolution GetResolution() {
        return new Resolution {
            width = resWidth,
            height = resHeight,
            refreshRate = resRefreshRate
            };
    }
    public void SetResolution(int width, int height, int refreshRate) {
        resWidth = width;
        resHeight  = height;
        resRefreshRate = refreshRate;
    }

    public void SetResolution(Resolution res) {
        SetResolution(res.width,res.height,res.refreshRate);
        /*
        resWidth = res.width;
        resHeight  = res.height;
        resRefreshRate = res.refreshRate;
        */
    }

}