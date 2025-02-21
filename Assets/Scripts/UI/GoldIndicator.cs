using System;
using Core;
using TMPro;
using UnityEngine;
using Mathf = UnityEngine.Mathf;

public class GoldIndicator : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private float elapsedLerp;
    [SerializeField] private float timeTowards;
    
    [SerializeField] private float speed = 5f;
    private Wallet wallet;
    private float textGoal;
    
    private void UpdateText()
    {
        if (!this.wallet) return;
        
        this.textGoal = this.wallet.Gold;
    }
    
    private void Start()
    {
        this.wallet = FindFirstObjectByType<Wallet>();

        this.wallet.GoldChanged.AddListener(UpdateText);
        UpdateText();
    }

    private void Update()
    {
        float lerpedValue = Mathf.Lerp(Int32.Parse(this.goldText.text), this.textGoal, Easing.Transform(EasingStyle.Sine, EasingDirection.Out, 10f * Time.deltaTime));
        
        this.goldText.text = lerpedValue.ToString("0");
        if (Mathf.Abs(this.textGoal - lerpedValue) < 2f) this.goldText.text = this.textGoal.ToString("0");
    }
}
