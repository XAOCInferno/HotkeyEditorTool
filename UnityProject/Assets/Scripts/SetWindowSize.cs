using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetWindowSize : MonoBehaviour
{

    //Currently program does not support dynamic size, simply set it to a reasonable size for most modern devices.
    void Start()
    {

        Screen.SetResolution(1366, 768, FullScreenMode.Windowed);

    }

}
