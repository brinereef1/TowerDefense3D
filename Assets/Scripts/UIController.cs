using System;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI waveText;

    private void OnEnable()
    {
        SpawnManager.onWaveChanged += UpdateWaveText;
    }

    private void OnDisable()
    {
        SpawnManager.onWaveChanged -= UpdateWaveText;
    }

    void UpdateWaveText(int waveCounter)
    {
        waveText.text = $"Wave : {waveCounter}";
    }
}
