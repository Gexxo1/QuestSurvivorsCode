using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Da non confondere con MagnetCollectable
public class MagnetPickup : Collectable
{
    [Header("Magnet Pickup")]
    [SerializeField] float sizeToSet;
    [SerializeField] float duration;
    private Magnet magnetRef;
    private SpriteRenderer spriteRenderer;
    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    protected override void Start() {
        base.Start();
        magnetRef = GameManager.instance.player.inventory.GetMagnet();
    }
    protected override void OnEnable() {
        base.OnEnable();
        spriteRenderer.enabled = true;
    }

    public override void OnPickup() {
        if(collected) return;
        spriteRenderer.enabled = false;
        IncreaseSize();
        MenuManager.instance.ShowMagnetIndicator(duration);
        AudioManager.instance.PlayPooledSFX("Magnet",transform,1f);
        base.OnPickup(); //coroutine gets called here
    }
    protected override void DestroyCollectable() {
        StartCoroutine(ResetSizeAfterDuration());
    }
    private void IncreaseSize() {
        magnetRef.SetSize(sizeToSet);
    }
    private IEnumerator ResetSizeAfterDuration()
    {
        yield return new WaitForSeconds(duration);
        magnetRef.ResetSize();
        base.DestroyCollectable();
    }
}
