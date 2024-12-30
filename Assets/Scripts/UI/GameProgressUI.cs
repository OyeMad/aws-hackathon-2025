using Supyrb;
using TMPro;
using UnityEngine;

public class GameProgressUI : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI progressTMP;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Signals.Get<GameProgressSignal>().AddListener(OnGameProgress);
    }

    void OnDestroy()
    {
        Signals.Get<GameProgressSignal>().RemoveListener(OnGameProgress);
    }

    void OnGameProgress(string progress)
    {
        progressTMP.text = progress;
    }


}
