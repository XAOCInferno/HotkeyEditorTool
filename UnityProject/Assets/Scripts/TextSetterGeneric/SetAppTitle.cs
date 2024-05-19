using UnityEngine;

public class SetAppTitle : MonoBehaviour
{

    //Simply set the title for the app then destroy this component
    void Start()
    {
        
        GetComponent<UnityEngine.UI.Text>().text = GlobalConstants.ToolName + " v" + GlobalConstants.ToolVersion.ToString();
        Destroy(this);

    }

}
