using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Queue))]
public class QueueVisualised : MonoBehaviour
{
    //
    //Extra Queue functionality for visuals
    //

    [SerializeField] private GameObject DataEntryPrefab;
    [SerializeField] private DataEntryVisual PeekDataVisual;
    [SerializeField] private Transform QueueVisualParent;
    [SerializeField] private TextMeshProUGUI QueueLimitText;
    [SerializeField] private TextMeshProUGUI IsFullText;
    [SerializeField] private TextMeshProUGUI IsEmptyText;

    private Queue DataHolder;
    private List<DataEntryVisual> AllDataEntryVisuals = new();

    public void DelayedStart()
    {
        TryGetComponent(out DataHolder);
        PeekDataVisual.gameObject.SetActive(false);
    }

    public void EnqueueVisualised(DataObject newDataObject)
    {
        DataEntryVisual NewVisual = GameObject.Instantiate(DataEntryPrefab, QueueVisualParent).GetComponent<DataEntryVisual>();
        AllDataEntryVisuals.Add(NewVisual);
        NewVisual.SetID(newDataObject.GetID());
        NewVisual.gameObject.SetActive(true);

        IsEmptyVisualised();
        IsFullVisualised();
    }

    public void DequeueVisualised()
    {
        GameObject.Destroy(AllDataEntryVisuals[0].gameObject);
        AllDataEntryVisuals.RemoveAt(0);

        IsEmptyVisualised();
        IsFullVisualised();
    }

    public void PeekVisualised()
    {
        PeekDataVisual.SetID(DataHolder.Peek().GetID());
    }

    public void IsFullVisualised()
    {
        if (DataHolder.IsFull()) 
        { 
            IsFullText.text = "Queue full";
            IsFullText.color = Color.red;
        } 
        else 
        { 
            IsFullText.text = "Queue not full";
            IsFullText.color = Color.white;
        }
    }

    public void IsEmptyVisualised()
    {
        if (DataHolder.IsEmpty()) 
        { 
            IsEmptyText.text = "Queue empty";
            PeekDataVisual.gameObject.SetActive(false);
            IsEmptyText.color = Color.red;
        } 
        else 
        { 
            IsEmptyText.text = "Queue not empty";
            PeekDataVisual.gameObject.SetActive(true);
            IsEmptyText.color = Color.white;
        }
    }

    public void ChangeLimitVisualised()
    {
        QueueLimitText.text = DataHolder.GetQueueLimit().ToString();
    }
}
