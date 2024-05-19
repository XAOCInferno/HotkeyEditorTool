using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queue : MonoBehaviour
{
    //
    //Basic Queue Functionality
    //

    private int QueueLimit = 0;
    private List<DataObject> Data = new();

    public void Enqueue(DataObject newData)
    {
        Data.Add(newData);
    }

    public void Dequeue()
    {
        Data.RemoveAt(0);        
    }

    public DataObject Peek()
    {
        return Data[0];
    }

    public bool IsEmpty()
    {
        if(Data.Count == 0)
        {
            return true;
        }

        return false;
    }

    public bool IsFull()
    {
        if (Data.Count >= QueueLimit)
        {
            return true;
        }

        return false;
    }

    public int ChangeQueueLimit(int ChangeBy)
    {
        int PreviousQueueLimit = QueueLimit;
        QueueLimit = Mathf.Max(0, QueueLimit + ChangeBy);

        int NumberOfDequeues = PreviousQueueLimit - QueueLimit;
        print(NumberOfDequeues);

        return NumberOfDequeues;
    }

    public int GetQueueLimit() { return QueueLimit; }
    public int GetQueueSize() { return Data.Count; }
}
