﻿using System.ComponentModel;
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

		[LabelKey("$Mods.StarlightRiver.Configs.GUIConfig.OverheadStaminaDisplay")]
		[DrawTicks]
		[TooltipKey("$Mods.StarlightRiver.Configs.GUIConfig.OverheadStaminaDisplayTooltip")]
		[DefaultValue(typeof(OverlayState), "WhileNotFull")]
		public OverlayState OverheadStaminaState = OverlayState.WhileNotFull;

		[LabelKey("$Mods.StarlightRiver.Configs.GUIConfig.KeywordStyle")]
		[DrawTicks]
		[TooltipKey("$Mods.StarlightRiver.Configs.GUIConfig.KeywordStyleTooltip")]
		public KeywordStyle KeywordStyle = KeywordStyle.Both;
	}
}