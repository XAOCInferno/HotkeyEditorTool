using UnityEngine;
using TMPro;
using System.Linq;

public class AddHotkeyPopup : Popup
{
    [SerializeField] private GameObject PopupWindow;
    [SerializeField] private TMP_InputField BindingNameInput;

    private int PreviousPositionInAllEntries;
    private readonly char[] ValidCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz_".ToCharArray();

    private void OnEnable()
    {

        Actions.OnOpenAddHotkeyPopup += Open;

    }

    private void OnDisable()
    {

        Actions.OnOpenAddHotkeyPopup -= Open;

    }

    public void Open(int zEntryID)
    {

        //Reset popup text
        BindingNameInput.text = "";
        BindingText.text = "Enter Binding Name Here";
        BindingTextName.text = "";

        //Save which entry has been pressed so that we know where to insert the new one
        PreviousPositionInAllEntries = zEntryID;

        //Enable popup
        PopupWindow.SetActive(true);

    }

    public void Confirm()
    {

        string TextToUse = "";

        foreach (char Character in BindingText.text)
        {

            //Convert spaces to underscores, spaces not supported
            if (char.IsWhiteSpace(Character))
            {

                TextToUse += '_';

            }
            else if (LetterIsValid(Character))
            {

                //If letter is a valid character then add it to the binding 
                TextToUse += Character;

            }

        }

        if(TextToUse.Length > 0)
        {

            //Order the desired binding to be instantiated 
            Actions.OnAddEntryAfterSpecificEntry.InvokeAction(PreviousPositionInAllEntries, TextToUse, BindingTextName.text);

            //Close popup as a successfully completed entry
            Close(false);

        }

    }

    public void Close(bool CloseFromCancelButton = true)
    {

        //Close popup
        PopupWindow.SetActive(false);

        if (CloseFromCancelButton)
        {

            //If closed from cancel need to reactivate the buttons as no other logic will be called
            Actions.OnSetBasicButtonInteractability.InvokeAction(true);

        }

    }

    //Note: Can probably be removed as I don't envison any other rules for if is valid except for being contained in list
    private bool LetterIsValid(char zCharacter)
    {

        return ValidCharacters.Contains(zCharacter);

    }

}
