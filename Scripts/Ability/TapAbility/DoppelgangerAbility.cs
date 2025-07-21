using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoppelgangerAbility : ActivationAbility //old doppelganger (hold version is used)
{
    [SerializeField] private Doppelganger doppelganger;
    private Doppelganger cloneInstance;

    protected override void AbilityActivation() {
        CreateDoppleganger();
    }

    protected override void AbilityDeactivation() {
        DeactivateDoppelganger();
    }
    private void CreateDoppleganger() {
        if(cloneInstance == null)
            cloneInstance = Instantiate(doppelganger, player.transform.position, Quaternion.identity);
        ActivateDoppelganger();
    }
    private void ActivateDoppelganger() {
        cloneInstance.gameObject.transform.SetPositionAndRotation(player.transform.position, Quaternion.identity);
        cloneInstance.gameObject.SetActive(true);
    }
    private void DeactivateDoppelganger() {
        cloneInstance.gameObject.SetActive(false);
    }
}
