using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;
using Tween;
using UnityEngine.Events;

public class GoldObject : MonoBehaviour
{
    private TweenManager tweenManager;
    
    [SerializeField] private RectTransform goldScreen;

    public UnityEvent GoldMoved = new UnityEvent();

    private IEnumerator MoveTo()
    {
        yield return new WaitForSeconds(.5f);
        
        Vector3 targetPosition = Camera.main.ScreenToWorldPoint(this.goldScreen.transform.position);
        
        (TweenObject, string) tweenObject = this.tweenManager.AddTweenObject(new TweenObject(this.gameObject, this.transform, 1f, EasingStyle.Quadratic,
            EasingDirection.Out, new Dictionary<string, object>()
            {
                { "position", targetPosition },
            }));
        
        tweenObject.Item1.OnCompleted.AddListener(() =>
        {
            Debug.Log("Gold Moved");
            this.GoldMoved.Invoke();
            Destroy(this.gameObject);
        });
    }
    private void Start()
    {
        this.goldScreen = GameObject.FindGameObjectWithTag("GoldUI").GetComponent<RectTransform>();
        this.tweenManager = TweenManager.instance;
        
        if (this.goldScreen == null) Destroy(this.gameObject);
        StartCoroutine(MoveTo());
    }
}
