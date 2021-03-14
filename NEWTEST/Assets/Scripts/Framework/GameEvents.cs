using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameEvents : Singleton<GameEvents>
{
    // public event Action<int> onEventName;
    // public void EventName(int para)
    // {
    //     if (onEventName != null)
    //         onEventName(para);
    // }

    public event Action onCoreStart;
    public void CoreStart()
    {
        if (onCoreStart != null)
            onCoreStart();
    }

    public event Action onCoreEnd;
    public void CoreEnd()
    {
        if (onCoreEnd != null)
            onCoreEnd();
    }
}
