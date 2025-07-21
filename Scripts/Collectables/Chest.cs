using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Collectable
{
    [SerializeField] private Sprite openChest;
    //On Collect
    //private int goldAmount;
    //public int rangeGoldMax = 5;
    //private Vector3 pos;
    [SerializeField] private bool guaranteeOneDrop = false;
    [SerializeField] private Drop[] drop;
    private SpriteRenderer spriterRenderer;
    protected override void Start() {
        base.Start();
        //pos = GetComponent<Transform>().position;
        spriterRenderer = GetComponent<SpriteRenderer>();
    }
    public override void OnPickup()
    {
        if(collected) 
            return;
        if(AudioManager.instance != null)
            AudioManager.instance.PlayPooledSFX("Dum",transform);

        spriterRenderer.sprite = openChest;

        if(guaranteeOneDrop)
            GameManager.instance.dropOneGuaranteeItem(drop,transform.position,transform.localScale/2);
        else
            GameManager.instance.dropOneItem(drop,transform.position,transform.localScale/2);
        collected = true;
    }
}
