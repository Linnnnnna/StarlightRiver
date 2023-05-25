using System.ComponentModel;
using Terraria.ModLoader.Config;
using Terraria.Localization;

namespace StarlightRiver.Content.Configs
{
	public enum OverlayState
	{
		AlwaysOn = 0,
		WhileNotFull = 1,
		WhileUsing = 2,
		Never = 3
	}

	public enum KeywordStyle
	{
		Colors = 0,
		Brackets = 1,
		Both = 2,
		Neither = 3
	}

	public class GUIConfig : ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ClientSide;
//TODO 无法正常显示
		[Label("$Mods.StarlightRiver.Configs.GUIConfig.Label.OverheadStaminaDisplay")]
		[DrawTicks]
		[Tooltip("$Mods.StarlightRiver.Configs.GUIConfig.Tooltip.OverheadStaminaDisplay")]
		[DefaultValue(typeof(OverlayState), "WhileNotFull")]
		public OverlayState OverheadStaminaState = OverlayState.WhileNotFull;

		[Label("$Mods.StarlightRiver.Configs.GUIConfig.Label.KeywordStyle")]
		[DrawTicks]
		[Tooltip("$Mods.StarlightRiver.Configs.GUIConfig.Tooltip.KeywordStyle")]
		public KeywordStyle KeywordStyle = KeywordStyle.Both;
	}
}