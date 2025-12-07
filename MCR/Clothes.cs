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
            SetClothesItemgrid();
            SetClothingBackgroundSize();
            SetButtonVisibility();
            MoveDoneButton();
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

        public static void SetClothesItemgrid()
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

        public static void SetButtonVisibility()
        {
            var buttons = new[]
            {
                CASClothingCategory.gSingleton.mTrashButton,
                CASClothingCategory.gSingleton.mSaveButton,
                CASClothingCategory.gSingleton.mShareButton,
                CASClothingCategory.gSingleton.mDesignButton,
            };
            foreach (var button in buttons)
            {
                button.Visible = false;
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
    }
}