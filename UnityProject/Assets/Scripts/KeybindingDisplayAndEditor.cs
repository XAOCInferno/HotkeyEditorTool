using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System.Linq;

public struct Keybind
{
	public string KeyBindVariable;
	public string KeyBindValue;
    public bool IsAKeyBinding;

    public Keybind(string _KeyBindVariable = "", string _KeyBindValue = "")
	{
		KeyBindVariable = _KeyBindVariable;
		KeyBindValue = _KeyBindValue;

		if(KeyBindValue != "")
		{

            IsAKeyBinding = true;

		}
		else
		{

            IsAKeyBinding = false;

		}

	}
}

public class KeybindingDisplayAndEditor : MonoBehaviour
{
	[SerializeField] private Tile ExampleTile;

	[SerializeField] private RectTransform ParentWindow;
	[SerializeField] private Scrollbar GridViewScrollBar;

	private Dictionary<int, Tile> AllEntries = new();
	private char[] Ignore = new char[4] { '{', '}', '=', '-' };
	private float[] ParentWindowOriginalYOffset;
	private int NextUniqueID;
	private string OpenedPath;
	private float LoadingUpdateRate = 0.025f;
	private GridLayoutGroup LayoutGroupForEntries;

	private const int IndexOfIdentifiableWatermark = 3; // -- Created using Dawn of War Hotkey Editor Tool. https://www.moddb.com/mods/hotkey-editor-tool",
    private readonly string[] Watermark = new string[6]
	{
		"----------------------------------------------------------------------------------------------------------------",
		"",
		"        ----------------------------------------------------------------------------------------------------------------",
		"        -- Created using Dawn of War Hotkey Editor Tool. https://www.moddb.com/mods/hotkey-editor-tool",
		"        ----------------------------------------------------------------------------------------------------------------",
		""
	};

	Tile InstantiateNewExampleTile(int PositionInFile, int IDInDictionary)
	{

		//Create new tile, assign its position in the file, then activate it
		Tile newTile = Instantiate(ExampleTile, ExampleTile.transform.parent);

		newTile.PositionInFile = PositionInFile;
		newTile.IDInDictionary = IDInDictionary;

		newTile.gameObject.SetActive(true);

		return newTile;

	}

	private void Awake()
	{

		//Find the top of parent window
		ParentWindowOriginalYOffset = new float[2] { ParentWindow.offsetMin[1], ParentWindow.offsetMax[1] };

		//Find the grid in parent window
		ParentWindow.TryGetComponent(out LayoutGroupForEntries);

		//Disable the example tile which is not being used
		ExampleTile.gameObject.SetActive(false);

	}

	private void OnEnable()
	{

		Actions.OnDisplayData += DisplayFileData;
		Actions.OnSetBasicButtonInteractability += SetAllEntryButtonInput;
		Actions.OnDeleteSpecificEntry += DeleteSpecificEntry;
		Actions.OnAddEntryAfterSpecificEntry += AddEntryAfterSpecificEntry;
		Actions.OnSaveFile += SaveFile;

	}

	private void OnDisable()
	{

		Actions.OnDisplayData -= DisplayFileData;
		Actions.OnSetBasicButtonInteractability -= SetAllEntryButtonInput;
		Actions.OnDeleteSpecificEntry -= DeleteSpecificEntry;
		Actions.OnAddEntryAfterSpecificEntry -= AddEntryAfterSpecificEntry;
		Actions.OnSaveFile -= SaveFile;

	}

	public void ClearOldEntries()
	{

		//Change popup message and enable loading bar
		Actions.OnOpenPleaseWaitPopup(true, "Deleting Previous Data");

		//Reset scrollbar and next free ID
		GridViewScrollBar.value = 0;
		NextUniqueID = 0;

		//Reset window offset
		ParentWindow.offsetMin = new Vector2(ParentWindow.offsetMin[0], ParentWindowOriginalYOffset[0]);
		ParentWindow.offsetMax = new Vector2(ParentWindow.offsetMax[0], ParentWindowOriginalYOffset[1]);

		//Gradually delete tiles so not to freeze app
		StartCoroutine(nameof(ClearOldEntriesAsync));

	}

	private IEnumerator ClearOldEntriesAsync()
	{

		int updateRateForModulus = Mathf.RoundToInt(AllEntries.Count * LoadingUpdateRate);

		//Destroy all tiles
		foreach (Tile entry in AllEntries.Values)
		{

			//Waits for every N entry, letting the rest of the program update
			if (entry.PositionInFile % updateRateForModulus == 0 && entry.PositionInFile != 0)
			{

				Actions.OnSetLoadingPercent.InvokeAction((float)entry.PositionInFile / AllEntries.Count);
				yield return new WaitForEndOfFrame();

			}

			Destroy(entry.gameObject);

		}

		//Clear tiles from dictionary
		AllEntries.Clear();

		//Display new tiles now that we've cleared out the old ones
		StartCoroutine(nameof(DisplayFileDataAsync));

	}


	private void SetAllEntryButtonInput(bool status)
	{

		if (status)
		{

			EnableAllEntryButtonInput();

		}
		else
		{

			DisableAllEntryButtonInput();

		}

	}

	public void DisableAllEntryButtonInput()
	{

		foreach (KeyValuePair<int, Tile> entry in AllEntries)
		{

			entry.Value.DisableButtonInteractivity();

		}

	}

	public void EnableAllEntryButtonInput()
	{

		foreach (KeyValuePair<int, Tile> entry in AllEntries)
		{

			entry.Value.EnableButtonInteractivityIfValid();

		}

	}

	private IEnumerator DisplayFileDataAsync()
	{

        //Update message to show we are now loading the data
        Actions.OnOpenPleaseWaitPopup(true, "Loading Data");

        //Get all lines in the file
        List<string> fileLines = File.ReadAllLines(OpenedPath).ToList();

		//Calculate how often we should wait for end of frame so to stop program from freezing up
        int updateRateForModulus = Mathf.RoundToInt(fileLines.Count * LoadingUpdateRate);

		//Iterate over file lines and create a new tile for each
		for (int i = 0; i < fileLines.Count; i++)
		{

            //Generate tile and disable interation with the button while we set it up
            Tile entry = InstantiateNewExampleTile(i, NextUniqueID);
            entry.DisableButtonInteractivity();

			//Add to dictionary
            AllEntries.Add(NextUniqueID, entry);

            //Increment unique ID for the next tile, must be done after adding to dictionary
            NextUniqueID++;

			//Convert file line into a usable binding pair
			Keybind binding = GetKeybindingAndBindingName(fileLines[i]);

			//Assign display text for the entry
            entry.SetCaptionText(fileLines[i]);

			//Ensure tile is positioned correctly in the grid
			entry.SetTilePositionInParentDirectory();


			if (binding.IsAKeyBinding)
			{

				//Entry is a keybinding so needs interactability
				SetupTileInteraction(entry, binding);

			}
			else
			{

				//Entry is not a keybinding so should have no interactability 
				entry.DisableButton();

			}

			//Waits for every N entry, letting the rest of the program update
			if (i % updateRateForModulus == 0 && i != 0)
			{

				//Send a message to the loading bar to update its view
				Actions.OnSetLoadingPercent.InvokeAction((float)i / fileLines.Count);

				//Wait for message + rest of app to be processed before continuing
				yield return new WaitForEndOfFrame();

			}

		}

		//Change offset of grid to ensure all tiles fit
		ParentWindow.offsetMin = new Vector2(ParentWindow.offsetMin[0], ParentWindow.offsetMin[1] - (LayoutGroupForEntries.cellSize[1] + LayoutGroupForEntries.spacing[1]) * fileLines.Count);

		//Close please wait popup and enable button interaction
		Actions.OnClosePleaseWaitPopup.InvokeAction();
		Actions.OnSetBasicButtonInteractability(true);

		yield return null;

	}

	public void DisplayFileData(string path)
	{

		Actions.OnOpenPleaseWaitPopup.InvokeAction(false, "Selecting File");
		OpenedPath = path;
		ExampleTile.gameObject.SetActive(false);
		ClearOldEntries();

	}

	public void OrderDeleteSpecificEntry(Tile zEntry)
	{
		Keybind bindingCaption = GetKeybindingAndBindingName(zEntry.GetCaptionText());
		Actions.OnOpenDeleteHotkeyPopup.InvokeAction(bindingCaption.KeyBindValue, bindingCaption.KeyBindVariable, zEntry.PositionInFile, zEntry.IDInDictionary);
	}

	public void DeleteSpecificEntry(int EntryID, int PositionInDict)
	{

		//Destroy the targeted tile then remove the now null object from dictionary
		Destroy(AllEntries[PositionInDict].gameObject);
		AllEntries.Remove(PositionInDict);

		//Move tiles upwards to fill gap left by destroyed tile
		Actions.OnShiftAllTilesUp.InvokeAction(EntryID);

	}

	public void OrderMakeNewEntry(Tile zEntry)
	{
		Actions.OnOpenAddHotkeyPopup.InvokeAction(zEntry.PositionInFile);
	}

	public void AddEntryAfterSpecificEntry(int PreviousEntryID, string BindingName, string Binding)
	{

		//Firstly make space for the new tile
		Actions.OnShiftAllTilesDown.InvokeAction(PreviousEntryID);

		//Create button and disable it immedietly 
		Tile entry = InstantiateNewExampleTile(PreviousEntryID + 1, NextUniqueID);
		entry.DisableButtonInteractivity();

		//Calculate keybinding and name of binding then assign its caption
		string line = "    " + BindingName + " = " + '"' + Binding + '"' + ',';
		Keybind binding = GetKeybindingAndBindingName(line);
		entry.SetCaptionText(line);

		//Add to dictionary
		AllEntries.Add(NextUniqueID, entry);

		//Shift all tiles beneath new tile down then insert new tile in the correct position 
		Actions.OnChangeTileLayout.InvokeAction(entry.PositionInFile, 0);

		//Increment Unique ID
		NextUniqueID++;

		//Assign on press behaviour to the tile and its buttons
		SetupTileInteraction(entry, binding);

		//Open tile we just made to assign its binding
		Actions.OnOpenPopupAsBinding.InvokeAction(binding.KeyBindVariable, binding.KeyBindValue, entry);
	}

	private void SetupTileInteraction(Tile entry, Keybind binding)
	{

		{

			//The entry should be interactable
			entry.ValidEntry = true;

			//Button to open edit popup
			entry.SetButtonDelegate(
				() =>
				{
					Actions.OnSetBasicButtonInteractability.InvokeAction(false);
					Actions.OnOpenPopupAsBinding.InvokeAction(binding.KeyBindVariable, binding.KeyBindValue, entry);
				}
			);

			//Button to add entry
			entry.SetPlusButtonDelegate(
				 () =>
				 {
					 Actions.OnSetBasicButtonInteractability.InvokeAction(false);
					 OrderMakeNewEntry(entry);
				 }
			 );

			//Button to destroy entry
			entry.SetMinusButtonDelegate(
				  () =>
				  {
					  Actions.OnSetBasicButtonInteractability.InvokeAction(false);
					  OrderDeleteSpecificEntry(entry);
				  }
			  );
		}

	}

	public void SaveFile(string path)
	{

		bool HasAlreadyGotWatermark = false;
		string[] AllLines = new string[NextUniqueID + 1];

		//Iterate over every tile and add its caption to the saving array
		//Caption is formated correctly for bindings so does not need any manipulation
		for (int i = 0; i <= NextUniqueID; i++)
		{

			//Get tile data to be interpreted
			Tile Value;
			AllEntries.TryGetValue(i, out Value);

			if (Value != null)
			{

				//Add entry to the array for saving later
				AllLines[Value.transform.GetSiblingIndex()] = Value.GetCaptionText();

				if (!HasAlreadyGotWatermark)
				{

					//Check if line is equal to a unique part of the watermark
					if (Value.GetCaptionText() == Watermark[IndexOfIdentifiableWatermark])
					{

						HasAlreadyGotWatermark = true;

					}

				}
			}
			else
			{

				//Cleanup an invalid entry. Realistically, this should never happen
				AllEntries.Remove(i);

			}

		}

		//If we're missing watermark, add it on
		if (HasAlreadyGotWatermark == false)
		{

			AllLines = Watermark.Concat(AllLines).ToArray();

		}

		//Write to file
		File.WriteAllLines(path, AllLines);

	}

	private bool GetIfCharacterBreaksVariableSyntax(char character)
	{
		return Ignore.Contains(character) || System.Char.IsWhiteSpace(character);

    }

	private Keybind GetKeybindingAndBindingName(string line)
	{

		char[] lineCharArr = line.ToArray();

		if (line.Length <= 2)
		{

			//The line is too short to contain keybinding
			return new Keybind();

		}

		//Assign initial variables
		string KeybindingName = null;
		string Keybinding = null;

		bool SettingActualKeybinding = false;
		bool SettingKeybindingName = true;
		bool BegunSettingKeybindingName = false;

		//Iterate over eachcharacter of the line
		for (int i = 0; i < lineCharArr.Length; i++)
		{

			//First step, determine variable name
			if (SettingKeybindingName)
			{

				if (BegunSettingKeybindingName)
				{
					//Variable start has been found and we are assigning it
					if (GetIfCharacterBreaksVariableSyntax(lineCharArr[i]))
					{

						//Detected something that's not compatable with variable syntax, assume
						SettingKeybindingName = false;

					}
					else
					{

						//Continue updating the name
						KeybindingName += lineCharArr[i];

						if (KeybindingName == "bindings_version")
						{

							//Bindings version is a float that will require additional behaviour, so for now just ignoring him.
							//Note: Ultimate Apocalypse mod decided to change their hotkeys mod version (for whatever reason....)
							//...so for compatability will need functionality later for this float
							return new Keybind();

						}

					}

				}
				else
				{
					//Still trying to find the start of the variable.
					if (i == lineCharArr.Count() - 1)
					{

						//No chance to have a keybinding anymore as remaining length of line < 2
						return new Keybind();

					}

					if (lineCharArr[i] == '-' && lineCharArr[i + 1] == '-')
					{

						//Comment before keybinding, returning
						return new Keybind();

					}

					//Ensure character is following variable syntax correctly
					if (GetIfCharacterBreaksVariableSyntax(lineCharArr[i]) == false)
					{

						KeybindingName += lineCharArr[i];
						BegunSettingKeybindingName = true;

					}

				}

			}
			else if (SettingActualKeybinding)
			{

				if (lineCharArr[i] == '"')
				{

					// " indicates end of the binding so return
					return new Keybind(KeybindingName, Keybinding);

				}

				//If not end, add the character to the binding
				Keybinding += lineCharArr[i];

			}
			else if (lineCharArr[i] == '"')
			{

				// " Indicates start of binding so now we look for binding
				SettingActualKeybinding = true;

			}


		}

		if (Keybinding == null)
		{

			//Has var = format but no keybinding associated! This is likely the "binding = " bit.
			//Note: This could also be a syntax error, in future might want to highlight / rectify this.
			return new Keybind();

		}

		return new Keybind(KeybindingName, Keybinding);

	}

}