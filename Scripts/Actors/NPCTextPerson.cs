using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCTextPerson : Collidable
{
    public string message;
    private Canvas canvas;
    private Text textToReplace;
    [SerializeField] private float cooldown = 4.0f;
    private float lastShout;

    protected override void Start()
    {
        base.Start();
        lastShout = -cooldown;
        canvas = GetComponentInChildren<Canvas>();
        textToReplace = canvas.GetComponentInChildren<Text>();
        textToReplace.text = message;
        canvas.gameObject.SetActive(false);
    }
    protected override void OnCollide(Collider2D coll)
    {
        if(coll.tag != "Player")
            return;
//        Debug.Log(Time.time  + " - " + lastShout + " > " + cooldown);    
        if (Time.time - lastShout > cooldown) {
            lastShout = Time.time;
            StartCoroutine(DialogueText());
            return;
            //GameManager.istance.ShowText(message, 25, Color.white, transform.position + new Vector3(0,0.16f,0), Vector3.zero, cooldown);
        }
    }

    IEnumerator DialogueText()
    {
        //Debug.Log("Coroutine started");
        canvas.gameObject.SetActive(true);
        
        yield return new WaitForSeconds(cooldown);

        //Debug.Log("Coroutine resumed after waiting for " + cooldown + " seconds");
        if(Time.time - lastShout > cooldown)
            canvas.gameObject.SetActive(false);

        yield return null;

        //Debug.Log("Coroutine finished");
    }
}
