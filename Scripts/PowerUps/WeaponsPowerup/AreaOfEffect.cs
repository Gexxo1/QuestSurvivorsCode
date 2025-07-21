using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaOfEffect : WeaponPowerup
{
    //Hit
    //private CircleCollider2D circleCollider;
    //parametri usati dai figli
    [SerializeField] BulletAreaDamage damageArea;
    public void Start() {
        //circleCollider = GetComponent<CircleCollider2D>();

        spriteRenderer.enabled = false;
        damageArea.gameObject.SetActive(true);
        UpdateArea();
    }
    public override void onUpgrade() {
        base.onUpgrade();
        if(damageArea != null) //se è stato setuppato (caso nel quale faccio upgrade prima di partire)
            UpdateArea();
    }
    private void UpdateArea() {
        float size = stats.weaponSize;
        damageArea.transform.localScale = new Vector3(size, size, 0);
        damageArea.Setup(stats, this);
        //circleCollider.radius = size/2;
    } 
    /*
    protected override void OnHit(Collider2D coll, IHittable hitObj) {
        Fighter target = coll.GetComponent<Fighter>();
        //if(coll.TryGetComponent(out IEffectable e)) e.ApplyMultipleStatus(p.globalWpnStats.statusEffects);
        Damage dmg = GameManager.instance.DamageCalculation(player, player.stats, player.transform.position, target, stats, 
            coll.ClosestPoint(transform.position),DamageType.normal,true,player.globalWpnStats.statusEffects);
        hitObj.getHit(dmg, stats.atkCd);
    }
    */

    public override void AddToStats(WeaponStats statToAdd) {
        base.AddToStats(statToAdd);
        UpdateArea();
    }
}
