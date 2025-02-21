using UnityEngine;

namespace Core
{
    public enum EasingStyle
    {
        SmoothStep = 0,
        Sine = 1,
        Cubic = 2,
        Circular = 3,
        Quadratic = 4,
        Back = 5,
        Linear = 6,
    }

    public enum EasingDirection
    {
        In = 0,
        Out = 1,
    }
    
    public abstract class Easing
    {
        public static float Transform(EasingStyle style, EasingDirection direction, float t)
        {
            return (direction, style) switch
            {
                // In
                (EasingDirection.In, EasingStyle.SmoothStep) => SmoothStep(t),
                (EasingDirection.In, EasingStyle.Sine) => SineIn(t),
                (EasingDirection.In, EasingStyle.Cubic) => CubicIn(t),
                (EasingDirection.In, EasingStyle.Circular) => CircIn(t),
                (EasingDirection.In, EasingStyle.Quadratic) => QuadIn(t),
                (EasingDirection.In, EasingStyle.Back) => BackIn(t),
                // Out
                (EasingDirection.Out, EasingStyle.SmoothStep) => SmoothStep(t),
                (EasingDirection.Out, EasingStyle.Sine) => SineOut(t),
                (EasingDirection.Out, EasingStyle.Cubic) => CubicOut(t),
                (EasingDirection.Out, EasingStyle.Circular) => CircOut(t),
                (EasingDirection.Out, EasingStyle.Quadratic) => QuadOut(t),
                (EasingDirection.Out, EasingStyle.Back) => BackOut(t),
                _ => Linear(t)
            };
        }
        
        public static float SmoothStep(float theta) => Mathf.SmoothStep(0f, 1f, theta);

        public static float Linear(float t) => t;

        // in
        public static float SineIn(float theta) => 1 - Mathf.Cos((theta * Mathf.PI) / 2f);

        public static float CubicIn(float theta) => Mathf.Pow(theta, 3f);

        public static float CircIn(float theta) => 1 - Mathf.Sqrt(1 - Mathf.Pow(theta, 2f));

        public static float QuadIn(float theta) => Mathf.Pow(theta, 2f);

        public static float BackIn(float theta)
        {
            // derived from easings.net
            float c1 = 1.70158f;
            float c3 = c1 + 1;

            return (c3 * theta * theta * theta) - (c1 * theta * theta);
        }

        // out
        public static float SineOut(float theta) => Mathf.Sin((theta * Mathf.PI) / 2f);

        public static float CubicOut(float theta) => 1 - Mathf.Pow(1 - theta, 2f);

        public static float CircOut(float theta) => Mathf.Sqrt(1 - Mathf.Pow(1 - theta, 2f));

        public static float QuadOut(float theta) => 1 - (1 - theta) * (1 - theta);

        public static float BackOut(float theta)
        {
            float c1 = 1.70158f;
            float c3 = c1 + 1;

            return 1 + c3 * Mathf.Pow(theta - 1, 3f) + c1 * Mathf.Pow(theta - 1, 2f);
        }
    }
}
