using System;
using Arro.Common;
using Sims3.Gameplay.Utilities;
using Sims3.UI;
using Sims3.SimIFace;
using Sims3.UI.CAS;

namespace Arro.MCR;

public abstract class Config
{
    private const string FileName = "Arro_MCR_Settings";

    public static class Data
    {
        public static class Clothes
        {
            public static float Version;
            public static int RowCount = 3;
            public static int ColumnCount = 1;
            public static bool SmoothPatchEnabled;
            public static bool CompactModeClothesEnabled;
            public static bool CompactModeAccessoriesEnabled;
            public static bool AnimationEnabled;
        }
    }

    public class ConfigSchema
    {
        public float Version { get; set; }
        public int RowCount { get; set; }
        public int ColumnCount { get; set; }
        public bool SmoothPatchEnabled { get; set; }
        public bool CompactModeClothesEnabled { get; set; }
        public bool CompactModeAccessoriesEnabled { get; set; }
        public bool AnimationEnabled { get; set; }
    }

    public static void LoadConfig()
    {
        try
        {
            ConfigSchema loaded = IniConfig.Load<ConfigSchema>(FileName);

            if (loaded != null && loaded.Version > 0)
            {
                if (loaded.Version > Main.ModVersion)
                {
                    SetDefaults();
                    Logger.Log("Config file is higher version than mod. Creating new file with defaults...");
                    SaveConfig();
                    return;
                }

                if (loaded.RowCount < 3)
                {
                    loaded.RowCount = 3;
                }
                if (loaded.ColumnCount < 3)
                {
                    loaded.ColumnCount = 1;
                }
                
                Data.Clothes.Version = loaded.Version;
                Data.Clothes.RowCount = loaded.RowCount;
                Data.Clothes.ColumnCount = loaded.ColumnCount;
                Data.Clothes.SmoothPatchEnabled = loaded.SmoothPatchEnabled;
                Data.Clothes.CompactModeClothesEnabled = loaded.CompactModeClothesEnabled;
                Data.Clothes.CompactModeAccessoriesEnabled = loaded.CompactModeAccessoriesEnabled;
                Data.Clothes.AnimationEnabled = loaded.AnimationEnabled;

                Logger.Log($"Config loaded successfully:\n" +
                           $"- Version: {Data.Clothes.Version}\n" +
                           $"- RowCount: {Data.Clothes.RowCount}\n" +
                           $"- ColumnCount: {Data.Clothes.ColumnCount}\n" +
                           $"- SmoothPatch: {Data.Clothes.SmoothPatchEnabled}\n" +
                           $"- CompactClothes: {Data.Clothes.CompactModeClothesEnabled}\n" +
                           $"- CompactAccessories: {Data.Clothes.CompactModeAccessoriesEnabled}" +
                           $"- Animation: {Data.Clothes.AnimationEnabled}\n");
                            

                if (loaded.Version < Main.ModVersion)
                {
                    Logger.Log("Updating config file...");
                    SaveConfig();
                }
            }
            else
            {
                SetDefaults();
                Logger.Log("Config file not found or empty. Creating new file with defaults...");
                SaveConfig();
            }
        }
        catch (Exception e)
        {
            Logger.Log("Error during loading config: " + e.Message);
        }
    }

    public static void SaveConfig()
    {
        try
        {
            ConfigSchema toSave = new ConfigSchema
            {
                Version = Main.ModVersion,
                RowCount = Data.Clothes.RowCount,
                ColumnCount = Data.Clothes.ColumnCount,
                SmoothPatchEnabled = Data.Clothes.SmoothPatchEnabled,
                CompactModeClothesEnabled = Data.Clothes.CompactModeClothesEnabled,
                CompactModeAccessoriesEnabled = Data.Clothes.CompactModeAccessoriesEnabled,
                AnimationEnabled = Data.Clothes.AnimationEnabled,
            };

            IniConfig.Save(FileName, toSave);
            Logger.Log("Config saved to file");
        }
        catch (Exception e)
        {
            Logger.Log("Error during saving config to file: " + e.Message);
        }
    }
    
    private static void SetDefaults()
    {
        Data.Clothes.Version = Main.ModVersion;
        Data.Clothes.RowCount = 3;
        Data.Clothes.ColumnCount = 1;
        Data.Clothes.SmoothPatchEnabled = false;
        Data.Clothes.CompactModeClothesEnabled = false;
        Data.Clothes.CompactModeAccessoriesEnabled = false;
        Data.Clothes.AnimationEnabled = false;
    }

    public static void ShowMCRDialog(WindowBase sender, UIButtonClickEventArgs eventArgs)
    {
        Simulator.AddObject(new OneShotFunctionTask(RunDialog, StopWatch.TickStyles.Seconds, 0.1f));
    }
    
    private static void RunDialog()
    {
        try
        {
            MCRInputDialog dialog = MCRInputDialog.Show(
                Data.Clothes.RowCount.ToString(),
                Data.Clothes.ColumnCount.ToString(),
                Localization.LocalizeString("Ui/Caption/Global:Accept"),
                Localization.LocalizeString("Ui/Caption/Global:Cancel"),
                new Vector2(-1f, -1f), false
            );

            if (dialog != null && dialog.Result != null && dialog.Result.Count >= 2)
            {
                if (int.TryParse(dialog.Result[0], out int newRows) && int.TryParse(dialog.Result[1], out int newCols))
                {
                    if (Data.Clothes.RowCount == newRows && Data.Clothes.ColumnCount == newCols &&
                        Data.Clothes.SmoothPatchEnabled == dialog.TempSmoothPatch &&
                        Data.Clothes.CompactModeClothesEnabled == dialog.TempCompactClothes &&
                        Data.Clothes.CompactModeAccessoriesEnabled == dialog.TempCompactAccessories &&
                        Data.Clothes.AnimationEnabled == dialog.TempAnimation)
                    {
                        return;
                    }
                    if (newRows < 3 || newCols < 1)
                    {
                        Audio.StartSound("ui_error");
                        SimpleMessageDialog.Show(Localization.LocalizeString("Arro/MCR/InvalidGridConfigurationTitle"), Localization.LocalizeString("Arro/MCR/InvalidGridConfigurationMessage"));
                        Simulator.AddObject(new OneShotFunctionTask(RunDialog, StopWatch.TickStyles.Seconds, 0.1f));
                        return;
                    }
                    Data.Clothes.RowCount = newRows;
                    Data.Clothes.ColumnCount = newCols;

                    Data.Clothes.SmoothPatchEnabled = dialog.TempSmoothPatch;
                    Data.Clothes.CompactModeClothesEnabled = dialog.TempCompactClothes;
                    Data.Clothes.CompactModeAccessoriesEnabled = dialog.TempCompactAccessories;
                    Data.Clothes.AnimationEnabled = dialog.TempAnimation;

                    SaveConfig();
                    RefreshCASUI();
                }
            }

            dialog?.Dispose();
        }
        catch (Exception e)
        {
            Logger.Log("Error in RunDialog: " + e.Message);
        }
    }

    private static void RefreshCASUI()
    {
        try
        {
            var cas = CASController.Singleton;
            var currentState = cas.CurrentState;
            cas.SetCurrentState(new CASState(currentState.mTopState, CASMidState.Summary, CASPhysicalState.None,
                CASClothingState.None));

            Simulator.AddObject(new OneShotFunctionTask(() =>
            {
                cas.SetCurrentState(new CASState(currentState.mTopState, currentState.mMidState, CASPhysicalState.None,
                    currentState.mClothingState));
                cas.Activate(false);
            }, StopWatch.TickStyles.Seconds, 0.1f));
        }
        catch (Exception e)
        {
            Logger.Log(e);
        }
    }
}