using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AddHotkeyPopup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI BindingText;
    [SerializeField] private TextMeshProUGUI BindingTextName;
    [SerializeField] private GameObject PopupWindow;
    [SerializeField] private KeybindingDisplayAndEditor KDAE;
    [SerializeField] private TMP_InputField BindingNameInput;

    private int PreviousPositionInAllEntries;
    private char[] ValidCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz_".ToCharArray();
    public void Open(int zEntryID)
    {
        BindingNameInput.text = "";
        BindingText.text = "Enter Binding Name Here";
        BindingTextName.text = "";
        PreviousPositionInAllEntries = zEntryID;
        PopupWindow.SetActive(true);
    }

    public void Confirm()
    {
        string TextToUse = "";
        foreach (char Character in BindingText.text)
        {
            if (char.IsWhiteSpace(Character))
            {
                TextToUse += '_';
            }
            else if (LetterIsValid(Character))
            {
                TextToUse += Character;
            }
        }
        if(TextToUse.Length > 0)
        {
            KDAE.AddEntryAfterSpecificEntry(PreviousPositionInAllEntries, TextToUse, BindingTextName.text);
            Close(false);
        }
    }

    public void Close(bool CloseFromButton = true)
    {
        BindingText.text = "";
        BindingTextName.text = "";
        PopupWindow.SetActive(false);
        if (CloseFromButton)
        {
            KDAE.EnableAllEntryButtonInput();
        }
    }

    private bool LetterIsValid(char zCharacter)
    {
        foreach(char letter in ValidCharacters)
        {
            if(letter == zCharacter)
            {
                return true;
            }
        }
        return false;
    }
}
