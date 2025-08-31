using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Interface defining the contract for pattern evaluation in the slot machine game.
/// </summary>
/// 
public interface IPattern
{
    /// <summary>
    /// Evaluates the pattern based on given symbol indexes and returns the reward value.
    /// </summary>
    /// <param name="idxToCheck">Array of indexes indicating positions of symbols to evaluate.</param>
    /// <param name="symbolsToCheck">List of symbols currently in play.</param>
    /// <param name="activeSquare">List of active square GameObjects to highlight matching symbols.</param>
    /// <returns>The amount of credits won if the pattern matches; otherwise, 0.</returns>
    int Evaluate(int[] idxToCheck, List<Symbols> symbolsToCheck, List<GameObject> activeSquare);
    /// <summary>
    /// Gets the name of the pattern.
    /// </summary>
    [Tooltip("Name identifier of the pattern.")]
    string PatternName { get; }

   
}