using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueController : MonoBehaviour
{
    [SerializeField] private Queue DataHolder;
    [SerializeField] private QueueVisualised DataVisualiser;
    private int BaseQueueLimit = 5;

    private void Start()
    {
        DataVisualiser.DelayedStart();
        ChangeLimit(BaseQueueLimit);
    }

    public void Enqueue()
    {
        if (!DataHolder.IsFull())
        {
            DataObject newDataEntry = gameObject.AddComponent<DataObject>();
            newDataEntry.SetID(DataHolder.GetQueueSize());
            DataHolder.Enqueue(newDataEntry);
            DataVisualiser.EnqueueVisualised(newDataEntry);
            Peek();
        }
    }

    public void Dequeue()
    {
        if (!DataHolder.IsEmpty())
        {
            DataHolder.Dequeue();
            DataVisualiser.DequeueVisualised();
            Peek();
        }
    }

    public void Peek()
    {
        if (!DataHolder.IsEmpty())
        {
            DataVisualiser.PeekVisualised();
        }
    }

    public void ChangeLimit(int ChangeBy)
    {
        int AmmountQueueSizeChanged = DataHolder.ChangeQueueLimit(ChangeBy);

        //If queue has too many entries, dequeue
        for (int i = AmmountQueueSizeChanged; i > 0; i--)
        {
            Dequeue();
        }

        DataVisualiser.ChangeLimitVisualised();
    }

    public void Clear()
    {
        while (!DataHolder.IsEmpty())
        {
            Dequeue();
        }
    }
}
