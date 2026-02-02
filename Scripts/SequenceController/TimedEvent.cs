using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class TimedEvent
{
    [Range(0f, 1f)]
    public float normalizedTime;

    public TimelineEvent action;
}

[System.Serializable]
public class TimelineEvent : UnityEvent<TimelineController> { }
