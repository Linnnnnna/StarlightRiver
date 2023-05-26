using System.ComponentModel;
using Terraria.ModLoader.Config;
using Terraria.Localization;

namespace StarlightRiver.Content.Configs
{
	public class GraphicsConfig : ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ClientSide;
//TODO 文本无法正常显示
		[Label("$Mods.StarlightRiver.Configs.GraphicsConfig.Screenshake")]
		[Tooltip("$Mods.StarlightRiver.Configs.GraphicsConfig.ScreenshakeTooltip")]
		[Range(0, 1)]
		[Slider]
		[DefaultValue(1f)]
		public float ScreenshakeMult = 1;

		[Label("$Mods.StarlightRiver.Configs.GraphicsConfig.LightingBufferPollRate")]
		[Tooltip("$Mods.StarlightRiver.Configs.GraphicsConfig.LightingBufferPollRateTooltip")]
		[Range(1, 30)]
		[DrawTicks]
		[Slider]
		[DefaultValue(5f)]
		public int LightingPollRate = 5;

		[Label("$Mods.StarlightRiver.Configs.GraphicsConfig.ScrollingLightingBufferBuilding")]
		[Tooltip("$Mods.StarlightRiver.Configs.GraphicsConfig.ScrollingLightingBufferBuildingTooltip")]
		[DefaultValue(false)]
		public bool ScrollingLightingPoll = false;

		[Label("$Mods.StarlightRiver.Configs.GraphicsConfig.ExtraParticles")]
		[Tooltip("$Mods.StarlightRiver.Configs.GraphicsConfig.ExtraParticlesTooltip")]
		[DefaultValue(true)]
		public bool ParticlesActive = true;

		[Label("$Mods.StarlightRiver.Configs.GraphicsConfig.HighQualityLitTextures")]
		[Tooltip("$Mods.StarlightRiver.Configs.GraphicsConfig.HighQualityLitTexturesTooltip")]
		[DefaultValue(true)]
		public bool HighQualityLighting = true;

		[Label("$Mods.StarlightRiver.Configs.GraphicsConfig.BackgroundReflections")]
		[Tooltip("$Mods.StarlightRiver.Configs.GraphicsConfig.BackgroundReflectionsTooltip")]
		public ReflectionSubConfig ReflectionConfig = new();
	}

	public class ReflectionSubConfig
	{
		[Label("$Mods.StarlightRiver.Configs.ReflectionSubConfig.BackgroundReflections")]
		[Tooltip("$Mods.StarlightRiver.Configs.ReflectionSubConfig.BackgroundReflectionsTooltip")]
		[DefaultValue(true)]
		public bool ReflectionsOn = true;

		[Label("$Mods.StarlightRiver.Configs.ReflectionSubConfig.ReflectPlayers")]
		[Tooltip("$Mods.StarlightRiver.Configs.ReflectionSubConfig.ReflectPlayersTooltip")]
		[DefaultValue(true)]
		public bool PlayerReflectionsOn = true;

		[Label("$Mods.StarlightRiver.Configs.ReflectionSubConfig.ReflectNPCs")]
		[Tooltip("$Mods.StarlightRiver.Configs.ReflectionSubConfig.ReflectNPCsTooltip")]
		[DefaultValue(true)]
		public bool NpcReflectionsOn = true;

		[Label("$Mods.StarlightRiver.Configs.ReflectionSubConfig.ReflectProjectiles")]
		[Tooltip("$Mods.StarlightRiver.Configs.ReflectionSubConfig.ReflectProjectilesTooltip")]
		[DefaultValue(true)]
		public bool ProjReflectionsOn = true;

		[Label("$Mods.StarlightRiver.Configs.ReflectionSubConfig.ReflectParticles")]
		[Tooltip("$Mods.StarlightRiver.Configs.ReflectionSubConfig.ReflectParticlesTooltip")]
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