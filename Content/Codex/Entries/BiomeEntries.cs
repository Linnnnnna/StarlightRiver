using StarlightRiver.Helpers;
using static Terraria.ModLoader.ModContent;
using Terraria.Localization;

namespace StarlightRiver.Content.Codex.Entries
{
	internal class VitricEntry : CodexEntry
	{
		public VitricEntry()
		{
			Category = Categories.Biomes;
			Title = Language.GetTextValue("Mods.StarlightRiver.VitricDesertBiome.Codex.VitricEntryHint");
			Body = Helper.WrapString("",
				500, Terraria.GameContent.FontAssets.DeathText.Value, 0.8f);
			Hint = Language.GetTextValue("Mods.StarlightRiver.Common.BiomeEntriesHint");
			Image = Request<Texture2D>("StarlightRiver/Assets/Codex/BiomeImageVitric").Value;
			Icon = Request<Texture2D>("StarlightRiver/Assets/Codex/BiomeIconVitric").Value;
		}
	}
}