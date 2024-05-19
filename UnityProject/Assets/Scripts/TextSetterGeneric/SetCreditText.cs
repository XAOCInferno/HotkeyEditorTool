using UnityEngine;

public class SetCreditText : MonoBehaviour
{

    //Simply set the title for the app then destroy this component
    void Start()
    {

        GetComponent<UnityEngine.UI.Text>().text = "Created By " + GlobalConstants.DeveloperRealName + "(" + GlobalConstants.DeveloperAlias + "),                                                                    Tested By: " + GlobalConstants.TesterRealName + " (" + GlobalConstants.TesterAlias + ")";
        Destroy(this);

    }

}
