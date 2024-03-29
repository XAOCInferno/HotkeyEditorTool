using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataObject : MonoBehaviour
{
    private int ID;

    public void SetID(int newID) => ID = newID; 
    public int GetID() { return ID; }
}
