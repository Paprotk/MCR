using System.Collections.Generic;
using Sims3.SimIFace;
using Sims3.UI;

namespace Arro.MCR
{
    public static class EffectManager
    {
        
        public static void AddFadeEffect(WindowBase window, 
            float duration = 0.2f, 
            EffectBase.TriggerTypes triggerType = EffectBase.TriggerTypes.Invisible,
            EffectBase.InterpolationTypes interpolationType = EffectBase.InterpolationTypes.EaseInOut)
        
        {
            FadeEffect fade = new FadeEffect();
            fade.Duration = duration;
            fade.TriggerType = triggerType;
            fade.InterpolationType = interpolationType;
            window.EffectList.Add(fade);
            window.Tag = fade;
        }
        
        public static void AddScaleEffect(WindowBase window, 
            float scale = 1.1f,
            float duration = 0.2f,
            EffectBase.TriggerTypes triggerType = EffectBase.TriggerTypes.MouseFocus,
            EffectBase.InterpolationTypes interpolationType = EffectBase.InterpolationTypes.EaseInOut,
            bool autoReverse = true)
        {
            InflateEffect effect = new InflateEffect();
            effect.Scale = scale;
            effect.Duration = duration;
            effect.TriggerType = triggerType;
            if (autoReverse) effect.ResetEffect(true);
            effect.InterpolationType = interpolationType;
            window.EffectList.Add(effect);
            window.Tag = effect;
        }

        public static void AddRotateEffect(WindowBase window,
            float angle = 10f,
            Vector3 axis = default,
            float duration = 0.2f,
            EffectBase.TriggerTypes triggerType = EffectBase.TriggerTypes.MouseFocus,
            EffectBase.InterpolationTypes interpolationType = EffectBase.InterpolationTypes.EaseInOut,
            bool autoReverse = true)
        {
            if (axis == default) axis = new Vector3(0, 0, 1);
    
            RotateEffect rotate = new RotateEffect();
            rotate.Angle = angle;
            rotate.RotationAxis = axis;
            rotate.Duration = duration;
            rotate.TriggerType = triggerType;
            if (autoReverse) rotate.ResetEffect(true);
            rotate.InterpolationType = interpolationType;
            window.EffectList.Add(rotate);
            window.Tag = rotate;
        }
        
        public static void AddGlideEffect(WindowBase window, 
            Vector2 offset,
            float duration = 0.2f,
            EffectBase.TriggerTypes triggerType = EffectBase.TriggerTypes.MouseFocus,
            EffectBase.InterpolationTypes interpolationType = EffectBase.InterpolationTypes.EaseInOut,
            bool autoReverse = true)
        {
            GlideEffect glide = new GlideEffect();
            glide.Offset = offset * TinyUIFixForTS3Integration.getUIScale();
            glide.Duration = duration;
            glide.TriggerType = triggerType;
            if (autoReverse) glide.ResetEffect(true);
            glide.InterpolationType = interpolationType;
            window.EffectList.Add(glide);
            window.Tag = glide;
        }
        
        public static void AddGrowEffect(WindowBase window, 
            float leftChange = 0f, 
            float topChange = 0f, 
            float rightChange = 0f, 
            float bottomChange = 0f,
            float duration = 0.2f,
            EffectBase.TriggerTypes triggerType = EffectBase.TriggerTypes.MouseFocus,
            EffectBase.InterpolationTypes interpolationType = EffectBase.InterpolationTypes.EaseInOut)
        {
            GrowEffect grow = new GrowEffect();
            grow.BoundChangeRect = new Rect(leftChange * TinyUIFixForTS3Integration.getUIScale(), topChange * TinyUIFixForTS3Integration.getUIScale(), rightChange * TinyUIFixForTS3Integration.getUIScale(), bottomChange * TinyUIFixForTS3Integration.getUIScale());
            grow.Duration = duration;
            grow.TriggerType = triggerType;
            grow.InterpolationType = interpolationType;
            window.EffectList.Add(grow);
            window.Tag = grow;
        }
        
        public static void RemoveAllEffects(WindowBase window)
        {
            List<EffectBase> effectsToRemove = new List<EffectBase>();
    
            foreach (object obj in window.EffectList)
            {
                if (obj is EffectBase effect)
                {
                    effectsToRemove.Add(effect);
                }
            }
            
            foreach (EffectBase effect in effectsToRemove)
            {
                window.EffectList.Remove(effect);
                effect.Dispose();
            }
        }
        
        public static class TinyUIFixForTS3Integration
        {
            public delegate float FloatGetter();

            public static FloatGetter getUIScale = () => 1f;
        }
    }
}