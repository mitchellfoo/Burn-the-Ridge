using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuUI : MonoBehaviour
{
    [Header("UI Variables")]
    public TextMeshProUGUI totalScore;
    public TextMeshProUGUI prestige;

    // Start is called before the first frame update
    void Start()
    {
        totalScore.text = "" + GameManager.S.GetGameScore();

        prestige.text = "" + GameManager.S.GetPrestige();
    }
}
