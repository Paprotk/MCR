using Sims3.SimIFace;
using Sims3.UI;
using Sims3.UI.CAS;

namespace Arro.MCR
{
	public class ClothingPerformanceHookTask : Task
	{
		public override void Simulate()
		{
			if (Responder.Instance.InCasMode)
			{
				ClothingPerformance.Tick();
				if (CASController.gSingleton != null)
				{
					if (!this.hookedCASController)
					{
						ClothingPerformanceHookTask.HookDelegate cascontrollerHook = ClothingPerformanceHookTask.CASControllerHook;
						if (cascontrollerHook != null)
						{
							cascontrollerHook();
						}
						this.hookedCASController = true;
					}
				}
				else if (this.hookedCASController)
				{
					ClothingPerformanceHookTask.HookDelegate cascontrollerUnhook = ClothingPerformanceHookTask.CASControllerUnhook;
					if (cascontrollerUnhook != null)
					{
						cascontrollerUnhook();
					}
					this.hookedCASController = false;
				}
				if (CASDresserClothing.gSingleton != null)
				{
					if (!this.hookedCASDresserClothing)
					{
						ClothingPerformanceHookTask.HookDelegate casdresserClothingHook = ClothingPerformanceHookTask.CASDresserClothingHook;
						if (casdresserClothingHook != null)
						{
							casdresserClothingHook();
						}
						this.hookedCASDresserClothing = true;
					}
				}
				else if (this.hookedCASDresserClothing)
				{
					ClothingPerformanceHookTask.HookDelegate casdresserClothingUnhook = ClothingPerformanceHookTask.CASDresserClothingUnhook;
					if (casdresserClothingUnhook != null)
					{
						casdresserClothingUnhook();
					}
					this.hookedCASDresserClothing = false;
				}
				if (CASClothing.gSingleton != null)
				{
					if (!this.hookedCASClothing)
					{
						ClothingPerformanceHookTask.HookDelegate casclothingHook = ClothingPerformanceHookTask.CASClothingHook;
						if (casclothingHook != null)
						{
							casclothingHook();
						}
						this.hookedCASClothing = true;
					}
				}
				else if (this.hookedCASClothing)
				{
					ClothingPerformanceHookTask.HookDelegate casclothingUnhook = ClothingPerformanceHookTask.CASClothingUnhook;
					if (casclothingUnhook != null)
					{
						casclothingUnhook();
					}
					this.hookedCASClothing = false;
				}
				CASClothingCategory gSingleton = CASClothingCategory.gSingleton;
				if (gSingleton != null)
				{
					if (!this.hookedCASClothingCategory)
					{
						ClothingPerformanceHookTask.HookDelegate casclothingCategoryHook = ClothingPerformanceHookTask.CASClothingCategoryHook;
						if (casclothingCategoryHook != null)
						{
							casclothingCategoryHook();
						}
						this.hookedCASClothingCategory = true;
					}
					if (gSingleton.mClothingTypesGrid != null && (gSingleton.mClothingTypesGrid.mPopulateCallback != null || gSingleton.mClothingTypesGrid.mPopulateContext != null || gSingleton.mClothingTypesGrid.mPopulating || gSingleton.mClothingTypesGrid.mPopulateStride > 0))
					{
						gSingleton.mSortButton.Enabled = true;
						gSingleton.mClothingTypesGrid.mPopulating = false;
						gSingleton.mClothingTypesGrid.mPopulateCallback = null;
						gSingleton.mClothingTypesGrid.AbortPopulating();
						ClothingPerformance.HookedSetTypeCategory(CASClothingCategory.CurrentTypeCategory, false);
						return;
					}
				}
				else if (this.hookedCASClothingCategory)
				{
					ClothingPerformanceHookTask.HookDelegate casclothingCategoryUnhook = ClothingPerformanceHookTask.CASClothingCategoryUnhook;
					if (casclothingCategoryUnhook != null)
					{
						casclothingCategoryUnhook();
					}
					this.hookedCASClothingCategory = false;
				}
			}
		}
		
		private bool hookedCASClothingCategory;
		
		private bool hookedCASDresserClothing;
		
		private bool hookedCASClothing;
		
		private bool hookedCASController;

		// Token: 0x04000020 RID: 32
		public static ClothingPerformanceHookTask.HookDelegate nraasDelegate;

		// Token: 0x04000021 RID: 33
		public static ClothingPerformanceHookTask.HookDelegate CASClothingCategoryHook = new ClothingPerformanceHookTask.HookDelegate(ClothingPerformance.CASClothingCategoryHook);

		// Token: 0x04000022 RID: 34
		public static ClothingPerformanceHookTask.HookDelegate CASClothingCategoryUnhook = new ClothingPerformanceHookTask.HookDelegate(ClothingPerformance.CASClothingCategoryUnHook);

		// Token: 0x04000023 RID: 35
		public static ClothingPerformanceHookTask.HookDelegate CASDresserClothingHook = new ClothingPerformanceHookTask.HookDelegate(ClothingPerformance.CASDresserClothingHook);

		// Token: 0x04000024 RID: 36
		public static ClothingPerformanceHookTask.HookDelegate CASDresserClothingUnhook;

		// Token: 0x04000025 RID: 37
		public static ClothingPerformanceHookTask.HookDelegate CASClothingHook = new ClothingPerformanceHookTask.HookDelegate(ClothingPerformance.CASClothingHook);

		// Token: 0x04000026 RID: 38
		public static ClothingPerformanceHookTask.HookDelegate CASClothingUnhook = new ClothingPerformanceHookTask.HookDelegate(ClothingPerformance.CASClothingUnhook);

		// Token: 0x04000027 RID: 39
		public static ClothingPerformanceHookTask.HookDelegate CASControllerHook = new ClothingPerformanceHookTask.HookDelegate(ClothingPerformance.CASControllerHook);

		// Token: 0x04000028 RID: 40
		public static ClothingPerformanceHookTask.HookDelegate CASControllerUnhook;

		// Token: 0x02000018 RID: 24
		// (Invoke) Token: 0x0600005D RID: 93
		public delegate void HookDelegate();
	}
}
