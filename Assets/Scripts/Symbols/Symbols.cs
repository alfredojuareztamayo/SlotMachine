using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Symbols
{
    /// <summary>
    /// The name identifier of the symbol.
    /// </summary>
    [Header("Symbol Data")]
    [Tooltip("Unique name identifier of the symbol.")]
    public string name;
    /// <summary>
    /// The sprite image associated with the symbol.
    /// </summary>
    [Header("Symbol Sprite")]
    [Tooltip("Sprite image used to visually represent this symbol.")]
    public Sprite Sprite;
}
