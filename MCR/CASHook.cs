using Sims3.SimIFace;
using Sims3.UI;
using Sims3.UI.CAS;
using Sims3.UI.CAS.CAP;

namespace Arro.MCR
{
    public class CASHook : Task
    {
        public static bool isClothesProcessing;
        public static bool isFaceProcessing;
        public static bool isHairProcessing;
        
        public override void Simulate()
        {
            if (!Responder.Instance.InCasMode) return;
            
            if (!isClothesProcessing && (CASClothing.gSingleton != null || CASDresserClothing.gSingleton != null || CAPAccessories.gSingleton != null))
            {
                SetBool(true, false, false);
                if (Config.ClothesModuleInstalled)
                {
                    Clothes.Hook();
                }
            }
            else if (!isFaceProcessing && CASFacialDetails.gSingleton != null)
            {
                SetBool(false, true, false);
                if (Config.FaceModuleInstalled)
                {
                    //Face.Hook;
                }
            }
            else if (!isHairProcessing && CASPhysical.gSingleton != null)
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
        
        public static void SetBool(bool clothes, bool face, bool hair)
        {
            isClothesProcessing = clothes;
            isFaceProcessing = face;
            isHairProcessing = hair;
        }
    }
}