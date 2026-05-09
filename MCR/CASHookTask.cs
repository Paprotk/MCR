using Arro.Common;
using Sims3.SimIFace;
using Sims3.UI;
using Sims3.UI.CAS;
using Sims3.UI.CAS.CAP;

namespace Arro.MCR;

public class CASHookTask : Task
{
    public static bool IsClothesProcessing;
    public static bool IsFaceProcessing;
    public static bool IsHairProcessing;

    public CASHookTask()
    {
        Logger.Log("CASHookTask created");
    }
    public override void Simulate()
    {
        if (!Responder.Instance.InCasMode) return;
            
        if (!IsClothesProcessing && (CASClothing.gSingleton != null || CASDresserClothing.gSingleton != null || CAPAccessories.gSingleton != null))
        {
            SetBool(true, false, false);
            if (Config.ClothesModuleInstalled)
            {
                Clothes.Hook();
            }
        }
        else if (!IsFaceProcessing && CASFacialDetails.gSingleton != null)
        {
            SetBool(false, true, false);
            if (Config.FaceModuleInstalled)
            {
                //Face.Hook;
            }
        }
        else if (!IsHairProcessing && CASPhysical.gSingleton != null)
        {
            SetBool(false, false, true);
            if (Config.HairModuleInstalled)
            {
                //Hair.Hook
            }
        }
        else if (CASClothing.gSingleton == null && CASDresserClothing.gSingleton == null && CAPAccessories.gSingleton == null && CASFacialDetails.gSingleton == null && CASPhysical.gSingleton == null)
        {
            SetBool(false, false, false);
        }
    }

    public override void Dispose()
    {
        Logger.Log("CASHookTask disposed");
        Clothes.Cleanup();
    }

    public static void SetBool(bool clothes, bool face, bool hair)
    {
        IsClothesProcessing = clothes;
        IsFaceProcessing = face;
        IsHairProcessing = hair;
    }
}