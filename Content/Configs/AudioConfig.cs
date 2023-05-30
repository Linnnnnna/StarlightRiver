using System.ComponentModel;
using Terraria.ModLoader.Config;
using Terraria.Localization;

namespace StarlightRiver.Content.Configs
{
	public enum CustomSounds
	{
		All = 0,
		Specific = 1,
		None = 2
	}

	public class AudioConfig : ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ClientSide;

		[LabelKey("$Mods.StarlightRiver.Configuration.AudioConfig")]
		[DrawTicks]
		[TooltipKey("$Mods.StarlightRiver.Configuration.AudioConfigTooltip")]
		[DefaultValue(typeof(CustomSounds), "All")]
		public CustomSounds InvSounds = CustomSounds.All;
	}
}