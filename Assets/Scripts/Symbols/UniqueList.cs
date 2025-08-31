using System.Collections.Generic;
using UnityEngine;

public class UniqueList : MonoBehaviour
{
    /// <summary>
    /// List of unique symbols used in this context.
    /// </summary>
    [Header("Symbol Collection")]
    [Tooltip("List containing unique symbols used for gameplay or pattern checking.")]
    public List<Symbols> symbols = new List<Symbols>();
}
