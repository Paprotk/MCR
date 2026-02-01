using Arro.Common;
using Sims3.SimIFace;
using Sims3.UI;
using Sims3.UI.CAS;

namespace Arro.MCR
{
    public class LazyLoadingHookTask : Task
    {
        public override void Simulate()
        {
            var gSingleton = CASClothingCategory.gSingleton;
            if (gSingleton == null || !Responder.Instance.InCasMode) 
            {
                LazyLoading.CASClothingCategoryUnHook(); // -> Clean caches
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