using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// owo, wha- what are you doing in this scrip ba- baka!
/// 7w7, it is just way for animation events to be able to execute a funtion from another gameobject script,
/// its not like I wanted you to look at it or anithing you baka.
/// Now go away you baka! (>﹏<)
/// </summary>

public class ActionGatherer : MonoBehaviour
{
    [SerializeField] List<UnityEvent> events;

    public void InvokeEvent(int i)
    {
        events[i].Invoke();
    }
}
