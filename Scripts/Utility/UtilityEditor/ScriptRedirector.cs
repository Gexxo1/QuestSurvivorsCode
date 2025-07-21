using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptRedirector : MonoBehaviour
{
    private MonoBehaviour scriptOnOtherObject;
    [SerializeField] private string methodToCall;
    [SerializeField] protected GameObject interestedGO;
    private void Awake()
    {
        scriptOnOtherObject = getGoScript();
//        Debug.Log(scriptOnOtherObject);
    }
    public void DelegateMethod()
    {
        scriptOnOtherObject.SendMessage(methodToCall);
    }

    public MonoBehaviour getGoScript() {
        return interestedGO.GetComponentInChildren<MonoBehaviour>();
    }

}
