using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetPatternButton : MonoBehaviour
{
    [SerializeField]
    private Pattern pattern;
    private Game game;
    void Start()
    {
        GetComponent<Image>().sprite = pattern.patternSprite;
        name = pattern.name;
        game = FindObjectOfType<Game>();
    }

    public void SetActivePattern() {
        game.SetActivePattern(pattern);
    }
    public void SetPattern(Pattern pattern) {
        this.pattern = pattern;
    }
    public void SetPatternAliveOnDragRelease() {
        game.SetPatternAliveOnDragRelease();
    }
}
