using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
	[SerializeField] private Button Button;
	[SerializeField] private Button PlusButton;
	[SerializeField] private Button MinusButton;
	[SerializeField] private Button UpButton;
	[SerializeField] private Button DownButton;
	[SerializeField] private Text Caption;

	//Note: Later on make these private
	public int PositionInFile;
	public int IDInDictionary; //Constant, this should never change
	public bool ValidEntry = false;

    private void OnEnable()
    {

		Actions.OnChangeTileLayout += OnPositionsShifting;
		Actions.OnShiftAllTilesUp += ShiftAllEntiresUp;
        Actions.OnShiftAllTilesDown += ShiftAllEntiresDown;

    }

    private void OnDisable()
    {

        Actions.OnChangeTileLayout -= OnPositionsShifting;
        Actions.OnShiftAllTilesUp -= ShiftAllEntiresUp;
        Actions.OnShiftAllTilesDown -= ShiftAllEntiresDown;

    }

    public void SetTilePositionInParentDirectory()
	{

		//Set where this object should be in the grid
        transform.SetSiblingIndex(PositionInFile);

    }

	public void SetCaptionText(string caption)
	{

		Caption.text = caption;

	}

	public string GetCaptionText()
	{

		return Caption.text;

	}

	public void SetButtonDelegate(System.Action action)
	{

		//What should happen when pressing the button?
		Button.onClick.AddListener(
			delegate
			{
				action();
			}
		);

	}

	public void SetPlusButtonDelegate(System.Action action)
	{

        //What should happen when pressing the plus button?
        PlusButton.onClick.AddListener(
			delegate
			{
				action();
			}
		);

	}

	public void SetMinusButtonDelegate(System.Action action)
	{

        //What should happen when pressing the minus button?
        MinusButton.onClick.AddListener(
			delegate
			{
				action();
			}
		);

	}

	public void DisableButton()
	{

		//Tile is a note or syntax, not a keybinding therefore shouldn't have any 
		//Destroy unnecessary game objects
		Destroy(Button);
		Destroy(PlusButton.gameObject);
        Destroy(MinusButton.gameObject);
        Destroy(UpButton.gameObject);
        Destroy(DownButton.gameObject);

	}

	private void SetButtonInteractability(bool status)
    {

        Button.interactable = status;
        PlusButton.interactable = status;
        MinusButton.interactable = status;
        UpButton.interactable = status;
        DownButton.interactable = status;

    }

	public void DisableButtonInteractivity()
	{

		SetButtonInteractability(false);

	}

	public void EnableButtonInteractivityIfValid()
	{

		if (ValidEntry == false)
		{

			return;

		}

		SetButtonInteractability(true);

	}

	public void ShiftPosition(int zDirection)
	{

		Actions.OnChangeTileLayout.InvokeAction(PositionInFile, zDirection);

	}

	private void OnPositionsShifting(int PositionOfTileToMove, int DirectionOfMovement)
	{
		
		if(PositionInFile == PositionOfTileToMove)
		{

            //If we're the tile that's ordered the move, change and update the position
            PositionInFile += DirectionOfMovement;
            SetTilePositionInParentDirectory();

        }
		else if(PositionInFile == PositionOfTileToMove + DirectionOfMovement && DirectionOfMovement != 0)
		{

            //If we're a desired tile to move, not the one that's ordered the move, change and update the position
            PositionInFile -= DirectionOfMovement;
            SetTilePositionInParentDirectory();

        }

    }

	private void ShiftAllEntiresUp(int PositionOfTileToMove)
	{
		if(PositionInFile < PositionOfTileToMove)
		{

			PositionInFile--;
            SetTilePositionInParentDirectory();

        }
    }

    private void ShiftAllEntiresDown(int PositionOfTileToMove)
    {
        if (PositionInFile > PositionOfTileToMove)
        {

            PositionInFile++;
            SetTilePositionInParentDirectory();

        }
    }
}