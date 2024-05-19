using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PleaseWaitPopup : Popup
{
    [SerializeField] private GameObject LoadingPopup;
    [SerializeField] private GameObject LoadingBar;

    private void OnEnable()
    {

        Actions.OnOpenPleaseWaitPopup += OpenLoadingPopup;
        Actions.OnClosePleaseWaitPopup += CloseLoadingPopup;

    }

    private void OnDisable()
    {

        Actions.OnOpenPleaseWaitPopup -= OpenLoadingPopup;
        Actions.OnClosePleaseWaitPopup -= CloseLoadingPopup;

    }
    
    //Enable popup, assign the popup reason as the subheading, optionally show loading bar
    private void OpenLoadingPopup(bool ShowLoadingBar, string LoadingReason)
    {

        BindingTextName.text = LoadingReason;

        LoadingPopup.SetActive(true);
        LoadingBar.SetActive(ShowLoadingBar);

    }

    //Ensure loading bar is child of popup
    private void CloseLoadingPopup()
    {

        LoadingPopup.SetActive(false);

    }
}
