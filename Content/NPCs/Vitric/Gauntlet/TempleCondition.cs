using StarlightRiver.Content.Biomes;
using Terraria.Localization;
using Terraria.GameContent.ItemDropRules;

namespace StarlightRiver.Content.NPCs.Vitric.Gauntlet
{
	internal class TempleCondition : IItemDropRuleCondition
	{
		public bool CanDrop(DropAttemptInfo info)
		{
			return info.player.InModBiome<VitricTempleBiome>();
		}

		public bool CanShowItemDropInUI()
		{
			return true;
		}

		public string GetConditionDescription()
		{
			return Language.GetTextValue("Mods.StarlightRiver.Condition.TempleCondition");
		}
	}
}