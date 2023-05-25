using Terraria.Localization;

namespace StarlightRiver.Core.Systems.BossRushSystem
{
	internal class HushArmorSystem : ModSystem
	{
		public static float resistance;

		public static float DPSTarget;

		private static int pollTimer;

		public static int totalDamage;

		public override void UpdateUI(GameTime gameTime)
		{
			if (!BossRushSystem.isBossRush)
				return;

			pollTimer++;

			if (pollTimer % 20 == 0)
			{
				float thisDPS = totalDamage * 3f;
				totalDamage = 0;

				if (thisDPS > DPSTarget && (DPSTarget / thisDPS) < resistance)
					resistance = Helpers.Helper.LerpFloat(resistance, DPSTarget / thisDPS, 0.66f);
				else if (resistance < 1)
					resistance += 0.001f;

				if (!StarlightRiver.debugMode)
					return;

				Main.NewText(Language.GetTextValue("Mods.StarlightRiver.Common.System.BossRushText.Separate"), new Color(200, 200, 200));
				Main.NewText(Language.GetTextValue("Mods.StarlightRiver.Common.System.BossRushText.AdapativeDamage"));
				Main.NewText(Language.GetTextValue("Mods.StarlightRiver.Common.System.BossRushText.CurrentResistance") + resistance);
				Main.NewText(Language.GetTextValue("Mods.StarlightRiver.Common.System.BossRushText.PerfectResistance") + DPSTarget / thisDPS, new Color(200, 255, 255));
				Main.NewText(Language.GetTextValue("Mods.StarlightRiver.Common.System.BossRushText.UnadjustedDPS") + thisDPS, new Color(255, 200, 200));
				Main.NewText(Language.GetTextValue("Mods.StarlightRiver.Common.System.BossRushText.AdjustedDPS") + thisDPS * resistance, new Color(255, 225, 200));
				Main.NewText(Language.GetTextValue("Mods.StarlightRiver.Common.System.BossRushText.DPSTarget") + DPSTarget, new Color(255, 255, 200));
				Main.NewText(Language.GetTextValue("Mods.StarlightRiver.Common.System.BossRushText.CurrentBoss") + BossRushSystem.trackedBossType, new Color(225, 255, 200));
				Main.NewText(Language.GetTextValue("Mods.StarlightRiver.Common.System.BossRushText.CurrentTtage") + BossRushSystem.currentStage, new Color(200, 255, 200));
				Main.NewText(Language.GetTextValue("Mods.StarlightRiver.Common.System.BossRushText.Separate"), new Color(200, 200, 200));
			}
		}
	}

	internal class BossRushNPC : GlobalNPC
	{
		public float storedPartialDamage;

		public override bool InstancePerEntity => true;

		public override void SetDefaults(NPC npc)
		{
			if (!BossRushSystem.isBossRush)
				return;

			npc.SpawnedFromStatue = true; //nothing should drop items in boss rush
		}

		public override void ModifyIncomingHit(NPC npc, ref NPC.HitModifiers modifiers)
		{
			if (!BossRushSystem.isBossRush)
				return;

			modifiers.ModifyHitInfo += (ref NPC.HitInfo n) => AdaptiveDR(ref n, npc);
		}

		private void AdaptiveDR(ref NPC.HitInfo info, NPC npc)
		{
			int damage = (int)(info.Damage * HushArmorSystem.resistance);

			if (damage == 0)
			{
				storedPartialDamage += damage * HushArmorSystem.resistance;
			}

			if (storedPartialDamage >= 1)
			{
				damage = 1;
				storedPartialDamage = 0;
			}

			if (damage == 0)
				npc.life++;

			info.Damage = damage;
		}

		public override void HitEffect(NPC npc, NPC.HitInfo hit)
		{
			if (!BossRushSystem.isBossRush)
				return;

			HushArmorSystem.totalDamage += (int)(hit.Damage * (1 / HushArmorSystem.resistance));
		}
	}
}