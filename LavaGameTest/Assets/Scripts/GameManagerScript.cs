using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    [SerializeField]
    private GameObject jumpPlace;
    [SerializeField]
    private GameObject player;
    [Space]
    [SerializeField]
    private List<LevelData> levels;
    [Space]
    [SerializeField]
    private TextMeshProUGUI jumpCounter;

    private int currentLevel;
    private int jumpCount;

    public int CurrentLevel
    {
        get => currentLevel;
        set
        {
            currentLevel = value;
            if (currentLevel == levels.Count)
            {
                currentLevel = 0;
            }
            jumpPlace.transform.localScale = new Vector3(levels[currentLevel].JumpPlaceSize, 0.01f, levels[currentLevel].JumpPlaceSize);
            player.GetComponent<PlayerScript>().JumpHeight = levels[currentLevel].JumpHeight;
        }
    }

    public int JumpCount
    {
        get => jumpCount;
        set
        {
            jumpCount = value;
            PlayerPrefs.SetInt("JumpCount", jumpCount);
            jumpCounter.text = $"Jump Count: {jumpCount}";
            CurrentLevel++;
        }
    }

    void Start()
    {
        JumpCount = PlayerPrefs.GetInt("JumpCount");
        CurrentLevel = 0;
    }
}