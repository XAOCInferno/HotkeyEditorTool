using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenURL : MonoBehaviour
{
    public void OpenURLBtn()
    {
        Application.OpenURL("https://www.moddb.com/mods/hotkey-editor-tool");
    }
}
