using Terraria.Localization;

namespace StarlightRiver.Content.NPCs.TownUpgrade
{
	class MerchantUpgrade : TownUpgrade
	{
		public MerchantUpgrade() : base(Language.GetTextValue("Mods.StarlightRiver.TownNpcQuestThing.Merchant.NpcName"), Language.GetTextValue("Mods.StarlightRiver.TownNpcQuestThing.Merchant.QuestName"), Language.GetTextValue("Mods.StarlightRiver.TownNpcQuestThing.Merchant.QuestTip"), Language.GetTextValue("Mods.StarlightRiver.TownNpcQuestThing.Merchant.ButtonName"), Language.GetTextValue("Mods.StarlightRiver.TownNpcQuestThing.Merchant.TitleName")) { }

		public override void ClickButton()
		{
			Main.NewText(Language.GetTextValue("Mods.StarlightRiver.TownNpcQuestThing.Merchant.ClickButton"), Color.Brown);
		}
	}
}