using Sims3.SimIFace;
using Sims3.UI;
using System;
using System.Reflection;
using Arro.Common;
using Simulator = Sims3.SimIFace.Simulator;
using static Arro.Common.Logger;
using Event = Arro.Common.Event;
using StopWatch = Sims3.SimIFace.StopWatch;

namespace Arro.MCR;

public class Main
{
    [GetAssembly("NRaasMasterController")] public static Assembly NraasMC;

    [GetAssembly("LazyDuchess.SmoothPatch")]
    public static Assembly LD_SmoothPatch;
    
    public static float ModVersion = 2.2f;

    [Tunable]
#pragma warning disable CS0169 // Field is never used
    private static bool kInstantiator;
#pragma warning restore CS0169 // Field is never used

    static Main()
    {
        Core.Initialize("MCR");
    }

    [InvokeOnWorldEvent(Event.OnWorldLoadFinished)]
    public static void OnWorldLoadFinished(object sender, EventArgs e)
    {
        if (Sims3.Gameplay.UI.Responder.Instance != null)
        {
            Sims3.Gameplay.UI.Responder.Instance.GameStateChanging -= OnGameStateChanged;
            Sims3.Gameplay.UI.Responder.Instance.GameStateChanging += OnGameStateChanged;
        }
        if (LD_SmoothPatch != null) DestroyLDTask();
        Config.LoadConfig();
    }

    private static void DestroyLDTask()
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

    [InvokeOnWorldEvent(Event.OnWorldQuit)]
    public static void OnWorldQuit(object sender, EventArgs e)
    {
        CASHook.Dispose();
        if (!Config.Data.Clothes.SmoothPatchEnabled) return;
        LazyLoading.TaskGuid.Dispose();
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
            Simulator.AddObject(new OneShotFunctionTask(() => { CASHook = Simulator.AddObject(new CASHookTask()); }, StopWatch.TickStyles.Seconds, 1f));
        }
        else
        {
            CASHook.Dispose();
        }
    }

    public static ObjectGuid CASHook;
}