using StarlightRiver.Content.GUI;
using StarlightRiver.Core.Loaders.UILoading;
using Terraria.Localization;

namespace StarlightRiver.Content.CustomHooks
{
	class PrehardmodeWarning : HookGroup
	{
		public override void Load()
		{
			On_WorldGen.StartHardmode += WorldGen_StartHardmode;
		}

		private void WorldGen_StartHardmode(On_WorldGen.orig_StartHardmode orig)
		{
			orig();
			UILoader.GetUIState<MessageBox>().Display(Language.GetTextValue("Mods.StarlightRiver.Common.CustomHooksText.PrehardmodeWarning.Text1"), Language.GetTextValue("Mods.StarlightRiver.Common.CustomHooksText.PrehardmodeWarning.Text2"));
		}
	}
}