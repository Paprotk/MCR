using System;
using System.Collections;
using System.Collections.Generic;
using Sims3.SimIFace;
using Sims3.SimIFace.CAS;
using Sims3.UI;
using Sims3.UI.CAS;
using Sims3.UI.Store;

namespace Arro.MCR
{
    public static class ClothingPerformance
    {
        
        private static Dictionary<string, int> LoadedItems = new Dictionary<string, int>();

        public static void Tick()
        {
            CASDresserClothing gSingleton = CASDresserClothing.gSingleton;
            if (gSingleton != null)
            {
                Responder.Instance.CASModel.OnSimOutfitIndexChanged -= gSingleton.OnSimOutfitIndexChanged;
                Responder.Instance.CASModel.OnSimOutfitCategoryChanged -= gSingleton.OnSimOutfitCategoryChanged;
                Responder.Instance.CASModel.OnSimOutfitIndexChanged -= OnSimOutfitIndexChanged;
                Responder.Instance.CASModel.OnSimOutfitCategoryChanged -= DresserOnSimOutfitCategoryChanged;
                Responder.Instance.CASModel.OnSimOutfitIndexChanged += OnSimOutfitIndexChanged;
                Responder.Instance.CASModel.OnSimOutfitCategoryChanged += DresserOnSimOutfitCategoryChanged;
            }

            if (CASClothing.gSingleton != null)
            {
                Responder.Instance.CASModel.OnSimOutfitCategoryChanged -= OnSimOutfitCategoryChanged;
                Responder.Instance.CASModel.OnSimOutfitCategoryChanged += OnSimOutfitCategoryChanged;
            }

            CASClothingCategory gSingleton2 = CASClothingCategory.gSingleton;
            if (gSingleton2 != null)
            {
                CatalogProductFilter mContentTypeFilter = gSingleton2.mContentTypeFilter;
                mContentTypeFilter.FiltersChanged = (VoidEventHandler)Delegate.Remove(mContentTypeFilter.FiltersChanged,
                    new VoidEventHandler(gSingleton2.PopulateTypesGrid));
                gSingleton2.mTopsButton.Click -= gSingleton2.OnCategoryButtonClick;
                gSingleton2.mBottomsButton.Click -= gSingleton2.OnCategoryButtonClick;
                gSingleton2.mShoesButton.Click -= gSingleton2.OnCategoryButtonClick;
                gSingleton2.mOutfitsButton.Click -= gSingleton2.OnCategoryButtonClick;
                gSingleton2.mAccessoriesButton.Click -= gSingleton2.OnCategoryButtonClick;
                gSingleton2.mHorseBridlesButton.Click -= gSingleton2.OnCategoryButtonClick;
                gSingleton2.mHorseSaddleButton.Click -= gSingleton2.OnCategoryButtonClick;
                gSingleton2.FadeTransitionFinished -= gSingleton2.OnFadeFinished;
                CatalogProductFilter mContentTypeFilter2 = gSingleton2.mContentTypeFilter;
                mContentTypeFilter2.FiltersChanged =
                    (VoidEventHandler)Delegate.Remove(mContentTypeFilter2.FiltersChanged,
                        new VoidEventHandler(HookedPopulateTypesGrid));
                gSingleton2.mTopsButton.Click -= HookedOnCategoryButtonClick;
                gSingleton2.mBottomsButton.Click -= HookedOnCategoryButtonClick;
                gSingleton2.mShoesButton.Click -= HookedOnCategoryButtonClick;
                gSingleton2.mOutfitsButton.Click -= HookedOnCategoryButtonClick;
                gSingleton2.mAccessoriesButton.Click -= HookedOnCategoryButtonClick;
                gSingleton2.mHorseBridlesButton.Click -= HookedOnCategoryButtonClick;
                gSingleton2.mHorseSaddleButton.Click -= HookedOnCategoryButtonClick;
                gSingleton2.FadeTransitionFinished -= HookedOnFadeFinished;
                CatalogProductFilter mContentTypeFilter3 = gSingleton2.mContentTypeFilter;
                mContentTypeFilter3.FiltersChanged =
                    (VoidEventHandler)Delegate.Combine(mContentTypeFilter3.FiltersChanged,
                        new VoidEventHandler(HookedPopulateTypesGrid));
                gSingleton2.mTopsButton.Click += HookedOnCategoryButtonClick;
                gSingleton2.mBottomsButton.Click += HookedOnCategoryButtonClick;
                gSingleton2.mShoesButton.Click += HookedOnCategoryButtonClick;
                gSingleton2.mOutfitsButton.Click += HookedOnCategoryButtonClick;
                gSingleton2.mAccessoriesButton.Click += HookedOnCategoryButtonClick;
                gSingleton2.mHorseBridlesButton.Click += HookedOnCategoryButtonClick;
                gSingleton2.mHorseSaddleButton.Click += HookedOnCategoryButtonClick;
                gSingleton2.FadeTransitionFinished += HookedOnFadeFinished;
            }
        }

        public static void CASControllerHook()
        {
        }

        public static void OnSimOutfitIndexChanged(int index)
        {
            CASDresserClothing gSingleton = CASDresserClothing.gSingleton;
            if (!(gSingleton == null))
            {
                Button[] mOutfitButtons = gSingleton.mOutfitButtons;
                for (int i = 0; i < mOutfitButtons.Length; i++)
                {
                    mOutfitButtons[i].Selected = false;
                }

                if (index < gSingleton.mOutfitButtons.Length)
                {
                    gSingleton.mOutfitButtons[index].Selected = true;
                }

                CASController.Singleton.Activate(true);
                HookedPopulateTypesGrid();
                gSingleton.UpdateOutfitButtons(Responder.Instance.CASModel.OutfitCategory);
                gSingleton.UpdateOutfitDeleteButtons(index);
                if (CASPuck.Instance != null)
                {
                    CASPuck.Instance.OnDynamicUpdateCurrentSimThumbnail();
                }
            }
        }

        public static void CASDresserClothingHook()
        {
            CASDresserClothing gSingleton = CASDresserClothing.gSingleton;
            Responder.Instance.CASModel.OnSimOutfitIndexChanged -= gSingleton.OnSimOutfitIndexChanged;
            Responder.Instance.CASModel.OnSimOutfitIndexChanged += OnSimOutfitIndexChanged;
            Responder.Instance.CASModel.OnSimOutfitCategoryChanged -= gSingleton.OnSimOutfitCategoryChanged;
            Responder.Instance.CASModel.OnSimOutfitCategoryChanged += DresserOnSimOutfitCategoryChanged;
        }

        private static void DresserOnSimOutfitCategoryChanged(OutfitCategories outfitCategory)
        {
            CASDresserClothing gSingleton = CASDresserClothing.gSingleton;
            gSingleton.UpdateOutfitButtons(outfitCategory);
            Button[] mOutfitButtons = gSingleton.mOutfitButtons;
            for (int i = 0; i < mOutfitButtons.Length; i++)
            {
                mOutfitButtons[i].Selected = false;
            }

            gSingleton.mOutfitButtons[Responder.Instance.CASModel.OutfitIndex].Selected = true;
            CASClothingState clothingState = CASClothing.CASClothingStateFromOutfitCategory(outfitCategory);
            if (gSingleton.mDefaultText != null)
            {
                gSingleton.mDefaultText.Visible = false;
            }

            gSingleton.UpdateOutfitDeleteButtons(0);
            CASPuck.Instance.OnDynamicUpdateCurrentSimThumbnail();
            CASController.Singleton.SetCurrentState(new CASState(CASTopState.Dresser, CASMidState.Clothing,
                CASPhysicalState.None, clothingState));
            CASController.Singleton.Activate(true);
            CASDresserSheet.UpdateCategory(outfitCategory);
            HookedPopulateTypesGrid();
        }

        public static void CASClothingHook()
        {
            Responder.Instance.CASModel.OnSimOutfitCategoryChanged += OnSimOutfitCategoryChanged;
        }

        public static void CASClothingUnhook()
        {
            Responder.Instance.CASModel.OnSimOutfitCategoryChanged -= OnSimOutfitCategoryChanged;
        }

        public static void OnSimOutfitCategoryChanged(OutfitCategories outfitCategory)
        {
            HookedPopulateTypesGrid();
        }

        public static void CASClothingCategoryHook()
        {
            CASClothingCategory gSingleton = CASClothingCategory.gSingleton;
            CatalogProductFilter mContentTypeFilter = gSingleton.mContentTypeFilter;
            mContentTypeFilter.FiltersChanged = (VoidEventHandler)Delegate.Remove(mContentTypeFilter.FiltersChanged,
                new VoidEventHandler(gSingleton.PopulateTypesGrid));
            gSingleton.mTopsButton.Click -= gSingleton.OnCategoryButtonClick;
            gSingleton.mBottomsButton.Click -= gSingleton.OnCategoryButtonClick;
            gSingleton.mShoesButton.Click -= gSingleton.OnCategoryButtonClick;
            gSingleton.mOutfitsButton.Click -= gSingleton.OnCategoryButtonClick;
            gSingleton.mAccessoriesButton.Click -= gSingleton.OnCategoryButtonClick;
            gSingleton.mHorseBridlesButton.Click -= gSingleton.OnCategoryButtonClick;
            gSingleton.mHorseSaddleButton.Click -= gSingleton.OnCategoryButtonClick;
            gSingleton.FadeTransitionFinished -= gSingleton.OnFadeFinished;
            CatalogProductFilter mContentTypeFilter2 = gSingleton.mContentTypeFilter;
            mContentTypeFilter2.FiltersChanged = (VoidEventHandler)Delegate.Combine(mContentTypeFilter2.FiltersChanged,
                new VoidEventHandler(HookedPopulateTypesGrid));
            gSingleton.mTopsButton.Click += HookedOnCategoryButtonClick;
            gSingleton.mBottomsButton.Click += HookedOnCategoryButtonClick;
            gSingleton.mShoesButton.Click += HookedOnCategoryButtonClick;
            gSingleton.mOutfitsButton.Click += HookedOnCategoryButtonClick;
            gSingleton.mAccessoriesButton.Click += HookedOnCategoryButtonClick;
            gSingleton.mHorseBridlesButton.Click += HookedOnCategoryButtonClick;
            gSingleton.mHorseSaddleButton.Click += HookedOnCategoryButtonClick;
            gSingleton.FadeTransitionFinished += HookedOnFadeFinished;
            layoutKey = ResourceKey.CreateUILayoutKey("CASClothingRow", 0U);
            placeHolderRow = (UIManager.LoadLayout(layoutKey).GetWindowByExportID(1) as CASClothingRow);
            placeHolderRow.Visible = false;
        }

        public static void CASClothingCategoryUnHook()
        {
            ClearWornParts();
            if (placeHolderRow != null && !placeHolderRow.Disposed)
            {
                placeHolderRow.Dispose();
            }
        }

        private static void HookedOnFadeFinished(WindowBase sender, UIHandledEventArgs args)
        {
            CASClothingCategory gSingleton = CASClothingCategory.gSingleton;
            if (!gSingleton.Visible)
            {
                Simulator.AddObject(new OneShotFunctionTask(new Function(CASClothingCategory.Unload)));
                if (CASCompositorController.Instance != null)
                {
                    CASCompositorController.Instance.DesignModeToolActive = false;
                }
            }
            else
            {
                gSingleton.mFadeEffect.Duration = gSingleton.mFadeTime;
                BodyTypes bodyTypes = gSingleton.GetBodyTypeFromCategory(CASClothingCategory.sCurrentTypeCategory);
                if (bodyTypes == BodyTypes.Accessories)
                {
                    bodyTypes = (BodyTypes)CASClothingCategory.sAccessoriesSelection;
                }

                if (gSingleton.GetWornPart(bodyTypes).Key == ResourceKey.kInvalidResourceKey)
                {
                    CASAgeGenderFlags species = Responder.Instance.CASModel.Species;
                    if (gSingleton.mbIsHuman)
                    {
                        CASClothingCategory.sCurrentTypeCategory = CASClothingCategory.Category.Tops;
                        if (gSingleton.GetWornPart(BodyTypes.UpperBody).Key == ResourceKey.kInvalidResourceKey)
                        {
                            CASClothingCategory.sCurrentTypeCategory = CASClothingCategory.Category.Outfits;
                        }
                    }
                    else
                    {
                        CASClothingCategory.sCurrentTypeCategory = CASClothingCategory.Category.CollarBridle;
                    }
                }

                HookedSetTypeCategory(CASClothingCategory.CurrentTypeCategory, false);
            }

            gSingleton.mFading = false;
            args.Handled = true;
        }

        public static void HookedSetTypeCategory(CASClothingCategory.Category cat, bool activateDesigner)
        {
            CASClothingCategory gSingleton = CASClothingCategory.gSingleton;
            gSingleton.mTopsButton.Selected = false;
            gSingleton.mBottomsButton.Selected = false;
            gSingleton.mShoesButton.Selected = false;
            gSingleton.mOutfitsButton.Selected = false;
            gSingleton.mAccessoriesButton.Selected = false;
            gSingleton.mHorseBridlesButton.Selected = false;
            gSingleton.mHorseSaddleButton.Selected = false;
            switch (cat)
            {
                case CASClothingCategory.Category.Tops:
                    CASController.Singleton.SetTopCam(true);
                    gSingleton.mTopsButton.Selected = true;
                    break;
                case CASClothingCategory.Category.Bottoms:
                    CASController.Singleton.SetFeetCam(true);
                    gSingleton.mBottomsButton.Selected = true;
                    break;
                case CASClothingCategory.Category.Outfits:
                    CASController.Singleton.SetTopCam(true);
                    gSingleton.mOutfitsButton.Selected = true;
                    break;
                case CASClothingCategory.Category.Shoes:
                    CASController.Singleton.SetFeetCam(true);
                    gSingleton.mShoesButton.Selected = true;
                    break;
                case CASClothingCategory.Category.CollarBridle:
                    if (gSingleton.mModel.Species == CASAgeGenderFlags.Horse)
                    {
                        CASController.Singleton.SetFaceCam(true);
                        gSingleton.mHorseBridlesButton.Selected = true;
                    }
                    else
                    {
                        CASController.Singleton.SetCollarCam(true);
                    }

                    break;
                case CASClothingCategory.Category.Saddles:
                    CASController.Singleton.SetAccessoryCam(BodyTypes.PetSaddle, true);
                    gSingleton.mHorseSaddleButton.Selected = true;
                    break;
                case CASClothingCategory.Category.Accessories:
                    if (gSingleton.mModel.Species == CASAgeGenderFlags.Horse)
                    {
                        CASController.Singleton.SetSaddleCam(true);
                    }
                    else
                    {
                        CASController.Singleton.SetFullbodyCam(true);
                    }

                    gSingleton.mAccessoriesButton.Selected = true;
                    break;
            }

            CASClothingCategory.CurrentTypeCategory = cat;
            gSingleton.mCurrentPart = gSingleton.GetBodyTypeFromCategory(cat);
            CASPart wornPart = gSingleton.GetWornPart(gSingleton.mCurrentPart);
            if (cat == CASClothingCategory.Category.Accessories)
            {
                wornPart = gSingleton.GetWornPart((BodyTypes)CASClothingCategory.sAccessoriesSelection);
            }

            if (wornPart.Key != gSingleton.mInvalidCASPart.Key &&
                CompositorUtil.GetNumPatternsInPart(CompositorUtil.GetPatternsFromCASPart(wornPart)) != 0)
            {
                if (activateDesigner)
                {
                    CASCompositorController.Instance.SetTargetObject(wornPart);
                    return;
                }

                CASSelectionGrid.SetSelectionIndex((uint)wornPart.BodyType);
                Responder.Instance.CASModel.HighlightIndex = 0U;
                CASCompositorController.Instance.SetTargetObject(null);
            }
            else
            {
                CASSelectionGrid.SetSelectionIndex(0U);
                CASCompositorController.Instance.SetTargetObject(null);
            }

            gSingleton.HideUnusedIcons();
            gSingleton.LoadParts();
            HookedPopulateTypesGrid();
            gSingleton.UpdateDesignModeAvailability();
        }

        private static void HookedOnCategoryButtonClick(WindowBase sender, UIButtonClickEventArgs eventArgs)
        {
            CASClothingCategory gSingleton = CASClothingCategory.gSingleton;
            CASClothingCategory.ControlIDs buttonID = (CASClothingCategory.ControlIDs)eventArgs.ButtonID;
            switch (buttonID)
            {
                case CASClothingCategory.ControlIDs.CategoryTopsButton:
                    if (CASClothingCategory.CurrentTypeCategory != CASClothingCategory.Category.Tops)
                    {
                        HookedSetTypeCategory(CASClothingCategory.Category.Tops, false);
                        return;
                    }

                    break;
                case CASClothingCategory.ControlIDs.CategoryBottomsButton:
                    if (CASClothingCategory.CurrentTypeCategory != CASClothingCategory.Category.Bottoms)
                    {
                        HookedSetTypeCategory(CASClothingCategory.Category.Bottoms, false);
                        return;
                    }

                    break;
                case CASClothingCategory.ControlIDs.CategoryShoesButton:
                    if (CASClothingCategory.CurrentTypeCategory != CASClothingCategory.Category.Shoes)
                    {
                        HookedSetTypeCategory(CASClothingCategory.Category.Shoes, false);
                        return;
                    }

                    break;
                case CASClothingCategory.ControlIDs.CategoryOutfitsButton:
                    if (CASClothingCategory.CurrentTypeCategory != CASClothingCategory.Category.Outfits)
                    {
                        HookedSetTypeCategory(CASClothingCategory.Category.Outfits, false);
                        return;
                    }

                    break;
                default:
                    if (buttonID != CASClothingCategory.ControlIDs.HorseCategoryBridlesButton)
                    {
                        if (buttonID != CASClothingCategory.ControlIDs.HorseCategorySaddlesButton)
                        {
                            if (buttonID != CASClothingCategory.ControlIDs.CategoryAccessoriesButton)
                            {
                                return;
                            }

                            if (CASClothingCategory.CurrentTypeCategory != CASClothingCategory.Category.Accessories)
                            {
                                HookedSetTypeCategory(CASClothingCategory.Category.Accessories, false);
                                return;
                            }
                        }
                        else if (CASClothingCategory.CurrentTypeCategory != CASClothingCategory.Category.Saddles)
                        {
                            HookedSetTypeCategory(CASClothingCategory.Category.Saddles, false);
                            return;
                        }
                    }
                    else if (CASClothingCategory.CurrentTypeCategory != CASClothingCategory.Category.CollarBridle)
                    {
                        HookedSetTypeCategory(CASClothingCategory.Category.CollarBridle, false);
                        return;
                    }

                    break;
            }
        }

        private static ArrayList CreateGridItems(CASClothingRow row)
        {
            return CreateGridItems(row, false);
        }

        private static ArrayList CreateGridItems(CASClothingRow row, bool allowTemp)
        {
            row.mItems.Clear();
            row.mTempWindow = null;
            row.mTempWindowValid = false;
            AddClothingItemAndPresets(row, row.mObjectOfInterest, allowTemp);
            if (row.mFeaturedStoreItemBorder != null)
            {
                row.mFeaturedStoreItemBorder.Visible = !(row.mObjectOfInterest is CASPart);
            }

            row.mNumItems = row.mItems.Count;
            return row.mItems;
        }

        private static void CacheWornParts()
        {
            ClearWornParts();
            foreach (object obj in Enum.GetValues(typeof(BodyTypes)))
            {
                List<CASPart> list = Responder.Instance.CASModel.GetWornParts((BodyTypes)obj);
                wornParts[(BodyTypes)obj] = list;
                foreach (CASPart caspart in list)
                {
                    string designPreset = Responder.Instance.CASModel.GetDesignPreset(caspart);
                    wornPresets[caspart.Key] = designPreset;
                }
            }
        }

        private static void ClearWornParts()
        {
            wornParts.Clear();
            wornPresets.Clear();
        }

        private static void AddClothingItemAndPresets(CASClothingRow row, object objectOfInterest, bool allowTemp)
        {
            if (objectOfInterest is CASPart)
            {
                CASPart caspart = (CASPart)objectOfInterest;
                int num = 0;
                ObjectDesigner.SetCASPart(caspart.Key);
                row.mHasFilterableContent = false;
                row.mIsWardrobePart = Responder.Instance.CASModel.ActiveWardrobeContains(caspart);
                string text =
                    Simulator.LoadXMLString(new ResourceKey(caspart.Key.InstanceId, 53690476U, caspart.Key.GroupId));
                CASPartPreset caspartPreset = new CASPartPreset(caspart, text);
                CASPart caspart2 = default(CASPart);
                if (wornParts.ContainsKey(caspart.BodyType) && wornParts[caspart.BodyType].Count > 0)
                {
                    caspart2 = wornParts[caspart.BodyType][0];
                }

                string text2 = string.Empty;
                if (wornPresets.ContainsKey(caspart2.Key))
                {
                    text2 = wornPresets[caspart2.Key];
                }

                bool flag = true;
                if (caspart2.Key == caspartPreset.mPart.Key)
                {
                    flag = false;
                }

                if (caspartPreset.Valid && ObjectDesigner.DefaultPresetId == 4294967295U)
                {
                    if (caspart.Key == caspart2.Key && text2 != string.Empty &&
                        CASUtils.DesignPresetCompare(text2, text))
                    {
                        row.mSelectedItem = row.mItems.Count;
                        flag = true;
                    }

                    if (row.AddPresetGridItem(caspartPreset, num, 4294967295U))
                    {
                        num++;
                    }
                }

                uint num2 = CASUtils.PartDataNumPresets(caspart.Key);
                for (int i = 0; i < (int)num2; i++)
                {
                    caspartPreset = new CASPartPreset(caspart, CASUtils.PartDataGetPresetId(caspart.Key, (uint)i),
                        CASUtils.PartDataGetPreset(caspart.Key, (uint)i));
                    if (caspartPreset.Valid && row.AddPresetGridItem(caspartPreset, num, (uint)i))
                    {
                        if (text2 != string.Empty && CASUtils.DesignPresetCompare(text2, caspartPreset.mPresetString))
                        {
                            row.mSelectedItem = row.mItems.Count - 1;
                            flag = true;
                        }

                        num++;
                    }
                }

                if (row.mItems.Count > 0 && !flag && allowTemp)
                {
                    row.AddTempItem();
                    return;
                }
            }
            else
            {
                List<object> list = objectOfInterest as List<object>;
                if (list != null)
                {
                    List<object> list2 = new List<object>(list);
                    CASPuck.GetContentTypeFilter().FilterObjects(list2, out row.mHasFilterableContent);
                    int num3 = 0;
                    foreach (object obj in list2)
                    {
                        IFeaturedStoreItem featuredStoreItem = obj as IFeaturedStoreItem;
                        CASClothingRow.ClothingThumbnail clothingThumbnail = new CASClothingRow.ClothingThumbnail();
                        clothingThumbnail.mData = featuredStoreItem;
                        clothingThumbnail.mIndex = num3++;
                        clothingThumbnail.mThumbnail = UIUtils.GetUIImageFromThumbnailKey(featuredStoreItem.ThumbKey);
                        row.mItems.Add(clothingThumbnail);
                    }
                }
            }
        }
        
        private static ArrayList CreateGridItemsForGroup(CASClothingRow row, List<object> group)
        {
            row.mItems.Clear();
            row.mTempWindow = null;
            row.mTempWindowValid = false;
            
            int orderIndex = 0;
            row.mHasFilterableContent = false;
            
            foreach (object obj in group)
            {
                if (obj is CASPart part)
                {
                    bool hasFilterableContent = false;
                    CASPuck.GetContentTypeFilter().ObjectMatchesFilter(part, ref hasFilterableContent);
                    if (hasFilterableContent)
                    {
                        row.mHasFilterableContent = true;
                    }
                    
                    string presetXML = Simulator.LoadXMLString(
                        new ResourceKey(part.Key.InstanceId, 53690476U, part.Key.GroupId));
                    
                    CASPartPreset preset = new CASPartPreset(part, presetXML);
                    
                    if (!preset.Valid)
                    {
                        uint numPresets = CASUtils.PartDataNumPresets(part.Key);
                        if (numPresets > 0)
                        {
                            preset = new CASPartPreset(part, 0, 
                                CASUtils.PartDataGetPreset(part.Key, 0));
                            if (!preset.Valid) continue;
                        }
                        else continue;
                    }
                    
                    if (row.AddPresetGridItem(preset, orderIndex, preset.mPresetId))
                    {
                        if (wornParts.ContainsKey(part.BodyType))
                        {
                            foreach (CASPart wornPart in wornParts[part.BodyType])
                            {
                                if (wornPart.Key == part.Key)
                                {
                                    row.mSelectedItem = orderIndex;
                                    break;
                                }
                            }
                        }
                        
                        orderIndex++;
                    }
                }
            }
            
            row.mNumItems = row.mItems.Count;
            return row.mItems;
        }

        private static bool AddGridItem(ItemGrid grid, object current, ResourceKey layoutKey, object context)
        {
            CASClothingCategory gSingleton = CASClothingCategory.gSingleton;
            bool result = false;

            if (current != null)
            {
                int totalItemsSoFar = grid.Count;
                int visibleColumns = (int)grid.VisibleColumns;

                int targetRow = totalItemsSoFar / visibleColumns;
                int targetCol = totalItemsSoFar % visibleColumns;

                return SetGridItemAtPosition(grid, current, layoutKey, context, targetRow, targetCol);
            }
            else
            {
                gSingleton.mContentTypeFilter.UpdateFilterButtonState();
                gSingleton.UpdateButtons(gSingleton.mSelectedType);
                if (CASClothingCategory.OnClothingGridFinishedPopulating != null)
                {
                    CASClothingCategory.OnClothingGridFinishedPopulating();
                }
            }

            return result;
        }

        private static bool SetGridItem(ItemGrid grid, object current, ResourceKey layoutKey, object context, int row)
        {
            CASClothingCategory gSingleton = CASClothingCategory.gSingleton;
            bool result = false;
            if (current != null)
            {
                if (current is CASPart)
                {
                    CASPart caspart = (CASPart)current;
                    CASClothingRow casclothingRow =
                        UIManager.LoadLayout(layoutKey).GetWindowByExportID(1) as CASClothingRow;
                    casclothingRow.UseEp5AsBaseContent = gSingleton.mIsEp5Base;
                    casclothingRow.CASPart = caspart;
                    casclothingRow.RowController = gSingleton;
                    ArrayList arrayList = CreateGridItems(casclothingRow, true);
                    gSingleton.mSortButton.Tag =
                        ((bool)gSingleton.mSortButton.Tag | casclothingRow.HasFilterableContent);
                    if (arrayList.Count > 0)
                    {
                        SetItem(grid, new ItemGridCellItem(casclothingRow, null), row);
                        result = true;
                        if (casclothingRow.SelectedItem != -1)
                        {
                            if (gSingleton.IsAccessoryType(caspart.BodyType))
                            {
                                if (gSingleton.GetWornPart((BodyTypes)CASClothingCategory.sAccessoriesSelection).Key !=
                                    ResourceKey.kInvalidResourceKey)
                                {
                                    if (caspart.BodyType == (BodyTypes)CASClothingCategory.sAccessoriesSelection)
                                    {
                                        grid.SelectedItem = grid.Count - 1;
                                        gSingleton.mSelectedType = casclothingRow.SelectedType;
                                        CASClothingCategory.sAccessoriesSelection = (int)caspart.BodyType;
                                        gSingleton.mCurrentPreset = (casclothingRow.Selection as CASPartPreset);
                                    }
                                }
                                else
                                {
                                    grid.SelectedItem = grid.Count - 1;
                                    gSingleton.mSelectedType = casclothingRow.SelectedType;
                                    CASClothingCategory.sAccessoriesSelection = (int)caspart.BodyType;
                                    gSingleton.mCurrentPreset = (casclothingRow.Selection as CASPartPreset);
                                }
                            }
                            else
                            {
                                grid.SelectedItem = grid.Count - 1;
                                gSingleton.mSelectedType = casclothingRow.SelectedType;
                                gSingleton.mCurrentPreset = (casclothingRow.Selection as CASPartPreset);
                            }
                        }
                    }
                }
                else
                {
                    List<object> list = current as List<object>;
                    if (list != null)
                    {
                        CASClothingRow casclothingRow2 =
                            UIManager.LoadLayout(layoutKey).GetWindowByExportID(1) as CASClothingRow;
                        casclothingRow2.ObjectOfInterest = list;
                        casclothingRow2.RowController = gSingleton;
                        ArrayList arrayList2 = CreateGridItems(casclothingRow2, true);
                        gSingleton.mSortButton.Tag =
                            ((bool)gSingleton.mSortButton.Tag | casclothingRow2.HasFilterableContent);
                        if (arrayList2.Count > 0)
                        {
                            SetItem(grid, new ItemGridCellItem(casclothingRow2, null), row);
                        }

                        result = true;
                    }
                }
            }
            else
            {
                gSingleton.mContentTypeFilter.UpdateFilterButtonState();
                gSingleton.UpdateButtons(gSingleton.mSelectedType);
                if (CASClothingCategory.OnClothingGridFinishedPopulating != null)
                {
                    CASClothingCategory.OnClothingGridFinishedPopulating();
                }
            }

            return result;
        }

        private static void SetItem(ItemGrid itemGrid, ItemGridCellItem item, int row)
        {
            if (itemGrid.LegalToPlaceItem())
            {
                int column;
                if (itemGrid.mTempEntryI == -1)
                {
                    itemGrid.mLastEntryI = (itemGrid.mLastEntryI + 1) % itemGrid.EntriesCountI;
                    if (itemGrid.mLastEntryI == 0)
                    {
                        itemGrid.mLastEntryJ++;
                        itemGrid.EntriesCountJ = itemGrid.mLastEntryJ + 1;
                        if (itemGrid.mbHorizontalScrolling)
                        {
                            itemGrid.mGrid.SetColumnWidth(itemGrid.mLastEntryJ, itemGrid.mGrid.DefaultColumnWidth);
                        }
                        else
                        {
                            itemGrid.mGrid.SetRowHeight(itemGrid.mLastEntryJ, itemGrid.mGrid.DefaultRowHeight);
                        }
                    }

                    column = (itemGrid.mbHorizontalScrolling ? itemGrid.mLastEntryJ : itemGrid.mLastEntryI);
                }
                else
                {
                    itemGrid.mLastEntryJ = itemGrid.mTempEntryJ;
                    itemGrid.mLastEntryI = itemGrid.mTempEntryI;
                    column = (itemGrid.mbHorizontalScrolling ? itemGrid.mLastEntryJ : itemGrid.mLastEntryI);
                    itemGrid.mGrid.ClearCell(column, row);
                    itemGrid.mTempEntryI = -1;
                    itemGrid.mTempEntryJ = -1;
                }

                item.mWin.Visible = false;
                itemGrid.mGrid.SetCellWindow(column, row, item.mWin, itemGrid.mbStretchCellWindows);
                itemGrid.mGrid.CellTags[column, row] = item.mTag;
                if (!itemGrid.mPopulating)
                {
                    itemGrid.mGrid.Refresh();
                    itemGrid.UpdateScrollbar();
                    if (itemGrid.ItemRowsChanged != null)
                    {
                        itemGrid.ItemRowsChanged();
                    }
                }
            }
        }

        private static void AddItem(ItemGrid itemGrid, ItemGridCellItem item)
        {
            if (itemGrid.LegalToPlaceItem())
            {
                int column;
                int row;
                if (itemGrid.mTempEntryI == -1)
                {
                    itemGrid.mLastEntryI = (itemGrid.mLastEntryI + 1) % itemGrid.EntriesCountI;
                    if (itemGrid.mLastEntryI == 0)
                    {
                        itemGrid.mLastEntryJ++;
                        itemGrid.EntriesCountJ = itemGrid.mLastEntryJ + 1;
                        if (itemGrid.mbHorizontalScrolling)
                        {
                            itemGrid.mGrid.SetColumnWidth(itemGrid.mLastEntryJ, itemGrid.mGrid.DefaultColumnWidth);
                        }
                        else
                        {
                            itemGrid.mGrid.SetRowHeight(itemGrid.mLastEntryJ, itemGrid.mGrid.DefaultRowHeight);
                        }
                    }

                    column = (itemGrid.mbHorizontalScrolling ? itemGrid.mLastEntryJ : itemGrid.mLastEntryI);
                    row = (itemGrid.mbHorizontalScrolling ? itemGrid.mLastEntryI : itemGrid.mLastEntryJ);
                }
                else
                {
                    itemGrid.mLastEntryJ = itemGrid.mTempEntryJ;
                    itemGrid.mLastEntryI = itemGrid.mTempEntryI;
                    column = (itemGrid.mbHorizontalScrolling ? itemGrid.mLastEntryJ : itemGrid.mLastEntryI);
                    row = (itemGrid.mbHorizontalScrolling ? itemGrid.mLastEntryI : itemGrid.mLastEntryJ);
                    itemGrid.mGrid.ClearCell(column, row);
                    itemGrid.mTempEntryI = -1;
                    itemGrid.mTempEntryJ = -1;
                }

                item.mWin.Visible = false;
                itemGrid.mGrid.SetCellWindow(column, row, item.mWin, itemGrid.mbStretchCellWindows);
                itemGrid.mGrid.CellTags[column, row] = item.mTag;
                if (!itemGrid.mPopulating)
                {
                    itemGrid.mGrid.Refresh();
                    itemGrid.UpdateScrollbar();
                    if (itemGrid.ItemRowsChanged != null)
                    {
                        itemGrid.ItemRowsChanged();
                    }
                }
            }
        }

        private static void HookedPopulateTypesGrid()
        {
            LoadedItems.Clear();
            CASClothingCategory gSingleton = CASClothingCategory.gSingleton;
            if (gSingleton == null)
            {
                return;
            }

            ItemGrid mClothingTypesGrid = gSingleton.mClothingTypesGrid;
            if (mClothingTypesGrid == null)
            {
                return;
            }

            mClothingTypesGrid.Tick -= ItemGrid_Tick;

            int visibleColumns = (int)mClothingTypesGrid.VisibleColumns;
            int visibleRows = (int)mClothingTypesGrid.VisibleRows;

            mClothingTypesGrid.mPopulateStride = 0;
            mClothingTypesGrid.AbortPopulating();
            mClothingTypesGrid.Clear();

            if (gSingleton.mCategoryText.Caption.Equals(gSingleton.GetClothingStateName(CASClothingState.Career)))
            {
                ICASModel casmodel = Responder.Instance.CASModel;
                if (casmodel == null)
                {
                    return;
                }

                if (casmodel.OutfitIndex == 0)
                {
                    if (CASClothingCategory.OnClothingGridFinishedPopulating != null)
                    {
                        CASClothingCategory.OnClothingGridFinishedPopulating();
                    }

                    if (!CASController.Singleton.AccessCareer && !Responder.Instance.CASModel.IsEditingUniform)
                    {
                        return;
                    }
                }
            }

            gSingleton.mCurrentPreset = null;
            gSingleton.mCurrentFocusedRow = null;
            gSingleton.mTempFocusedRow = null;
            gSingleton.mSelectedType = CASClothingRow.SelectedTypes.None;
            gSingleton.mShareButton.Enabled = false;
            gSingleton.mTrashButton.Enabled = false;
            gSingleton.mSaveButton.Enabled = false;
            gSingleton.mSortButton.Enabled = true;
            gSingleton.mSortButton.Tag = false;
            mClothingTypesGrid.mPopulateCallback = null;

            partList.Clear();
            CacheWornParts();
            
            //Compact mode
            var compactMode = false;
            if (gSingleton.mCurrentPart == BodyTypes.Accessories)
            {
                compactMode = Config.Data.Clothes.CompactModeAccessories;
            }
            else
            {
                compactMode = Config.Data.Clothes.CompactModeClothes;
            }

            if (compactMode)
            {
                List<object> currentGroup = new List<object>();
                
                foreach (object obj in gSingleton.mPartsList)
                {
                    if (obj != null && CASPuck.GetContentTypeFilter().ObjectMatchesFilter(obj))
                    {
                        currentGroup.Add(obj);
                        
                        if (currentGroup.Count == 3)
                        {
                            partList.Add(currentGroup);
                            currentGroup = new List<object>();
                        }
                    }
                }
                
                if (currentGroup.Count > 0)
                {
                    partList.Add(currentGroup);
                }
            }
            else
            {
                foreach (object obj in gSingleton.mPartsList)
                {
                    if (obj != null && CASPuck.GetContentTypeFilter().ObjectMatchesFilter(obj))
                    {
                        partList.Add(obj);
                    }
                }
            }

            int itemsPerPage = visibleColumns * visibleRows;

            int loadedCount = 0;
            int currentItemIndex = 0;
            mClothingTypesGrid.mPopulating = true;

            while (loadedCount < itemsPerPage && currentItemIndex < partList.Count)
            {
                object obj = partList[currentItemIndex];
                if (obj == null)
                {
                    break;
                }

                int row = loadedCount / visibleColumns;
                int col = loadedCount % visibleColumns;

                if (SetGridItemAtPosition(mClothingTypesGrid, obj, layoutKey,
                        null, row, col))
                {
                    LoadedItems[$"{row}_{col}"] = currentItemIndex;
                    loadedCount++;
                }

                currentItemIndex++;
            }

            if (loadedCount > 0)
            {
                int lastRow = (loadedCount - 1) / visibleColumns;
                int lastCol = (loadedCount - 1) % visibleColumns;
                mClothingTypesGrid.mLastEntryI = lastCol;
                mClothingTypesGrid.mLastEntryJ = lastRow;
            }
            else
            {
                mClothingTypesGrid.mLastEntryI = -1;
                mClothingTypesGrid.mLastEntryJ = 0;
            }

            while (loadedCount < itemsPerPage)
            {
                int row = loadedCount / visibleColumns;
                int col = loadedCount % visibleColumns;
                LoadedItems[$"{row}_{col}"] = -1;
                loadedCount++;
            }

            int totalItems = partList.Count;
            int totalRows = (int)Math.Ceiling(totalItems / (double)visibleColumns);

            for (int row = visibleRows; row < totalRows; row++)
            {
                for (int col = 0; col < visibleColumns; col++)
                {
                    if (row == totalRows - 1 && col == visibleColumns - 1)
                    {
                        mClothingTypesGrid.mPopulating = false;
                    }

                    AddItem(mClothingTypesGrid,
                        new ItemGridCellItem(placeHolderRow, null));
                }
            }

            mClothingTypesGrid.mPopulating = false;

            mClothingTypesGrid.Tick += ItemGrid_Tick;

            gSingleton.mContentTypeFilter.UpdateFilterButtonState();
            gSingleton.UpdateButtons(gSingleton.mSelectedType);

            if (CASClothingCategory.OnClothingGridFinishedPopulating != null)
            {
                CASClothingCategory.OnClothingGridFinishedPopulating();
            }
        }

        private static void ItemGrid_Tick(WindowBase sender, UIEventArgs eventArgs)
        {
            ItemGrid mClothingTypesGrid = CASClothingCategory.gSingleton.mClothingTypesGrid;

            int visibleColumns = (int)mClothingTypesGrid.VisibleColumns;
            int visibleRows = (int)mClothingTypesGrid.VisibleRows;

            double scrollPosition = (double)mClothingTypesGrid.VScrollbar.Value / 135.0; // Row height
            int firstVisibleRow = (int)Math.Floor(scrollPosition);
            int lastVisibleRow = firstVisibleRow + visibleRows;

            for (int row = firstVisibleRow; row <= lastVisibleRow; row++)
            {
                for (int col = 0; col < visibleColumns; col++)
                {
                    string key = $"{row}_{col}";

                    if (!LoadedItems.ContainsKey(key) ||
                        LoadedItems[key] == -1)
                    {
                        int itemIndex = (row * visibleColumns) + col;

                        if (itemIndex < partList.Count)
                        {
                            object obj = partList[itemIndex];
                            if (obj != null)
                            {
                                if (SetGridItemAtPosition(
                                        mClothingTypesGrid, obj, layoutKey,
                                        null, row, col))
                                {
                                    LoadedItems[key] = itemIndex;
                                    return;
                                }
                            }
                        }
                        else
                        {
                            LoadedItems[key] = -2;
                        }
                    }
                }
            }
        }

        private static bool SetGridItemAtPosition(ItemGrid grid, object current, ResourceKey layoutKey, object context,
            int targetRow, int targetCol)
        {
            CASClothingCategory gSingleton = CASClothingCategory.gSingleton;
            bool result = false;

            if (current != null)
            {
                if (current is List<object> group)
                {
                    CASClothingRow casclothingRow = 
                        UIManager.LoadLayout(layoutKey).GetWindowByExportID(1) as CASClothingRow;
                    if (casclothingRow == null) return false;
                    
                    casclothingRow.RowController = gSingleton;
                    
                    if (group.Count > 0 && group[0] is CASPart)
                    {
                        casclothingRow.CASPart = (CASPart)group[0];
                        casclothingRow.UseEp5AsBaseContent = gSingleton.mIsEp5Base;
                    }
                    
                    ArrayList arrayList = CreateGridItemsForGroup(casclothingRow, group);
                    
                    gSingleton.mSortButton.Tag = ((bool)gSingleton.mSortButton.Tag | casclothingRow.HasFilterableContent);
                    
                    if (arrayList.Count > 0)
                    {
                        if (targetRow >= grid.EntriesCountJ)
                        {
                            grid.EntriesCountJ = targetRow + 1;
                            grid.mGrid.SetRowHeight(targetRow, grid.mGrid.DefaultRowHeight);
                        }

                        grid.InternalGrid.SetCellWindow(targetCol, targetRow, casclothingRow,
                            grid.mbStretchCellWindows);
                        grid.InternalGrid.CellTags[targetCol, targetRow] = casclothingRow;
                        result = true;

                        grid.mLastEntryI = targetCol;
                        grid.mLastEntryJ = targetRow;
                        
                        if (casclothingRow.SelectedItem != -1)
                        {
                            BodyTypes bodyType = casclothingRow.CASPart.BodyType;
                            
                            if (gSingleton.IsAccessoryType(bodyType))
                            {
                                if (gSingleton.GetWornPart((BodyTypes)CASClothingCategory.sAccessoriesSelection).Key !=
                                    ResourceKey.kInvalidResourceKey)
                                {
                                    if (bodyType == (BodyTypes)CASClothingCategory.sAccessoriesSelection)
                                    {
                                        int gridIndex = (targetRow * (int)grid.VisibleColumns) + targetCol;
                                        grid.SelectedItem = gridIndex;
                                        gSingleton.mSelectedType = casclothingRow.SelectedType;
                                        CASClothingCategory.sAccessoriesSelection = (int)bodyType;
                                        gSingleton.mCurrentPreset = casclothingRow.Selection as CASPartPreset;
                                    }
                                }
                                else
                                {
                                    int gridIndex = (targetRow * (int)grid.VisibleColumns) + targetCol;
                                    grid.SelectedItem = gridIndex;
                                    gSingleton.mSelectedType = casclothingRow.SelectedType;
                                    CASClothingCategory.sAccessoriesSelection = (int)bodyType;
                                    gSingleton.mCurrentPreset = casclothingRow.Selection as CASPartPreset;
                                }
                            }
                            else
                            {
                                int gridIndex = (targetRow * (int)grid.VisibleColumns) + targetCol;
                                grid.SelectedItem = gridIndex;
                                gSingleton.mSelectedType = casclothingRow.SelectedType;
                                gSingleton.mCurrentPreset = casclothingRow.Selection as CASPartPreset;
                            }
                        }
                    }
                }
                else if (current is CASPart)
                {
                    CASPart caspart = (CASPart)current;
                    CASClothingRow casclothingRow =
                        UIManager.LoadLayout(layoutKey).GetWindowByExportID(1) as CASClothingRow;
                    casclothingRow.UseEp5AsBaseContent = gSingleton.mIsEp5Base;
                    casclothingRow.CASPart = caspart;
                    casclothingRow.RowController = gSingleton;
                    ArrayList arrayList = CreateGridItems(casclothingRow, true);
                    gSingleton.mSortButton.Tag =
                        ((bool)gSingleton.mSortButton.Tag | casclothingRow.HasFilterableContent);

                    if (arrayList.Count > 0)
                    {
                        if (targetRow >= grid.EntriesCountJ)
                        {
                            grid.EntriesCountJ = targetRow + 1;
                            grid.mGrid.SetRowHeight(targetRow, grid.mGrid.DefaultRowHeight);
                        }

                        grid.InternalGrid.SetCellWindow(targetCol, targetRow, casclothingRow,
                            grid.mbStretchCellWindows);
                        grid.InternalGrid.CellTags[targetCol, targetRow] = casclothingRow;
                        result = true;

                        grid.mLastEntryI = targetCol;
                        grid.mLastEntryJ = targetRow;

                        if (casclothingRow.SelectedItem != -1)
                        {
                            if (gSingleton.IsAccessoryType(caspart.BodyType))
                            {
                                if (gSingleton.GetWornPart((BodyTypes)CASClothingCategory.sAccessoriesSelection).Key !=
                                    ResourceKey.kInvalidResourceKey)
                                {
                                    if (caspart.BodyType == (BodyTypes)CASClothingCategory.sAccessoriesSelection)
                                    {
                                        int gridIndex = (targetRow * (int)grid.VisibleColumns) + targetCol;
                                        grid.SelectedItem = gridIndex;
                                        gSingleton.mSelectedType = casclothingRow.SelectedType;
                                        CASClothingCategory.sAccessoriesSelection = (int)caspart.BodyType;
                                        gSingleton.mCurrentPreset = (casclothingRow.Selection as CASPartPreset);
                                    }
                                }
                                else
                                {
                                    int gridIndex = (targetRow * (int)grid.VisibleColumns) + targetCol;
                                    grid.SelectedItem = gridIndex;
                                    gSingleton.mSelectedType = casclothingRow.SelectedType;
                                    CASClothingCategory.sAccessoriesSelection = (int)caspart.BodyType;
                                    gSingleton.mCurrentPreset = (casclothingRow.Selection as CASPartPreset);
                                }
                            }
                            else
                            {
                                int gridIndex = (targetRow * (int)grid.VisibleColumns) + targetCol;
                                grid.SelectedItem = gridIndex;
                                gSingleton.mSelectedType = casclothingRow.SelectedType;
                                gSingleton.mCurrentPreset = (casclothingRow.Selection as CASPartPreset);
                            }
                        }
                    }
                }
                else
                {
                    List<object> list = current as List<object>;
                    if (list != null)
                    {
                        CASClothingRow casclothingRow =
                            UIManager.LoadLayout(layoutKey).GetWindowByExportID(1) as CASClothingRow;
                        casclothingRow.ObjectOfInterest = list;
                        casclothingRow.RowController = gSingleton;
                        ArrayList arrayList = CreateGridItems(casclothingRow, true);
                        gSingleton.mSortButton.Tag =
                            ((bool)gSingleton.mSortButton.Tag | casclothingRow.HasFilterableContent);

                        if (arrayList.Count > 0)
                        {
                            if (targetRow >= grid.EntriesCountJ)
                            {
                                grid.EntriesCountJ = targetRow + 1;
                                grid.mGrid.SetRowHeight(targetRow, grid.mGrid.DefaultRowHeight);
                            }

                            grid.InternalGrid.SetCellWindow(targetCol, targetRow, casclothingRow,
                                grid.mbStretchCellWindows);
                            grid.InternalGrid.CellTags[targetCol, targetRow] = casclothingRow;

                            grid.mLastEntryI = targetCol;
                            grid.mLastEntryJ = targetRow;

                            result = true;
                        }
                    }
                }
            }

            return result;
        }

        private static void UpdateScrollbar(ItemGrid grid, int VisibleRange = -1, int UpperBoundValue = -1)
        {
            if (UIManager.IsRunningDesigner)
            {
                if (grid.mHScrollLeftButton != null && grid.mHScrollRightButton != null)
                {
                    grid.mHScrollbar.Visible = (grid.mbHorizontalScrolling && !grid.mbUseArrowsForScrolling);
                    grid.mVScrollbar.Visible = (!grid.mbHorizontalScrolling && !grid.mbUseArrowsForScrolling);
                    if (grid.mbHorizontalScrolling)
                    {
                        grid.mHScrollLeftButton.Visible = grid.mbUseArrowsForScrolling;
                        grid.mHScrollRightButton.Visible = grid.mbUseArrowsForScrolling;
                        grid.mVScrollUpButton.Visible = false;
                        grid.mVScrollDownButton.Visible = false;
                        return;
                    }

                    grid.mHScrollLeftButton.Visible = false;
                    grid.mHScrollRightButton.Visible = false;
                    grid.mVScrollUpButton.Visible = grid.mbUseArrowsForScrolling;
                    grid.mVScrollDownButton.Visible = grid.mbUseArrowsForScrolling;
                    return;
                }
            }
            else
            {
                Scrollbar scrollbar = grid.mbHorizontalScrolling ? grid.mHScrollbar : grid.mVScrollbar;
                (grid.mbHorizontalScrolling ? grid.mVScrollbar : grid.mHScrollbar).Visible = false;
                int num = 0;
                int num2 = 0;
                grid.mGrid.GetScrollMetrics(grid.mbHorizontalScrolling, ref num, ref num2, ref grid.mScrollIncrement);
                scrollbar.UpperBoundValue = num2;
                scrollbar.VisibleRange = num;
                if (UpperBoundValue != -1)
                {
                    scrollbar.UpperBoundValue = UpperBoundValue;
                }

                if (VisibleRange != -1)
                {
                    scrollbar.VisibleRange = VisibleRange;
                }

                scrollbar.ArrowDelta = (int)grid.mScrollIncrement;
                if (!grid.mbUseArrowsForScrolling)
                {
                    grid.mHScrollLeftButton.Visible = false;
                    grid.mHScrollRightButton.Visible = false;
                    grid.mVScrollUpButton.Visible = false;
                    grid.mVScrollDownButton.Visible = false;
                    scrollbar.Visible = (num2 > num);
                    return;
                }

                if (!grid.mbTickEventRegisteredForScroll)
                {
                    grid.Tick += grid.OnTickForUpdateScrollbar;
                    grid.mbTickEventRegisteredForScroll = true;
                }
            }
        }

        public static ObjectGuid taskGuid;

        private static ResourceKey layoutKey;

        private static CASClothingRow placeHolderRow;

        private static Dictionary<BodyTypes, List<CASPart>> wornParts = new Dictionary<BodyTypes, List<CASPart>>();

        private static Dictionary<ResourceKey, string> wornPresets = new Dictionary<ResourceKey, string>();

        public static ArrayList partList = new ArrayList();
    }
}