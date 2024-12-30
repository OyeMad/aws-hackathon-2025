using System.Threading.Tasks;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    DroneController droneController;

    [SerializeField]
    float TurnSpeed = 0.7f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        droneController.Activate();
        //CheckpointManager.Instance.OnPlayerRespawn += HandleRespawn;

    }

    void Update()
    {
        float v = Input.GetAxis( "Vertical");

        float h = Input.GetAxis( "Horizontal");

        // if( Input.GetKeyUp(KeyCode.Q) )
        // {
        //     TakeOff(true);
        // }
        // else if( Input.GetKeyUp(KeyCode.E) )
        // {
        //     Land(true);
        // }

        if( h < 0 )
        {
            TurnLeft();
        }
        else if( h > 0 )
        {
            TurnRight();
        }


    }

    [SerializeField]
    int ms_FlyPowerTime = 500;
    async public void Land(bool OnKey = false)
    {
        if( OnKey )
        {
            droneController.Descend();
            await Task.Delay(ms_FlyPowerTime);
            droneController.MobileStopAscendOrDescend();
            return;
        }

        float time = 0.2f;
        while( time > 0 )
        {
            time -= Time.deltaTime;
            droneController.Descend();
            await Task.Yield();
        }


        droneController.MobileStopAscendOrDescend();

    }

    async public void TakeOff(bool OnKey = false)
    {
        if( OnKey )
        {
            droneController.Ascend();
            await Task.Delay(ms_FlyPowerTime);
            droneController.MobileStopAscendOrDescend();
            return;
        }

        float time = 0.2f;
        while( time > 0 )
        {
            time -= Time.deltaTime;
            droneController.Ascend();
            await Task.Yield();
        }

        Debug.Log("TakeOff Stop Ascend");
        droneController.MobileStopAscendOrDescend();
    }

    public void TurnLeft()
    {
        droneController.CameraControlLogic( -TurnSpeed , 0);
    }

    public void TurnRight()
    {
        droneController.CameraControlLogic( TurnSpeed, 0);
    }

    #region Checkpoints
    private bool isRespawning = false;


    private void OnDestroy()
    {
        // if (CheckpointManager.Instance != null)
        // {
        //     //CheckpointManager.Instance.OnPlayerRespawn -= HandleRespawn;
        // }
    }

    private void HandleRespawn(Vector3 respawnPosition)
    {
        isRespawning = true;
        transform.position = respawnPosition;
        // Add any additional respawn logic here
        isRespawning = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DeathZone") && !isRespawning)
        {
            //CheckpointManager.Instance.RespawnPlayerAtLastCheckpoint();
        }
    }

    #endregion
    
}
