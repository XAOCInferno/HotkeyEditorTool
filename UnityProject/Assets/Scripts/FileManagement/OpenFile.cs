using System.Collections;
using UnityEngine;
using SFB;
using Unity.VisualScripting;

public class OpenFile : MonoBehaviour
{
    private UnityEngine.UI.Button selfBtn;

    private void OnEnable()
    {

        Actions.OnSetBasicButtonInteractability += SetInteractability;

    }

    private void OnDisable()
    {

        Actions.OnSetBasicButtonInteractability += SetInteractability;

    }

    private void Start()
    {

        gameObject.TryGetComponent(out selfBtn);

    }

    private void SetInteractability(bool status)
    {

        selfBtn.interactable = status;

    }

    public void OnClickOpen()
    {

        StartCoroutine(nameof(OpenFileAsync));

    }

    private IEnumerator OpenFileAsync()
    {

        //Disable buttons, prevents double opening of file
        Actions.OnSetBasicButtonInteractability.InvokeAction(false);

        //Open file will pause the app 
        //Ensure we open the popup before doing file logic
        Actions.OnOpenPleaseWaitPopup.InvokeAction(false, "Selecting File");
        yield return new WaitForNextFrameUnit();

        //Get path, program will pause until path is got by user
        string[] paths = StandaloneFileBrowser.OpenFilePanel("Open File", "", "lua", false);

        //If path = 0 user pressed cancel
        if (paths.Length != 0)
        {

            //App only supports 1 file at a time currently so we only care about the first file in the array
            Actions.OnDisplayData.InvokeAction(paths[0]);

        }
        else
        {

            //No file paths selected so no more logic will occur, close popup from here
            Actions.OnClosePleaseWaitPopup.InvokeAction();
            Actions.OnSetBasicButtonInteractability.InvokeAction(true);

        }

        yield return null;

    }

}