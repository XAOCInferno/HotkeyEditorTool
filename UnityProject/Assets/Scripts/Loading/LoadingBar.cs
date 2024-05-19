using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingBar : MonoBehaviour
{

    [SerializeField] private RectTransform LoadingBarRT;
    [SerializeField] private UnityEngine.UI.Image LoadingBarImage;

    private readonly Color StartingColour = Color.red;
    private readonly Color EndingColour = Color.green;

    private Vector2 StartingSize;

    private void Awake()
    {

        StartingSize = LoadingBarRT.sizeDelta;

    }

    private void OnEnable()
    {

        Actions.OnSetLoadingPercent += SetLoadingPercent;

        //Reset to 0 every time we enable the object so not to display previous load progress
        SetLoadingPercent(0);

    }

    private void OnDisable()
    {

        Actions.OnSetLoadingPercent -= SetLoadingPercent;

    }

    private void SetLoadingPercent(float Percent)
    {

        //Ensure in valid range
        Percent = Mathf.Clamp(Percent, 0, 1);

        //Lerp colour and size
        LoadingBarImage.color = Color.Lerp(StartingColour, EndingColour, Percent);
        LoadingBarRT.sizeDelta = new(StartingSize.x * Percent, StartingSize.y);

    }
}
