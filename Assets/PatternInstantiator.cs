using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternInstantiator : MonoBehaviour
{
    [SerializeField]
    private GameObject patternUIPrefab;

    void Start()
    {
        List<Pattern> patterns = FindObjectOfType<PatternsHolder>().patterns;
        foreach (var pattern in patterns) {
            GameObject prefab = Instantiate(patternUIPrefab, this.transform);
            prefab.GetComponent<SetPatternButton>().SetPattern(pattern);
        }
    }
}
