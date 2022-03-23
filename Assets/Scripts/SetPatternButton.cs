using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetPatternButton : MonoBehaviour
{
    [SerializeField]
    private Pattern pattern;

    void Start()
    {
        GetComponent<Image>().sprite = pattern.patternSprite;
        name = pattern.name;
    }

    public void SetActivePattern() {
        FindObjectOfType<Game>().SetActivePattern(pattern);
    }
    public void SetPattern(Pattern pattern) {
        this.pattern = pattern;
    }
    public void SetPatternAliveOnDragRelease() {
        FindObjectOfType<Game>().SetPatternAliveOnDragRelease();
    }
}
