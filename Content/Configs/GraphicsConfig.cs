using System.ComponentModel;
using Terraria.ModLoader.Config;
using Terraria.Localization;

namespace StarlightRiver.Content.Configs
{
	public class GraphicsConfig : ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ClientSide;
//TODO 无法正常显示
		[Label("$Mods.StarlightRiver.Configs.GraphicsConfig.Label.Screenshake")]
		[Tooltip("$Mods.StarlightRiver.Configs.GraphicsConfig.Tooltip.Screenshake")]
		[Range(0, 1)]
		[Slider]
		[DefaultValue(1f)]
		public float ScreenshakeMult = 1;

		[Label("$Mods.StarlightRiver.Configs.GraphicsConfig.Label.LightingBufferPollRate")]
		[Tooltip("$Mods.StarlightRiver.Configs.GraphicsConfig.Tooltip.LightingBufferPollRate")]
		[Range(1, 30)]
		[DrawTicks]
		[Slider]
		[DefaultValue(5f)]
		public int LightingPollRate = 5;

		[Label("$Mods.StarlightRiver.Configs.GraphicsConfig.Label.ScrollingLightingBufferBuilding")]
		[Tooltip("$Mods.StarlightRiver.Configs.GraphicsConfig.Tooltip.ScrollingLightingBufferBuilding")]
		[DefaultValue(false)]
		public bool ScrollingLightingPoll = false;

		[Label("$Mods.StarlightRiver.Configs.GraphicsConfig.Label.ExtraParticles")]
		[Tooltip("$Mods.StarlightRiver.Configs.GraphicsConfig.Tooltip.ExtraParticles")]
		[DefaultValue(true)]
		public bool ParticlesActive = true;

		[Label("$Mods.StarlightRiver.Configs.GraphicsConfig.Label.HighQualityLitTextures")]
		[Tooltip("$Mods.StarlightRiver.Configs.GraphicsConfig.Tooltip.HighQualityLitTextures")]
		[DefaultValue(true)]
		public bool HighQualityLighting = true;

		[Label("$Mods.StarlightRiver.Configs.GraphicsConfig.Label.BackgroundReflections")]
		[Tooltip("$Mods.StarlightRiver.Configs.GraphicsConfig.Tooltip.BackgroundReflections")]
		public ReflectionSubConfig ReflectionConfig = new();
	}

	public class ReflectionSubConfig
	{
		[Label("$Mods.StarlightRiver.Configs.ReflectionSubConfig.Label.BackgroundReflections")]
		[Tooltip("$Mods.StarlightRiver.Configs.ReflectionSubConfig.Tooltip.BackgroundReflections")]
		[DefaultValue(true)]
		public bool ReflectionsOn = true;

		[Label("$Mods.StarlightRiver.Configs.ReflectionSubConfig.Label.ReflectPlayers")]
		[Tooltip("$Mods.StarlightRiver.Configs.ReflectionSubConfig.Tooltip.ReflectPlayers")]
		[DefaultValue(true)]
		public bool PlayerReflectionsOn = true;

		[Label("$Mods.StarlightRiver.Configs.ReflectionSubConfig.Label.ReflectNPCs")]
		[Tooltip("$Mods.StarlightRiver.Configs.ReflectionSubConfig.Tooltip.ReflectNPCs")]
		[DefaultValue(true)]
		public bool NpcReflectionsOn = true;

		[Label("$Mods.StarlightRiver.Configs.ReflectionSubConfig.Label.ReflectProjectiles")]
		[Tooltip("$Mods.StarlightRiver.Configs.ReflectionSubConfig.Tooltip.ReflectProjectiles")]
		[DefaultValue(true)]
		public bool ProjReflectionsOn = true;

		[Label("$Mods.StarlightRiver.Configs.ReflectionSubConfig.Label.ReflectParticles")]
		[Tooltip("$Mods.StarlightRiver.Configs.ReflectionSubConfig.Tooltip.ReflectParticles")]
		[DefaultValue(true)]
		public bool DustReflectionsOn = true;

		public override bool Equals(object obj)
		{
			if (obj is ReflectionSubConfig other)
			{
				return other.ReflectionsOn == ReflectionsOn
						&& other.PlayerReflectionsOn == PlayerReflectionsOn
						&& other.NpcReflectionsOn == NpcReflectionsOn
						&& other.ProjReflectionsOn == ProjReflectionsOn
						&& other.DustReflectionsOn == DustReflectionsOn;
			}

			return base.Equals(obj);
		}

		/// <summary>
		/// Incase someone disables all the individual reflection components without disabling the entire system, we still want the extra optimization of disabling the entire system
		/// so this checks each indiviudal reflection component and returns true if any are reflecting while the system is on
		/// </summary>
		/// <returns></returns>
		public bool isReflectingAnything()
		{
			return ReflectionsOn && (PlayerReflectionsOn || NpcReflectionsOn || ProjReflectionsOn || DustReflectionsOn);
		}

		public override int GetHashCode()
		{
			return new { ReflectionsOn, PlayerReflectionsOn, NpcReflectionsOn, ProjReflectionsOn, DustReflectionsOn }.GetHashCode();
		}
	}
}