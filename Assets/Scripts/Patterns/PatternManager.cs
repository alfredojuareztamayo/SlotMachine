using UnityEngine;
using System.Collections.Generic;
using TMPro;
/// <summary>
/// Manages symbol and reel data and evaluates winning patterns in the slot machine game.
/// Supports multiple pattern types and updates UI with reward results.
/// Handles activation and deactivation of indicators globally to avoid conflicts.
/// </summary>
public class PatternManager : MonoBehaviour
{
    [Header("Symbol and Reel References")]
    [Tooltip("List of symbols from the fifth reel used for pattern evaluation.")]
    public List<Symbols> Roller5 = new List<Symbols>();

    [Tooltip("List of active reel GameObjects corresponding to visible symbols.")]
    public List<GameObject> ReelsActives = new List<GameObject>();

    [Header("Patterns")]
    BasicLinearPattern patternLinear = new BasicLinearPattern();
    WildPattern patternWild = new WildPattern();

    private int reward;

    [Header("Reward UI")]
    [Tooltip("Text UI element to display the reward amount.")]
    public TMP_Text reward_text;

    [Header("Patterns")]
    [Tooltip("List of linear patterns to evaluate.")]
    [SerializeField]
    private List<Pattern> linearPatterns = new List<Pattern>
{
    new Pattern { values = new int[] { 0, 3, 6, 9, 12 } },
    new Pattern { values = new int[] { 1, 4, 7, 10, 13 } },
    new Pattern { values = new int[] { 2, 5, 8, 11, 14 } }
};

    [Tooltip("List of wild patterns to evaluate.")]
    [SerializeField]
    private List<Pattern> wildPatterns = new List<Pattern>
{
    new Pattern { values = new int[] { 0, 4, 8, 10, 12 } },
    new Pattern { values = new int[] { 2, 4, 6, 10, 14 } },
    new Pattern { values = new int[] { 2, 3, 8, 9, 14 } },
    new Pattern { values = new int[] { 0, 5, 6, 11, 12 } },
    new Pattern { values = new int[] { 0, 3, 7, 11, 14 } },
    new Pattern { values = new int[] { 2, 5, 7, 9, 12 } }
};


    /// <summary>
    /// Adds symbols from a reel to the evaluation list.
    /// </summary>
    /// <param name="rollerTemp">List of symbols to add.</param>
    public void FillListRollers(List<Symbols> rollerTemp)
    {
        if (rollerTemp == null)
        {
            Debug.LogError("Received null roller symbols list.");
            return;
        }
        Roller5.AddRange(rollerTemp);
    }

    /// <summary>
    /// Adds active reel GameObjects for pattern evaluation.
    /// </summary>
    /// <param name="g">List of GameObjects representing active reels.</param>
    public void FillReels(List<GameObject> g)
    {
        if (g == null)
        {
            Debug.LogError("Received null reels list.");
            return;
        }
        ReelsActives.AddRange(g);
    }

    /// <summary>
    /// Checks all defined patterns, prioritizing Wild patterns over Linear patterns.
    /// Ensures no symbol indexes are rewarded twice.
    /// Updates the reward UI text and manages activation/deactivation of indicators.
    /// </summary>
    
    public void CheckPatterns()
    {
        try
        {
            reward = 0;
            HashSet<int> indicesToActivate = new HashSet<int>();
            HashSet<int> rewardedIndices = new HashSet<int>(); // Tracks symbols that have already been rewarded

            //Evaluate Wild patterns first
            foreach (var pattern in wildPatterns)
            {
                int r = patternWild.Evaluate(pattern.values, Roller5, ReelsActives);
                if (r > 0)
                {
                    reward += r;
                    foreach (var i in pattern.values)
                    {
                        rewardedIndices.Add(i);
                        indicesToActivate.Add(i);
                    }
                }
            }

            //Evaluate Linear patterns, skipping any symbols already rewarded by Wild
            foreach (var pattern in linearPatterns)
            {
                if (!PatternAlreadyRewarded(pattern.values, rewardedIndices))
                {
                    int r = patternLinear.Evaluate(pattern.values, Roller5, ReelsActives);
                    if (r > 0)
                    {
                        reward += r;
                        foreach (var i in pattern.values)
                        {
                            rewardedIndices.Add(i);
                            indicesToActivate.Add(i);
                        }
                    }
                }
            }

            //Deactivate indicators for unused positions
            for (int i = 0; i < ReelsActives.Count; i++)
            {
                if (ReelsActives[i] != null && ReelsActives[i].transform.childCount > 0)
                {
                    GameObject indicator = ReelsActives[i].transform.GetChild(0).gameObject;
                    if (!indicesToActivate.Contains(i) && indicator.activeInHierarchy)
                    {
                        indicator.SetActive(false);
                    }
                }
            }

            // Update reward UI text
            if (reward_text != null)
                reward_text.text = reward > 0 ? $"Congratulations, you won: {reward}" : "";
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error in CheckPatterns: {ex.Message}");
            if (reward_text != null)
                reward_text.text = "Error calculating patterns.";
        }
    }

    /// <summary>
    /// Checks if the given pattern shares any symbol index that has already been rewarded.
    /// </summary>
    /// <param name="values">Array of symbol indexes to check.</param>
    /// <param name="rewardedIndices">Set of indexes that have already been rewarded.</param>
    /// <returns>True if the pattern contains any already rewarded symbol index, otherwise false.</returns>
    private bool PatternAlreadyRewarded(int[] values, HashSet<int> rewardedIndices)
    {
        foreach (int i in values)
        {
            if (rewardedIndices.Contains(i))
                return true;
        }
        return false;
    }
}

    /// <summary>
    /// Serializable container for a pattern's index sequence.
    /// </summary>
    [System.Serializable]
public class Pattern
{
    [Tooltip("Array of symbol indices that form this pattern.")]
    public int[] values;
}
