using UnityEngine;
public class EnemyMeleeWeapon : EnemyProjectileWeapon
{
    [HideInInspector] bool isFirstSwing = true;
    public void invertSwing() {
        isFirstSwing  = !isFirstSwing;
    }
    public bool IsFirstSwing() { return isFirstSwing; }

}