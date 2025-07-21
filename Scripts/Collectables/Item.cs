using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("Regular Item")]
    public string title = "title not set";
    [TextArea(2,4)] public string description = "description not set";
    public string note = "note not set";
    public virtual string GetDescription() {
        string noteD = "";
        if(note != "note not set")
            noteD = "\n<i>*" + note + "</i>";
        return description + "\n" + noteD;
    }

    public Sprite GetSprite() {
        return gameObject.GetComponent<SpriteRenderer>().sprite;
    }

    public override string ToString() {
        return "Title [" + title + "] description [" + description + "] ";
    }

    public virtual string GetTitle() { //to do: make title private and change all references to GetTitle
        return title;
    }

    

}
