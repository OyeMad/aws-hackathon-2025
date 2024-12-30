using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;
using UnityEngine.Events;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance { get; private set; }

    [SerializeField]
    CheckpointLogic checkpointLogic;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        InitializeCheckpoints();
    }

    private void InitializeCheckpoints()
    {
        checkpointLogic.Initilize(gameObject);

    }

    public Vector3 GetLastCheckpointPosition()
    {
        return checkpointLogic.GetLastCheckpointPosition();
    }

    public void ResetCheckpoints()
    {
        checkpointLogic.ResetCheckpoints();
    }

    public float GetCompletionPercentage()
    {
        return checkpointLogic.GetCompletionPercentage();
    }

    public void RegisterCheckpointReached( Action<Checkpoint> OnCheckpointReached )
    {
        checkpointLogic.OnCheckpointReached += OnCheckpointReached;
    }

    public void RegisterAllCheckpointsReached(Action OnAllCheckpointsReached)
    {
        checkpointLogic.OnAllCheckpointsReached += OnAllCheckpointsReached;
    }
}
