using UnityEngine;

public class SaveFilePopup : MonoBehaviour
{
    [SerializeField] private GameObject SaveFilePopupView;

    private void OnEnable()
    {

        Actions.OnSaveFile += Open;
        Actions.OnCloseSavePopup += Close;

    }

    private void OnDisable()
    {

        Actions.OnSaveFile -= Open;
        Actions.OnCloseSavePopup -= Close;

    }

    //Open popup then disable all non-popup buttons
    private void Open(string _)
    {

        SaveFilePopupView.SetActive(true);
        Actions.OnSetBasicButtonInteractability(false);

    }

    //Close popup then enable all non-popup buttons
    public void Close()
    {

        SaveFilePopupView.SetActive(false);
        Actions.OnSetBasicButtonInteractability(true);

    }
}
