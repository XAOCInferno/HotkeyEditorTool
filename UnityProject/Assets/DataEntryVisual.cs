using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DataEntryVisual : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI DataEntryIDText;

    public void SetID(int newID) { print(newID); DataEntryIDText.text = newID.ToString(); }
    public void SetID(string newID) { DataEntryIDText.text = newID; }
}
