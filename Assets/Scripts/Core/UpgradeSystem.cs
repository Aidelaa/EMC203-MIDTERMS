using System;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class UpgradeSystem : MonoBehaviour
{
    private Wallet wallet;
    
    public UnityEvent<string> UpgradeCall;

    public int rangeUpgraded = 1;
    public int turretSpeedUpgraded = 1;
    public int bulletStrengthUpgraded = 1;
    
    private void Start() => this.wallet = Wallet.instance;

    // &var equivalent
    public void BuyUpgrade(ref int upgradeReference, ref TextMeshProUGUI text, string name, GameObject button)
    {
        if (this.wallet.Gold < 5) return;
        this.wallet.Gold -= 5;
                
        upgradeReference = Mathf.Clamp(upgradeReference + 1, 0, 5);
        if (upgradeReference >= 5) button.SetActive(false);   
        text.text = $"LV: {upgradeReference}";
        this.UpgradeCall.Invoke(name);
    }

    public void UpdateRange(GameObject button)
    {
        TextMeshProUGUI textMeshProUGUI = button.transform.Find("Text")?.GetComponent<TextMeshProUGUI>();
        BuyUpgrade(ref this.rangeUpgraded,  ref textMeshProUGUI, "Range", button);
    }

    public void UpdateTurretSpeed(GameObject button)
    {
        TextMeshProUGUI textMeshProUGUI = button.transform.Find("Text")?.GetComponent<TextMeshProUGUI>();
        BuyUpgrade(ref this.turretSpeedUpgraded, ref textMeshProUGUI, "Speed", button);
    }

    public void UpdateBulletStrength(GameObject button)
    {
        TextMeshProUGUI textMeshProUGUI = button.transform.Find("Text")?.GetComponent<TextMeshProUGUI>();
        BuyUpgrade(ref this.bulletStrengthUpgraded, ref textMeshProUGUI, "Strength", button);
    }
}
