using System;
using System.Collections.Generic;
using Arro.Common;
using Sims3.SimIFace;
using Sims3.SimIFace.CAS;
using Sims3.UI;
using Sims3.UI.CAS;
using Sims3.UI.CAS.CAP;
using Simulator = Sims3.SimIFace.Simulator;
using StopWatch = Sims3.SimIFace.StopWatch;
using UIManager = Sims3.UI.UIManager;
using static Arro.Common.Logger;

namespace Arro.MCR;

public class Clothes
{
    public static Window currentLayout;

    public static void Hook()
    {
        GetCurrentLayout();
        if (Config.Data.Clothes.SmoothPatchEnabled)
        {
            LazyLoading.InitializeLazyLoading();
        }
        SetClothesItemGrid();
        SetClothingBackgroundSize();
        SetButtonVisibilityAndEffect();
        MoveDoneButton();
        SetContentTypeFilter();
        if (CASClothing.gSingleton.mCareerButton.Visible)
        {
            CareerButtonFix(); 
        }
    }

    public static void GetCurrentLayout()
    {
        if (CASClothing.sClothingLayout != null && CASDresserClothing.sClothingLayout == null &&
            CAPAccessories.sCAPAccessoriesLayout == null)
        {
            currentLayout = CASClothing.gSingleton;
        }
        else if (CASClothing.sClothingLayout == null && CASDresserClothing.sClothingLayout != null &&
                 CAPAccessories.sCAPAccessoriesLayout == null)
        {
            currentLayout = CASDresserClothing.gSingleton;
        }
        else if (CASClothing.sClothingLayout == null && CASDresserClothing.sClothingLayout == null &&
                 CAPAccessories.sCAPAccessoriesLayout != null)
        {
            currentLayout = CAPAccessories.gSingleton;
        }
    }

    public static void SetClothesItemGrid()
    {
        var gridArea = CASClothingCategory.gSingleton.mClothingTypesGrid.Area;
        var visibleRows = (uint)Config.Data.Clothes.RowCount;
        gridArea.Height = (139f * Config.Data.Clothes.RowCount) * TinyUIFix.Scale;
        var visibleColumns = (uint)Config.Data.Clothes.ColumnCount;
        gridArea.Width = (305f * Config.Data.Clothes.ColumnCount + 20f) * TinyUIFix.Scale;
        CASClothingCategory.gSingleton.mClothingTypesGrid.VisibleColumns = visibleColumns;
        CASClothingCategory.gSingleton.mClothingTypesGrid.VisibleRows = visibleRows;
        CASClothingCategory.gSingleton.mClothingTypesGrid.Area = gridArea;
    }

    public static void SetButtonVisibilityAndEffect()
    {
        var designButtons = new[]
        {
            CASClothingCategory.gSingleton.mTrashButton,
            CASClothingCategory.gSingleton.mSaveButton,
            CASClothingCategory.gSingleton.mShareButton,
        };
        foreach (var button in designButtons)
        {
            button.Visible = false;
        }

        if (Config.Data.Clothes.ColumnCount > 1)
        {
            CASClothingCategory.gSingleton.mDesignButton.Position = new Vector2(
                CASClothingCategory.gSingleton.mTrashButton.Position.x +
                310f * TinyUIFix.Scale,
                CASClothingCategory.gSingleton.mSortButton.Position.y -
                10.5f * TinyUIFix.Scale);

            CASClothingCategory.gSingleton.mSortButton.Position = new Vector2(
                CASClothingCategory.gSingleton.mSortButton.Position.x,
                CASClothingCategory.gSingleton.mSortButton.Position.y -
                10f * TinyUIFix.Scale);
        }
        else
        {
            CASClothingCategory.gSingleton.mDesignButton.Visible = false;
        }

        if (currentLayout == CAPAccessories.gSingleton) return;

        var topButtons = new[]
        {
            CASClothingCategory.gSingleton.mTopsButton,
            CASClothingCategory.gSingleton.mBottomsButton,
            CASClothingCategory.gSingleton.mOutfitsButton,
            CASClothingCategory.gSingleton.mShoesButton,
            CASClothingCategory.gSingleton.mAccessoriesButton,
            CASClothingCategory.gSingleton.mSortButton,
            CASClothingCategory.gSingleton.mDesignButton
        };
        foreach (var button in topButtons)
        {
            try
            {
                if (button != null)
                {
                    EffectManager.AddScaleEffect(button, duration: 0.1f,
                        triggerType: EffectBase.TriggerTypes.MouseFocus);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        if (currentLayout != CASClothing.gSingleton) return;

        var casClothingSideButtons = new[]
        {
            CASClothing.gSingleton?.mEverydayButton,
            CASClothing.gSingleton?.mFormalButton,
            CASClothing.gSingleton?.mSleepwearButton,
            CASClothing.gSingleton?.mSwimwearButton,
            CASClothing.gSingleton?.mExerciseButton,
            CASClothing.gSingleton?.mOuterwearButton,
            CASClothing.gSingleton?.mCareerButton,
            CASClothing.gSingleton?.GetChildByID(98288906U, true) as Button,
        };
        foreach (var button in casClothingSideButtons)
        {
            if (button != null)
            {
                button.FocusAcquired -= OnSideButtonOnFocusAcquired;
                button.FocusAcquired += OnSideButtonOnFocusAcquired;
                EffectManager.AddGrowEffect(button, leftChange: 1.5f,
                    triggerType: EffectBase.TriggerTypes.Manual);
                EffectManager.AddGrowEffect(button, leftChange: 4f,
                    triggerType: EffectBase.TriggerTypes.ButtonSelected);
            }
        }
    }

    private static void OnSideButtonOnFocusAcquired(WindowBase sender, UIFocusChangeEventArgs eventArgs)
    {
        if (sender.Tag is GrowEffect grow && sender.DrawState != (uint)WindowBase.DrawStateFlags.kDrawStateActive)
        {
            grow.Duration = 0.09f;
            grow.TriggerEffect(false);
        }
    }

    public static Button mDoneButton;

    public static void MoveDoneButton()
    {
        if (currentLayout == null) return;
        float startingPositionX;
        float startingPositionY;

        switch (currentLayout)
        {
            case CASClothing _:
                mDoneButton = CASClothing.gSingleton.GetChildByID(98278400U, true) as Button;
                startingPositionX = -8f;
                startingPositionY = 6f;
                mDoneButton.Position =
                    new Vector2(
                        startingPositionX + (300 * TinyUIFix.Scale *
                                             (Config.Data.Clothes.ColumnCount - 1)), startingPositionY);
                break;
            case CASDresserClothing _:
                mDoneButton = CASDresserClothing.gSingleton.GetChildByID(98278400U, true) as Button;
                startingPositionX = -8f;
                startingPositionY = 6f;
                mDoneButton.Position =
                    new Vector2(
                        startingPositionX + (300 * TinyUIFix.Scale *
                                             (Config.Data.Clothes.ColumnCount - 1)), startingPositionY);
                break;
            case CAPAccessories _:
                mDoneButton = CAPAccessories.gSingleton.GetChildByID(2095900161U, true) as Button;
                startingPositionX = 353f;
                startingPositionY = 35f;
                mDoneButton.Position =
                    new Vector2(
                        startingPositionX + (300 * TinyUIFix.Scale *
                                             (Config.Data.Clothes.ColumnCount - 1)), startingPositionY);
                break;
        }

        mDoneButton.Click += OnDoneButtonClick;
        EffectManager.AddScaleEffect(mDoneButton, scale: 1.05f, duration: 0.1f);
    }

    private static void OnDoneButtonClick(WindowBase sender, UIButtonClickEventArgs eventArgs)
    {
        mDoneButton.Click -= OnDoneButtonClick;
        LazyLoading.TaskGuid.Dispose();
    }


    public static void SetClothingBackgroundSize()
    {
        var backgroundHeight = (534f + (139f * (Config.Data.Clothes.RowCount - 3))) *
                               TinyUIFix.Scale;
        var backgroundWidth = (300f * Config.Data.Clothes.ColumnCount + 109f) * TinyUIFix.Scale;
        if (currentLayout == null) return;
        Rect rect;
        switch (currentLayout)
        {
            case CASClothing _:
                rect = CASClothing.gSingleton.Area;
                rect.Height = backgroundHeight;
                rect.Width = backgroundWidth;
                CASClothing.gSingleton.Area = rect;
                break;

            case CASDresserClothing _:
                rect = CASDresserClothing.gSingleton.Area;
                rect.Height = backgroundHeight;
                rect.Width = backgroundWidth;
                CASDresserClothing.gSingleton.Area = rect;
                break;

            case CAPAccessories _:
                rect = CAPAccessories.gSingleton.Area;
                rect.Height = backgroundHeight;
                rect.Width = backgroundWidth;
                CAPAccessories.gSingleton.Area = rect;
                break;
        }
    }

    private static int GetVisibleRowsSetting(int desiredActualRows)
    {
        if (desiredActualRows <= 6)
            return desiredActualRows;
        return (int)Math.Ceiling((desiredActualRows + 5) / 2f);
    }

    private static int GetFilterRowsFromClothingRows(int clothingRows)
    {
        switch (clothingRows)
        {
            case 3:
                return 10;
            case 4:
                return 15;
            case 5:
                return 19;
            default:
                return 24;
        }
    }

    private static void SetContentTypeFilter()
    {
        var holderWin = UIManager.GetMainWindow().GetChildByID(0x092ccf31, true) as Window;
        var sortItemGrid = UIManager.GetMainWindow().GetChildByID(0x0b4fbf70, true) as ItemGrid;
        var itemGridImage = sortItemGrid.GetChildByIndex(1);
        var mainBG = holderWin.GetChildByIndex(1);
        if (mainBG != null)
        {
            holderWin.DestroyChild(mainBG);
            holderWin.DestroyChild(itemGridImage);
        }

        var vector2sortItemGridPosition = sortItemGrid.Position;
        if (Config.Data.Clothes.RowCount >= 6 || CASPuck.gSingleton.mContentTypeFilter.mCells.Count <
            GetFilterRowsFromClothingRows(Config.Data.Clothes.RowCount))
        {
            vector2sortItemGridPosition.y = 48 * TinyUIFix.Scale;
        }
        else
        {
            vector2sortItemGridPosition.y = 67 * TinyUIFix.Scale;
        }

        sortItemGrid.Position = vector2sortItemGridPosition;

        FadeEffect existingFade = null;
        foreach (var obj in holderWin.EffectList)
        {
            if (obj is FadeEffect fade)
            {
                existingFade = fade;
                break;
            }
        }

        if (existingFade != null)
        {
            existingFade.Duration = 0.1f;
        }

        if (Config.Data.Clothes.RowCount < 6 && CASPuck.gSingleton.mContentTypeFilter.mCells.Count >
            GetFilterRowsFromClothingRows(Config.Data.Clothes.RowCount))
        {
            sortItemGrid.mGrid.MouseWheel -= sortItemGrid.OnGridMouseWheel;
            sortItemGrid.mGrid.MouseWheel += (_, e) => HandleMouseWheel(e, sortItemGrid);
            sortItemGrid.mVScrollDownButton.Position = new Vector2(sortItemGrid.mVScrollDownButton.Position.x,
                CASClothingCategory.gSingleton.ContainerGrid.mVScrollDownButton.Position.y);
        }

        var desiredVisibleRows = GetFilterRowsFromClothingRows(Config.Data.Clothes.RowCount);
        var visibleRowsSetting = GetVisibleRowsSetting(desiredVisibleRows);

        sortItemGrid.VisibleRows = (uint)visibleRowsSetting;

        var baseItemGridHeight = -126f;
        var baseMGridHeight = 0f;
        var baseButtonY = 191f * TinyUIFix.Scale;

        var itemGridHeight = baseItemGridHeight + (64f * (sortItemGrid.VisibleRows - 5)) * TinyUIFix.Scale;
        var mGridHeight = baseMGridHeight + (32f * (sortItemGrid.VisibleRows - 5)) * TinyUIFix.Scale;
        var buttonY = baseButtonY + (64f * (sortItemGrid.VisibleRows - 5)) * TinyUIFix.Scale;

        var itemGridArea = sortItemGrid.Area;
        itemGridArea.Height = itemGridHeight;
        sortItemGrid.Area = itemGridArea;

        var vector2 = sortItemGrid.mVScrollDownButton.Position;
        vector2.y = buttonY;
        sortItemGrid.mVScrollDownButton.Position = vector2;

        var gridArea = sortItemGrid.mGrid.Area;
        gridArea.Height = mGridHeight;
        sortItemGrid.mGrid.Area = gridArea;

        var holderArea = holderWin.Area;
        var originalWidth = holderArea.Width;
        var columnWidth = 300f * TinyUIFix.Scale;
        var extraWidth = columnWidth * (Config.Data.Clothes.ColumnCount - 1);
        holderArea.Width = originalWidth + extraWidth - 30f * TinyUIFix.Scale;
        holderWin.Area = holderArea;

        sortItemGrid.UpdateGridSize();
        sortItemGrid.UpdateScrollbar();

        var cellButtons = new List<Button>();

        foreach (var cellItem in sortItemGrid.Items)
        {
            var cell = cellItem.mWin as CatalogProductFilter.Cell;
            if (cell != null)
            {
                var checkBox = cell.GetChildByID(1U, true) as Button;
                if (checkBox != null)
                {
                    cellButtons.Add(checkBox);
                    if (Config.Data.Clothes.RowCount < 6)
                    {
                        checkBox.MouseWheel -= OnCellButtonMouseWheel;
                        checkBox.MouseWheel += OnCellButtonMouseWheel;
                    }
                }
            }
        }

        foreach (var button in cellButtons)
        {
            EffectManager.RemoveAllEffects(button);
            EffectManager.AddScaleEffect(button, scale: 0.9f, duration: 0.1f);
        }

        EffectManager.AddGrowEffect(currentLayout, rightChange: 35f, duration: 0.1f,
            triggerType: EffectBase.TriggerTypes.Manual);
        EffectManager.AddGlideEffect(mDoneButton, offset: new Vector2(35f, 0f), duration: 0.1f,
            triggerType: EffectBase.TriggerTypes.Manual);
        CASPuck.gSingleton.mContentTypeFilter.VisibilityChange -= OnContentTypeFilterVisibilityChange;
        CASPuck.gSingleton.mContentTypeFilter.VisibilityChange += OnContentTypeFilterVisibilityChange;
    }

    private static void OnContentTypeFilterVisibilityChange(WindowBase sender,
        UIVisibilityChangeEventArgs eventArgs)
    {
        if (eventArgs.Visible)
        {
            if (currentLayout.Tag is GrowEffect grow)
            {
                grow.TriggerEffect(false);
            }

            if (mDoneButton.Tag is GlideEffect glide)
            {
                glide.TriggerEffect(false);
            }
        }
        else
        {
            if (currentLayout.Tag is GrowEffect grow)
            {
                grow.TriggerEffect(true);
            }

            if (mDoneButton.Tag is GlideEffect glide)
            {
                glide.TriggerEffect(true);
            }
        }
    }

    private static void HandleMouseWheel(UIMouseEventArgs eventArgs, ItemGrid itemGrid)
    {
        if (itemGrid == null) return;

        var num = eventArgs.MouseWheelDelta / 100;
        if (num == 0)
        {
            num = ((eventArgs.MouseWheelDelta < 0) ? -1 : 1);
        }

        var scrollbar = itemGrid.mbHorizontalScrolling ? itemGrid.mHScrollbar : itemGrid.mVScrollbar;
        if (scrollbar == null) return;

        if (itemGrid.mGrid.SmoothScrolling)
        {
            scrollbar.TargetValue -= num * (int)itemGrid.mScrollIncrement;
        }
        else
        {
            scrollbar.Value -= num * (int)itemGrid.mScrollIncrement;
            itemGrid.mGrid.ScrollUnits(
                itemGrid.mbHorizontalScrolling ? Grid.Orientation.Horizontal : Grid.Orientation.Vertical,
                -(num * (int)itemGrid.mScrollIncrement));
        }

        Simulator.AddObject(new OneShotFunctionTask(() =>
        {
            if (itemGrid.mVScrollUpButton != null)
                itemGrid.mVScrollUpButton.Visible =
                    (itemGrid.Count > 0 && !itemGrid.mGrid.IsCellVisible(0, 0));
            if (itemGrid.mVScrollDownButton != null)
                itemGrid.mVScrollDownButton.Visible =
                    (itemGrid.Count >
                     itemGrid.VisibleColumns * itemGrid.VisibleRows &&
                     !itemGrid.mGrid.IsCellVisible(0, itemGrid.mGrid.RowCount - 1));
        }, StopWatch.TickStyles.Seconds, 0.2f));

        eventArgs.Handled = true;
    }


    private static void OnCellButtonMouseWheel(WindowBase sender, UIMouseEventArgs e)
    {
        var checkBox = sender as Button;
        if (checkBox == null) return;

        var cell = checkBox.Parent as CatalogProductFilter.Cell;
        if (cell == null) return;

        var cellItemWin = cell.Parent;
        if (cellItemWin == null) return;

        ItemGrid itemGrid = null;
        var current = cellItemWin;
        while (current != null)
        {
            if (current is ItemGrid grid)
            {
                itemGrid = grid;
                break;
            }

            current = current.Parent;
        }

        if (itemGrid == null) return;
        HandleMouseWheel(e, itemGrid);
    }

    private static void CareerButtonFix()
    {
        var careerButton = currentLayout.GetChildByID(0x05dbc509, true) as Button;
        careerButton.Visible = true;

        var multiDrawable = careerButton.Drawable as MultiDrawable;
        var buttonIconDrawable = multiDrawable[1U];
        var iconDrawable = buttonIconDrawable as IconDrawable;
        iconDrawable.Image = UIManager.LoadUIImage(ResourceKey.CreatePNGKey("hud_icon_career_r2", 0U));
        iconDrawable.Scale = 0.8f * TinyUIFix.Scale;
        careerButton.Invalidate();
    }
}