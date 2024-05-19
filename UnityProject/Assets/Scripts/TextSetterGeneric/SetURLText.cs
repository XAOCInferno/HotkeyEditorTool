using UnityEngine;

public class SetURLText : MonoBehaviour
{
    //Simply set the title for the app then destroy this component
    void Start()
    {

        GetComponent<TMPro.TextMeshProUGUI>().text = GlobalConstants.ModDBInternetLink;
        Destroy(this);

    }
}
