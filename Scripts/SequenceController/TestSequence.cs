using UnityEngine;
using DG.Tweening;

public class TestSequence : MonoBehaviour
{
    public void PulseScale(TimelineController timelineController)
    {
        Tween t = transform.DOScale(1.2f, 0.3f)
            .SetLoops(2, LoopType.Yoyo);

        timelineController.RegisterTween(t);
    }

    public void SayHi(TimelineController timelineController)
    {
        Debug.Log("Hi");
    }
}
