using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentedAnimationEvent : MonoBehaviour
{
   public void NotifyAnscestors(string message)
    {
        SendMessageUpwards(message);
    }
}
