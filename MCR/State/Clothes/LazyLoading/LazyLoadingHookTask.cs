using System;
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
            // DZIAŁAJ TYLKO W CAS
            if (!Responder.Instance.InCasMode) 
            {
                if (hookedCASClothingCategory)
                {
                    LazyLoading.CASClothingCategoryUnHook();
                    hookedCASClothingCategory = false;
                }
                return;
            }
            
            Logger.Log("ticking");
            
            // Tick systemu leniwego ładowania
            LazyLoading.Tick();
            
            // Sprawdź czy istnieje ClothingCategory
            CASClothingCategory gSingleton = CASClothingCategory.gSingleton;
            if (gSingleton == null) 
            {
                if (hookedCASClothingCategory)
                {
                    LazyLoading.CASClothingCategoryUnHook();
                    hookedCASClothingCategory = false;
                }
                return;
            }
            
            // Podepnij hooka jeśli jeszcze nie podpięty
            if (!hookedCASClothingCategory)
            {
                LazyLoading.CASClothingCategoryHook();
                hookedCASClothingCategory = true;
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