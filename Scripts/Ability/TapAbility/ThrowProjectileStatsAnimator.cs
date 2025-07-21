using UnityEngine;

public class ThrowProjectileStatsAnimator : ThrowProjectileStats
{
    [Header("Projectile Stats Animator")]
    [SerializeField] protected string animationName;
    protected Animator animator;
    protected override void Start()
    {
        base.Start();
        animator = GameManager.instance.player.inventory.getCurrRootWeapon().GetComponent<Animator>();
    }
    protected override void UseSkill() {
        base.UseSkill();
        animator.SetTrigger(animationName);
    }
}
