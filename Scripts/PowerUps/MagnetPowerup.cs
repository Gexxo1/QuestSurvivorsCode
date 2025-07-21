using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetPowerup : PowerUp
{
    [SerializeField] float sizeIncrease;
    private CircleCollider2D magnetColl;
    private void Start() {
        magnetColl = GameManager.instance.player.inventory.GetMagnet().GetComponent<CircleCollider2D>();
        IncreaseSize();
    }
    public override void onUpgrade() {        
        IncreaseSize();
    }
    /*
    public override void Setup() {
        base.Setup();
        GameObject magnet = GameManager.istance.player.inventory.GetMagnet();
        magnetColl = magnet.GetComponent<CircleCollider2D>();
        IncreaseSize();
    }
    */
    private void IncreaseSize() {
        magnetColl.radius += sizeIncrease;
    }
}
