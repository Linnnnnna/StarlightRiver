using static Terraria.ModLoader.ModContent;
using Terraria.Localization;

namespace StarlightRiver.Content.Codex.Entries
{
	internal class PlaceholderRelicEntry : CodexEntry
	{
		public PlaceholderRelicEntry()
		{
			Category = Categories.Relics;
			Title = Language.GetTextValue("Mods.StarlightRiver.Common.Codex.PlaceholderRelicEntryTitle");
			Body = Language.GetTextValue("Mods.StarlightRiver.Common.Codex.PlaceholderRelicEntryBody");
			Hint = Language.GetTextValue("Mods.StarlightRiver.Common.Codex.PlaceholderRelicEntryHint");
			Image = Request<Texture2D>("StarlightRiver/Assets/Codex/AbilityImageLore").Value;
			Icon = Request<Texture2D>("StarlightRiver/Assets/Codex/StarlightWaterIcon").Value;
		}
	}
}