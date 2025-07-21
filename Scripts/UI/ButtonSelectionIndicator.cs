using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonSelectionIndicator : MonoBehaviour, ISelectHandler
//, IDeselectHandler
{
    public GameObject selectionIndicator;
    [HideInInspector] public static ButtonSelectionIndicator selectedButton;
    /*
    private void Start()
    {
        if (selectedButton != null && selectedButton != this)
        {
            selectedButton.Deselect();
        }
    }
    
    public void OnDeselect(BaseEventData eventData)
    {
        Deselect();
    }
    */
    
    public void Deselect()
    {
//        Debug.Log("Deselected [" + name + "]");
        selectionIndicator.SetActive(false);
        selectedButton = null;
    }

    public void OnSelect(BaseEventData eventData)
    {   
        //Debug.Log("Selected [" + name + "] - Current Selected Button [" + selectedButton + "] This [" + this + "]");
        if (selectedButton != null && selectedButton != this)
        {
            //Debug.Log("Deselecting from OnSelect [" + selectedButton.name + "]");
            selectedButton.Deselect();
        }

        selectionIndicator.SetActive(true);
        selectedButton = this;
    }
    
    private void OnDisable()
    {
        Deselect();
    }
}
