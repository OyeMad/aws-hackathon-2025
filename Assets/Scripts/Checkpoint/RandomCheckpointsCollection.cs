using System.Collections.Generic;
using System.Linq;
using Supyrb;
using UnityEngine;

[CreateAssetMenu(fileName ="SequenetialCheckpoints" ,menuName = "Checkpoint/Random Collection Checkpoints")]
public class RandomCheckpointsCollection : CheckpointLogic
{
    [SerializeField]
    [Range(1, 20)]
    int maxCheckpointsToCollect = 10;

    private List<Checkpoint> availableCheckpoints = new List<Checkpoint>();
    private List<Checkpoint> selectedCheckpoints = new List<Checkpoint>();
    private List<Checkpoint> collectedCheckpoints = new List<Checkpoint>();
    private Vector3 lastCollectedPosition;



    public override void Initilize(GameObject a_parent)
    {
        parent = a_parent;
        ResetCheckpoints();
        SetupRandomCheckpoints();
    }

private void SetupRandomCheckpoints()
    {
        // Find all checkpoints in the level
        availableCheckpoints = Object.FindObjectsByType<Checkpoint>(FindObjectsInactive.Include , FindObjectsSortMode.None).ToList();

        // Clear previous selections
        selectedCheckpoints.Clear();
        collectedCheckpoints.Clear();

        // Determine how many checkpoints to select
        int numCheckpointsToSelect = Mathf.Min(maxCheckpointsToCollect, availableCheckpoints.Count);

        // Randomly select checkpoints
        while (selectedCheckpoints.Count < numCheckpointsToSelect && availableCheckpoints.Count > 0)
        {
            int randomIndex = Random.Range(0, availableCheckpoints.Count);
            Checkpoint selectedCheckpoint = availableCheckpoints[randomIndex];
            
            selectedCheckpoints.Add(selectedCheckpoint);
            availableCheckpoints.RemoveAt(randomIndex);

            // Activate selected checkpoint
            selectedCheckpoint.gameObject.SetActive(true);
            selectedCheckpoint.Initialize(selectedCheckpoints.Count);
            selectedCheckpoint.OnCheckpointReached += HandleCheckpointReached;
        }

        // Deactivate unused checkpoints
        foreach (var checkpoint in availableCheckpoints)
        {
            checkpoint.gameObject.SetActive(false);
        }
        UpdateText();

    }

    private void HandleCheckpointReached(Checkpoint checkpoint)
    {
        if (selectedCheckpoints.Contains(checkpoint) && !collectedCheckpoints.Contains(checkpoint))
        {
            lastCollectedPosition = checkpoint.transform.position;
            collectedCheckpoints.Add(checkpoint);
            checkpoint.OnCheckpointReached -= HandleCheckpointReached;

            // Optional: Deactivate or change appearance of collected checkpoint
            checkpoint.gameObject.SetActive(false);

            // Check if all checkpoints are collected
            if (IsCollectionComplete())
            {
                OnCollectionComplete();
            }
        }
        UpdateText();
    }

    void UpdateText()
    {
        string val = "" + collectedCheckpoints.Count + " / " + selectedCheckpoints.Count; 
        Signals.Get<GameProgressSignal>().Dispatch(val);

    }

    private bool IsCollectionComplete()
    {
        return collectedCheckpoints.Count >= selectedCheckpoints.Count;
    }

    private void OnCollectionComplete()
    {
        Debug.Log("All checkpoints collected! Game Over!");
        AllCheckpointsReached();
    }

    public override float GetCompletionPercentage()
    {
        if (selectedCheckpoints.Count == 0) return 0f;
        return (float)collectedCheckpoints.Count / selectedCheckpoints.Count;
    }

    public override Vector3 GetLastCheckpointPosition()
    {
        return lastCollectedPosition;
    }

    public override void ResetCheckpoints()
    {
        // Unsubscribe from all checkpoint events
        if (selectedCheckpoints != null)
        {
            foreach (var checkpoint in selectedCheckpoints)
            {
                if (checkpoint != null)
                {
                    checkpoint.OnCheckpointReached -= HandleCheckpointReached;
                }
            }
        }

        availableCheckpoints.Clear();
        selectedCheckpoints.Clear();
        collectedCheckpoints.Clear();
        lastCollectedPosition = Vector3.zero;
    }

    public override void Reset()
    {
        availableCheckpoints.Clear();
        selectedCheckpoints.Clear();
        collectedCheckpoints.Clear();
    }
}
