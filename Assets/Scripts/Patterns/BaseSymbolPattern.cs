using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract base class for defining symbol-based patterns in a slot machine game.
/// Provides logic to evaluate patterns, activate squares, 
/// and calculate rewards based on matching symbols.
/// </summary>
public abstract class BaseSymbolPattern : IPattern
{
    /// <summary>
    /// Reward table for each symbol, mapping the number of matches to the credits awarded.
    /// </summary>
    [Header("Reward table per symbol")]
    [Tooltip("Mapping from symbol names to rewards based on number of matches.")]
    protected static readonly Dictionary<string, Dictionary<int, int>> rewards =
       new Dictionary<string, Dictionary<int, int>>()
   {
        { "Campana", new Dictionary<int, int> { { 5, 100 }, { 4, 75 }, { 3, 50 }, { 2, 25 } } },
        { "ciruela", new Dictionary<int, int> { { 5, 40 }, { 4, 20 }, { 3, 10 }, { 2, 5 } } },
        { "LaCereza", new Dictionary<int, int> { { 5, 10 }, { 4, 5 }, { 3, 2 }, { 2, 1 } } },
        { "Sandia", new Dictionary<int, int> { { 5, 60 }, { 4, 30 }, { 3, 20 }, { 2, 10 } } },
        { "naranja", new Dictionary<int, int> { { 5, 30 }, { 4, 15 }, { 3, 10 }, { 2, 5 } } },
        { "uvas", new Dictionary<int, int> { { 5, 50 }, { 4, 20 }, { 3, 10 }, { 2, 5 } } },
        { "limon", new Dictionary<int, int> { { 5, 20 }, { 4, 10 }, { 3, 5 }, { 2, 2 } } }
    };

    /// <summary>
    /// Name of the pattern. Can be overridden by derived classes.
    /// </summary>
    
    [Tooltip("Name of the pattern. Can be overridden by derived classes.")]
    public virtual string PatternName { get; protected set; } = "BaseSymbolPattern";

    /// <summary>
    /// Evaluates the given pattern to determine if it matches and calculates the reward.
    /// Activates the corresponding squares but does not deactivate any squares.
    /// </summary>
    /// <param name="patternIndexes">Indexes of the symbols to evaluate in the pattern.</param>
    /// <param name="symbolsToCheck">List of symbols currently in play.</param>
    /// <param name="activeSquares">List of active square GameObjects to highlight matches.</param>
    /// <returns>The amount of credits won based on the pattern match; otherwise, 0.</returns>
    public virtual int Evaluate(int[] patternIndexes, List<Symbols> symbolsToCheck, List<GameObject> activeSquares)
    {
        if (patternIndexes == null || patternIndexes.Length == 0) return 0;
        if (symbolsToCheck == null || activeSquares == null) return 0;

        string firstSymbol = symbolsToCheck[patternIndexes[0]].name;
        int matchCount = 1;

        for (int i = 1; i < patternIndexes.Length; i++)
        {
            if (symbolsToCheck[patternIndexes[i]].name == firstSymbol)
            {
                ActivateSquare(activeSquares, patternIndexes[i]);
                matchCount++;
            }
            else
            {
                break;
            }
        }

        if (rewards.TryGetValue(firstSymbol, out var symbolRewards) &&
            symbolRewards.TryGetValue(matchCount, out int reward))
        {
            ActivateSquare(activeSquares, patternIndexes[0]);
            return reward;
        }

        
        return 0;
    }

    /// <summary>
    /// Activates the highlight for a specific square at the given index.
    /// </summary>
    /// <param name="activeSquares">List of active square GameObjects.</param>
    /// <param name="index">Index of the square to activate.</param>
    protected void ActivateSquare(List<GameObject> activeSquares, int index)
    {
        if (index >= 0 && index < activeSquares.Count)
        {
            Transform child = activeSquares[index].transform.GetChild(0);
            if (child != null) child.gameObject.SetActive(true);
        }
    }
}