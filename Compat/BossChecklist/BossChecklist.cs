using StarlightRiver.Content.Items.Permafrost;
using System.Collections.Generic;
using Terraria.Localization;

namespace StarlightRiver.Compat.BossChecklist
{
	public static class BossChecklistCalls
	{
		public static void CallBossChecklist()
		{
			if (ModLoader.TryGetMod("BossChecklist", out Mod bcl))
			{
				//Auroracle
				var SquidBossCollection = new List<int>();
				string SquidBossInfo = Language.GetTextValue("Mods.StarlightRiver.BossChecklist.BossDescription.Auroracle");
				bcl.Call("AddBoss", StarlightRiver.Instance, "$Mods.StarlightRiver.BossChecklist.BossName.Auroracle", ModContent.NPCType<Content.Bosses.SquidBoss.SquidBoss>(), 2.6f,
					() => StarlightWorld.HasFlag(WorldFlags.SquidBossDowned),
					() => true,
					SquidBossCollection, ModContent.ItemType<SquidBossSpawn>(), SquidBossInfo, "$Mods.StarlightRiver.BossChecklist.BossDisappear.Auroracle", AuroraclePortrait.DrawAuroraclePortrait);

				//Glassweaver
				var vitricMinibossCollection = new List<int>();
				string vitricMinibossInfo = Language.GetTextValue("Mods.StarlightRiver.BossChecklist.BossDescription.Glassweaver");
				bcl.Call("AddMiniBoss", StarlightRiver.Instance, "$Mods.StarlightRiver.BossChecklist.BossName.Glassweaver", ModContent.NPCType<Content.Bosses.GlassMiniboss.Glassweaver>(), 4.8999f,
					() => StarlightWorld.HasFlag(WorldFlags.DesertOpen),
					() => true,
					vitricMinibossCollection, ModContent.ItemType<Content.Items.Vitric.GlassIdol>(), vitricMinibossInfo, "$Mods.StarlightRiver.BossChecklist.BossDisappear.Glassweaver");

				//Ceiros
				var vitricBossCollection = new List<int>()
				{
					ModContent.ItemType<Content.Tiles.Trophies.CeirosTrophyItem>()
				};

				string vitricBossInfo = "$Mods.StarlightRiver.BossChecklist.BossName.Ceiros";
				bcl.Call("AddBoss", StarlightRiver.Instance, "$Mods.StarlightRiver.BossChecklist.BossName.Ceiros", ModContent.NPCType<Content.Bosses.VitricBoss.VitricBoss>(), 4.9f,
					() => StarlightWorld.HasFlag(WorldFlags.VitricBossDowned),
					() => true,
					vitricBossCollection, ModContent.ItemType<Content.Items.Vitric.GlassIdol>(), vitricBossInfo, "$Mods.StarlightRiver.BossChecklist.BossName.Ceiros", CeirosPortrait.DrawCeirosPortrait);

				//OG Boss
				var ogBossCollection = new List<int>();
				string ogBossInfo = "$Mods.StarlightRiver.BossChecklist.BossName.OGBoss";
				bcl.Call("AddBoss", StarlightRiver.Instance, "$Mods.StarlightRiver.BossChecklist.BossName.OGBoss", ModContent.NPCType<Content.NPCs.Overgrow.Crusher>(), 7f,
					() => StarlightWorld.HasFlag(WorldFlags.OvergrowBossDowned),
					() => true,
					ogBossCollection, ModContent.ItemType<Content.Items.Vitric.GlassIdol>(), ogBossInfo, "$Mods.StarlightRiver.BossChecklist.BossName.OGBoss");
			}
		}
	}
}