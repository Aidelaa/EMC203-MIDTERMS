using System;
using UnityEngine;
using UnityEngine.Events;

public class Wallet : MonoBehaviour
{
    public static Wallet instance;
    public UnityEvent GoldChanged;

    [SerializeField] private GameObject goldObject;

    public void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this);

        instance = this;
    }

    public int Gold
    {
        get => this.gold;
        set
        {
            this.gold = Mathf.Clamp(value, 0, int.MaxValue);
            this.GoldChanged.Invoke();
        }
    }
    private int gold;

    public void SpawnGold(int amount, Vector3 worldPosition)
    {
        GameObject nGoldObject = Instantiate(this.goldObject, worldPosition, Quaternion.identity);
        GoldObject goldObjectScript = nGoldObject.GetComponent<GoldObject>();
        goldObjectScript.GoldMoved.AddListener(() => this.Gold += amount);
    }
}
