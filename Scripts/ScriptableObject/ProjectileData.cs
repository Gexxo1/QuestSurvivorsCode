using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

[CreateAssetMenu(fileName = "ProjectileData", menuName = "ScriptableObjects/ProjectileData")]
public class ProjectileData : ScriptableObject //unused for now
{
    [SerializeField] Bullet projectile;
    [SerializeField] HitParticle hitParticle;
    [SerializeField] SpriteLibraryAsset hitSkin;

    public void InstantiateProjectile() {
        Instantiate(projectile);
    }
}
