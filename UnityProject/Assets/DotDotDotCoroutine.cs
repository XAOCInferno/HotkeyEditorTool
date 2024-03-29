using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DotDotDotCoroutine : MonoBehaviour
{
    public TextMeshProUGUI Target;
    private int NumberOfDots = 0;

    private void OnEnable()
    {
        Target.text = "";
        StartCoroutine(Fade());
    }

    private void OnDisable()
    {
        StopCoroutine(Fade());
        Target.text = "";
    }

    IEnumerator Fade()
    {
        while(true)
        {
            if(NumberOfDots == 3)
            {
                Target.text = "";
                NumberOfDots = 0;
            }
            else
            {
                Target.text += '.';
                NumberOfDots++;
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
}
