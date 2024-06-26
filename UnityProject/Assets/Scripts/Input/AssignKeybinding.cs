using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using TMPro;

public class AssignKeybinding : MonoBehaviour
{

    //Popup text display
    public TextMeshProUGUI BindingText;

    //WinAPI keyboard input
    [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
    private static extern short GetKeyState(int keyCode);

    //Valid inputs with their string counterparts
    //Arr[0] = Numlock On | Arr[2] = Numlock Off
    private Dictionary<KeyCode, string[]> AcceptedKeyCodePairs = new Dictionary<KeyCode, string[]>
    {
        { KeyCode.A, new string[2]{"A", "A" } },
        { KeyCode.B, new string[2]{"B", "B" } },
        { KeyCode.C, new string[2]{"C", "C" } },
        { KeyCode.D, new string[2]{"D", "D" } },
        { KeyCode.E, new string[2]{"E", "E" } },
        { KeyCode.F, new string[2]{"F", "F" } },
        { KeyCode.G, new string[2]{"G", "G" } },
        { KeyCode.H, new string[2]{"H", "H" } },
        { KeyCode.I, new string[2]{"I", "I" } },
        { KeyCode.J, new string[2]{"J", "J" } },
        { KeyCode.K, new string[2]{"K", "K" } },
        { KeyCode.L, new string[2]{"L", "L" } },
        { KeyCode.M, new string[2]{"M", "M" } },
        { KeyCode.N, new string[2]{"N", "N" } },
        { KeyCode.O, new string[2]{"O", "O" } },
        { KeyCode.P, new string[2]{"P", "P" } },
        { KeyCode.Q, new string[2]{"Q", "Q" } },
        { KeyCode.R, new string[2]{"R", "R" } },
        { KeyCode.S, new string[2]{"S", "S" } },
        { KeyCode.T, new string[2]{"T", "T" } },
        { KeyCode.U, new string[2]{"U", "U" } },
        { KeyCode.V, new string[2]{"V", "V" } },
        { KeyCode.W, new string[2]{"W", "W" } },
        { KeyCode.X, new string[2]{"X", "X" } },
        { KeyCode.Y, new string[2]{"Y", "Y" } },
        { KeyCode.Z, new string[2]{"Z", "Z" } },
        { KeyCode.Alpha0, new string[2]{"0", "Numpad0" } },
        { KeyCode.Alpha1, new string[2]{"1", "Numpad1" } },
        { KeyCode.Alpha2, new string[2]{"2", "Numpad2" } },
        { KeyCode.Alpha3, new string[2]{"3", "Numpad3" } },
        { KeyCode.Alpha4, new string[2]{"4", "Numpad4" } },
        { KeyCode.Alpha5, new string[2]{"5", "Numpad5" } },
        { KeyCode.Alpha6, new string[2]{"6", "Numpad6" } },
        { KeyCode.Alpha7, new string[2]{"7", "Numpad7" } },
        { KeyCode.Alpha8, new string[2]{"8", "Numpad8" } },
        { KeyCode.Alpha9, new string[2]{"9", "Numpad9" } },
        { KeyCode.Backspace, new string[2]{"Backspace", "Backspace" } },
        { KeyCode.Tab, new string[2]{ "Tab", "Tab" } },
        { KeyCode.Return, new string[2]{ "Enter", "Enter" } },
        { KeyCode.Space, new string[2]{ "Space", "Space" } },
        { KeyCode.Quote, new string[2]{ "Apostrophe", "Apostrophe" } },
        { KeyCode.Comma, new string[2]{ "Comma", "Comma" } },
        { KeyCode.Minus, new string[2]{ "Minus", "NumpadMinus" } },
        { KeyCode.Period, new string[2]{ "Period", "NumpadPeriod" } },
        { KeyCode.Slash, new string[2]{ "Slash", "NumpadSlash" } },
        { KeyCode.Semicolon, new string[2]{ "Semicolon", "Semicolon" } },
        { KeyCode.Equals, new string[2]{ "Equal", "Equal" } },
        { KeyCode.LeftBracket, new string[2]{ "LBracket", "LBracket" } },
        { KeyCode.RightBracket, new string[2]{ "RBracket", "RBracket" } },
        { KeyCode.Backslash, new string[2]{ "Backslash", "Backslash" } },
        { KeyCode.BackQuote, new string[2]{ "`", "`" } },
        { KeyCode.UpArrow, new string[2]{ "Up", "Up" } },
        { KeyCode.DownArrow, new string[2]{ "Down", "Down" } },
        { KeyCode.LeftArrow, new string[2]{ "Left", "Left" } },
        { KeyCode.RightArrow, new string[2]{ "Right", "Right" } },
        { KeyCode.LeftControl, new string[2]{ "Control", "Control" } },
        { KeyCode.RightControl, new string[2]{ "Control", "Control" } },
        { KeyCode.LeftShift, new string[2]{ "Shift", "Shift" } },
        { KeyCode.RightShift, new string[2]{ "Shift", "Shift" } },
        { KeyCode.LeftAlt, new string[2]{ "Alt", "Alt" } },
        { KeyCode.RightAlt, new string[2]{ "Alt", "Alt" } },
        { KeyCode.CapsLock, new string[2]{ "CapsLock", "CapsLock" } },
        { KeyCode.Numlock, new string[2]{ "Numlock", "Numlock" } },
        { KeyCode.ScrollLock, new string[2]{ "ScrollLock", "ScrollLock" } },
        { KeyCode.Insert, new string[2]{ "Insert", "Insert" } },
        { KeyCode.Delete, new string[2]{ "Delete", "NumpadSeparator" } },
        { KeyCode.Home, new string[2]{ "Home", "Home" } },
        { KeyCode.End, new string[2]{ "End", "End" } },
        { KeyCode.PageUp, new string[2]{ "PageUp", "PageUp" } },
        { KeyCode.PageDown, new string[2]{ "PageDown", "PageDown" } },
        { KeyCode.F1, new string[2]{ "F1", "F1" } },
        { KeyCode.F2, new string[2]{ "F2", "F2" } },
        { KeyCode.F3, new string[2]{ "F3", "F3" } },
        { KeyCode.F4, new string[2]{ "F4", "F4" } },
        { KeyCode.F5, new string[2]{ "F5", "F5" } },
        { KeyCode.F6, new string[2]{ "F6", "F6" } },
        { KeyCode.F7, new string[2]{ "F7", "F7" } },
        { KeyCode.F8, new string[2]{ "F8", "F8" } },
        { KeyCode.F9, new string[2]{ "F9", "F9" } },
        { KeyCode.F10, new string[2]{ "F10", "F10" } },
        { KeyCode.F11, new string[2]{ "F11", "F11" } },
        { KeyCode.F12, new string[2]{ "F12", "F12" } },
        { KeyCode.Print, new string[2]{ "PrintScreen", "PrintScreen" } },
        { KeyCode.Pause, new string[2]{ "Pause", "Pause" } },
        { KeyCode.KeypadMultiply, new string[2]{ "NumpadMultiply", "NumpadMultiply" } },
        { KeyCode.KeypadPlus, new string[2]{ "NumpadPlus", "NumpadPlus" } },
        { KeyCode.Plus, new string[2]{ "NumpadPlus", "NumpadPlus" } },
        { KeyCode.KeypadMinus, new string[2]{ "NumpadMinus", "NumpadMinus" } },
        { KeyCode.Keypad0, new string[2]{ "Numpad0", "Insert" } },
        { KeyCode.Keypad1, new string[2]{ "Numpad1", "End" } },
        { KeyCode.Keypad2, new string[2]{ "Numpad2", "Down" } },
        { KeyCode.Keypad3, new string[2]{ "Numpad3", "PageDown" } },
        { KeyCode.Keypad4, new string[2]{ "Numpad4", "Left" } },
        { KeyCode.Keypad5, new string[2]{ "Numpad5", "Numpad5" } },
        { KeyCode.Keypad6, new string[2]{ "Numpad6", "Right" } },
        { KeyCode.Keypad7, new string[2]{ "Numpad7", "Home" } },
        { KeyCode.Keypad8, new string[2]{ "Numpad8", "Up" } },
        { KeyCode.Keypad9, new string[2]{ "Numpad9", "PageUp" } },
        { KeyCode.KeypadDivide, new string[2]{ "NumpadSlash", "NumpadSlash" } },
        { KeyCode.Escape, new string[2]{ "Escape", "Escape" } }
    };

    private List<KeyCode> SpecialBindings = new() { KeyCode.LeftAlt, KeyCode.RightAlt, KeyCode.LeftControl, KeyCode.RightControl, KeyCode.LeftShift, KeyCode.RightShift };

    private string CurrentSpecialNonKeyBindings;
    private List<KeyCode> CurrentSpecialKeyCodes = new();
    private List<KeyCode> CurrentKeyCodes = new();

    void Update()
    {

        GetKeyInputs();

    }

    private void GetKeyInputs()
    {

        //No inputs being pressed
        if (!Input.anyKey)
        {
            return;
        }

        foreach(KeyCode element in AcceptedKeyCodePairs.Keys)
        {

            //Find which button is pressed
            if (Input.GetKey(element))
            {

                if (SpecialBindings.Contains(element))
                {

                    //Binding is special, EG: shift, alt, ctrl
                    if (CurrentSpecialKeyCodes.Contains(element) == false)
                    {

                        CurrentSpecialKeyCodes.Add(element);

                    }

                }
                else
                {
                    //Generic key EG; abc123

                    //Cannot combine generic keys with Non-Key bindings
                    CurrentSpecialNonKeyBindings = "";

                    //Must be unique
                    if (!CurrentKeyCodes.Contains(element))
                    {

                        //Add input, can only have 1 generic binding key
                        AddOrAmmendBindingToList(CurrentKeyCodes, element);

                    }

                }

                //Update displayed binding on the popup
                UpdateBindingText();

                //Found the pressed key so can now return, no need to support multi-press
                return;

            }
        }
        
    }

    public void SetSpecialKeybinding(string binding)
    {

        //Must be unique
        if (CurrentSpecialNonKeyBindings == binding)
        {
            return;
        }

        //Cannot combine generic keys with Non-Key bindings
        CurrentKeyCodes.Clear();

        //Add input, can only have 1 special non key binding
        CurrentSpecialNonKeyBindings = binding;

        //Display input
        UpdateBindingText();

    }

    //Use for lists that can only contain 1 item
    private void AddOrAmmendBindingToList<A>( List<A> list, A newItem )
    {
        
        if (list.Count == 0)
        {

            //List is empty so use add
            list.Add(newItem);

        }
        else
        {

            //List has entry so replace it
            list[0] = newItem;

        }

    }

    private bool GetNumLock()
    {
        return (((ushort)GetKeyState(0x90)) & 0xffff) != 0;
    }

    void UpdateBindingText()
    {
        //Begin string, ensuring correct syntax
        BindingText.text = "";
        BindingText.text += '"';

        //Get if we're using numlock
        int NumLockOffset;

        if (GetNumLock())
        {

            //Use second array of bindings 
            NumLockOffset = 1;

        }
        else
        {

            NumLockOffset = 0;

        }

        //Firstly add the special keys (shift, ctrl, alt)
        for (int i = 0; i < CurrentSpecialKeyCodes.Count; i++)
        {

            //Add the binding
            BindingText.text += AcceptedKeyCodePairs[CurrentSpecialKeyCodes[i]][0 + NumLockOffset];

            //Breakoff early if there are no other bindings at all to add
            if (i == CurrentSpecialKeyCodes.Count - 1 && CurrentSpecialNonKeyBindings == "" && CurrentKeyCodes.Count == 0)
            {

                break;

            }

            //There is more bindings later on so add the + sign
            BindingText.text += " + ";

        }

        for (int i = 0; i < CurrentKeyCodes.Count; i++)
        {

            //Add the binding
            BindingText.text += AcceptedKeyCodePairs[CurrentKeyCodes[i]][0 + NumLockOffset];

            //Breakoff early if there are no other bindings at all to add
            if (i == CurrentKeyCodes.Count-1 && CurrentSpecialNonKeyBindings == "")
            {

                break;

            }

            //There is more bindings later on so add the + sign
            BindingText.text += " + ";            
        }

        //Assign mouse / screen related bindings at the end
        if(CurrentSpecialNonKeyBindings != "") 
        { 

            BindingText.text += CurrentSpecialNonKeyBindings;

        }

        //Close off string, ensuring correct syntax
        BindingText.text += '"';
        BindingText.text += ",";

    }

    public void ClearKeybinding()
    {

        //Clear the entire binding
        CurrentKeyCodes.Clear();
        CurrentSpecialNonKeyBindings = "";
        CurrentSpecialKeyCodes.Clear();

        //Update the text to reflect recent change
        UpdateBindingText();

    }
}
