using System.Collections;
using UnityEngine;
using TMPro;

public class DotDotDotCoroutine : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Target;
    private const float AnimationRate = 0.5f;

    private void OnEnable()
    {

        //Reset text and begin playing dot dot dot
        Target.text = "";
        StartCoroutine(DotDotDot());

    }

    private void OnDisable()
    {

        StopCoroutine(DotDotDot());

    }

    IEnumerator DotDotDot()
    {

        int NumberOfDots = 0;

        while(true)
        {

            //Wait first so dot doesn't show initially
            yield return new WaitForSeconds(AnimationRate);

            if (NumberOfDots == 3)
            {
                Target.text = "";
                NumberOfDots = 0;
            }
            else
            {
                Target.text += '.';
                NumberOfDots++;
            }
        }
    }
}
