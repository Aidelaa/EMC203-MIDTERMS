using System;
using System.Collections.Generic;
using System.Reflection;
using Core;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Tween
{
    public class PropertyValues
    {
        public PropertyInfo property;
        public object startingValue;

        public PropertyValues(PropertyInfo property, object startingValue)
        {
            this.property = property;
            this.startingValue = startingValue;
        }
    }
    public class TweenObject
    {
        public GameObject targetObject;
        public Component targetComponent;
        
        public Dictionary<string, object> targetProperty { get; private set; }
        public Dictionary<string, PropertyValues> startingValues { get; private set; }
        public EasingStyle style;
        public EasingDirection direction;
        public float duration = 0f;
        public float timeframe = 0f;
        
        public Type componentType { get; private set; }
        public bool completed { get; private set; } = false;
        public bool paused { get; private set; } = false;

        private string identifier;
        public float theta { get; private set; } = 0f;
        public float computedTheta { get; private set; } = 0f;
        public readonly UnityEvent OnCompleted = new UnityEvent();
        public readonly UnityEvent OnPaused = new UnityEvent();
        public readonly UnityEvent OnResume = new UnityEvent();

        public TweenObject(GameObject targetObject, Component target, float duration = 1f, EasingStyle style = EasingStyle.Linear, EasingDirection direction = EasingDirection.In, Dictionary<string, object> properties = null)
        {
            if (properties == null) throw new Exception("Target property must not be null");
            this.targetObject = targetObject;
            this.targetProperty = properties;
            this.targetComponent = target;
            
            this.duration = duration;
            this.style = style;
            this.direction = direction;
            
            FillStartingPropertyValues();
            this.OnCompleted.AddListener(() =>
            {
                this.completed = true;
                
                Destroy();
            });
        }

        public void FillStartingPropertyValues()
        {
            this.componentType = this.targetComponent?.GetType();
            this.startingValues = new Dictionary<string, PropertyValues>();
            
            foreach (KeyValuePair<string, object> pair in this.targetProperty)
            {
                string propertyName = pair.Key;
                
                PropertyInfo property = this.componentType.GetProperty(propertyName);
                if (property != null && property.CanWrite)
                {
                    object propertyValue = property.GetValue(this.targetComponent);
                    
                    if (propertyValue is Vector3 vector3) 
                        this.startingValues[propertyName] = new PropertyValues(property, vector3);
                    else if (propertyValue is Quaternion quaternion)
                        this.startingValues[propertyName] = new PropertyValues(property, quaternion);
                    else if (propertyValue is Color color)
                        this.startingValues[propertyName] = new PropertyValues(property, color);
                    else if (propertyValue is float f)
                        this.startingValues[propertyName] = new PropertyValues(property, f);
                    else if (propertyValue is int i)
                        this.startingValues[propertyName] = new PropertyValues(property, i);
                    else if (propertyValue is Vector2 vector2)
                        this.startingValues[propertyName] = new PropertyValues(property, vector2);
                    else
                        throw new NotSupportedException("Unsupported type: " + propertyValue.GetType());
                    
                } else
                    throw new Exception($"Unknown property: {propertyName} of Component {this.targetComponent.GetType()}");
            }
        }
        
        public void Destroy()
        {
            this.targetComponent = null;
            this.targetObject = null;

            this.targetProperty.Clear();
            this.targetProperty = null;
        }
        
        public void Step(float? dt)
        {
            if (this.targetProperty == null) return;
            if (!this.targetComponent || !this.targetObject) 
            {
                Destroy();
                return;
            };
            
            if (this.timeframe >= this.duration-.05f)
            {
                this.OnCompleted.Invoke();
            };
            
            
            this.timeframe += dt ?? Time.deltaTime;
            this.theta = Mathf.Clamp01(this.timeframe / this.duration);
            
            this.computedTheta = Easing.Transform(this.style, this.direction, this.theta);
            if (this.componentType == null) return;
            
            foreach (KeyValuePair<string, PropertyValues> pair in this.startingValues)
            {
                if (this.targetProperty == null) return;
                string propertyName = pair.Key;
                object targetValue = this.targetProperty[propertyName];
                
                Debug.Log(targetValue);
                if (targetValue == null) continue;
                
                PropertyValues value = pair.Value;
                if (value == null) continue;
                
                PropertyInfo property = value.property;
                if (property != null && property.CanWrite)
                {
                    object propertyValue = value.startingValue;

                    if (propertyValue is Vector3)
                    {
                        Vector3 Lerped = Vector3.Lerp((Vector3)propertyValue, (Vector3)targetValue, this.computedTheta);
                        Debug.Log($"Transformed Lerp: {Lerped}");
                        property.SetValue(this.targetComponent, Lerped);
                        
                    }
                    else if (propertyValue is Quaternion)
                        property.SetValue(this.targetComponent, Quaternion.Lerp((Quaternion)propertyValue, (Quaternion)targetValue, this.computedTheta));
                    else if (propertyValue is Color)
                        property.SetValue(this.targetComponent, Color.Lerp((Color)propertyValue, (Color)targetValue, this.computedTheta));
                    else if (propertyValue is float)
                        property.SetValue(this.targetComponent, Mathf.Lerp((float)propertyValue, (float)targetValue, this.computedTheta));
                    else if (propertyValue is int)
                        property.SetValue(this.targetComponent, (int)Mathf.Lerp((int)propertyValue, (float)targetValue, this.computedTheta));
                    else if (propertyValue is Vector2)
                        property.SetValue(this.targetComponent, Vector2.Lerp((Vector2)propertyValue, (Vector2)targetValue, this.computedTheta));
                    else
                        throw new NotSupportedException("Unsupported type: " + propertyValue.GetType());
                    
                } else
                    throw new Exception($"Unknown property: {propertyName} of Component {this.targetComponent.GetType()}");
            }
        }

        public void Pause()
        {
            this.paused = true;
            this.OnPaused.Invoke();
        }
        public void Resume()
        {
            this.paused = false;
            this.OnResume.Invoke();
        }
        public void Cancel()
        {
            this.timeframe = this.duration;
            this.completed = true;
        }
        
        public void End() => Cancel();
        
        public string GetIdentity() => this.identifier;
        public string SetIdentity(string id) => this.identifier = id;
    }
}