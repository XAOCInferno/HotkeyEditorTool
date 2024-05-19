using UnityEngine;

public class OpenURL : MonoBehaviour
{

    //Open the URL for the mod
    public void OpenURLBtn()
    {

        Application.OpenURL(GlobalConstants.ModDBInternetLink);

    }

}
