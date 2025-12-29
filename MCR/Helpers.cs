using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using Sims3.SimIFace;
using Sims3.UI;

namespace Arro.MCR
{
    public class Helpers
    {
        public static void CheckForMods()
        {
            AppDomain currentDomain = AppDomain.CurrentDomain;
            Assembly[] assems = currentDomain.GetAssemblies();
            foreach (Assembly assembly in assems)
            {
                if (assembly.GetName().Name == "NRaasMasterController")
                {
                    return;
                }
                if (assembly.GetName().Name == "LazyDuchess.SmoothPatch")
                {
                    var clothingPerformanceType = assembly.GetType("LazyDuchess.SmoothPatch.ClothingPerformance");
                    if (clothingPerformanceType != null)
                    {
                        var originalMethod = GetMethod(clothingPerformanceType, "OnWorldLoad");
                        var originalMethodHandle = originalMethod.MethodHandle.Value;
                        var hookMethod = GetMethod(typeof(Helpers), "OnWorldLoad");
                        var hookMethodHandle = hookMethod.MethodHandle.Value;
                        var replacementByteArray1 = new byte[40];
                        Marshal.Copy(hookMethodHandle, replacementByteArray1, 0, 40);
                        Marshal.Copy(replacementByteArray1, 0, originalMethodHandle, 40);
                        break;
                    }
                }
            }
        }
        
        private static void OnWorldLoad(object sender, EventArgs e)
        {
            return;
        }

        private static MethodInfo GetMethod(Type type, string methodName)
        {
            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static |
                                          BindingFlags.Instance);
            foreach (var method in methods)
            {
                if (method.Name == methodName)
                    return method;
            }

            return null;
        }
    }
    
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
            bool autoReverse = true)
        {
            InflateEffect effect = new InflateEffect();
            effect.Scale = scale;
            effect.Duration = duration;
            effect.TriggerType = triggerType;
            if (autoReverse) effect.ResetEffect(true);
            window.EffectList.Add(effect);
            window.Tag = effect;
        }

        public static void AddRotateEffect(WindowBase window,
            float angle = 10f,
            Vector3 axis = default,
            float duration = 0.2f,
            EffectBase.TriggerTypes triggerType = EffectBase.TriggerTypes.MouseFocus,
            bool autoReverse = true)
        {
            if (axis == default) axis = new Vector3(0, 0, 1);
    
            RotateEffect rotate = new RotateEffect();
            rotate.Angle = angle;
            rotate.RotationAxis = axis;
            rotate.Duration = duration;
            rotate.TriggerType = triggerType;
            if (autoReverse) rotate.ResetEffect(true);
            window.EffectList.Add(rotate);
            window.Tag = rotate;
        }
        
        public static void AddGlideEffect(WindowBase window, 
            Vector2 offset,
            float duration = 0.2f,
            EffectBase.TriggerTypes triggerType = EffectBase.TriggerTypes.MouseFocus,
            bool autoReverse = true)
        {
            GlideEffect glide = new GlideEffect();
            glide.Offset = offset;
            glide.Duration = duration;
            glide.TriggerType = triggerType;
            if (autoReverse) glide.ResetEffect(true);
            window.EffectList.Add(glide);
            window.Tag = glide;
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
    }
}