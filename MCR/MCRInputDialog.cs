using Sims3.SimIFace;
using Sims3.UI;
using Arro.Common;
using Sims3.Gameplay.Utilities;

namespace Arro.MCR
{
    public class MCRInputDialog : TwoStringInputDialog
    {
        public bool TempSmoothPatch;
        public bool TempCompactClothes;
        public bool TempCompactAccessories;
        public bool TempAnimation;
        
        public static MCRInputDialog Show(string defaultEntryText, string defaultSecondEntryText, string oKText, string cancelText, Vector2 position,
            bool verifyFilename)
        {
            if (EnableModalDialogs)
            {
                MCRInputDialog mcrDialog = new MCRInputDialog(defaultEntryText, defaultSecondEntryText, oKText, cancelText, position,
                    verifyFilename);
                
                Audio.StartSound("ui_window_drop");
                mcrDialog.StartModal();
                return mcrDialog; 
            }
            return null;
        }

        public MCRInputDialog( string defaultEntryText, string defaultSecondEntryText, string oKText, string cancelText, Vector2 position, bool verifyFilename)
            : base("MCRInputDialog", 4096, "", "", "", defaultEntryText, defaultSecondEntryText, oKText, cancelText, position, verifyFilename)
        {
            if (mModalDialogWindow != null)
            {
                Text header = mModalDialogWindow.GetChildByID(7u, true) as Text;
                Text rowText = mModalDialogWindow.GetChildByID(1u, true) as Text;
                Text columnText = mModalDialogWindow.GetChildByID(6u, true) as Text;
                if (header != null) header.Caption = Localization.LocalizeString("Arro/MCR/ConfigureGrid");
                if (rowText != null) rowText.Caption = Localization.LocalizeString("Arro/MCR/RowCount");
                if (columnText != null) columnText.Caption = Localization.LocalizeString("Arro/MCR/ColumnCount");

                Button button1 = mModalDialogWindow.GetChildByID(0x10000001, true) as Button;
                Button button2 = mModalDialogWindow.GetChildByID(0x10000002, true) as Button;
                Button button3 = mModalDialogWindow.GetChildByID(0x10000003, true) as Button;
                Button button4 = mModalDialogWindow.GetChildByID(0x10000004, true) as Button;
                
                TempSmoothPatch = Config.Data.Clothes.SmoothPatchEnabled;
                TempCompactClothes = Config.Data.Clothes.CompactModeClothesEnabled;
                TempCompactAccessories = Config.Data.Clothes.CompactModeAccessoriesEnabled;
                TempAnimation = Config.Data.Clothes.AnimationEnabled;

                if (button1 != null)
                {
                    button1.Caption = Localization.LocalizeString("Arro/MCR/EnableSmoothpatchFeatures");
                    button1.TooltipText = Localization.LocalizeString("Arro/MCR/EnableSmoothpatchFeaturesTooltip");
                    button1.Click += (s, e) => {
                        TempSmoothPatch = !TempSmoothPatch;
                        RefreshCompactButtons(button1, button2, button3, button4);
                    };
                }

                if (button2 != null)
                {
                    button2.Caption = Localization.LocalizeString("Arro/MCR/CompactModeClothes");
                    button2.TooltipText = Localization.LocalizeString("Arro/MCR/CompactModeClothesTooltip");
                    button2.Click += (s, e) => {
                        if (TempSmoothPatch) {
                            TempCompactClothes = !TempCompactClothes;
                            RefreshCompactButtons(button1, button2, button3, button4);
                        }
                    };
                }

                if (button3 != null)
                {
                    button3.Caption = Localization.LocalizeString("Arro/MCR/CompactModeAccessories");
                    button3.TooltipText = Localization.LocalizeString("Arro/MCR/CompactModeAccessoriesTooltip");
                    button3.Click += (s, e) => {
                        if (TempSmoothPatch) {
                            TempCompactAccessories = !TempCompactAccessories;
                            RefreshCompactButtons(button1, button2, button3, button4);
                        }
                    };
                }
                
                if (button4 != null)
                {
                    button4.Caption = Localization.LocalizeString("Arro/MCR/EnableAnimations");
                    button4.TooltipText = Localization.LocalizeString("Arro/MCR/EnableAnimationsTooltip");
                    button4.Click += (s, e) => {
                        if (TempSmoothPatch) {
                            TempAnimation = !TempAnimation;
                            RefreshCompactButtons(button1, button2, button3, button4);
                        }
                    };
                }
                
                RefreshCompactButtons(button1, button2, button3, button4);
            }
            mSecondEntryTextEdit.MaxTextLength = 2U;
            mEntryTextEdit.MaxTextLength = 2U;
            mEntryTextEdit.TextValidate += TextValidateNumeric;
            mSecondEntryTextEdit.TextValidate += TextValidateNumeric;
            if (mModalDialogWindow != null) mModalDialogWindow.TriggerDown += OnTriggerDown;
        }
        
        private void TextValidateNumeric(WindowBase sender, UITextValidateEventArgs eventArgs)
        {
            if (eventArgs.TextChange.Length != 0 && !int.TryParse(eventArgs.TextChange, out _))
            {
                eventArgs.TextValidated = false;
            }
        }

        private void RefreshCompactButtons(Button b1, Button b2, Button b3, Button b4)
        {
            if (b1 == null) return;
            b1.Selected = TempSmoothPatch;
            
            if (b2 != null)
            {
                b2.Enabled = TempSmoothPatch;
                b2.SetOpacity(TempSmoothPatch ? (byte)255 : (byte)125);
                b2.Selected = TempSmoothPatch && TempCompactClothes;
            }
            
            if (b3 != null)
            {
                b3.Enabled = TempSmoothPatch;
                b3.SetOpacity(TempSmoothPatch ? (byte)255 : (byte)125);
                b3.Selected = TempSmoothPatch && TempCompactAccessories;
            }
            
            if (b4 != null)
            {
                b4.Enabled = TempSmoothPatch;
                b4.SetOpacity(TempSmoothPatch ? (byte)255 : (byte)125);
                b4.Selected = TempSmoothPatch && TempAnimation;
            }
        }
        
        private new void OnTriggerDown(WindowBase sender, UITriggerEventArgs eventArgs)
        {
            switch ((Triggers)eventArgs.TriggerCode)
            {
                case Triggers.kOKTrigger:
                    Audio.StartSound("ui_secondary_button");
                    OnTriggerOk();
                    eventArgs.Handled = true;
                    break;
                case Triggers.kCancelTrigger:
                    Audio.StartSound("ui_secondary_button");
                    OnTriggerCancel();
                    eventArgs.Handled = true;
                    break;
            }
        }
    }
}