using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System.Linq;

public class KeybindingDisplayAndEditor : MonoBehaviour
{
	public OneSingleTile ExemplarTile;

	public RectTransform parentWindow;
	[SerializeField] private ChangeHKPopup CHKP;
	[SerializeField] private DeleteHotkeyPopup DHP;
	[SerializeField] private AddHotkeyPopup AHP;
	[SerializeField] private Scrollbar SB;
	public GameObject PleaseWaitPopup;

	private Dictionary<int, OneSingleTile> AllEntries = new();
	private char[] Ignore = new char[4] { '{', '}', '=', '-' };
	private float[] parentWindowOriginalYOffset;
	private int NextUniqueID;
	private string OpenedPath;

	OneSingleTile MakeFreshCopyOfExampleTile(int PositionInParent, int PositionInWrittenFile)
	{
		// create it and simultaneously parent it to the same place in the UI
		var copy = Instantiate<OneSingleTile>(ExemplarTile, ExemplarTile.transform.parent);
		copy.PositionInWrittenFile = PositionInWrittenFile;
		// make it visible
		copy.gameObject.SetActive(true);

		return copy;
	}

	private void Start()
	{
		parentWindowOriginalYOffset = new float[2] { parentWindow.offsetMin[1], parentWindow.offsetMax[1] };
		ExemplarTile.gameObject.SetActive(false);
	}

    public void ClearOldEntries()
    {
		SB.value = 0;
		for(int i = 0; i < NextUniqueID; i++)
		{
			OneSingleTile Value;
			AllEntries.TryGetValue(i, out Value);
			if (Value != null)
			{
				GameObject.Destroy(Value.gameObject);
			}
		}
		AllEntries.Clear();

		parentWindow.offsetMin = new Vector2(parentWindow.offsetMin[0], parentWindowOriginalYOffset[0]);
		parentWindow.offsetMax = new Vector2(parentWindow.offsetMax[0], parentWindowOriginalYOffset[1]);
		NextUniqueID = 0;
	}

	public void DisableAllEntryButtonInput()
    {
		for(int i = 0; i < NextUniqueID; i++)
		{
			OneSingleTile Value;
			AllEntries.TryGetValue(i, out Value);
			if (Value != null)
			{
				AllEntries[i].DisableButtonInteractivity();
			}
        }
	}

	public void EnableAllEntryButtonInput()
	{
		for (int i = 0; i < NextUniqueID; i++)
		{
			OneSingleTile Value;
			AllEntries.TryGetValue(i, out Value);
			if (Value != null)
			{
				AllEntries[i].EnbableButtonInteractivityIfValid();
            }
            else
            {
				AllEntries.Remove(i);
            }
		}
	}

	private void DisplayFileDataDelayed()
    {
		string path = OpenedPath;
		
		ClearOldEntries();
		List<string> fileLines = File.ReadAllLines(path).ToList();
		int numberOfEntries = 0;
		// now make buttons out of each one
		for (int i = 0; i < fileLines.Count; i++)
		{
			string line = fileLines[i];
			bool IsBinding = false;
			numberOfEntries++;
			OneSingleTile entry = MakeFreshCopyOfExampleTile(i, i);
			entry.DisableButtonInteractivity();
			string[] bindings = GetKeybindingAndBindingName(line);
			if (bindings.Length == 2) { IsBinding = true; }
			entry.SetCaptionText(line);
			AllEntries.Add(NextUniqueID, entry);
			entry.PositionInParentDictionary = NextUniqueID;
			NextUniqueID++;

			if (IsBinding)
			{
				entry.ValidEntry = true;
				// set up what each button does when pressed
				{
					entry.SetButtonDelegate(
						() =>
						{
							DisableAllEntryButtonInput();
							CHKP.OpenPopupAsBinding(bindings[0], bindings[1], entry);
						}
					);
					entry.SetPlusButtonDelegate(
						 () =>
						 {
							 DisableAllEntryButtonInput();
							 OrderMakeNewEntry(entry);
						 }
					 );
					entry.SetMinusButtonDelegate(
						  () =>
						  {
							  DisableAllEntryButtonInput();
							  OrderDeleteSpecificEntry(entry);
						  }
					  );
				}
			}
			else
			{
				entry.DisableButton();
			}

		}

		parentWindow.offsetMin = new Vector2(parentWindow.offsetMin[0], parentWindow.offsetMin[1] - (parentWindow.GetComponent<GridLayoutGroup>().cellSize[1] + parentWindow.GetComponent<GridLayoutGroup>().spacing[1]) * numberOfEntries);
		PleaseWaitPopup.SetActive(false);
		EnableAllEntryButtonInput();
	}

	public void DisplayFileData (string path)
	{
		OpenedPath = path;
		Invoke(nameof(DisplayFileDataDelayed), 1);
		PleaseWaitPopup.SetActive(true);
		ExemplarTile.gameObject.SetActive(false);		
	}

	public void OrderDeleteSpecificEntry(OneSingleTile zEntry)
    {
		string[] caption = GetKeybindingAndBindingName(zEntry.GetCaptionText());
		DHP.Open(caption[1], caption[0], zEntry.PositionInParentDictionary);
    }

	public void DeleteSpecificEntry(int EntryID)
	{
		GameObject.Destroy(AllEntries[EntryID].gameObject);
		AllEntries.Remove(EntryID);
		//ShiftEntriesLeft(EntryID);
	}

	public void OrderMakeNewEntry(OneSingleTile zEntry)
    {
		AHP.Open(zEntry.PositionInWrittenFile);
    }

	public void ShiftEntriesRight(int zStartIndex)
	{
		AllEntries.Add(NextUniqueID, null);
		for (int i = NextUniqueID-1; i > zStartIndex; i--)
        {
			ShiftAnEntry(i, 1);
		}
		NextUniqueID++;
	}

	public void ShiftEntriesLeft(int zStartIndex)
	{
		for (int i = zStartIndex; i < NextUniqueID; i++)
		{
			ShiftAnEntry(i, -1);
		}

		AllEntries.Remove(NextUniqueID);
		NextUniqueID--;
	}

	private void ShiftAnEntry(int zEntryIndex, int zDirection)
	{
		if (AllEntries.ContainsKey(zEntryIndex))
		{
			AllEntries[zEntryIndex].PositionInParentDictionary += zDirection;
			AllEntries[zEntryIndex].PositionInWrittenFile += zDirection;
			AllEntries[zEntryIndex + zDirection] = AllEntries[zEntryIndex];
		}
	}

	public void AddEntryAfterSpecificEntry(int PreviousEntryID, string BindingName, string Binding)
	{
		ShiftEntriesRight(PreviousEntryID+1);
		string line = "    " + BindingName + " = " + '"' + Binding + '"' + ',';
		OneSingleTile entry = MakeFreshCopyOfExampleTile(NextUniqueID,PreviousEntryID + 1);
		entry.DisableButtonInteractivity();
		string[] bindings = GetKeybindingAndBindingName(line);
		entry.SetCaptionText(line);
		AllEntries.Add(NextUniqueID, entry);
		entry.PositionInParentDictionary = NextUniqueID;
		NextUniqueID++;

		// set up what each button does when pressed
		{
			entry.ValidEntry = true;
			entry.SetButtonDelegate(
				() =>
				{
					DisableAllEntryButtonInput();
					CHKP.OpenPopupAsBinding(bindings[0], bindings[1], entry);
				}
			);
			entry.SetPlusButtonDelegate(
				 () =>
				 {
					 DisableAllEntryButtonInput();
					 OrderMakeNewEntry(entry);
				 }
			 );
			entry.SetMinusButtonDelegate(
				  () =>
				  {
					  DisableAllEntryButtonInput();
					  OrderDeleteSpecificEntry(entry);
				  }
			  );
		}
		CHKP.OpenPopupAsBinding(bindings[0], bindings[1], entry);
	}

	public void SaveFile(string path)
    {
		bool HasAlreadyGotWatermark = false;
		string[] AllLines = new string[NextUniqueID+1];
		string[] AllLinesExtra = new string[NextUniqueID + 6];
		for (int i = 0; i <= NextUniqueID; i++)
		{
			OneSingleTile Value;
			AllEntries.TryGetValue(i, out Value);
			if (Value != null)
			{
				AllLines[Value.transform.GetSiblingIndex()] = Value.GetCaptionText();
				if (!HasAlreadyGotWatermark)
				{
					AllLinesExtra[Value.transform.GetSiblingIndex() + 5] = Value.GetCaptionText();
					if (Value.GetCaptionText() == "-- Created using Dawn of War Hotkey Editor Tool. https://www.moddb.com/mods/hotkey-editor-tool")
					{
						HasAlreadyGotWatermark = true;
					}
				}
			}
			else
			{
				AllEntries.Remove(i);
			}
		}

		if (HasAlreadyGotWatermark == false)
		{
			AllLinesExtra[0] = "";
			AllLinesExtra[1] = "        ----------------------------------------------------------------------------------------------------------------";
			AllLinesExtra[2] = "        -- Created using Dawn of War Hotkey Editor Tool. https://www.moddb.com/mods/hotkey-editor-tool";
			AllLinesExtra[3] = "        ----------------------------------------------------------------------------------------------------------------";
			AllLinesExtra[4] = "";
			File.WriteAllLines(path, AllLinesExtra);
        }
        else
        {
			File.WriteAllLines(path, AllLines);
		}
    }

	private string[] GetKeybindingAndBindingName(string line)
    {
		char[] lineCharArr = line.ToArray();
		if(line.Length > 2)
        {
			string KeybindingName = null;
			string Keybinding = null;
			bool LookingForKeybinding = false;
			bool SettingKeybindingName = true;
			bool BegunSettingKeybindingName = false;
			for (int i = 0; i < lineCharArr.Length; i++)
			{
				if (LookingForKeybinding)
				{
					if (lineCharArr[i] == '"')
                    {
						return new string[2] { KeybindingName, Keybinding };
                    }
					Keybinding += lineCharArr[i];
				}
				else if (lineCharArr[i] == '"')
				{
					LookingForKeybinding = true;
                }
                else if(SettingKeybindingName)
                {
                    if (BegunSettingKeybindingName)
					{
						if (Ignore.Contains(lineCharArr[i]) || System.Char.IsWhiteSpace(lineCharArr[i]))
						{
							SettingKeybindingName = false;
						}
                        else
                        {
							KeybindingName += lineCharArr[i];

							if (KeybindingName == "bindings_version")
							{
								//Bindings version is a float that will require additional behaviour, so for now just ignoring him. maybe behaviour added in future.
								return new string[0];
							}
						}
					}
					else
					{
						if (i != lineCharArr.Count()-1)
						{
							if (lineCharArr[i] == '-' && lineCharArr[i + 1] == '-')
							{
								//Comment before keybinding, returning
								return new string[0];
							}
                        }
                        else
                        {
							//No chance to have a keybinding anymore, length < 2
							return new string[0];
						}

						if (!Ignore.Contains(lineCharArr[i]) && !System.Char.IsWhiteSpace(lineCharArr[i]))
						{
							KeybindingName += lineCharArr[i];
							BegunSettingKeybindingName = true;
						}
                    }
                }
            }

			if (Keybinding == null) 
			{
				//Has var = format but no keybinding associated! This is likely the "binding = " bit.
				return new string[0]; 
			}
			return new string[2] { KeybindingName, Keybinding };
		}
        else
        {
			//The line is too short to contain keybinding
			return new string[0];
        }
    }

}
