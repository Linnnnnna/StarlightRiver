﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StarlightRiver.Content.Abilities;
using StarlightRiver.Core;
using StarlightRiver.Content.Items.Gravedigger;
using StarlightRiver.Helpers;

using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.GameContent;

namespace StarlightRiver.Content.Items.Misc
{
	public class FryingPan : ModItem
	{
		public override string Texture => AssetDirectory.MiscItem + Name;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Frying Pan");
			Tooltip.SetDefault("Update this egshels");
		}

		public override void SetDefaults()
		{
			Item.damage = 16;
			Item.DamageType = DamageClass.Melee;
			Item.width = 36;
			Item.height = 44;
			Item.useTime = 12;
			Item.useAnimation = 12;
			Item.reuseDelay = 20;
			Item.channel = true;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 6.5f;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.crit = 4;
			Item.rare = 2;
			Item.shootSpeed = 14f;
			Item.autoReuse = false;
			Item.shoot = ProjectileType<FryingPanProj>();
			Item.noUseGraphic = true;
			Item.noMelee = true;
			Item.autoReuse = false;
		}
	}

	enum CurrentAttack : int
	{
		Down = 0,
		FirstUp = 1,
		Spin = 2,
		SecondUp = 3,
		Crit = 4,
		Reset = 5
	}
	internal class FryingPanProj : ModProjectile
	{
		public override string Texture => AssetDirectory.MiscItem + "FryingPan";

		private CurrentAttack currentAttack = CurrentAttack.Down;

		private bool initialized = false;
		Player owner => Main.player[Projectile.owner];

		private int attackDuration = 0;

		private float startRotation = 0f;

		private float endRotation = 0f;

		private bool facingRight;

		private float zRotation = 0;

		private float rotVel = 0f;

		private int growCounter = 0;

		private Trail trail;
		private List<Vector2> cache;

		private bool FirstTickOfSwing
		{
			get => Projectile.ai[0] == 0;
		}


		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Frying Pan");
			Main.projFrames[Projectile.type] = 1;
		}

		public override void SetDefaults()
		{
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.tileCollide = false;
			Projectile.Size = new Vector2(42, 42);
			Projectile.penetrate = -1;
			Projectile.ownerHitCheck = true;
			Projectile.extraUpdates = 3;
		}

        public override void AI()
        {
			Projectile.velocity = Vector2.Zero;
			Projectile.Center = Main.GetPlayerArmPosition(Projectile);
			if (currentAttack == CurrentAttack.Spin)
            {
				Vector2 spinOffset = Main.GetPlayerArmPosition(Projectile) - owner.Center;
				spinOffset.X *= (float)Math.Cos(zRotation);
				Projectile.Center = owner.Center + spinOffset;
            }
			owner.heldProj = Projectile.whoAmI;

			if (FirstTickOfSwing)
            {
				if (owner.DirectionTo(Main.MouseWorld).X > 0)
					facingRight = true;
				else
					facingRight = false;

				float rot = owner.DirectionTo(Main.MouseWorld).ToRotation();
				if (!initialized)
                {
					initialized = true;
					endRotation = rot - (1f * owner.direction);
                }
				else
                {
					currentAttack = (CurrentAttack)((int)currentAttack + 1);
                }

				startRotation = endRotation;

				switch (currentAttack)
                {
					case CurrentAttack.Down:
						endRotation = rot + (2f * owner.direction);
						attackDuration = 120;
						break;
                    case CurrentAttack.FirstUp:
						endRotation = rot - (2f * owner.direction);
						attackDuration = 120;
						break;
					case CurrentAttack.Spin:
						attackDuration = 140;
						endRotation = rot;
                        break;
                    case CurrentAttack.SecondUp:
						endRotation = rot - (2f * owner.direction);
						attackDuration = 120;
						break;
                    case CurrentAttack.Crit:
						attackDuration = 220;
						endRotation = rot + (7f * owner.direction);
						break;
                    case CurrentAttack.Reset:
						Projectile.active = false;
						break;
                }
            }

			if (Projectile.ai[0] < 1)
            {
				Projectile.timeLeft = 50;
				Projectile.ai[0] += 1f / attackDuration;
				rotVel = Math.Abs(EaseProgress(Projectile.ai[0]) - EaseProgress(Projectile.ai[0] - (1f / attackDuration))) * 2;
			}
			else
            {
				rotVel = 0f;
				if (Main.mouseLeft)
                {
					Projectile.ai[0] = 0;
					return;
                }
            }
			if (currentAttack == CurrentAttack.Spin && Projectile.ai[0] < 1)
			{
				zRotation = 6.28f * EaseFunction.EaseQuadInOut.Ease(Projectile.ai[0]);
				owner.UpdateRotation(zRotation + (facingRight ? 3.14f : 0));
			}
			else
				owner.UpdateRotation(0);

			float progress = EaseProgress(Projectile.ai[0]);

			Projectile.scale = MathHelper.Min(MathHelper.Min(growCounter++ / 30f, 1 + (rotVel * 4)), 1.3f);

			Projectile.rotation = MathHelper.Lerp(startRotation, endRotation, progress);

			owner.ChangeDir(facingRight ? 1 : -1);

			float wrappedRotation = MathHelper.WrapAngle(Projectile.rotation);
			if (facingRight)
				owner.itemRotation = MathHelper.Clamp(wrappedRotation, -1.57f, 1.57f);
			else if (wrappedRotation > 0)
				owner.itemRotation = MathHelper.Clamp(wrappedRotation, 1.57f, 4.71f);
			else
				owner.itemRotation = MathHelper.Clamp(wrappedRotation, -1.57f, -4.71f);
			owner.itemRotation = MathHelper.WrapAngle(owner.itemRotation - (facingRight ? 0 : MathHelper.Pi));
			owner.itemAnimation = owner.itemTime = 5;

			if (Main.netMode != NetmodeID.Server)
			{
				ManageCaches();
				ManageTrail();
			}
		}

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
			if (rotVel < 0.005f)
				return false;
			float collisionPoint = 0f;
			if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + (42 * Projectile.rotation.ToRotationVector2()), 20, ref collisionPoint))
				return true;
			return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
			DrawTrail(Main.spriteBatch);
			Texture2D tex = ModContent.Request<Texture2D>(Texture).Value;

			bool flip = false;
			SpriteEffects effects = SpriteEffects.None;
			if (zRotation > 1.57f && zRotation < 4.71f)
			{
				flip = true;
				effects = facingRight ? SpriteEffects.FlipHorizontally : SpriteEffects.FlipVertically;
			}

			Vector2 origin = new Vector2(0, tex.Height);

			Vector2 scaleVec = Vector2.One;
			if (flip)
            {
				if (facingRight)
				{
					scaleVec.X = (float)Math.Abs(Math.Cos(zRotation));
					origin = new Vector2(tex.Width, tex.Height);
				}
				else
				{
					scaleVec.Y = (float)Math.Abs(Math.Cos(zRotation));
					origin = new Vector2(0, 0);
				}
            }
			Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation + 0.78f, origin, Projectile.scale * scaleVec, effects, 0f);
			return false;
        }

		private void ManageCaches()
        {
			Vector2 off = Projectile.rotation.ToRotationVector2() * 35;
			off.X *= (float)Math.Cos(zRotation);
			if (cache == null)
			{
				cache = new List<Vector2>();

				for (int i = 0; i < 60; i++)
				{
					cache.Add(Projectile.Center + off);
				}
			}

			cache.Add(Projectile.Center + off);

			while (cache.Count > 60)
			{
				cache.RemoveAt(0);
			}
		}
		private void ManageTrail()
        {
			Vector2 off = (Projectile.rotation + rotVel).ToRotationVector2() * 35;
			off.X *= (float)Math.Cos(zRotation);

			trail = trail ?? new Trail(Main.instance.GraphicsDevice, 60, new TriangularTip(4), factor => 12, factor =>
			{
				Color trailColor = Color.DarkGray * MathHelper.Min(rotVel * 18, 0.75f);
				return trailColor;
			});

			trail.Positions = cache.ToArray();
			trail.NextPosition = Projectile.Center + off;
		}

		private void DrawTrail(SpriteBatch spriteBatch)
		{
			spriteBatch.End();
			Effect effect = Filters.Scene["CoachBombTrail"].GetShader().Shader;

			Matrix world = Matrix.CreateTranslation(-Main.screenPosition.Vec3());
			Matrix view = Main.GameViewMatrix.ZoomMatrix;
			Matrix projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, -1, 1);

			effect.Parameters["transformMatrix"].SetValue(world * view * projection);
			effect.Parameters["sampleTexture"].SetValue(ModContent.Request<Texture2D>("StarlightRiver/Assets/MotionTrail").Value);

			trail?.Render(effect);

			spriteBatch.Begin(default, default, default, default, default, default, Main.GameViewMatrix.TransformationMatrix);
		}

		private float EaseProgress(float input)
        {
			switch (currentAttack)
            {
				case CurrentAttack.Down:
					return EaseFunction.EaseCircularInOut.Ease(input);
				case CurrentAttack.FirstUp:
					return EaseFunction.EaseCircularInOut.Ease(input);
				case CurrentAttack.Spin:
					return input;
				case CurrentAttack.SecondUp:
					return EaseFunction.EaseCircularInOut.Ease(input);
				case CurrentAttack.Crit:
					return EaseFunction.EaseCircularInOut.Ease(input);
				default:
					return input;
            }
        }
    }
}