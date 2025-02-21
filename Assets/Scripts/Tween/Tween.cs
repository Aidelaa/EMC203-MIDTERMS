using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace Tween
{
    public class TweenManager : MonoBehaviour
    {
        public static TweenManager instance;
        
        public Dictionary<string, TweenObject> activeTweenObjects { get; private set; } = new Dictionary<string, TweenObject>();
        
        public void Awake()
        {
            if (instance != null && instance != this) throw new Exception("There can only be one instance of TweenManager.");
            
            instance = this;
        }

        public (TweenObject, string) AddTweenObject(TweenObject tweenObject)
        {
            string tweenIdentifier = Guid.NewGuid().ToString();
            
            tweenObject.SetIdentity(tweenIdentifier);
            
            this.activeTweenObjects[tweenIdentifier] = tweenObject;
            return (tweenObject, tweenIdentifier);
        }

        [CanBeNull]
        public TweenObject CutTweenObject(string tweenIdentifier)
        {
            TweenObject tweenObject = this.activeTweenObjects[tweenIdentifier];
            
            if (tweenObject == null)
                return null;
            
            tweenObject.OnCompleted.AddListener(() => this.activeTweenObjects.Remove(tweenIdentifier));
            this.activeTweenObjects.Remove(tweenIdentifier);
            return tweenObject;
        }

        public void RemoveTweenObject(string tweenIdentifier)
        {
            TweenObject tweenObject = this.activeTweenObjects[tweenIdentifier];
            if (tweenObject == null) return;
            
            this.activeTweenObjects.Remove(tweenIdentifier);
            tweenObject.Destroy();
        }

        public void Update()
        {
            foreach (KeyValuePair<string, TweenObject> tweenElement in this.activeTweenObjects.ToList())
            {
                string identifier = tweenElement.Key;
                TweenObject tween = tweenElement.Value;
                if (tween == null) 
                { 
                    this.activeTweenObjects.Remove(identifier);
                    continue;
                }

                if (tween.completed)
                {
                    this.activeTweenObjects.Remove(identifier);
                    continue;
                }
                
                tween.Step(Time.deltaTime);
            }
        }

        // public void OnGUI()
        // {
        //     int offset = 0;
        //     foreach (KeyValuePair<string, TweenObject> tweenElement in this.activeTweenObjects.ToList())
        //     {
        //         string identifier = tweenElement.Key;
        //         TweenObject tween = tweenElement.Value;
        //         if (tween.completed) continue;
        //
        //         GUI.Label(new Rect(0, 24 * offset, 1920, 24),
        //             $"Tween: {identifier} t:{tween.theta} et: {tween.computedTheta} {tween.completed}");
        //         offset += 1;
        //     }
        // }
    }
}
