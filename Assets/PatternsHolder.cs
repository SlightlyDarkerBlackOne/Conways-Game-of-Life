using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PatternsHolder : MonoBehaviour
{
    public List<Pattern> patterns;

    void Awake()
    {
        patterns = new List<Pattern>(Resources.LoadAll<Pattern>("Scriptable Objects"));
    }
}
