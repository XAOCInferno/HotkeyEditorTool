using UnityEngine;

public class ChangeHKPopup : Popup
{
    [SerializeField] private GameObject PopupWindow;
    [SerializeField] private AssignKeybinding KeybindingInput;

    private Tile CurrentLinkedEntry;

    private void OnEnable()
    {

        Actions.OnOpenPopupAsBinding += OpenPopupAsBinding;

    }

    private void OnDisable()
    {

        Actions.OnOpenPopupAsBinding -= OpenPopupAsBinding;

    }

    public void OpenPopupAsBinding(string BindingName, string BindingKey, Tile LinkedEntry)
    {

        //Set correct text based on entry 
        BindingTextName.text = BindingName;
        BindingText.text = '"' + BindingKey + '"' + ',';

        //Save which button is currently pressed
        CurrentLinkedEntry = LinkedEntry;

        //Enable popup
        PopupWindow.SetActive(true);

        //Enable input to change the binding
        KeybindingInput.enabled = true; 

    }

    public void SaveKeybindingAndClose()
    {

        //Save change to linked entry
        CurrentLinkedEntry.SetCaptionText("    " + BindingTextName.text + " = " + BindingText.text);

        //Disable keybinding input, must be done after setting linked entry caption
        KeybindingInput.ClearKeybinding();
        KeybindingInput.enabled = false;

        //Close popup
        PopupWindow.SetActive(false);

        //Enable button interaction
        Actions.OnSetBasicButtonInteractability.InvokeAction(true);

    }
}
