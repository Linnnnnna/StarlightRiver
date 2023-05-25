using StarlightRiver.Content.Tiles;
using System.Collections.Generic;
using Terraria.ID;
using Terraria.Localization;

namespace StarlightRiver.Content.NPCs.TownUpgrade
{
	class GuideUpgrade : TownUpgrade
	{//TODO 精简代码
		public GuideUpgrade() : base(Language.GetTextValue("Mods.StarlightRiver.TownNpcQuestThing.Guide.NpcName"), Language.GetTextValue("Mods.StarlightRiver.TownNpcQuestThing.Guide.QuestName"), Language.GetTextValue("Mods.StarlightRiver.TownNpcQuestThing.Guide.QuestTip"), Language.GetTextValue("Mods.StarlightRiver.TownNpcQuestThing.Guide.ButtonName"), Language.GetTextValue("Mods.StarlightRiver.TownNpcQuestThing.Guide.TitleName")) { }

		public override List<Loot> Requirements => new()
		{
			new Loot(ItemID.DirtBlock, 20),
			new Loot(ItemID.Gel, 10),
			new Loot(ItemID.Wood, 50)
		};

		public override void ClickButton()
		{
			Main.NewText(Language.GetTextValue("Mods.StarlightRiver.TownNpcQuestThing.Guide.ClickButton"), Color.Brown);
		}
	}
}