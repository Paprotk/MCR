using System;
using System.Reflection;
using System.Runtime.InteropServices;

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
}