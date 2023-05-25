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
//TODO 无法正常显示
		[Label("$Mods.StarlightRiver.Configs.AudioConfig.Label")]
		[DrawTicks]
		[Tooltip("$Mods.StarlightRiver.Configs.AudioConfig.Tooltip")]
		[DefaultValue(typeof(CustomSounds), "All")]
		public CustomSounds InvSounds = CustomSounds.All;
	}
}