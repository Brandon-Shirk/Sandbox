using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class TimelineController : MonoBehaviour
{
    [Header("Timeline")]
    public float duration = 5f;
    public TimedEvent[] events;

    public float NormalizedProgress { get; private set; }

    private Sequence sequence;
    readonly List<Tween> spawnedTweens = new();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Play();
        }
    }

    public void Play()
    {
        Kill();

        sequence = DOTween.Sequence();
        sequence.SetAutoKill(false);

        sequence.AppendInterval(duration);
        sequence.OnUpdate(UpdateProgress);
        sequence.OnKill(CleanupSpawnedTweens);

        foreach (var e in events)
        {
            float time = Mathf.Clamp01(e.normalizedTime) * duration;

            sequence.InsertCallback(time, () =>
            {
                e.action.Invoke(this);
            });
        }

        sequence.Play();
    }

    private void UpdateProgress()
    {
        NormalizedProgress = sequence.Elapsed() / duration;
    }

    public void RegisterTween(Tween tween)
    {
        if (tween == null)
            return;

        spawnedTweens.Add(tween);

        tween.OnKill(() =>
        {
            spawnedTweens.Remove(tween);
        });
    }

    void CleanupSpawnedTweens()
    {
        foreach (var t in spawnedTweens)
        {
            if (t != null && t.IsActive())
                t.Kill();
        }

        spawnedTweens.Clear();
    }

    public void Kill()
    {
        if (sequence != null && sequence.IsActive())
        {
            sequence.Kill();
            sequence = null;
        }

        NormalizedProgress = 0f;
    }

    private void OnDisable()
    {
        Kill();
    }
}
