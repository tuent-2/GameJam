using System;
using System.Collections.Generic;
using UnityEngine;


public class HeaderNoticeUtils : Singleton<HeaderNoticeUtils>
{
    private Queue<string> messageQueue = new();
    public event Action OnNewMessageAvailable;

    public bool HasMessages => messageQueue.Count > 0;
    public int count => messageQueue.Count;

    public void EnqueueMessage(string message)
    {
        if (!string.IsNullOrEmpty(message))
        {
            messageQueue.Enqueue(message);
            OnNewMessageAvailable?.Invoke();
        }

        Debug.Log("Queue count = " + messageQueue.Count);
    }

    public string DequeueMessage()
    {
        return messageQueue.Count > 0 ? messageQueue.Dequeue() : null;
    }

    public void ClearMessages()
    {
        messageQueue.Clear();
    }
}