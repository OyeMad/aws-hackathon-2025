using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [SerializeField]
    GameObject  GameUI;

    [SerializeField]
    GameObject  StartUI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    public async void OnGameFinshed()
    {
        Debug.Log("OnGameFinshed");
        await Task.Delay(1000);
        GameUI.SetActive( false);
        StartUI.SetActive( true);
        await SceneManager.UnloadSceneAsync(1);
    }

    public void OnCheckpointCollected(Checkpoint cp)
    {
    }

    public void OnGameStart()
    {
        SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);

        GameUI.SetActive( true);
        StartUI.SetActive( false);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Debug.Log("OnLevelWasLoaded :" + scene);
        if( scene.name.Equals("Game") )
        {
            CheckpointManager.Instance.RegisterCheckpointReached( OnCheckpointCollected);
            CheckpointManager.Instance.RegisterAllCheckpointsReached(OnGameFinshed);
        }
    }

}
