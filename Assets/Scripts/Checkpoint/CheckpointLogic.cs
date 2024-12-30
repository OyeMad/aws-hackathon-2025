using System;
using UnityEngine;
using UnityEngine.Events;

public abstract class CheckpointLogic : ScriptableObject
{
    [SerializeField]
    protected GameObject parent;

    public abstract void Initilize(GameObject parent);

    public event Action<Checkpoint> OnCheckpointReached;
    public event Action OnAllCheckpointsReached;

    public abstract  Vector3 GetLastCheckpointPosition();

    public abstract void ResetCheckpoints();

    public void CheckpointReached(Checkpoint checkpoint)
    {
        OnCheckpointReached?.Invoke(checkpoint);
    }

    public void AllCheckpointsReached()
    {
        OnAllCheckpointsReached?.Invoke();
    }

    public abstract float GetCompletionPercentage();

    public abstract void Reset();
}
