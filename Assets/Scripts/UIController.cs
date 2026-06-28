using System;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    // UI text displaying the current wave.
    [SerializeField] TextMeshProUGUI waveText;

    // UI text displaying the player's remaining lives.
    [SerializeField] TextMeshProUGUI livesText;

    private void OnEnable()
    {
        // Listen for wave and lives updates.
        SpawnManager.onWaveChanged += UpdateWaveText;
        GameManager.OnLivesChanged += UpdateLivesText;
    }

    private void OnDisable()
    {
        // Stop listening when this object is disabled.
        SpawnManager.onWaveChanged -= UpdateWaveText;
        GameManager.OnLivesChanged -= UpdateLivesText;
    }

    void UpdateWaveText(int waveCounter)
    {
        // Update the wave counter displayed on the UI.
        waveText.text = $"Wave : {waveCounter}";
    }

    void UpdateLivesText(int livesCounter)
    {
        // Update the player's remaining lives displayed on the UI.
        livesText.text = $"Lives : {livesCounter}";
    }
}