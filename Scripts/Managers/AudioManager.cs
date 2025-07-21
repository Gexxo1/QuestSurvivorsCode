using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] public AudioSource sfxSource;
    [SerializeField] public AudioSource bgmSource;
    [SerializeField] public AudioSource sfxSourceTemplate;

    [Header("Various Sound Effects")]
    [SerializeField] private AudioClip potClip;
    [SerializeField] private AudioClip buttonTapClip;
    [SerializeField] private AudioClip[] hitClips;
    [SerializeField] private AudioClip[] hitClips2;
    [SerializeField] private AudioClip[] strongerHitClips;
    [SerializeField] private AudioClip[] coinClips;
    [SerializeField] private AudioClip[] gemClips;
    [Header("Unused Clips")]
    [SerializeField] private AudioClip[] otherClips;
    [SerializeField] private AudioClip[] bowClips;
    [SerializeField] private AudioClip[] swordClips;
    [SerializeField] private AudioClip[] healClips;
    [SerializeField] private AudioClip[] healSkillClips;
    [SerializeField] private AudioClip[] dashClips;
    [SerializeField] private AudioClip[] buyClips;
    [SerializeField] private AudioClip[] closeUIClips;
    [SerializeField] private AudioClip[] hurtClips;
    [SerializeField] private AudioClip[] youcantClips;
    [SerializeField] private AudioClip magnetClip;
    [Header("Background Music")]
    [SerializeField] private AudioClip bgm1;
    [SerializeField] private AudioClip bgm2;
    [SerializeField] private AudioClip bgm3;

    void Awake() {
        if(instance == null)
            instance = this;
        else
            Debug.Log("Duplicate AudioManager instance");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private AudioClip GetRandomClip(AudioClip[] clips) {
        if(clips.Length == 1) 
            return clips[0];
        else if(clips.Length > 1)
            return clips[Random.Range(0, clips.Length)];
        return null;
    }
    //WARNING: this should be used only for single sound effects, like button taps, not with multiple sounds like hits
    public void PlaySingleSound(string s, bool dontOverlap = false, float volume = 1f) {
        //if((sfxSource.isPlaying && dontOverlap) || (sfxSource != null && !sfxSource.enabled))return;
        //else if(sfxSource.isPlaying) sfxSource.Stop();
        if(sfxSource == null) return;
        if(sfxSource.isPlaying && dontOverlap || !sfxSource.enabled) return;
        AudioClip clipToPlay;
        clipToPlay = GetAudioClipByString(s);
        if(clipToPlay == null) return;
        sfxSource.clip = clipToPlay;
        sfxSource.volume = volume;
//        Debug.Log("Playing clip: " + clipToPlay + " volume " + sfxSource.volume);
        sfxSource.Play();
    }

    public void PlaySFX(AudioClip audioClip, Transform spawnTransform, float volume) {
        AudioSource newSource = Instantiate(sfxSourceTemplate, spawnTransform.position, Quaternion.identity);
        newSource.clip = audioClip;
        newSource.volume = volume;
        newSource.Play();
        Destroy(newSource.gameObject, audioClip.length);
    }
    
    public void PlayPooledSFX(string name, Transform sfxTransform, float volume = 1f) {
        AudioClip audioClip = GetAudioClipByString(name);
        ObjectPoolManager.SpawnAudioSource(audioClip, sfxSourceTemplate, name, sfxTransform.position, volume);
    }
    /*
    public void PlaySoundFXClip(AudioClip[] audioClip, Transform spawnTransform, float volume) {
        if(audioClip.Length == 0) { Debug.LogWarning("Audio Clips Are null"); return; }
        int rand = Random.Range(0, audioClip.Length);
        AudioSource newSource = Instantiate(sfxSourceTemplate, spawnTransform.position, Quaternion.identity);
        newSource.clip = audioClip[rand];
        newSource.volume = volume;
        newSource.Play();
        Destroy(newSource.gameObject, audioClip[rand].length);
    }
    */
    public AudioClip GetAudioClipByString(string s) {
        switch(s) {
            case "Hit":
                //return GetRandomClip(hitClips);
                return hitClips[1];
            case "Hit2":
                return GetRandomClip(hitClips2);
            case "Break":
                return potClip;
            case "Dum":
                return buttonTapClip;
            case "Coin":
                return GetRandomClip(coinClips);
            case "Gem":
                return  GetRandomClip(gemClips);
            case "Close":
                return closeUIClips[0];
            case "Youcant":
                return youcantClips[0];
            case "Buy":
                return buyClips[0];
            case "Magnet":
                return magnetClip;
            case "Heal":
                return GetRandomClip(healClips);
            case "Dash": 
                return GetRandomClip(dashClips);
            //weapon shoot clips
            case "Bow":
                return GetRandomClip(bowClips);
            case "Swing1":
                return swordClips[0];
            case "Swing2":
                return swordClips[1];
        }
        Debug.LogWarning("No clip found for " + s);
        return null;
    }
    public void PlayBGM(int id) {
        switch(id) {
            case 0:
                PlayClipBGM(bgm1);
            break;
            case 1:
                PlayClipBGM(bgm2);
            break;
            case 2:
                PlayClipBGM(bgm3);
            break;
            default:
                Debug.LogWarning("No BGM found for id " + id);
            break;
        }
    }
    public void PlayBGMbyMap(Scene s) {
        switch(s.name) {
            case "MainMenu":
                PlayClipBGM(bgm1);
            break;
            case "GrassField":
                PlayClipBGM(bgm2);
            break;
            case "Dungeon1":
                PlayClipBGM(bgm3);
            break;
            default:
                Debug.LogWarning("No BGM found for name: " + s.name);
            break;
        }
    }
    private void PlayClipBGM(AudioClip clip) {
//        Debug.Log("Playing clip: " + clip);
        bgmSource.clip = clip;
        bgmSource.loop = true;
        bgmSource.Play();
    }
    public void setAudioBGM(float audio) {
        bgmSource.volume = audio;
    }
    public void setAudioSFX(float audio) {
        sfxSource.volume = audio;
    }
    public void OnSceneLoaded(Scene s, LoadSceneMode mode) {
//        Debug.Log("puzzola " + s.name + " " + mode.ToString());
        //PlayBGMbyMap(s);
    }
    /*
    //new
    private Queue<AudioSource> availableSources = new Queue<AudioSource>();
    private List<AudioSource> allSources = new List<AudioSource>();
    public void PlaySound(string clipName, bool ecchime = false)
    {
        if (availableSources.Count == 0) {
            // If no sources are available, add a new one
            AudioSource source = gameObject.AddComponent<AudioSource>();
            allSources.Add(source);
            availableSources.Enqueue(source);
        }

        AudioSource audioSource = availableSources.Dequeue();
        audioSource.clip = GetAudioClip(clipName);
        audioSource.Play();

        // Re-add the source to the available queue when it's done playing
        StartCoroutine(AddToAvailableSourcesWhenDone(audioSource));
    }

    private IEnumerator AddToAvailableSourcesWhenDone(AudioSource source)
    {
        yield return new WaitForSeconds(source.clip.length);
        availableSources.Enqueue(source);
    }

    public void StopAllSounds() {
        foreach (AudioSource source in allSources) 
            source.Stop();
    }
    */
}
