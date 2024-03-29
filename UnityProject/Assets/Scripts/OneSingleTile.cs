using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OneSingleTile : MonoBehaviour
{
	[SerializeField] Button Button;
	[SerializeField] Button PlusButton;
	[SerializeField] Button MinusButton;
	[SerializeField] Button UpButton;
	[SerializeField] Button DownButton;
	[SerializeField] Text Caption;
	public int PositionInParentDictionary;
	public int PositionInWrittenFile;
	public bool ValidEntry = false;

	private int PreviousPositionInWrittenFile;

    private void Update()
    {
		if (PreviousPositionInWrittenFile != PositionInWrittenFile)
		{
			transform.SetSiblingIndex(PositionInWrittenFile);
		}
		if (transform.GetSiblingIndex() != PositionInWrittenFile)
		{
			PositionInWrittenFile = transform.GetSiblingIndex();
		}
		PreviousPositionInWrittenFile = PositionInWrittenFile;
	}

    public void SetCaptionText( string caption)
	{
		Caption.text = caption;
	}
	public string GetCaptionText()
	{
		return Caption.text;
	}

	public void SetButtonDelegate( System.Action action)
	{
		Button.onClick.AddListener(
			delegate {
				action();
			}
		);
	}

	public void SetPlusButtonDelegate(System.Action action)
	{
		PlusButton.onClick.AddListener(
			delegate {
				action();
			}
		);
	}

	public void SetMinusButtonDelegate(System.Action action)
	{
		MinusButton.onClick.AddListener(
			delegate {
				action();
			}
		);
	}

	public void DisableButton()
	{
		Button.interactable = false;
		PlusButton.gameObject.SetActive(false);
		MinusButton.gameObject.SetActive(false);
		UpButton.gameObject.SetActive(false);
		DownButton.gameObject.SetActive(false);
	}

	public void DisableButtonInteractivity()
	{
		Button.interactable = false;
		PlusButton.interactable = false;
		MinusButton.interactable = false;
		UpButton.interactable = false;
		DownButton.interactable = false;
	}

	public void EnbableButtonInteractivityIfValid()
	{
        if (ValidEntry)
		{
			Button.interactable = true;
			PlusButton.interactable = true;
			MinusButton.interactable = true;
			UpButton.interactable = true;
			DownButton.interactable = true;
		}
	}

	public void ShiftPosition(int zDirection)
    {
		PositionInWrittenFile += zDirection;
	}
}
