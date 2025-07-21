using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;
public class SettingsManager : MonoBehaviour {

    [SerializeField] private AudioManager audioManager;
    [SerializeField] private AudioMixer audioMixer;
    [Header("Settings UI")]
    private Dropdown resolutionDropdown;
    private Slider bgmSlider;
    private Text bgmPercentText;
    private Slider sfxSlider;
    private Text sfxPercentText;
    private Slider masterSlider;
    private Text masterPercentText;
    private Toggle fullscreenCheckmark;
    private Toggle floatingTextCheckmark;
    private Toggle enemyHpBarCheckmark;
    private Resolution[] resolutions;
    private bool canPlaySound = false;
    public static SettingsManager instance;
    private void Awake() {
        if(instance != null) {
            Debug.LogWarning("Settings has a duped instance: " + instance);
            return;
        }
        instance = this;
        WebGLModeCheck();
    }
    private void Start() {
        SceneManager.sceneLoaded += OnSceneLoaded;
        
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        WebGLModeCheck();
    }
    private void WebGLModeCheck() {
        #if UNITY_WEBGL
            Debug.Log("WebGL mode is on");
            GameObject quitbtn = GameObject.Find("QuitButton");
            if(quitbtn != null) 
                quitbtn.GetComponent<Button>().interactable = false;

            GameObject res = GameObject.Find("ResolutionDropDown");
            if(res != null) 
                res.GetComponent<Dropdown>().interactable = false;

            GameObject fs = GameObject.Find("FullScreenCheckmark");
            if(fs != null) 
                fs.GetComponent<Toggle>().interactable = false;
                
            if(MenuManager.instance != null) 
                MenuManager.instance.HideQuitBtn(true);
        #endif
    }
    public void SetupSettingsManager(Dropdown res, Slider bgmSlider, Slider sfxSlider, Slider masterSlider, Text bgmPercent, Text sfxPercent, Text masterPercent, 
        Toggle fullscreen, Toggle floatingText, Toggle enemyHpBar) {
        SetupGOs(res,bgmSlider,sfxSlider,bgmPercent,sfxPercent, masterSlider, masterPercent, fullscreen, floatingText, enemyHpBar);
        LoadSettings();
        SetupSettingUI();
        AudioManager.instance.PlayBGM(1);
    }
    public void SetupGOs(Dropdown res, Slider bgmSlider, Slider sfxSlider, Text bgmPercent, Text sfxPercent, Slider masterSlider, Text masterPercent, 
        Toggle fullscreen, Toggle floatingText, Toggle enemyHpBar) { //used in game by gamemanager
        resolutionDropdown = res;
        this.bgmSlider = bgmSlider;
        this.sfxSlider = sfxSlider;
        sfxPercentText = sfxPercent;
        bgmPercentText = bgmPercent;
        this.masterSlider = masterSlider;
        masterPercentText = masterPercent;
        fullscreenCheckmark = fullscreen;
        floatingTextCheckmark = floatingText;
        enemyHpBarCheckmark = enemyHpBar;
    }
    public void SetupSettingUI() {
        //note: 16:9 aspect ratio is the only supported aspect ratio, cause the pixel perfect camera would zoom in and out
        //you can set that settings in -> edit -> project settings -> player -> aspect ratio
        resolutions = Screen.resolutions.Where(resolution => 
            //excluding refresh rates that are different from the player's one
            resolution.refreshRate == Screen.currentResolution.refreshRate 
            //excluding lower resolutions than 1280x720
            //&& (resolution.width >= 1280 || resolution.height >= 720) 
            ).ToArray();
        if(resolutionDropdown != null)
            resolutionDropdown.ClearOptions();
        List<string> options = new();
        int currentResolutionIndex = 0;
        for(int i=0; i < resolutions.Length; i++) {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            //if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height && resolutions[i].refreshRate == Screen.currentResolution.refreshRate) {
            if (resolutions[i].width == PlayerPrefs.GetInt("ResolutionWidth",Screen.currentResolution.width) 
                    && resolutions[i].height == PlayerPrefs.GetInt("ResolutionHeight",Screen.currentResolution.height)  
                    && resolutions[i].refreshRate == PlayerPrefs.GetInt("ResolutionRefreshRate",Screen.currentResolution.refreshRate)) {
                currentResolutionIndex = i;
            }
        }

        if(resolutionDropdown != null) {
            resolutionDropdown.AddOptions(options);
            resolutionDropdown.value = currentResolutionIndex;
            resolutionDropdown.RefreshShownValue();
        }
        canPlaySound = true;
    }
    private bool isFullscreen;
    public void SetFullscreen(bool isFs) {
        Debug.Log("Fullscreen Set " + isFs);
        isFullscreen = isFs;
        Screen.fullScreen = isFs;
    }
    private bool isFloatingTextEnabled = true;
    public void SetFloatingText(bool flag) {
        Debug.Log("Floating Text Set " + flag);
        //set floating text in some way
        isFloatingTextEnabled = flag;
    }
    private bool isEnemyHpBarEnabled = true;
    public void SetEnemyHpBar(bool flag) {
        Debug.Log("Enemy Hp Bar Set " + flag);
        isEnemyHpBarEnabled = flag;
    }
    private Resolution resolution;
    //used by ui dropdown
    public void SetResolution(int resolutionIndex) {
        Debug.Log("Resolution Index [" + resolutionIndex + "] Set: " + resolutions[resolutionIndex].width + "x" + resolutions[resolutionIndex].height + " " + resolutions[resolutionIndex].refreshRate + "Hz");
        resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen, resolution.refreshRate);
        //AdjustCameraSizeBasedOnResolution();
    }
    private void AdjustCameraSizeBasedOnResolution() {
        float targetAspect = 16f / 9f; // L'aspect ratio desiderato
        float currentAspect = (float)Screen.width / Screen.height;
        Debug.Log("Current Aspect: " + currentAspect + " Target Aspect: " + targetAspect);
        Camera.main.orthographicSize = targetAspect / currentAspect;
    }
    public void SetResolutionAndFullscreen(Resolution resolution, bool isFs) {
        Debug.Log("Resolution and Fullscreen Set " + resolution.width + "x" + resolution.height + " " + resolution.refreshRate + "Hz " + isFs);
        isFullscreen = isFs;
        Screen.SetResolution(resolution.width, resolution.height, isFullscreen, resolution.refreshRate);
    }
    public void SetMasterVolume(float value) {
        SetMasterVolumePercentText(masterSlider.value);
        audioMixer.SetFloat("masterVolume",Mathf.Log10(value) * 20f);
        if(canPlaySound) audioManager.PlaySingleSound("Dum",true);
    }
    public void SetMusicVolume(float value) {
        SetMusicVolumePercentText(bgmSlider.value);
        audioMixer.SetFloat("musicVolume",Mathf.Log10(value) * 20f);
        //audioManager.setAudioBGM(bgmSlider.value);
    }
    public void SetEffectsVolume(float value) {
        SetEffectsVolumePercentText(sfxSlider.value);
        //audioManager.setAudioSFX(sfxSlider.value);
        audioMixer.SetFloat("effectsVolume",Mathf.Log10(value) * 20f);
        if(canPlaySound) audioManager.PlaySingleSound("Dum",true);
    }
    //UI Text
    private void SetMasterVolumePercentText(float volume) {
        masterPercentText.text = (int)(volume*100) + "%";
    }
    private void SetEffectsVolumePercentText(float volume) {
        sfxPercentText.text = (int)(volume*100) + "%";
    }
    private void SetMusicVolumePercentText(float volume) {
        bgmPercentText.text = (int)(volume*100) + "%";
    }

    //Settings Data
    public void LoadSettings() {
        Debug.Log("Loading Settings: " + ToString());
        if (sfxSlider != null)  {
            float sfx = PlayerPrefs.GetFloat("SFXVolume",1f);
            sfxSlider.value = sfx;
            SetEffectsVolumePercentText(sfx);
        }
        else
            Debug.LogWarning("sfxSlider is null. Unable to assign sfxVolume.");
        
        if (bgmSlider != null)  {
            float bgm = PlayerPrefs.GetFloat("BGMVolume",1f);
            bgmSlider.value = bgm;
            SetMusicVolumePercentText(bgm);
        }
        else
            Debug.LogWarning("bgmSlider is null. Unable to assign bgmVolume.");

        if(masterSlider != null) {
            float master = PlayerPrefs.GetFloat("MasterVolume",0.5f);
            masterSlider.value = master;
            SetMasterVolumePercentText(master);
        }
        else
            Debug.LogWarning("masterSlider is null. Unable to assign masterVolume.");
        
        isFullscreen = PlayerPrefs.GetInt("Fullscreen",1) == 1;
        isFloatingTextEnabled = PlayerPrefs.GetInt("FloatingText",1) == 1;
        isEnemyHpBarEnabled = PlayerPrefs.GetInt("EnemyHpBar",1) == 1;

        if(fullscreenCheckmark != null)
            fullscreenCheckmark.isOn = isFullscreen;
        if(floatingTextCheckmark != null)
            floatingTextCheckmark.isOn = isFloatingTextEnabled;
        if(enemyHpBarCheckmark != null)
            enemyHpBarCheckmark.isOn = isEnemyHpBarEnabled;
    }

    public void SaveSettings() {        
        
        if(bgmSlider != null) 
            PlayerPrefs.SetFloat("BGMVolume",bgmSlider.value);
        else
            Debug.LogWarning("bgmSlider is null. Save failed.");

        if(sfxSlider != null)
            PlayerPrefs.SetFloat("SFXVolume",sfxSlider.value);
        else
            Debug.LogWarning("sfxSlider is null. Save failed.");
        
        if(masterSlider != null)
            PlayerPrefs.SetFloat("MasterVolume",masterSlider.value);
        else
            Debug.LogWarning("masterSlider is null. Save failed.");

        SaveResolution(resolution);
        PlayerPrefs.SetInt("Fullscreen",isFullscreen ? 1 : 0);
        
        PlayerPrefs.SetInt("FloatingText",isFloatingTextEnabled ? 1 : 0);
        if(FloatingTextManager.instance != null)
            FloatingTextManager.instance.UpdateFloatingTextSetting();

        PlayerPrefs.SetInt("EnemyHpBar",isEnemyHpBarEnabled ? 1 : 0);
        if(WaveManager.instance != null)
            WaveManager.instance.UpdateEnemiesHpFlag();

        Debug.Log("Saving Settings: " + ToString());
    }

    public void DeleteSettings() {
        PlayerPrefs.DeleteAll();
    }

    //Save Methods
    public override string ToString() {
        return "SettingsData: " + "master volume[" + PlayerPrefs.GetFloat("MasterVolume") + "] bgm volume[" + PlayerPrefs.GetFloat("BGMVolume") + "] sfx volume[" + PlayerPrefs.GetFloat("SFXVolume") + "] " 
            //+ "Resolution [ " + resolution + "] "
            + "Resolution[" + PlayerPrefs.GetInt("ResolutionWidth") 
                + "x" + PlayerPrefs.GetInt("ResolutionHeight",Screen.currentResolution.height) 
                + " " + PlayerPrefs.GetInt("ResolutionRefreshRate",Screen.currentResolution.refreshRate) + "Hz] " 
            + "Fullscreen [" + (PlayerPrefs.GetInt("Fullscreen",Screen.fullScreen ? 1 : 0) == 1 ? "true" : "false") + "]"
            + " Floating Text [" + PlayerPrefs.GetInt("FloatingText",1) + "]"
            + " Enemy Hp Bar [" + PlayerPrefs.GetInt("EnemyHpBar",1) + "]";
    }
    
    public Resolution GetResolution() {
        return new Resolution {
            width = PlayerPrefs.GetInt("ResolutionWidth",Screen.currentResolution.width),
            height = PlayerPrefs.GetInt("ResolutionHeight",Screen.currentResolution.height),
            refreshRate = PlayerPrefs.GetInt("ResolutionRefreshRate",Screen.currentResolution.refreshRate)
            };
    }
    private void SaveResolution(int width, int height, int refreshRate) {
        PlayerPrefs.SetInt("ResolutionWidth",width);
        PlayerPrefs.SetInt("ResolutionHeight",height);
        PlayerPrefs.SetInt("ResolutionRefreshRate",refreshRate);
    }
    public void SaveResolution(Resolution res) {
        SaveResolution(res.width,res.height,res.refreshRate);
    }
}
