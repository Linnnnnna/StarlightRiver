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
//TODO 文本无法正常显示
		[Label("$Mods.StarlightRiver.Configuration.AudioConfig")]
		[DrawTicks]
		[Tooltip("$Mods.StarlightRiver.Configuration.AudioConfigTooltip")]
		[DefaultValue(typeof(CustomSounds), "All")]
		public CustomSounds InvSounds = CustomSounds.All;
	}
}