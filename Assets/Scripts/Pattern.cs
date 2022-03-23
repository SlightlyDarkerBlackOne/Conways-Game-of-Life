using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Array2DEditor;

[CreateAssetMenu(fileName = "Pattern", menuName = "Patterns/Create new Pattern")]
public class Pattern : ScriptableObject
{
    public Sprite patternSprite;
    public string description;
    public Array2DBool patternArray;
}
