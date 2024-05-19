using System.Collections;
using UnityEngine;
using SFB;
using Unity.VisualScripting;

public class SaveFile : MonoBehaviour
{
    public bool BlockedFromInteraction = true;
    private UnityEngine.UI.Button selfBtn;

    private void OnEnable()
    {

        Actions.OnDisplayData += SetInteractionUnblocked;
        Actions.OnSetBasicButtonInteractability += SetInteractability;

    }

    private void OnDisable()
    {

        Actions.OnDisplayData -= SetInteractionUnblocked;
        Actions.OnSetBasicButtonInteractability += SetInteractability;

    }

    private void Start()
    {

        gameObject.TryGetComponent(out selfBtn);

    }

    private void SetInteractionUnblocked(string _)
    {

        BlockedFromInteraction = false;

    }

    //Require unique interaction blocker in case no hotkey data has been loaded
    private void SetInteractability(bool status)
    {

        if (BlockedFromInteraction == false)
        {

            selfBtn.interactable = status;

        }
        else
        {

            selfBtn.interactable = false;

        }

    }

    public void OnClickSave()
    {

        StartCoroutine(nameof(ClickSaveAsync));

    }

    private IEnumerator ClickSaveAsync()
    {

        //Ensure buttons are uninteracable before trying to save
        Actions.OnSetBasicButtonInteractability(false);
        yield return new WaitForNextFrameUnit();

        //Open explorer to get path
        string path = StandaloneFileBrowser.SaveFilePanel("Save File", "", "KEYDEFAULTS", "lua");

        if (string.IsNullOrEmpty(path))
        {

            //No more save logic as path is undefined, therefore allow button input again
            Actions.OnSetBasicButtonInteractability(true);
            yield return new WaitForNextFrameUnit();

        }
        else
        {

            Actions.OnSaveFile.InvokeAction(path);

        }

        //Actions.OnSetBasicButtonInteractability.InvokeAction(true);
        yield return null;

    }

}
