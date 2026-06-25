using System;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI waveText;
    [SerializeField] TextMeshProUGUI livesText;

    private void OnEnable()
    {
        SpawnManager.onWaveChanged += UpdateWaveText;
        GameManager.OnLivesChanged += UpdateLivesText;
    }

    private void OnDisable()
    {
        SpawnManager.onWaveChanged -= UpdateWaveText;
        GameManager.OnLivesChanged -= UpdateLivesText;
    }

    void UpdateWaveText(int waveCounter)
    {
        waveText.text = $"Wave : {waveCounter}";
    }

    void UpdateLivesText(int livesCounter)
    {
        livesText.text = $"Lives : {livesCounter}";
    }
}
