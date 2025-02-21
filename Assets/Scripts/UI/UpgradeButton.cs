using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UpgradeButton : MonoBehaviour
{
    [SerializeField] private GameObject upgradePanel;
    [SerializeField] private Transform upgradeContainer; // Parent of upgrade buttons
    [SerializeField] private GameObject upgradeButtonPrefab; // Prefab for each upgrade button

    private Dictionary<string, int> towerLevels = new Dictionary<string, int>
    {
        { "Arcane Bolt Tower", 1 },
        { "Frost Sniper Tower", 1 },
        { "Inferno Wave Tower", 1 }
    };

    public void FlickUpgrade()
    {
        upgradePanel.SetActive(!upgradePanel.activeInHierarchy);

        if (upgradePanel.activeInHierarchy)
        {
            PopulateUpgradeOptions();
        }
    }

    private void PopulateUpgradeOptions()
    {
        // Clear existing buttons before populating
        foreach (Transform child in upgradeContainer)
        {
            Destroy(child.gameObject);
        }

        // Create upgrade buttons for each tower
        foreach (var tower in towerLevels)
        {
            GameObject buttonObj = Instantiate(upgradeButtonPrefab, upgradeContainer);
            Button button = buttonObj.GetComponent<Button>();
            Text buttonText = buttonObj.GetComponentInChildren<Text>();

            buttonText.text = $"{tower.Key} Lvl {tower.Value} → Lvl {tower.Value + 1}";

            // Add click event
            string towerName = tower.Key;
            button.onClick.AddListener(() => UpgradeTower(towerName, buttonText));
        }
    }

    private void UpgradeTower(string towerName, Text buttonText)
    {
        if (towerLevels[towerName] < 5) // Max level 5
        {
            towerLevels[towerName]++;
            buttonText.text = $"{towerName} Lvl {towerLevels[towerName]} → Lvl {towerLevels[towerName] + 1}";
        }
        else
        {
            buttonText.text = $"{towerName} Lvl MAX";
        }
    }
}
