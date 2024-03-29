using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChangeHKPopup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI BindingText;
    [SerializeField] private TextMeshProUGUI BindingTextName;
    [SerializeField] private GameObject PopupWindow;
    [SerializeField] private AssignKeybinding AK;
    [SerializeField] private KeybindingDisplayAndEditor KDAE;

    private OneSingleTile CurrentLinkedEntry;

    public void OpenPopupAsBinding(string BindingName, string BindingKey, OneSingleTile LinkedEntry)
    {
        BindingTextName.text = BindingName;
        BindingText.text = '"'.ToString();
        BindingText.text += BindingKey;
        BindingText.text += '"'.ToString();
        BindingText.text += ",";
        CurrentLinkedEntry = LinkedEntry;
        PopupWindow.SetActive(true);
        AK.enabled = true; 
    }

    public void SaveKeybindingAndClose()
    {        
        CurrentLinkedEntry.SetCaptionText("    " + BindingTextName.text + " = " + BindingText.text);
        AK.ClearKeybinding();
        AK.enabled = false;
        PopupWindow.SetActive(false);
        KDAE.EnableAllEntryButtonInput();
    }
}
