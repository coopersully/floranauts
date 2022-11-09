using System.Collections;
using System.Collections.Generic;
using Player;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [Header("-Score UI-")]
    public TextMeshProUGUI bluePlayerScoreUI;
    public TextMeshProUGUI redPlayerScoreUI;

    public void Awake()
    {
        bluePlayerScoreUI.SetText("Score: 000");
        redPlayerScoreUI.SetText("Score: 000");
    }
    
}