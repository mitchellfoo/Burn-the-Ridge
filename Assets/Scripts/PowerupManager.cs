using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PowerupManager : MonoBehaviour
{
    [Header("Grade Variables")]
    public int aPercent = 70;
    public int bPercent = 40;

    [Header("UI Variables")]
    public TextMeshProUGUI roundGrade;
    public TextMeshProUGUI roundScore;
    public TextMeshProUGUI totalScore;

    public TextMeshProUGUI totalUpgrades;

    public TextMeshProUGUI gunsUpgrade;
    public TextMeshProUGUI gunsLevel;
    public TextMeshProUGUI shipUpgrade;
    public TextMeshProUGUI shipLevel;
    public TextMeshProUGUI techUpgrade;
    public TextMeshProUGUI techLevel;

    public TextMeshProUGUI gunsSelect;
    public TextMeshProUGUI shipSelect;
    public TextMeshProUGUI techSelect;

    private int upgradesAfforded;
    private string gunsOption;
    private string shipOption;
    private string techOption;

    private Dictionary<string, int> gradeValue = new Dictionary<string, int>()
    {
        { "A", 3 },
        { "B", 2 },
        { "C", 1 },
    };

    // Start is called before the first frame update
    void Start()
    {
        // Determine round grade and upgrade earned
        string grade = determineGrade();

        // Determine upgrade options
        gunsOption = getRandomUpgrade(GameManager.S.gunsUpgrades);
        shipOption = getRandomUpgrade(GameManager.S.shipUpgrades);
        techOption = getRandomUpgrade(GameManager.S.techUpgrades);

        // Set Text Values
        roundGrade.text = grade;
        roundScore.text = "" + GameManager.S.GetRoundScore();
        totalScore.text = "" + GameManager.S.GetGameScore();

        upgradesAfforded = gradeValue[grade];
        totalUpgrades.text = "" + upgradesAfforded;

        gunsUpgrade.text = gunsOption;
        gunsLevel.text = "" + GameManager.S.powerups[gunsOption];
        shipUpgrade.text = shipOption;
        shipLevel.text = "" + GameManager.S.powerups[shipOption];
        techUpgrade.text = techOption;
        techLevel.text = "" + GameManager.S.powerups[techOption];

        gunsSelect.text = gunsOption;
        shipSelect.text = shipOption;
        techSelect.text = techOption;

        // Set Buttons 
        gunsSelect.enabled = false;
        shipSelect.enabled = false;
        techSelect.enabled = false;
    }

    private string determineGrade()
    {
        int maxScore = GameManager.S.GetMaxRoundScore();
        int roundScore = GameManager.S.GetRoundScore();
        float divScore = (float)roundScore / (float)maxScore * 100;
        Debug.Log("" + divScore);

        if (divScore >= aPercent)
        {
            return "A";
        }
        else if (divScore >= bPercent)
        {
            return "B";
        }
        else
        {
            return "C";
        }
    }

    private string getRandomUpgrade(string[] upgrades)
    {
        int index = Random.Range(0, upgrades.Length);
        return upgrades[index];
    }

    public void SelectGunUpgrade()
    {
        if (gunsSelect.isActiveAndEnabled)
        {
            gunsSelect.enabled = false;
            upgradesAfforded++;
            totalUpgrades.text = "" + upgradesAfforded;
        }
        else if (upgradesAfforded > 0)
        {
            gunsSelect.enabled = true;
            upgradesAfforded--;
            totalUpgrades.text = "" + upgradesAfforded;
        }
    }

    public void SelectShipUpgrade()
    {
        if (shipSelect.isActiveAndEnabled)
        {
            shipSelect.enabled = false;
            upgradesAfforded++;
            totalUpgrades.text = "" + upgradesAfforded;
        }
        else if (upgradesAfforded > 0)
        {
            shipSelect.enabled = true;
            upgradesAfforded--;
            totalUpgrades.text = "" + upgradesAfforded;
        }
    }

    public void SelectTechUpgrade()
    {
        if (techSelect.isActiveAndEnabled)
        {
            techSelect.enabled = false;
            upgradesAfforded++;
            totalUpgrades.text = "" + upgradesAfforded;
        }
        else if (upgradesAfforded > 0)
        {
            techSelect.enabled = true;
            upgradesAfforded--;
            totalUpgrades.text = "" + upgradesAfforded;
        }
    }

    public void ApplyUpgrades()
    {
        if (gunsSelect.isActiveAndEnabled)
        {
            GameManager.S.powerups[gunsSelect.text]++;
        }

        if (shipSelect.isActiveAndEnabled)
        {
            GameManager.S.powerups[shipSelect.text]++;
        }
        if (techSelect.isActiveAndEnabled)
        {
            GameManager.S.powerups[techSelect.text]++;
        }
    }
}
