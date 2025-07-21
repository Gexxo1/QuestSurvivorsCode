using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonSound : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] public bool playSound = true;
    [SerializeField] public string soundName = "Dum";
    private Button btn;
    void Start()
    {
        if(AudioManager.instance == null) {
            Debug.Log("Audio Manager not present in scene. Button sound will not be played");
            playSound = false;
        }
        btn = GetComponent<Button>();

    }
    public void OnPointerClick(PointerEventData eventData) {
        if(playSound && btn.interactable)
            PlayClickSound();
    }
    void PlayClickSound() {
        AudioManager.instance.PlaySingleSound(soundName,false);
    }
}