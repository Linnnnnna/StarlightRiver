using static Terraria.ModLoader.ModContent;
using Terraria.Localization;

namespace StarlightRiver.Content.Codex.Entries
{
	internal class StarlightWaterEntry : CodexEntry
	{
		public StarlightWaterEntry()
		{
			Category = Categories.Crafting;
			Title = Language.GetTextValue("Mods.StarlightRiver.VitricDesertBiome.Codex.StarlightWaterTitle");
			Body = Language.GetTextValue("Mods.StarlightRiver.VitricDesertBiome.Codex.StarlightWaterBody");
			Image = Request<Texture2D>("StarlightRiver/Assets/Codex/AbilityImageLore").Value;
			Icon = Request<Texture2D>("StarlightRiver/Assets/Codex/StarlightWaterIcon").Value;
		}
	}
}