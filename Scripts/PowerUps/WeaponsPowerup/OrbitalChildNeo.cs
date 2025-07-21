using UnityEngine;


public class OrbitalChildNeo : BulletAreaDamage {
    public void UpdateStats(OrbitalStats stats, OrbitalNeo parent) {
        Setup(stats,parent);
        SetScale(stats.weaponSize);
    }

    private void SetScale(float newScale) {
        transform.localScale = new Vector3(newScale, newScale, 1);
    }
    /*
    protected void OnTriggerEnter2D(Collider2D coll) {
        if(coll.TryGetComponent(out IHittable hitObj)) 
            OnHit(coll,hitObj);
    }
    
    
    protected override void OnHit(IHittable hitObj, Collider2D coll) {
        Fighter target = coll.GetComponent<Fighter>();
        Player p = GameManager.instance.player;
        //if(coll.TryGetComponent(out IEffectable e)) e.ApplyMultipleStatus(p.globalWpnStats.statusEffects);
        Damage dmg = GameManager.instance.DamageCalculation(p, p.stats, p.transform.position,
            target, stats, coll.ClosestPoint(transform.position),DamageType.normal,true,p.globalWpnStats.statusEffects);
        hitObj.getHit(dmg, 0);
    }
    */
}
