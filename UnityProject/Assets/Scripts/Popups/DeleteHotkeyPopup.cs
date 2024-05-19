using UnityEngine;

public class DeleteHotkeyPopup : Popup
{
    [SerializeField] private GameObject PopupWindow;

    private int CurrentEntryToDelete;
    private int CurrentEntryToDeletePositionInDict;

    private void OnEnable()
    {

        Actions.OnOpenDeleteHotkeyPopup += Open;

    }

    private void OnDisable()
    {

        Actions.OnOpenDeleteHotkeyPopup -= Open;

    }

    public void Open(string zBindingText, string zBindingTextName, int zEntryID, int zEntryIDInDict)
    {

        //Set correct text based on entry 
        BindingText.text = zBindingText;
        BindingTextName.text = zBindingTextName;

        //Save entry to delete for later
        CurrentEntryToDelete = zEntryID;
        CurrentEntryToDeletePositionInDict = zEntryIDInDict;

        //Enable popup window
        PopupWindow.SetActive(true);

    }

    public void Confirm()
    {

        //Delete the clicked entry
        Actions.OnDeleteSpecificEntry(CurrentEntryToDelete, CurrentEntryToDeletePositionInDict);

        //Close the popup
        Close();

    }

    public void Close()
    {

        //Disable the popup
        PopupWindow.SetActive(false);

        //Enable button input
        Actions.OnSetBasicButtonInteractability.InvokeAction(true);

    }
}
