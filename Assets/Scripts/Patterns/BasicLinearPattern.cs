using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Concrete implementation of <see cref="BaseSymbolPattern"/> representing a basic linear pattern.
/// </summary>
public class BasicLinearPattern : BaseSymbolPattern
{
    [Header("Pattern Settings")]
    [Tooltip("Name of the pattern displayed in inspector and used for logging.")]
    [SerializeField] private string patternName = "Basic Line";

    /// <summary>
    /// Gets the name of the pattern.
    /// </summary>
    public override string PatternName => patternName;

    /// <summary>
    /// Overrides the base Evaluate method to evaluate this specific pattern.
    /// Catches exceptions and logs errors if evaluation fails.
    /// </summary>
    /// <param name="indx">Indexes of the symbols to evaluate in the pattern.</param>
    /// <param name="symbolsToCheck">List of symbols currently in play.</param>
    /// <param name="activeSquares">List of active square GameObjects to highlight matches.</param>
    /// <returns>The amount of credits won based on the pattern match; otherwise, 0.</returns>
    public override int Evaluate(int[] indx, List<Symbols> symbolsToCheck, List<GameObject> activeSquares)
    {
        try
        {
            return base.Evaluate(indx, symbolsToCheck, activeSquares);
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"[BasicLinearPattern] Error in Evaluate: {ex.Message}");
            return 0;
        }
    }
}