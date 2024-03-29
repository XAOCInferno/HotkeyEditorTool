using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System.Linq;

public class GetText : MonoBehaviour
{
    public Transform contentWindow;
    public GameObject recallTextObject;

    private string devFile = "Assets/test";
    private string fileType = ".lua";

    // Start is called before the first frame update
    void Start()
    {
        List<string> fileLines = File.ReadAllLines(devFile + fileType).ToList();

        for(int i = 0; i < fileLines.Count; i++)
        {
            Instantiate(recallTextObject, contentWindow);
            recallTextObject.GetComponent<Text>().text = fileLines[i];
        }
    }
}
