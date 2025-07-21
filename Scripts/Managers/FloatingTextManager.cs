using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingTextManager : MonoBehaviour
{
    public GameObject textContainer;
    public GameObject textPrefab;
    private List<FloatingText> floatingTexts = new List<FloatingText>();
    private bool isFloatingTextEnabled = false;
    public static FloatingTextManager instance;
    private Canvas canvas;
    private void Awake() {
        if(instance == null)
            instance = this;
        canvas = GetComponent<Canvas>();
    }
    private void Start() {
        UpdateFloatingTextSetting();
    }
    public void UpdateFloatingTextSetting() {
        isFloatingTextEnabled = PlayerPrefs.GetInt("FloatingText",1) == 1;
        canvas.enabled = isFloatingTextEnabled;
    }
    private void Update() {
        foreach (FloatingText txt in floatingTexts)
            txt.UpdateFloatingText();
    }
    public void Show(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration) {
        if(!isFloatingTextEnabled)
            return;
        FloatingText floatingText = GetFloatingText();

        floatingText.txt.text = msg;
        floatingText.txt.fontSize = fontSize;
        floatingText.txt.color = color;

        //floatingText.go.transform.position = position;
        floatingText.go.transform.position = Camera.main.WorldToScreenPoint(position); //world space to screen space, possiamo usarla nella UI
        floatingText.motion = motion;
        floatingText.duration = duration;

        floatingText.Show();
    }
    private FloatingText GetFloatingText() {
        FloatingText txt = floatingTexts.Find(t => !t.active);
        /* corrispettivo di cosa fa la funzione find
        for(int i=0; i < floatingTexts.Count; i++) {
            if(!floatingTexts[i].active)
                ...
        }
        */
        if(txt == null) {
            txt = new FloatingText
            {
                go = Instantiate(textPrefab)
            };
            txt.go.transform.SetParent(textContainer.transform);
            txt.txt = txt.go.GetComponent<Text>();

            floatingTexts.Add(txt);
        }
        return txt;
    }
}
