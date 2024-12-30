using System.Collections.Generic;
using Supyrb;
using UnityEngine;

[CreateAssetMenu(fileName ="SequenetialCheckpoints" ,menuName = "Checkpoint/Sequential Checkpoints")]
public class SequentialCheckpointLogic : CheckpointLogic
{
    [SerializeField]
    [Range(3, 100)]
    int MaxCheckpointsToUse = 10;
    [SerializeField]
    private List<Checkpoint> checkpoints = new List<Checkpoint>();
    [SerializeField]
    private int currentCheckpointIndex = -1;
    [SerializeField]
    private int lastReachedCheckpointIndex = -1;


    public override void Initilize(GameObject a_parent)
    {
        // Find all checkpoints in the scene and sort them by their position in hierarchy
        Checkpoint[] foundCheckpoints = FindObjectsByType<Checkpoint>( FindObjectsInactive.Include, FindObjectsSortMode.None);
        checkpoints = new List<Checkpoint>(foundCheckpoints);

        parent = a_parent;
        
        // Sort checkpoints based on their sibling index in hierarchy
        checkpoints.Sort((a, b) => a.transform.GetSiblingIndex().CompareTo(b.transform.GetSiblingIndex()));

        RemoveElementsAfterIndex( checkpoints , MaxCheckpointsToUse);

        // Initialize each checkpoint
        for (int i = 0; i < checkpoints.Count; i++)
        {
            int index = i;
                
            checkpoints[i].Initialize(i);
            
            checkpoints[i].OnCheckpointReached += (checkpoint) => HandleCheckpointReached(checkpoint);
        }

        currentCheckpointIndex = 0;

        UpdatePath();
    }

    public void RemoveElementsAfterIndex<T>(List<T> list, int startIndex)
    {
        if (startIndex < 0 || startIndex >= list.Count) return;
        
        int countToRemove = list.Count - startIndex;
        list.RemoveRange(startIndex, countToRemove);
    }

    void UpdatePath()
    {
        if( currentCheckpointIndex >= checkpoints.Count)
            return;
        List<Checkpoint> points = new List<Checkpoint>();

        if( currentCheckpointIndex > 0)
            points.Add(checkpoints[currentCheckpointIndex-1]);
        
        points.Add( checkpoints[currentCheckpointIndex]);

        if( currentCheckpointIndex < checkpoints.Count-1)
            points.Add( checkpoints[currentCheckpointIndex+1]);

        CheckpointPath path = parent.GetComponent<CheckpointPath>();
        if( path)
        {
            path.SetPathPoints( points);
        }

    }

    private void HandleCheckpointReached(Checkpoint checkpoint)
    {
        if ( checkpoint.CheckpointIndex != currentCheckpointIndex)
        {
            return;
        }

        lastReachedCheckpointIndex = checkpoint.CheckpointIndex;
        currentCheckpointIndex = checkpoint.CheckpointIndex + 1;

        UpdatePath();

        CheckpointReached(checkpoint);


        if( currentCheckpointIndex < checkpoints.Count)
            checkpoints[currentCheckpointIndex].HighlightSelf();

        if (currentCheckpointIndex >= checkpoints.Count)
        {
            AllCheckpointsReached();
        }

        UpdateText();
    }

        void UpdateText()
    {
        string val = "" + GetCompletionPercentage(); 
        Signals.Get<GameProgressSignal>().Dispatch(val);

    }



    public override  Vector3 GetLastCheckpointPosition()
    {
        if (lastReachedCheckpointIndex >= 0 && lastReachedCheckpointIndex < checkpoints.Count)
        {
            return checkpoints[lastReachedCheckpointIndex].transform.position;
        }
        return Vector3.zero;

    }

    public override void ResetCheckpoints()
    {
        foreach (var checkpoint in checkpoints)
        {
            checkpoint.Reset();
        }
        currentCheckpointIndex = 0;
        lastReachedCheckpointIndex = -1;
    }

    public override float GetCompletionPercentage()
    {
        if (checkpoints.Count == 0) return 0f;

        float v = (float)(lastReachedCheckpointIndex + 1) / checkpoints.Count * 100f;
        v = Mathf.RoundToInt( v);
        return v;
    }

    public override void Reset()
    {
        checkpoints.Clear();
        parent = null;
    }
}
