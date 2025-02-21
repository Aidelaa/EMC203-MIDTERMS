using System;
using System.Collections.Generic;
using Core;
using TMPro;
using Tween;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    public UnityEvent OnPause = new UnityEvent();
    private bool paused = false;
    
    [SerializeField] private GameObject pauseMenu;
    private Image pauseImage;
    private TextMeshProUGUI pauseLabel;

    private void Awake()
    {
        this.pauseImage = this.pauseMenu.GetComponent<Image>();
        this.pauseLabel = this.pauseMenu.transform.Find("Label").GetComponent<TextMeshProUGUI>();
    }
    
    private void PauseEvent()
    {
        this.paused = !this.paused;
        if (!this.paused) Time.timeScale = 1f;
        
        this.pauseMenu.SetActive(this.paused);
        
        this.pauseImage.color = new Color(0f, 0f, 0f, this.paused ? 0f : 1f);
        this.pauseLabel.color = new Color(1f, 1f, 1f, this.paused ? 0f : 1f);

        float duration = .5f;
        TweenObject pauseImageTween = new TweenObject(this.pauseImage.gameObject, this.pauseImage, duration,
            EasingStyle.Sine, EasingDirection.Out, new Dictionary<string, object>()
            {
                { "color", new Color(.25f, .25f, .25f, this.paused ? 1f : 0f) }
            });

        TweenObject pauseLabelTween = new TweenObject(this.pauseLabel.gameObject, this.pauseLabel, duration,
            EasingStyle.Sine, EasingDirection.Out, new Dictionary<string, object>()
            {
                { "color", new Color(1f, 1f, 1f, this.paused ? 1f : 0f) }
            });
        TweenManager.instance.AddTweenObject(pauseImageTween);
        TweenManager.instance.AddTweenObject(pauseLabelTween);
        
        pauseLabelTween.OnCompleted.AddListener(PauseTime);
    }
    private void Start()
    {
        this.OnPause.AddListener(PauseEvent);
    }

    private void PauseTime()
    {
        Time.timeScale = this.paused ? 0 : 1;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            PauseEvent();
    }
}
