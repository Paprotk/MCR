using System;
using Sims3.Gameplay.Abstracts;
using Sims3.Gameplay.Utilities;

namespace Arro.MCR
{
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
                        Data.Clothes.Rows = Math.Max(MinRows, xmlDbRow.GetInt("Rows"));
                        Data.Clothes.Columns = Math.Max(MinColumns, xmlDbRow.GetInt("Columns"));
                        Data.Clothes.SmoothPatch = xmlDbRow.GetBool("SmoothPatch");
                        Data.Clothes.CompactModeClothes = xmlDbRow.GetBool("CompactModeClothes");
                        Data.Clothes.CompactModeAccessories = xmlDbRow.GetBool("CompactModeAccessories");
                    }
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
                public static int Rows { get; set; }
                public static int Columns { get; set; }
                public static bool SmoothPatch { get; set; }
                public static bool CompactModeClothes { get; set; }
                public static bool CompactModeAccessories { get; set; }
            }
            public static class Hair
            {
            }
            public static class Face
            {
            }
        }
    }
}