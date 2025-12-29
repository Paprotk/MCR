using System.Collections.Generic;
using System.Diagnostics;
using Sims3.SimIFace;
using Sims3.UI;
using Sims3.UI.CAS;
using Sims3.UI.CAS.CAP;

namespace Arro.MCR
{
    public class Clothes
    {
        public static string currentLayout;

        public static void Hook()
        {
            GetCurrentLayout();
            SetClothesItemGrid();
            SetClothingBackgroundSize();
            SetButtonVisibilityAndEffect();
            MoveDoneButton();
            SetContentTypeFilter();
            CareerButtonFix();
        }

        public static void GetCurrentLayout()
        {
            if (CASClothing.sClothingLayout != null && CASDresserClothing.sClothingLayout == null &&
                CAPAccessories.sCAPAccessoriesLayout == null)
            {
                currentLayout = "CASClothing";
            }
            else if (CASClothing.sClothingLayout == null && CASDresserClothing.sClothingLayout != null &&
                     CAPAccessories.sCAPAccessoriesLayout == null)
            {
                currentLayout = "CASDresserClothing";
            }
            else if (CASClothing.sClothingLayout == null && CASDresserClothing.sClothingLayout == null &&
                     CAPAccessories.sCAPAccessoriesLayout != null)
            {
                currentLayout = "CAPAccessories";
            }
        }

        public static void SetClothesItemGrid()
        {
            if (CASClothingCategory.gSingleton == null) return;
            var gridArea = CASClothingCategory.gSingleton.mClothingTypesGrid.Area;
            var visibleRows = (uint)Config.Data.Clothes.Rows;
            gridArea.Height = (139f * Config.Data.Clothes.Rows) * TinyUIFixForTS3Integration.getUIScale();
            var visibleColumns = (uint)Config.Data.Clothes.Columns;
            gridArea.Width = (305f * Config.Data.Clothes.Columns + 20f) * TinyUIFixForTS3Integration.getUIScale();
            CASClothingCategory.gSingleton.mClothingTypesGrid.VisibleColumns = visibleColumns;
            CASClothingCategory.gSingleton.mClothingTypesGrid.VisibleRows = visibleRows;
            CASClothingCategory.gSingleton.mClothingTypesGrid.Area = gridArea;
        }

        public static void SetButtonVisibilityAndEffect()
        {
            var buttons = new[]
            {
                CASClothingCategory.gSingleton.mTrashButton,
                CASClothingCategory.gSingleton.mSaveButton,
                CASClothingCategory.gSingleton.mShareButton,
            };
            foreach (var button in buttons)
            {
                button.Visible = false;
            }

            if (Config.Data.Clothes.Columns > 1)
            {
                CASClothingCategory.gSingleton.mDesignButton.Position = new Vector2(
                    CASClothingCategory.gSingleton.mTrashButton.Position.x +
                    310f * TinyUIFixForTS3Integration.getUIScale(),
                    CASClothingCategory.gSingleton.mSortButton.Position.y -
                    10.5f * TinyUIFixForTS3Integration.getUIScale());

                CASClothingCategory.gSingleton.mSortButton.Position = new Vector2(
                    CASClothingCategory.gSingleton.mSortButton.Position.x,
                    CASClothingCategory.gSingleton.mSortButton.Position.y -
                    10f * TinyUIFixForTS3Integration.getUIScale());

                CASClothingCategory.gSingleton.mContentTypeFilter.Position = new Vector2(
                    CASClothingCategory.gSingleton.mContentTypeFilter.Position.x,
                    CASClothingCategory.gSingleton.mContentTypeFilter.Position.y -
                    10f * TinyUIFixForTS3Integration.getUIScale());
            }
            else
            {
                CASClothingCategory.gSingleton.mDesignButton.Visible = false;
            }
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
                if (button != null)
                {
                    EffectManager.AddScaleEffect(button);
                }
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
                case "CASClothing":
                    mDoneButton = CASClothing.gSingleton.GetChildByID(98278400U, true) as Button;
                    startingPositionX = -8f;
                    startingPositionY = 6f;
                    mDoneButton.Position =
                        new Vector2(
                            startingPositionX + (300 * TinyUIFixForTS3Integration.getUIScale() *
                                                 (Config.Data.Clothes.Columns - 1)), startingPositionY);
                    break;
                case "CASDresserClothing":
                    mDoneButton = CASDresserClothing.gSingleton.GetChildByID(98278400U, true) as Button;
                    startingPositionX = -8f;
                    startingPositionY = 6f;
                    mDoneButton.Position =
                        new Vector2(
                            startingPositionX + (300 * TinyUIFixForTS3Integration.getUIScale() *
                                                 (Config.Data.Clothes.Columns - 1)), startingPositionY);
                    break;
                case "CAPAccessories":
                    mDoneButton = CAPAccessories.gSingleton.GetChildByID(2095900161U, true) as Button;
                    startingPositionX = 353f;
                    startingPositionY = 35f;
                    mDoneButton.Position =
                        new Vector2(
                            startingPositionX + (300 * TinyUIFixForTS3Integration.getUIScale() *
                                                 (Config.Data.Clothes.Columns - 1)), startingPositionY);
                    break;
            }
            EffectManager.AddScaleEffect(mDoneButton, scale: 1.05f);
        }

        public static void SetClothingBackgroundSize()
        {
            var backgroundHeight = (534f + (139f * (Config.Data.Clothes.Rows - 3))) *
                                   TinyUIFixForTS3Integration.getUIScale();
            var backgroundWidth = (300f * Config.Data.Clothes.Columns + 109f) * TinyUIFixForTS3Integration.getUIScale();
            if (currentLayout == null) return;
            Rect rect;
            switch (currentLayout)
            {
                case "CASClothing":
                    rect = CASClothing.gSingleton.Area;
                    rect.Height = backgroundHeight;
                    rect.Width = backgroundWidth;
                    CASClothing.gSingleton.Area = rect;
                    break;

                case "CASDresserClothing":
                    rect = CASDresserClothing.gSingleton.Area;
                    rect.Height = backgroundHeight;
                    rect.Width = backgroundWidth;
                    CASDresserClothing.gSingleton.Area = rect;
                    break;

                case "CAPAccessories":
                    rect = CAPAccessories.gSingleton.Area;
                    rect.Height = backgroundHeight;
                    rect.Width = backgroundWidth;
                    CAPAccessories.gSingleton.Area = rect;
                    break;
            }
        }

        private static void SetContentTypeFilter()
        {
            var mainWindow = UIManager.GetMainWindow();
            var holder = mainWindow.GetChildByID(153931569, true) as Window;
            var sortItemGrid = mainWindow.GetChildByID(189775728, true) as ItemGrid;
            var imageInsideHolder = sortItemGrid.GetChildByIndex(1);

            sortItemGrid.VisibleRows = (uint)sortItemGrid.Count;
            var vector2 = sortItemGrid.Position;
            vector2.y = 40 * TinyUIFixForTS3Integration.getUIScale();
            sortItemGrid.Position = vector2;

            var backgroundHeightHolder = 910 * TinyUIFixForTS3Integration.getUIScale();
            var holderHeight = (backgroundHeightHolder - (24 - sortItemGrid.Count) * 32) *
                               TinyUIFixForTS3Integration.getUIScale();

            var holderArea = holder.Area;
            holderArea.Height = holderHeight;
            holder.Area = holderArea;

            var sortItemGridArea = sortItemGrid.Area;
            sortItemGridArea.Height = 500 * TinyUIFixForTS3Integration.getUIScale();
            sortItemGrid.Area = sortItemGridArea;

            var backgroundHeightHolderInsert = 774 * TinyUIFixForTS3Integration.getUIScale();
            var insertHeight = (backgroundHeightHolderInsert - (24 - sortItemGrid.Count) * 32) *
                               TinyUIFixForTS3Integration.getUIScale();
            var insertArea = imageInsideHolder.Area;
            insertArea.Height = insertHeight;
            imageInsideHolder.Area = insertArea;

            var area = holder.Area;
            float baseWidth = area.Width;
            float widthPerColumn = 300f * TinyUIFixForTS3Integration.getUIScale();
            area.Width = baseWidth + (widthPerColumn * (Config.Data.Clothes.Columns - 1));
            holder.Area = area;
            
            List<Button> cellButtons = new List<Button>();
            
            foreach (ItemGridCellItem cellItem in sortItemGrid.Items)
            {
                CatalogProductFilter.Cell cell = cellItem.mWin as CatalogProductFilter.Cell;
                if (cell != null)
                {
                    Button checkBox = cell.GetChildByID(1U, true) as Button;
                    if (checkBox != null)
                    {
                        cellButtons.Add(checkBox);
                    }
                }
            }
            foreach (var button in cellButtons)
            {
                EffectManager.RemoveAllEffects(button);
                EffectManager.AddScaleEffect(button, scale: 0.9f);
            }
            EffectManager.AddFadeEffect(CASClothingCategory.gSingleton.mDesignButton, duration: 0.2f, EffectBase.TriggerTypes.Manual);
            if (Config.Data.Clothes.Columns > 1)
            {
                CASPuck.gSingleton.mContentTypeFilter.VisibilityChange -= OnContentTypeFilterVisibilityChange;
                CASPuck.gSingleton.mContentTypeFilter.VisibilityChange += OnContentTypeFilterVisibilityChange;
            }
        }
        
        private static void OnContentTypeFilterVisibilityChange(WindowBase sender, UIVisibilityChangeEventArgs eventArgs)
        {
            if (CASClothingCategory.gSingleton.mDesignButton.Tag is FadeEffect fade)
            {
                if (eventArgs.Visible)
                {
                    fade.TriggerEffect(false);
                    Simulator.AddObject(new OneShotFunctionTask(() => { CASClothingCategory.gSingleton.mDesignButton.Visible = false; }, StopWatch.TickStyles.Seconds, 0.2f));
                }
                else
                {
                    fade.TriggerEffect(true);
                    CASClothingCategory.gSingleton.mDesignButton.Visible = true;
                }
            }
        }

        private static void CareerButtonFix()
        {
            var mainWindow = UIManager.GetMainWindow();
            var careerButton = mainWindow.GetChildByID(0x05dbc509, true) as Button;
            careerButton.Visible = true;

            MultiDrawable multiDrawable = careerButton.Drawable as MultiDrawable;
            DrawableBase buttonIconDrawable = multiDrawable[1U];
            IconDrawable iconDrawable = buttonIconDrawable as IconDrawable;
            iconDrawable.Image = UIManager.LoadUIImage(ResourceKey.CreatePNGKey("hud_icon_career_r2", 0U));
            iconDrawable.Scale = 0.8f * TinyUIFixForTS3Integration.getUIScale();
            careerButton.Invalidate();
        }
    }
}