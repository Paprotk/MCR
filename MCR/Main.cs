using Sims3.SimIFace;
using Sims3.UI;
using System;

namespace Arro.MCR
{
    public class Main
    {
        [Tunable]
#pragma warning disable CS0169 // Field is never used
        private static bool kInstantiator;
#pragma warning restore CS0169 // Field is never used

        static Main()
        {
            World.sOnStartupAppEventHandler += OnStartupApp;
            World.sOnWorldLoadFinishedEventHandler += OnWorldLoadFinished;
            World.sOnWorldQuitEventHandler += OnWorldQuit;
        }

        private static void OnStartupApp(object sender, EventArgs e)
        {
            Config.Parse();
            Helpers.CheckForMods();
        }

        private static void OnWorldLoadFinished(object sender, EventArgs e)
        {
            bool responder = Sims3.Gameplay.UI.Responder.Instance != null;
            if (responder)
            {
                Sims3.Gameplay.UI.Responder instance = Sims3.Gameplay.UI.Responder.Instance;
                instance.GameStateChanging = (GameStateChangingDelegate)Delegate.Remove(instance.GameStateChanging,
                    new GameStateChangingDelegate(OnGameStateChanged));
                Sims3.Gameplay.UI.Responder instance2 = Sims3.Gameplay.UI.Responder.Instance;
                instance2.GameStateChanging = (GameStateChangingDelegate)Delegate.Combine(instance2.GameStateChanging,
                    new GameStateChangingDelegate(OnGameStateChanged));
            }
        }

        private static void OnWorldQuit(object sender, EventArgs e)
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (CASHook != null)
            {
                Simulator.DestroyObject(CASHook);
            }
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
            }
            else
            {
                Simulator.DestroyObject(CASHook);
            }
        }

        public static ObjectGuid CASHook;
    }

    public static class TinyUIFixForTS3Integration
    {
        public delegate float FloatGetter();

        public static FloatGetter getUIScale = () => 1f;
    }
}