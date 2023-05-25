using static Terraria.ModLoader.ModContent;
using Terraria.Localization;
using Terraria.ModLoader;

namespace StarlightRiver.Content.Codex.Entries
{
	internal class LoreEntry : CodexEntry
	{
		public LoreEntry()
		{
			Category = Categories.Abilities;
			Title = Language.GetTextValue("Mods.StarlightRiver.Common.Codex.Abilities.StarlightCodexTitle");
			Body = Language.GetTextValue("Mods.StarlightRiver.Common.Codex.Abilities.StarlightCodexBody");
			Image = Request<Texture2D>("StarlightRiver/Assets/Codex/AbilityImageLore").Value;
			Icon = Request<Texture2D>("StarlightRiver/Assets/GUI/Book1Closed").Value;
		}
	}

	internal class WindsEntry : CodexEntry
	{
		public WindsEntry()
		{
			Category = Categories.Abilities;
			Title = Language.GetTextValue("Mods.StarlightRiver.Keybinds.ForbiddenWinds.DisplayName");
			Body = Language.GetTextValue("Mods.StarlightRiver.Common.Codex.Abilities.ForbiddenWindsBody");
			Hint = Language.GetTextValue("Mods.StarlightRiver.Common.Codex.Abilities.ForbiddenWindsHint");
			Image = Request<Texture2D>(AssetDirectory.Debug).Value;
			Icon = Request<Texture2D>("StarlightRiver/Assets/Abilities/ForbiddenWinds").Value;
		}
	}
}