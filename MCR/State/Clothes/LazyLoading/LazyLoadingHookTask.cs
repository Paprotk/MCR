using Arro.Common;
using Sims3.SimIFace;
using Sims3.UI;
using Sims3.UI.CAS;

namespace Arro.MCR
{
    public class LazyLoadingHookTask : Task
    {
        private bool hookedCASClothingCategory;
        
        public override void Simulate()
        {
            //Only in CAS
            if (!Responder.Instance.InCasMode) 
            {
                if (hookedCASClothingCategory)
                {
                    Logger.Log("CASClothingCategoryUnHook1");
                    LazyLoading.CASClothingCategoryUnHook();
                    hookedCASClothingCategory = false;
                }
                return;
            }
            
            //Is clothing category null?
            CASClothingCategory gSingleton = CASClothingCategory.gSingleton;
            if (gSingleton == null) 
            {
                    Logger.Log("CASClothingCategoryUnHook");
                    LazyLoading.CASClothingCategoryUnHook();
                    hookedCASClothingCategory = false;
                return;
            }
            
            if (gSingleton.mClothingTypesGrid != null &&
                (gSingleton.mClothingTypesGrid.mPopulateCallback != null ||
                 gSingleton.mClothingTypesGrid.mPopulateContext != null ||
                 gSingleton.mClothingTypesGrid.mPopulating ||
                 gSingleton.mClothingTypesGrid.mPopulateStride > 0))
            {
                gSingleton.mSortButton.Enabled = true;
                gSingleton.mClothingTypesGrid.mPopulating = false;
                gSingleton.mClothingTypesGrid.mPopulateCallback = null;
                gSingleton.mClothingTypesGrid.AbortPopulating();
                LazyLoading.HookedSetTypeCategory(CASClothingCategory.CurrentTypeCategory, false);
            }
        }
    }
}