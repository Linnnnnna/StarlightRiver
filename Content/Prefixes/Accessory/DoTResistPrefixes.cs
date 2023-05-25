﻿using System.Collections.Generic;
using Terraria.Localization;

namespace StarlightRiver.Content.Prefixes.Accessory
{
	internal abstract class DoTResistPrefix : CustomTooltipPrefix
	{
		private readonly float power;
		private readonly string name;
		private readonly string tip;

		internal DoTResistPrefix(float power, string name, string tip)
		{
			this.power = power;
			this.name = name;
			this.tip = tip;
		}

		public override bool CanRoll(Item Item)
		{
			return Item.accessory;
		}

		public override PrefixCategory Category => PrefixCategory.Accessory;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault(name);
		}

		public override void ModifyValue(ref float valueMult)
		{
			valueMult *= 1 + 0.05f * power;
		}

		public override void Update(Item Item, Player Player)
		{
			Player.GetModPlayer<DoTResistancePlayer>().DoTResist += power;
		}

		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
		{
			var newline = new TooltipLine(StarlightRiver.Instance, "DotResistTip", tip)
			{
				IsModifier = true
			};

			tooltips.Add(newline);
		}
	}

	internal class DoTResistPrefix1 : DoTResistPrefix
	{//TODO 我不知道
		public DoTResistPrefix1() : base(0.02f, "Healthy", "+2% Inoculation") { }
	}

	internal class DoTResistPrefix2 : DoTResistPrefix
	{
		public DoTResistPrefix2() : base(0.04f, "Protected", "+4% Inoculation") { }
	}

	internal class DoTResistPrefix3 : DoTResistPrefix
	{
		public DoTResistPrefix3() : base(0.05f, "Blessed", "+5% Inoculation") { }
	}
}