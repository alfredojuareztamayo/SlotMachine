using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnableButton : MonoBehaviour
{
    /// <summary>
    /// Reference to the UI button that will be enabled/disabled.
    /// </summary>
    [Header("UI References")]
    [Tooltip("Reference to the UI Button component to enable or disable.")]
    [SerializeField] private Button myButton;

    /// <summary>
    /// Reference to the TMP text element used to display rewards.
    /// </summary>
    [Tooltip("Reference to the TextMeshPro text element that displays the reward.")]
    [SerializeField] private TMP_Text rewardText;

    /// <summary>
    /// Unity's Awake method.
    /// Initializes component references and validates assignments.
    /// </summary>
    private void Awake()
    {
        try
        {

            if (myButton == null)
                myButton = GetComponent<Button>();

            if (rewardText == null)
                Debug.LogWarning("[EnableButton] Reward Text no está asignado en el inspector.");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"[EnableButton] Error en Awake: {ex.Message}");
        }
    }
    /// <summary>
    /// Called when the Spin button is pressed.
    /// Clears the reward text and disables the button.
    /// </summary>
    public void OnSpinPressed()
    {
        try
        {
            if (rewardText != null)
                rewardText.text = string.Empty;

            if (myButton != null)
                myButton.interactable = false;
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"[EnableButton] Error en OnSpinPressed: {ex.Message}");
        }
    }
    /// <summary>
    /// Called when a spin action finishes.
    /// Re-enables the button for further interaction.
    /// </summary>
    public void OnSpinFinish()
    {
        try
        {
            if (myButton != null)
                myButton.interactable = true;
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"[EnableButton] Error en OnSpinFinish: {ex.Message}");
        }
    }
}
