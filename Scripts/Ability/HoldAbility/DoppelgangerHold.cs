using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoppelgangerHold : HoldAbility
{
    [SerializeField] private Doppelganger doppelganger;
    private Doppelganger cloneInstance;
    protected override void UseSkill() {
        base.UseSkill();
        CreateDoppleganger();
    }
    protected override void DeactivateSkill() {
        base.DeactivateSkill();
        //note: cooldown must be ideally at least 1.5f seconds
        StartCoroutine(DeactivateDoppelgangerWitDelay(cooldown-0.5f));
        SetLastUsed();
    }
    private void CreateDoppleganger() {
        if(cloneInstance == null)
            cloneInstance = Instantiate(doppelganger, player.transform.position, Quaternion.identity);
        ActivateDoppelganger();
    }

    public void ActivateDoppelganger() {
        cloneInstance.gameObject.transform.SetPositionAndRotation(player.transform.position, Quaternion.identity);
        cloneInstance.gameObject.SetActive(true);
    }
    private IEnumerator DeactivateDoppelgangerWitDelay(float delay) {
        yield return new WaitForSeconds(delay);
        cloneInstance.gameObject.SetActive(false);
    }
}
