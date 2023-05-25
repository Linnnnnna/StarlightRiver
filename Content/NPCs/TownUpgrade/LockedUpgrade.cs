using Terraria.Localization;

namespace StarlightRiver.Content.NPCs.TownUpgrade
{
	class LockedUpgrade : TownUpgrade
	{//TODO 精简代码
		public LockedUpgrade() : base("", "", "", Language.GetTextValue("Mods.StarlightRiver.TownNpcQuestThing.Locked.ButtonName"), "") { }

		public override void ClickButton()
		{
			Main.NewText(Language.GetTextValue("Mods.StarlightRiver.TownNpcQuestThing.Locked.ClickButton"), Color.Red);
		}
	}
}