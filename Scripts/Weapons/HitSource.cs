using UnityEngine;

public abstract class HitSource : MonoBehaviour {
    protected Fighter wielder;
    protected WeaponStats weaponStats;

    public abstract void HandleCollision(Collider2D coll);
    protected abstract void OnHit(IHittable hitObj, Collider2D coll);
}
