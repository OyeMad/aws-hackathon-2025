using UnityEngine;
using System;
using System.Threading.Tasks;
using OnScreenPointerPlugin;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private bool isActive = true;
    [SerializeField] private float activationRadius = 5f;
    [SerializeField] private bool showGizmos = true;

    [SerializeField]
    GameObject normalParticle;

    [SerializeField]
    GameObject CapturedParticle;
    
    public int CheckpointIndex { get; private set; }
    public bool IsReached { get; private set; }
    
    public event Action<Checkpoint> OnCheckpointReached;

    private void OnTriggerEnter(Collider other)
    {
        if (!isActive || IsReached) return;

        Debug.Log($"Checkpoint {name} OnTriggerEnter-{other.name} {other.tag}");

        if (other.CompareTag("Player"))
        {
            IsReached = true;
            OnCheckpointReached?.Invoke(this);
            SetParticles( false);

            Disable();
        }
    }

    async void Disable()
    {
        await Task.Delay(2000);
        CapturedParticle?.SetActive(false);
        normalParticle?.SetActive( false);
        GetComponent<OnScreenPointerObject>()?.HidePointer();
    }

    public void HighlightSelf()
    {
        normalParticle.SetActive(true);
    }

    public void Initialize(int index)
    {
        CheckpointIndex = index;
        IsReached = false;
        if( CheckpointIndex == 0 )
            SetParticles( true);
    }

    void SetParticles( bool flag)
    {
        normalParticle.SetActive(flag);
        CapturedParticle.SetActive(!flag);
    }

    public void Reset()
    {
        IsReached = false;
    }

    private void OnDrawGizmos()
    {
        if (!showGizmos) return;

        Gizmos.color = IsReached ? Color.green : (isActive ? Color.yellow : Color.red);
        Gizmos.DrawWireSphere(transform.position, activationRadius);
    }
}
