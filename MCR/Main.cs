using Sims3.SimIFace;
using Sims3.UI;
using System;
using System.Reflection;
using System.Runtime.InteropServices;
using Arro.Common;
using Sims3.UI.CAS;
using Simulator = Sims3.SimIFace.Simulator;
using static Arro.Common.Logger;

namespace Arro.MCR
{
    public class Main
    {
        [GetAssembly("NRaasMasterController")]
        public static Assembly NraasMC;
        
        [GetAssembly("LazyDuchess.SmoothPatch")]
        public static Assembly LD_SmoothPatch;
        
        [Tunable]
#pragma warning disable CS0169 // Field is never used
        private static bool kInstantiator;
#pragma warning restore CS0169 // Field is never used
        
        static Main()
        {
            Core.Initialize("MCR");
        }

        [InvokeOnWorldEvent(Event.OnStartupApp)]
        public static void OnStartupApp(object sender, EventArgs e)
        {
            Config.Parse();
        }

        [InvokeOnWorldEvent(Event.OnWorldLoadFinished)]
        public static void OnWorldLoadFinished(object sender, EventArgs e)
        {
            var responder = Sims3.Gameplay.UI.Responder.Instance != null;
            if (responder)
            {
                Sims3.Gameplay.UI.Responder.Instance.GameStateChanging += OnGameStateChanged;
            }
            if (LD_SmoothPatch != null)
            {
                try
                {
                    var clothingPerfType = LD_SmoothPatch.GetType("LazyDuchess.SmoothPatch.ClothingPerformance");
                    if (clothingPerfType != null)
                    {
                        var guidField = clothingPerfType.GetField("taskGuid", BindingFlags.Static | BindingFlags.NonPublic);
                        if (guidField != null)
                        {
                            var currentGuid = (ObjectGuid)guidField.GetValue(null);
                            if (currentGuid == ObjectGuid.InvalidObjectGuid) return;
                            Simulator.DestroyObject(currentGuid);
                            guidField.SetValue(null, ObjectGuid.InvalidObjectGuid);
                            Log("SmoothPatch Clothing Performance task destroyed.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log("Failed to disable SmoothPatch via reflection: " + ex.Message);
                }
            }
        }
        
        [InvokeOnWorldEvent(Event.OnWorldQuit)]
        public static void OnWorldQuit(object sender, EventArgs e)
        {
            Simulator.DestroyObject(CASHook);
            Log("Task destroyed");
            if (!Config.Data.Clothes.SmoothPatch) return;
            Simulator.DestroyObject(LazyLoading.TaskGuid);
            Log("Tasks destroyed");
        }

        internal static void OnGameStateChanged(Responder.GameSubState previousState, Responder.GameSubState newState)
        {
            if (newState == Responder.GameSubState.CASFullMode || newState == Responder.GameSubState.CASMirrorMode ||
                newState == Responder.GameSubState.CASTackMode || newState == Responder.GameSubState.CASDresserMode ||
                newState == Responder.GameSubState.CASTattooMode || newState == Responder.GameSubState.CASStylistMode ||
                newState == Responder.GameSubState.CASCollarMode ||
                newState == Responder.GameSubState.CASSurgeryFaceMode ||
                newState == Responder.GameSubState.CASSurgeryBodyMode)
            {
                CASHook = Simulator.AddObject(new CASHook());
                Log("CASHook created");
            }
            else
            {
                Simulator.DestroyObject(CASHook);
                if (!Config.Data.Clothes.SmoothPatch) return;
                Simulator.DestroyObject(LazyLoading.TaskGuid);
                Log("Tasks destroyed");
            }
        }

        public static ObjectGuid CASHook;
    }
}