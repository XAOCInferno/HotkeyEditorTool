using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeleteHotkeyPopup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI BindingText;
    [SerializeField] private TextMeshProUGUI BindingTextName;
    [SerializeField] private GameObject PopupWindow;
    [SerializeField] private KeybindingDisplayAndEditor KDAE;

    private int CurrentEntryToDelete;

    public void Open(string zBindingText, string zBindingTextName, int zEntryID)
    {
        BindingText.text = zBindingText;
        BindingTextName.text = zBindingTextName;
        CurrentEntryToDelete = zEntryID;
        PopupWindow.SetActive(true);
    }

    public void Confirm()
    {
        KDAE.DeleteSpecificEntry(CurrentEntryToDelete);
        Close();
    }

    public void Close()
    {
        PopupWindow.SetActive(false);
        KDAE.EnableAllEntryButtonInput();
    }
}
