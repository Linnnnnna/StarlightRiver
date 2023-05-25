using StarlightRiver.Content.Abilities;
using StarlightRiver.Content.Abilities.ForbiddenWinds;
using Terraria.Localization;
using static Terraria.ModLoader.ModContent;

namespace StarlightRiver.Content.Codex.Entries
{
	internal class StaminaEntry : CodexEntry
	{
		public StaminaEntry()
		{
			Category = Categories.Misc;
			Title = Language.GetTextValue("Mods.StarlightRiver.Common.Codex.Mics.StaminaEntryTitle");
			Body = Language.GetTextValue("Mods.StarlightRiver.Common.Codex.Mics.StaminaEntryBody");
			Hint = Language.GetTextValue("Mods.StarlightRiver.Common.Codex.Mics.StaminaEntryHint");
			Image = Request<Texture2D>("StarlightRiver/Assets/GUI/Stamina").Value;
			Icon = Request<Texture2D>("StarlightRiver/Assets/GUI/Stamina").Value;
		}
	}

	internal class StaminaShardEntry : CodexEntry
	{
		public StaminaShardEntry()
		{
			Category = Categories.Misc;
			Title = Language.GetTextValue("Mods.StarlightRiver.Common.Codex.Mics.StaminaShardEntryTitle");
			Body = Language.GetTextValue("Mods.StarlightRiver.Common.Codex.Mics.StaminaShardEntryBody");
			Hint = Language.GetTextValue("Mods.StarlightRiver.Common.Codex.Mics.StaminaShardEntryHint");
			Image = Request<Texture2D>("StarlightRiver/Assets/GUI/StaminaEmpty").Value;
			Icon = Request<Texture2D>("StarlightRiver/Assets/Abilities/Stamina1").Value;
		}
	}

	internal class InfusionEntry : CodexEntry
	{
		public InfusionEntry()
		{
			Category = Categories.Misc;
			Title = Language.GetTextValue("Mods.StarlightRiver.Common.Codex.Mics.InfusionEntryTitle");
			Body = Language.GetTextValue("Mods.StarlightRiver.Common.Codex.Mics.InfusionEntryBody");
			Hint = Language.GetTextValue("Mods.StarlightRiver.Common.Codex.Mics.InfusionEntryHint");
			Image = Request<Texture2D>(GetInstance<Astral>().Texture).Value;
			Icon = Image;
		}
	}

	internal class BarrierEntry : CodexEntry
	{
		public BarrierEntry()
		{
			Category = Categories.Misc;
			Title = Language.GetTextValue("Mods.StarlightRiver.Common.Codex.Mics.BarrierEntryTitle");
			Body = Language.GetTextValue("Mods.StarlightRiver.Common.Codex.Mics.BarrierEntryBody");
			Hint = Language.GetTextValue("Mods.StarlightRiver.Common.Codex.Mics.IBarrierEntryHint");
			Image = Request<Texture2D>(AssetDirectory.GUI + "ShieldHeartOver").Value;
			Icon = Image;
		}
	}
}