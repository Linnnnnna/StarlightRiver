using StarlightRiver.Core.Systems.CombatMountSystem;
using System.Collections.Generic;
using Terraria.Localization;

namespace StarlightRiver.Content.Prefixes.CombatMountPrefixes
{
	public class Feral : CombatMountPrefix
	{
		public override void ApplyToMount(CombatMount mount)
		{
			mount.primarySpeedMultiplier -= 0.35f;
			mount.secondaryCooldownSpeedMultiplier += 0.5f;
		}

		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
		{
			var newline = new TooltipLine(StarlightRiver.Instance, "PrefixTip", Language.GetTextValue("Mods.StarlightRiver.Prefixes.Feral.Tip"))
			{
				IsModifier = true
			};

			tooltips.Add(newline);

			newline = new TooltipLine(StarlightRiver.Instance, "PrefixTip2", Language.GetTextValue("Mods.StarlightRiver.Prefixes.Feral.Tip2"))
			{
				IsModifier = true,
				IsModifierBad = true
			};

			tooltips.Add(newline);
		}
	}
}