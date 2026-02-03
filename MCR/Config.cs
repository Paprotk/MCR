using System;
using Arro.Common;
using Sims3.Gameplay.Abstracts;
using Sims3.Gameplay.Utilities;

namespace Arro.MCR;

public class Config
{
    public static void Parse()
    {
        var clothes = XmlDbData.ReadData("MCRClothes");
        if (clothes == null)
        {
            ClothesModuleInstalled = false;
        }
        else
        {
            ClothesModuleInstalled = true;
            clothes.Tables.TryGetValue("Config", out var xmlDbTable);
            if (xmlDbTable != null)
            {
                const int MinRows = 3;
                const int MinColumns = 1;
                foreach (var xmlDbRow in xmlDbTable.Rows)
                {
                    Data.Clothes.RowCount = Math.Max(MinRows, xmlDbRow.GetInt("Rows"));
                    Data.Clothes.ColumnCount = Math.Max(MinColumns, xmlDbRow.GetInt("Columns"));
                    Data.Clothes.SmoothPatchEnabled = xmlDbRow.GetBool("SmoothPatch");
                    Data.Clothes.CompactModeClothesEnabled = xmlDbRow.GetBool("CompactModeClothes");
                    Data.Clothes.CompactModeAccessoriesEnabled = xmlDbRow.GetBool("CompactModeAccessories");
                }
                Logger.Log(
                    $"[ClothesModule] Configuration initialized: Size=[{Data.Clothes.RowCount}x{Data.Clothes.ColumnCount}], SmoothPatch={Data.Clothes.SmoothPatchEnabled}, Compact(C/A)={Data.Clothes.CompactModeClothesEnabled}/{Data.Clothes.CompactModeAccessoriesEnabled}");
            }
        }
        var hair = XmlDbData.ReadData("MCRHair");
        if (hair == null)
        {
            HairModuleInstalled = false;
        }
        else
        {
            HairModuleInstalled = true;
        }
        var face = XmlDbData.ReadData("MCRFace");
        if (face == null)
        {
            FaceModuleInstalled = false;
        }
        else
        {
            FaceModuleInstalled = true;
        }
    }
    public static bool ClothesModuleInstalled { get; set; }
    public static bool HairModuleInstalled { get; set; }
    public static bool FaceModuleInstalled { get; set; }
    

    public static class Data
    {
        public static class Clothes
        {
            public static int RowCount { get; set; }
            public static int ColumnCount { get; set; }
            public static bool SmoothPatchEnabled { get; set; }
            public static bool CompactModeClothesEnabled { get; set; }
            public static bool CompactModeAccessoriesEnabled { get; set; }
        }
        public static class Hair
        {
        }
        public static class Face
        {
        }
    }
}