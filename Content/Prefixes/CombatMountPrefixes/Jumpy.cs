using StarlightRiver.Core.Systems.CombatMountSystem;
using System.Collections.Generic;
using Terraria.Localization;

namespace StarlightRiver.Content.Prefixes.CombatMountPrefixes
{
	public class Jumpy : CombatMountPrefix
	{
		public override void ApplyToMount(CombatMount mount)
		{
			mount.primarySpeedMultiplier -= 0.15f;
			mount.moveSpeedMultiplier += 0.1f;
		}

		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
		{
			var newline = new TooltipLine(StarlightRiver.Instance, "PrefixTip", Language.GetTextValue("Mods.StarlightRiver.Prefixes.Jumpy.Tip"))
			{
				IsModifier = true
			};

			tooltips.Add(newline);

			newline = new TooltipLine(StarlightRiver.Instance, "PrefixTip2", Language.GetTextValue("Mods.StarlightRiver.Prefixes.Jumpy.Tip2"))
			{
				IsModifier = true
			};

			tooltips.Add(newline);
		}
	}
}