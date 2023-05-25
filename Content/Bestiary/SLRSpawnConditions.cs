using Terraria.Localization;

namespace StarlightRiver.Content.Bestiary
{
	public static class SLRSpawnConditions
	{
		public static ModBiomeSpawnCondition VitricDesert = new(Language.GetTextValue("Mods.StarlightRiver.SLRSpawnConditions.VitricDesert.VitricDesert"), AssetDirectory.Biomes + "VitricDesertIcon", AssetDirectory.MapBackgrounds + "GlassMap", Color.White);

		public static ModBiomeSpawnCondition AuroraSquid = new(Language.GetTextValue("Mods.StarlightRiver.SLRSpawnConditions.AuroraSquid.VitricDesert"), AssetDirectory.Biomes + "AuroraIcon", AssetDirectory.Biomes + "AuroraBG", Color.White);

		public static ModBiomeSpawnCondition Moonstone = new(Language.GetTextValue("Mods.StarlightRiver.SLRSpawnConditions.Moonstone.VitricDesert"), AssetDirectory.Biomes + "MoonstoneIcon", AssetDirectory.Biomes + "MoonstoneBG", Color.White);

		public static void Unload()
		{
			VitricDesert = null;
			AuroraSquid = null;
			Moonstone = null;
		}
	}
}