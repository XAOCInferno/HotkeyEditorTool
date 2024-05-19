using System;

public static class ActionExtensions
{

    //Call action safely
    public static bool InvokeAction(this Action self)
    {

        if (self == null)
        {
            //No subscribers
            return false;

        }

        self.Invoke();
        return true;

    }

    //Call action with 1 arguments safely
    public static bool InvokeAction<A>(this Action<A> self, A arg1)
    {

        if (self == null)
        {

            //No subscribers
            return false;

        }

        self.Invoke(arg1);
        return true;

    }

    //Call action with 2 arguments safely
    public static bool InvokeAction<A, B>(this Action<A, B> self, A arg1, B arg2)
    {

        if (self == null)
        {

            //No subscribers
            return false;

        }

        self.Invoke(arg1, arg2);
        return true;

    }

    //Call action with 3 arguments safely
    public static bool InvokeAction<A, B, C>(this Action<A, B, C> self, A arg1, B arg2, C arg3)
    {

        if (self == null)
        {

            //No subscribers
            return false;

        }

        self.Invoke(arg1, arg2, arg3);
        return true;

    }

    //Call action with 4 arguments safely
    public static bool InvokeAction<A, B, C, D>(this Action<A, B, C, D> self, A arg1, B arg2, C arg3, D arg4)
    {

        if (self == null)
        {

            //No subscribers
            return false;

        }

        self.Invoke(arg1, arg2, arg3, arg4);
        return true;

    }

}

//All static actions that can be called and subscribed to
public static class Actions
{

    //[[Data]]
    public static Action<string> OnDisplayData; //String = File Path
    public static Action<int, string, string> OnAddEntryAfterSpecificEntry; //Int = Previous Entry ID | String = Name Of Binding | String = Actual Binding 
    public static Action<int, int> OnDeleteSpecificEntry; //Int = Entry ID | int = Position In Dictionary

    //[[Loading]]
    public static Action<float> OnSetLoadingPercent; //Float = Loading Percent

    //[[Button]]
    public static Action<bool> OnSetBasicButtonInteractability; //Bool = Is Interactable

    //[[File Management]]
    public static Action<string> OnSaveFile; //String = Filepath

    //[[Popups]]
    //Open
    public static Action<bool, string> OnOpenPleaseWaitPopup; //Bool = Use Loading Bar | String = Reason for waiting
    public static Action<string, string, Tile> OnOpenPopupAsBinding; //String = Keybinding Name | String = Actual Binding Key | Tile = Tile Object 
    public static Action<string, string, int, int> OnOpenDeleteHotkeyPopup; //String = Binding Text | String = Hotkey Name | int = Entry ID | int = Position In Dictionary
    public static Action<int> OnOpenAddHotkeyPopup; //Int = Position from where new binding is being added
    //Close
    public static Action OnClosePleaseWaitPopup;
    public static Action OnCloseSavePopup;

    //[[Tile Movement]]
    public static Action<int, int> OnChangeTileLayout; //Int = Position of tile to change | Int = Direction of movement
    public static Action<int> OnShiftAllTilesUp; //Int = Position where tile is being deleted
    public static Action<int> OnShiftAllTilesDown; //Int = Position where tile is being added

}

