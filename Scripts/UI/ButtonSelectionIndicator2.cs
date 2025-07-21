using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonSelectionIndicator2 : MonoBehaviour, ISelectHandler
//, IDeselectHandler
{
    public GameObject selectionIndicator;
    private static ButtonSelectionIndicator2 selectedButton;
    
    private void Deselect()
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
