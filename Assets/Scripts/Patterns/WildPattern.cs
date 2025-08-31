using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a pattern where all symbols in the given indexes must be the same
/// to trigger a reward. Inherits reward rules and utility methods from BaseSymbolPattern.
/// </summary>
public class WildPattern : BaseSymbolPattern
{
    /// <summary>
    /// Evaluates the provided indexes to determine if all symbols match
    /// and calculates the reward based on the length of the match.
    /// Activates corresponding squares but does NOT deactivate any squares.
    /// </summary>
    /// <param name="indx">Indexes of the symbols to evaluate in the pattern.</param>
    /// <param name="symbolsToCheck">List of symbols currently in play.</param>
    /// <param name="activeSquares">List of active square GameObjects to highlight matches.</param>
    /// <returns>The amount of credits won if all symbols match; otherwise, 0.</returns>
    public override int Evaluate(int[] indx, List<Symbols> symbolsToCheck, List<GameObject> activeSquares)
    {
        if (indx == null || indx.Length == 0) return 0;
        if (symbolsToCheck == null || activeSquares == null) return 0;

        string firstSymbol = symbolsToCheck[indx[0]].name;
        bool allEqual = true;

        for (int i = 1; i < indx.Length; i++)
        {
            if (symbolsToCheck[indx[i]].name != firstSymbol)
            {
                allEqual = false;
                break;
            }
        }

        if (allEqual)
        {
            foreach (int i in indx)
            {
                if (activeSquares[i] != null && activeSquares[i].transform.childCount > 0)
                {
                    GameObject indicator = activeSquares[i].transform.GetChild(0).gameObject;
                    if (!indicator.activeInHierarchy)
                        indicator.SetActive(true);
                }
            }
            if (rewards.ContainsKey(firstSymbol) && rewards[firstSymbol].ContainsKey(indx.Length))
            {
                return rewards[firstSymbol][indx.Length];
            }
        }
       
        return 0;
    }
}