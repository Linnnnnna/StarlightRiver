using StarlightRiver.Core.Loaders.UILoading;
using System;
using System.Collections.Generic;
using Terraria.UI;
using Terraria.Localization;

namespace StarlightRiver.Content.GUI
{
	public class MasterDeathTicker : SmartUIState
	{
		private static int animationTimer = 480;
		private static string name;
		private static int deaths;
		private static string tease;

		public override bool Visible { get => animationTimer < 480; set => base.Visible = value; }

		public override int InsertionIndex(List<GameInterfaceLayer> layers)
		{
			return layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			animationTimer++;

			var pos = new Vector2(Main.screenWidth / 2, Main.screenHeight / 2 - 120);
			string message = Language.GetTextValue("Mods.StarlightRiver.Common.GUI.MasterDeathTicker.DeathsTo", name) + (animationTimer < 60 ? (deaths - 1) : deaths);

			Color color = new Color(255, 100, 100) * (animationTimer > 420 ? 1 - (animationTimer - 420) / 60f : 1);

			Utils.DrawBorderStringBig(spriteBatch, message, pos, color, 1, 0.5f, 0.5f);

			if (animationTimer > 60 && animationTimer < 120)
			{
				float progress = (animationTimer - 60) / 60f;
				Utils.DrawBorderStringBig(spriteBatch, message, pos, color * (1 - progress), 1 + progress, 0.5f, 0.5f);
			}

			if (tease != "")
				Utils.DrawBorderStringBig(spriteBatch, tease, pos + new Vector2(0, 40), color, 0.6f, 0.5f, 0.5f);
		}

		public static void ShowDeathCounter(string name, int deaths)
		{
			MasterDeathTicker.name = name;
			MasterDeathTicker.deaths = deaths;
			animationTimer = 0;

			tease = "";

			if (deaths % 10 == 0)
			{
				int i = Main.rand.Next(14);
				tease = i switch 
				{
					>= 0 and <= 14 => Language.GetTextValue($"Mods.StarlightRiver.Common.GUI.MasterDeathTicker.DeathsText.{i}"),
					_ => Language.GetTextValue("Mods.StarlightRiver.Common.GUI.MasterDeathTicker.DeathsText.15"),
				};
			}
		}
	}
}