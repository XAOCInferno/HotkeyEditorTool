using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SFB;

public class OpenFile : MonoBehaviour
{
    public UnityEngine.UI.Button SaveButton;
    public KeybindingDisplayAndEditor KDAE;

    private UnityEngine.UI.Button selfBtn;

    private void Start()
    {
        gameObject.TryGetComponent(out selfBtn);
    }

    public void OnClickOpen()
    {
        selfBtn.interactable = false;
        string[] paths = StandaloneFileBrowser.OpenFilePanel("Open File", "", "lua", false);
        if (paths.Length > 0)
        {
            KDAE.DisplayFileData(paths[0]);
            SaveButton.interactable = true;
        }
        selfBtn.interactable = true;
    }
}