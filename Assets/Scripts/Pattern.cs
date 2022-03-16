using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Array2DEditor;

[CreateAssetMenu(fileName = "Pattern", menuName = "Patterns/Create new Pattern")]
public class Pattern : ScriptableObject
{
    public int x = 2, y = 2;
    public Array2DBool patternArray;
}
