using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Consumer;

public interface IPullMessage
{
    public void addToList(Consumer.Message newMessage);
    public void updatePullMessages(Consumer.Message newMessage);
}
