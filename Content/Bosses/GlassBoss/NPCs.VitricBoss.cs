﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StarlightRiver.Content.Foregrounds;
using StarlightRiver.Content.GUI;
using StarlightRiver.Content.Items.Vitric;
using StarlightRiver.Core;
using StarlightRiver.Core.Loaders;
using StarlightRiver.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static StarlightRiver.Helpers.Helper;
using static Terraria.ModLoader.ModContent;

namespace StarlightRiver.Content.Bosses.GlassBoss
{
    internal sealed partial class VitricBoss : ModNPC, IDynamicMapIcon
    {
        public Vector2 startPos;
        public Vector2 endPos;
        public Vector2 homePos;
        public List<NPC> crystals = new List<NPC>();
        public List<Vector2> crystalLocations = new List<Vector2>();
        public Rectangle arena;

        public int twistTimer;
        public int maxTwistTimer;
        public int lastTwistState;
        public int twistTarget;
        public int shieldShaderTimer;

        public bool rotationLocked;
        public float lockedRotation;

        private int favoriteCrystal = 0;
        private bool altAttack = false;
        public Color glowColor = Color.Transparent;

        private List<VitricBossEye> eyes;
        private List<VitricBossSwoosh> swooshes;
        private BodyHandler body;

        //Pain handler, possibly move this to a parent class at some point? Kind of a strange thing to parent for
        public float pain;
        public float painDirection;

        public Vector2 PainOffset => Vector2.UnitX.RotatedBy(painDirection) * (pain / 200f * 128);

        internal ref float GlobalTimer => ref npc.ai[0];
        internal ref float Phase => ref npc.ai[1];
        internal ref float AttackPhase => ref npc.ai[2];
        internal ref float AttackTimer => ref npc.ai[3];

        public override string Texture => AssetDirectory.GlassBoss + Name;

        #region tml hooks

        public override bool CheckActive() => Phase == (int)AIStates.Leaving;

        public override void SetStaticDefaults() => DisplayName.SetDefault("Ceiros");

        public override bool Autoload(ref string name)
        {
            BodyHandler.LoadGores();
            return base.Autoload(ref name);
        }

        public override void SetDefaults()
        {
            npc.aiStyle = -1;
            npc.lifeMax = 5000;
            npc.damage = 30;
            npc.defense = 18;
            npc.knockBackResist = 0f;
            npc.width = 80;
            npc.height = 120;
            npc.value = Item.buyPrice(0, 20, 0, 0);
            npc.npcSlots = 15f;
            npc.dontTakeDamage = true;
            npc.friendly = false;
            npc.boss = true;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.dontTakeDamageFromHostiles = true;
            npc.behindTiles = true;

            npc.HitSound = mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/GlassBoss/ceramicimpact");

            music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/GlassBoss1");

            eyes = new List<VitricBossEye>()
            {
            new VitricBossEye(new Vector2(16, 70), 0),
            new VitricBossEye(new Vector2(66, 70), 1)
            };

            swooshes = new List<VitricBossSwoosh>()
            {
            new VitricBossSwoosh(new Vector2(-16, -40), 6, this),
            new VitricBossSwoosh(new Vector2(16, -40), 6, this),
            new VitricBossSwoosh(new Vector2(-46, -34), 10, this),
            new VitricBossSwoosh(new Vector2(46, -34), 10, this)
            };

            body = new BodyHandler(this);
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(7500 * bossLifeScale);
            npc.damage = 40;
            npc.defense = 21;
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return Phase == (int)AIStates.FirstPhase && AttackPhase == 4 && AttackTimer % 240 < 120;
        }

        public override bool CheckDead()
        {
            if (Phase == (int)AIStates.Dying && GlobalTimer >= 659)
            {
                foreach (NPC npc in Main.npc.Where(n => n.modNPC is VitricBackdropLeft || n.modNPC is VitricBossPlatformUp)) npc.active = false; //reset arena
                StarlightWorld.Flag(WorldFlags.GlassBossDowned);
                return true;
            }

            if (Phase == (int)AIStates.SecondPhase || Phase == (int)AIStates.FirstPhase)
            {
                foreach (Player player in Main.player.Where(n => n.Hitbox.Intersects(arena)))
                {
                    player.GetModPlayer<StarlightPlayer>().ScreenMoveTarget = homePos;
                    player.GetModPlayer<StarlightPlayer>().ScreenMoveTime = 720;
                    player.immuneTime = 720;
                    player.immune = true;
                }

                foreach (NPC npc in Main.npc.Where(n => n.modNPC is VitricBackdropLeft || n.modNPC is VitricBossPlatformUp)) 
                    npc.ai[1] = 4;

                ChangePhase(AIStates.Dying, true);
                npc.dontTakeDamage = true;
                npc.life = 1;

                return false;
            }

            if (Phase == (int)AIStates.Dying)
                return true;

            else
                return false;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            swooshes.ForEach(n => n.Draw(spriteBatch));

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.Additive, default, default, default, default, Main.GameViewMatrix.ZoomMatrix);

            swooshes.ForEach(n => n.DrawAdditive(spriteBatch));

            spriteBatch.End();
            spriteBatch.Begin(default, default, default, default, default, default, Main.GameViewMatrix.ZoomMatrix);

            body.DrawBody(spriteBatch);

            npc.frame.Width = 204;
            npc.frame.Height = 190;
            var effects = npc.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : 0;
            spriteBatch.Draw(GetTexture(Texture), npc.Center - Main.screenPosition + PainOffset, npc.frame, new Color(Lighting.GetSubLight(npc.Center)), npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
            spriteBatch.Draw(GetTexture(Texture + "Glow"), npc.Center - Main.screenPosition + PainOffset, npc.frame, Color.White, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);

            return false;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            if (eyes.Any(n => n.Parent == null))
                eyes.ForEach(n => n.Parent = this);

            eyes.ForEach(n => n.Draw(spriteBatch));

            if (Phase == (int)AIStates.FirstPhase && npc.dontTakeDamage) //draws the npc's shield when immune and in the first phase
            {
                Texture2D tex = GetTexture("StarlightRiver/Assets/Bosses/GlassBoss/Shield");
                var effects = npc.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : 0;

                var effect = Terraria.Graphics.Effects.Filters.Scene["MoltenForm"].GetShader().Shader;
                effect.Parameters["sampleTexture2"].SetValue(GetTexture("StarlightRiver/Assets/Bosses/GlassBoss/ShieldMap"));
                effect.Parameters["uTime"].SetValue(2 - (shieldShaderTimer / 120f) * 2);
                effect.Parameters["sourceFrame"].SetValue(new Vector4(npc.frame.X, npc.frame.Y, npc.frame.Width, npc.frame.Height));
                effect.Parameters["texSize"].SetValue(tex.Size());

                spriteBatch.End();
                spriteBatch.Begin(default, BlendState.NonPremultiplied, default, default, default, effect, Main.GameViewMatrix.ZoomMatrix);

                spriteBatch.Draw(tex, npc.Center - Main.screenPosition + PainOffset, npc.frame, Color.White, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);

                spriteBatch.End();
                spriteBatch.Begin(default, default, default, default, default, default, Main.GameViewMatrix.ZoomMatrix);
            }
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            position.Y += 40;

            var spriteBatch = Main.spriteBatch;

            var tex = GetTexture(AssetDirectory.GlassBoss + "VitricBossBarUnder");
            var texOver = GetTexture(AssetDirectory.GlassBoss + "VitricBossBarOver");
            var progress = (float)npc.life / npc.lifeMax;

            Rectangle target = new Rectangle((int)(position.X - Main.screenPosition.X) + 2, (int)(position.Y - Main.screenPosition.Y), (int)(progress * tex.Width - 4), tex.Height);
            Rectangle source = new Rectangle(2, 0, (int)(progress * tex.Width - 4), tex.Height);

            var color = progress > 0.5f ?
                Color.Lerp(Color.Yellow, Color.LimeGreen, progress * 2 - 1) :
                Color.Lerp(Color.Red, Color.Yellow, progress * 2);

            spriteBatch.Draw(tex, position - Main.screenPosition, null, color, 0, tex.Size() / 2, 1, 0, 0);
            spriteBatch.Draw(texOver, target, source, color, 0, tex.Size() / 2, 0, 0);

            return false;
        }

        public override void NPCLoot()
        {
            body.SpawnGores2();

            if (Main.expertMode)
                npc.DropItemInstanced(npc.Center, Vector2.One, ItemType<VitricBossBag>());

            else
            {
                int weapon = Main.rand.Next(4);
                switch (weapon)
                {
                    case 0: Item.NewItem(npc.Center, ItemType<BossSpear>()); break;
                    case 1: Item.NewItem(npc.Center, ItemType<VitricBossBow>()); break;
                    case 3: Item.NewItem(npc.Center, ItemType<Needler>()); break;
                    case 4: Item.NewItem(npc.Center, ItemType<RefractiveBlade>()); break;
                }

                Item.NewItem(npc.Center, ItemType<Items.Vitric.VitricOre>(), Main.rand.Next(30, 50));
                Item.NewItem(npc.Center, ItemType<Items.Misc.StaminaUp>());
            }
        }

        public override void OnHitByItem(Player player, Item item, int damage, float knockback, bool crit)
        {
            if (pain > 0)
                painDirection += Helper.CompareAngle((npc.Center - player.Center).ToRotation(), painDirection) * Math.Min(damage / 200f, 0.5f);
            else
                painDirection = (npc.Center - player.Center).ToRotation();

            pain += damage;

            if (crit)
                pain += 40;
        }

        public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit)
        {
            if (pain > 0)
                painDirection += Helper.CompareAngle((npc.Center - projectile.Center).ToRotation(), painDirection) * Math.Min(damage / 200f, 0.5f);
            else
                painDirection = (npc.Center - projectile.Center).ToRotation();

            pain += damage;

            if (crit)
                pain += 40;
        }

        #endregion tml hooks

        #region helper methods

        //Used for the various differing passive animations of the different forms
        private void SetFrameX(int frame)
        {
            npc.frame.X = npc.frame.Width * frame;
        }

        private void SetFrameY(int frame)
        {
            npc.frame.Y = npc.frame.Height * frame;
        }

        //resets animation and changes phase
        private void ChangePhase(AIStates phase, bool resetTime = false)
        {
            npc.frame.Y = 0;
            Phase = (int)phase;
            if (resetTime) GlobalTimer = 0;
        }

        private int GetTwistDirection(float angle)
		{
            int direction = 0;

            if (angle > 1.57f && angle < 1.57f * 3)
                direction = -1;
            else
                direction = 1;

            if (Math.Abs(angle) > MathHelper.PiOver4 && Math.Abs(angle) < MathHelper.PiOver4 * 3)
                direction = 0;

            return direction;
        }

        private void Twist(int duration)
        {
            int direction = Main.player[npc.target].Center.X > npc.Center.X ? 1 : -1;

            float angle = (Main.player[npc.target].Center - npc.Center).ToRotation();
            if (Math.Abs(angle) > MathHelper.PiOver4 && Math.Abs(angle) < MathHelper.PiOver4 * 3)
                direction = 0;

            Twist(duration, direction);
        }

        private void Twist(int duration, int direction)
        {
            if (direction != lastTwistState)
            {
                twistTimer = 0;
                twistTarget = direction;
                maxTwistTimer = duration;
            }
        }

        #endregion helper methods

        #region AI
        public enum AIStates
        {
            SpawnEffects = 0,
            SpawnAnimation = 1,
            FirstPhase = 2,
            Anger = 3,
            FirstToSecond = 4,
            SecondPhase = 5,
            Leaving = 6,
            Dying = 7
        }

        public override void PostAI()
        {
            //TODO: Remove later, debug only
            if (Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Y)) //Boss Speed Up Key
            {
                for (int k = 0; k < 12; k++)
                    AI();
            }
        }

        public override void AI()
        {
            //Ticks the timer
            GlobalTimer++;
            AttackTimer++;

            //twisting
            if (twistTimer < maxTwistTimer)
                twistTimer++;

            if (twistTimer == maxTwistTimer)
            {
                lastTwistState = twistTarget;
            }

            //pain
            if (pain > 0)
                pain -= pain / 25f;

            pain = (int)MathHelper.Clamp(pain, 0, 100);

            //Main AI
            Lighting.AddLight(npc.Center, new Vector3(1, 0.8f, 0.4f)); //glow

            if (Phase != (int)AIStates.Leaving && arena != new Rectangle() && !Main.player.Any(n => n.active && n.statLife > 0 && n.Hitbox.Intersects(arena))) //if no valid players are detected
            {
                GlobalTimer = 0;
                Phase = (int)AIStates.Leaving; //begone thot!
                crystals.ForEach(n => n.ai[2] = 4);
                crystals.ForEach(n => n.ai[1] = 0);
            }

            switch (Phase)
            {
                //on spawn effects
                case (int)AIStates.SpawnEffects:

                    for (int k = 0; k < Main.maxNPCs; k++) //finds all the large platforms to add them to the list of possible locations for the nuke attack
                    {
                        NPC npc = Main.npc[k];
                        if (npc?.active == true && (npc.type == NPCType<VitricBossPlatformUp>() || npc.type == NPCType<VitricBossPlatformDown>())) crystalLocations.Add(npc.Center + new Vector2(0, -48));
                    }

                    const int arenaWidth = 1280;
                    const int arenaHeight = 884;
                    arena = new Rectangle((int)npc.Center.X + 8 - arenaWidth / 2, (int)npc.Center.Y - 832 - arenaHeight / 2, arenaWidth, arenaHeight);

                    ChangePhase(AIStates.SpawnAnimation, true);
                    break;

                case (int)AIStates.SpawnAnimation: //the animation that plays while the boss is spawning and the title card is shown

                    if (GlobalTimer == 2)
                    {
                        npc.friendly = true; //so he wont kill you during the animation
                        RandomizeTarget(); //pick a random target so the eyes will follow them
                        startPos = npc.Center;
                    }

                    if (GlobalTimer == 2)
                    {
                        StarlightPlayer mp = Main.LocalPlayer.GetModPlayer<StarlightPlayer>();
                        mp.ScreenMoveTarget = npc.Center + new Vector2(0, -600);
                        mp.ScreenMoveTime = 450;
                    }

                    if (GlobalTimer == 70)
                    {
                        ZoomHandler.SetZoomAnimation(1.2f, 60);
                    }

                    if (GlobalTimer == 194)
                    {
                        UILoader.GetUIState<TextCard>().Display(npc.FullName, Main.rand.Next(10000) == 0 ? "Glass tax returns" : "Shattered Sentinel", null, 310, 1.25f); //intro text

                        StarlightPlayer mp = Main.LocalPlayer.GetModPlayer<StarlightPlayer>();
                        mp.Shake += 30;

                        Helper.PlayPitched("GlassBoss/StoneBreak", 1, 0, npc.Center);
                        ZoomHandler.SetZoomAnimation(1, 20);

                        for (int k = 0; k < 10; k++)
                        {
                            Dust.NewDustPerfect(npc.Center, DustType<Dusts.Stone>(), Vector2.UnitY.RotatedByRandom(1) * -Main.rand.NextFloat(20), 0, default, 2);
                        }

                        for (int k = 0; k < 40; k++)
                            Gore.NewGorePerfect(npc.Center, Vector2.UnitY.RotatedByRandom(1) * -Main.rand.NextFloat(20), ModGore.GetGoreSlot(AssetDirectory.GlassBoss + "Gore/Cluster" + Main.rand.Next(1, 20)));

                        Gore.NewGorePerfect(npc.Center + new Vector2(-112, 50), Vector2.Zero, ModGore.GetGoreSlot(AssetDirectory.GlassBoss + "TempleHole"));
                    }

                    if (GlobalTimer > 180 && GlobalTimer <= 260)
                    {
                        float time = (GlobalTimer - 180) / 80f;
                        float progress = (float)(Math.Log(time * 3.6) + Math.E) / 4f;
                        npc.Center = Vector2.Lerp(startPos, startPos + new Vector2(0, -800), progress);
                    }

                    if (GlobalTimer > 340) //summon crystal babies
                        for (int k = 0; k <= 4; k++)
                            if (GlobalTimer == 340 + k * 5)
                            {
                                Vector2 target = new Vector2(npc.Center.X, StarlightWorld.VitricBiome.Top * 16 + 1180);
                                int index = NPC.NewNPC((int)target.X, (int)target.Y, NPCType<VitricBossCrystal>(), 0, 2); //spawn in state 2: sandstone forme
                                (Main.npc[index].modNPC as VitricBossCrystal).Parent = this;
                                (Main.npc[index].modNPC as VitricBossCrystal).StartPos = target;
                                (Main.npc[index].modNPC as VitricBossCrystal).TargetPos = npc.Center + new Vector2(0, -180).RotatedBy(6.28f / 4 * k);
                                crystals.Add(Main.npc[index]); //add this crystal to the list of crystals the boss controls
                            }

                    if (GlobalTimer > 680) //start the fight
                    {
                        GUI.BootlegHealthbar.SetTracked(npc, "Shit!", GetTexture(AssetDirectory.GlassBoss + "GUI/HealthBar"));

                        npc.dontTakeDamage = false; //make him vulnerable
                        npc.friendly = false; //and hurt when touched
                        homePos = npc.Center; //set the NPCs home so it can return here after attacks
                        int index = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, NPCType<ArenaBottom>());
                        (Main.npc[index].modNPC as ArenaBottom).Parent = this;
                        ChangePhase(AIStates.FirstPhase, true);
                        ResetAttack();
                    }

                    DoRotation();

                    break;

                case (int)AIStates.FirstPhase:

                    if (shieldShaderTimer > 0)
                        shieldShaderTimer--;

                    int healthGateAmount = npc.lifeMax / 7;
                    if (npc.life <= npc.lifeMax - (1 + crystals.Count(n => n.ai[0] == 3 || n.ai[0] == 1)) * healthGateAmount && !npc.dontTakeDamage)
                    {
                        shieldShaderTimer = 120;
                        npc.dontTakeDamage = true; //boss is immune at phase gate
                        npc.life = npc.lifeMax - (1 + crystals.Count(n => n.ai[0] == 3 || n.ai[0] == 1)) * healthGateAmount - 1; //set health at phase gate
                        Main.PlaySound(SoundID.ForceRoar, npc.Center);
                    }

                    if (AttackTimer == 1) //switching out attacks
                        if (npc.dontTakeDamage) AttackPhase = 0; //nuke attack once the boss turns immortal for a chance to break a crystal
                        else //otherwise proceed with attacking pattern
                        {
                            AttackPhase++;
                            if (AttackPhase > 4) AttackPhase = 1;
                        }

                    switch (AttackPhase) //Attacks
                    {
                        case 0: NukePlatforms(); break;
                        case 1: CrystalCage(); break;
                        case 2: CrystalSmash(); break;
                        case 3: RandomSpikes(); break;
                        case 4: PlatformDash(); break;
                    }

                    DoRotation();

                    break;

                case (int)AIStates.Anger: //the short anger phase attack when the boss loses a crystal
                    AngerAttack();
                    break;

                case (int)AIStates.FirstToSecond:

                    //Vignette.offset = (npc.Center - Main.LocalPlayer.Center) * 0.9f;
                    //Vignette.extraOpacity = 0.5f + (float)Math.Sin(GlobalTimer / 25f) * 0.2f;

                    rotationLocked = true;
                    lockedRotation = 3.14f;

                    if (GlobalTimer == 2)
                    {
                        foreach (NPC crystal in crystals)
                        {
                            crystal.ai[0] = 3;
                            crystal.ai[2] = 5; //turn the crystals to transform mode
                            (crystal.modNPC as VitricBossCrystal).StartPos = crystal.Center;
                            (crystal.modNPC as VitricBossCrystal).timer = 0;
                        }

                        crystals[0].ai[2] = 6;
                    }

                    if (GlobalTimer == 140)
                    {
                        SetFrameX(1);
                        npc.friendly = true; //so we wont get contact damage

                        StarlightPlayer mp2 = Main.LocalPlayer.GetModPlayer<StarlightPlayer>();
                        mp2.ScreenMoveTarget = arena.Center();
                        mp2.ScreenMoveTime = 540;
                    }

                    if(GlobalTimer > 140 && GlobalTimer < 400)
					{
                        ZoomHandler.AddFlatZoom(0.2f);
                    }

                    if (GlobalTimer > 20 && GlobalTimer < 140)
                    {
                        npc.Center = Vector2.SmoothStep(homePos, homePos + new Vector2(100, -60), (GlobalTimer - 20) / 120f);
                    }

                    if (GlobalTimer > 140 && GlobalTimer <= 170)
                    {
                        SetFrameY(1);
                        SetFrameX((int)((GlobalTimer - 140) / 30f * 4));
                    }

                    if(GlobalTimer > 140 && GlobalTimer < 200)
					{
                        foreach (NPC crystal in crystals)
                        {
                            Dust.NewDustPerfect(crystal.Center + Vector2.One.RotatedByRandom(6.28f) * Main.rand.NextFloat(20), DustType<Dusts.Glow>(), (crystal.Center - npc.Center) * -0.02f, 0, new Color(255, 200, 20), 0.4f);
                        }
                    }

                    if (GlobalTimer > 180 && GlobalTimer < 240)
                    {
                        float progress = (GlobalTimer - 180) / 60f;

                        foreach (NPC crystal in crystals)
                        {
                            var start = (crystal.modNPC as VitricBossCrystal).StartPos;
                            crystal.Center = Vector2.SmoothStep(start, arena.Center(), progress);
                        }
                    }

                    if (GlobalTimer >= 340 && GlobalTimer < 370)
                    {
                        npc.Center = Vector2.SmoothStep(homePos + new Vector2(100, -60), homePos, (GlobalTimer - 340) / 30f);
                    }

                    if(GlobalTimer > 350 && GlobalTimer <= 370)
					{
                        SetFrameY(1);
                        SetFrameX(4 + (int)((GlobalTimer - 350) / 20f * 6));
					}

                    if (GlobalTimer == 359) music = mod.GetSoundSlot(SoundType.Music, "VortexHasASmallPussy"); //handles the music transition
                    if (GlobalTimer == 360) music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/GlassBoss2");

                    if (GlobalTimer == 360)
					{
                        foreach (NPC crystal in crystals) //kill all the crystals
                            crystal.Kill();

                        StarlightPlayer mp2 = Main.LocalPlayer.GetModPlayer<StarlightPlayer>();
                        mp2.Shake += 40;

                        for (int k = 0; k < 40; k++)
                        {
                            Dust.NewDustPerfect(npc.Center, DustType<Dusts.Glow>(), Vector2.One.RotatedByRandom(6.28f) * Main.rand.NextFloat(5), 0, new Color(255, 200, 20), 0.4f);
                            Dust.NewDustPerfect(npc.Center, DustType<Dusts.Glow>(), Vector2.One.RotatedByRandom(6.28f) * Main.rand.NextFloat(2), 0, new Color(255, 100, 20), 0.6f);
                        }

                        swooshes = new List<VitricBossSwoosh>()
                        {
                        new VitricBossSwoosh(new Vector2(-16, -40), 10, this),
                        new VitricBossSwoosh(new Vector2(16, -40), 10, this),
                        new VitricBossSwoosh(new Vector2(-46, -34), 14, this),
                        new VitricBossSwoosh(new Vector2(46, -34), 14, this)
                        };
                    }

                    if (GlobalTimer > 480)
                    {
                        SetFrameX(2);
                        ChangePhase(AIStates.SecondPhase, true); //go on to the next phase
                        ResetAttack(); //reset attack
                        foreach (NPC wall in Main.npc.Where(n => n.modNPC is VitricBackdropLeft)) wall.ai[1] = 3; //make the walls scroll
                        foreach (NPC plat in Main.npc.Where(n => n.modNPC is VitricBossPlatformUp)) plat.ai[0] = 1; //make the platforms scroll

                        Vignette.visible = true;

                        break;
                    }

                    DoRotation();

                    break;

                case (int)AIStates.SecondPhase:

                    Vignette.offset = (npc.Center - Main.LocalPlayer.Center) * 0.8f;
                    Vignette.extraOpacity = 0.3f;

                    if (GlobalTimer == 60)
                    {
                        npc.dontTakeDamage = false; //damagable again
                        npc.friendly = false;
                        Vignette.visible = true;
                    }

                    if (AttackTimer == 1) //switching out attacks
                    {
                        AttackPhase++;
                        if (AttackPhase > 3)
                        {
                            if (!(AttackPhase == 4 && npc.life <= npc.lifeMax / 5)) //at low HP he can laser!
                                AttackPhase = 0;
                        }

                        altAttack = Main.rand.NextBool();
                        npc.netUpdate = true;
                    }

                    switch (AttackPhase) //switch for crystal behavior
                    {
                        case 0: if (altAttack) Darts(); else Volley(); break;
                        case 1: Mines(); break;
                        case 2: Whirl(); break;
                        case 3: Rest(); break;
                        case 4: Laser(); break;
                    }

                    DoRotation();

                    break;

                case (int)AIStates.Leaving:

                    npc.position.Y += 7;
                    Vignette.visible = false;

                    if (GlobalTimer >= 180)
                    {
                        npc.active = false; //leave
                        foreach (NPC npc in Main.npc.Where(n => n.modNPC is VitricBackdropLeft || n.modNPC is VitricBossPlatformUp)) npc.active = false; //arena reset
                    }
                    break;

                case (int)AIStates.Dying:

                    Vignette.offset = Vector2.Zero;
                    Vignette.extraOpacity = 0.5f + Math.Min(GlobalTimer / 60f, 0.5f);

                    if (GlobalTimer == 1)
                        npc.noTileCollide = false;

                    if (GlobalTimer <= 2)
                    {
                        npc.velocity = Vector2.UnitX.RotatedBy(painDirection) * 25;
                        GlobalTimer--;

                        for (int x = -8; x <= 8; x++)
                            for (int y = -8; y <= 8; y++)
                            {
                                Tile tile = Framing.GetTileSafely((int)(npc.Center.X / 16) + x, (int)(npc.Center.Y / 16) + y);

                                if (tile.collisionType != 0)
                                {
                                    GlobalTimer = 2;
                                    npc.velocity = Vector2.UnitX.RotatedBy(painDirection) * -10;
                                    Main.PlaySound(SoundID.DD2_ExplosiveTrapExplode, npc.Center);
                                    Main.LocalPlayer.GetModPlayer<StarlightPlayer>().Shake += 20;
                                    body.SpawnGores();
                                    npc.Kill();
                                    return;
                                }
                            }
                    }

                    if (GlobalTimer == 3)
                        Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/GlassBossDeath"));

                    if (GlobalTimer > 3 && GlobalTimer < 63)
                        Main.musicFade[Main.curMusic] = 1 - (GlobalTimer - 3) / 60f;

                    if(GlobalTimer > 3)
					{
                        npc.velocity *= 0.95f;
                        npc.velocity.Y += 0.2f;
					}

                    if (GlobalTimer == 600)
                    {
                        for (int k = 0; k < 50; k++) 
                            Dust.NewDustPerfect(npc.Center, DustType<Dusts.Glow>(), Vector2.One.RotatedByRandom(6.28f) * Main.rand.NextFloat(20), 0, new Color(255, 150, 50), 0.6f);

                        Vignette.visible = false;
                        npc.Kill();
                    }

                    break;
            }

            body.UpdateBody(); //update the physics on the body, last, so it can override framing
        }

		public override void ResetEffects()
		{
            rotationLocked = false;
        }

		private void DoRotation()
		{
            if (GlobalTimer % 30 == 0)
            {
                if (rotationLocked)
                    Twist(30, GetTwistDirection(lockedRotation));
                else
                    Twist(30);
            }

            if (twistTarget != 0)
            {
                float targetRot = rotationLocked ? lockedRotation : (Main.player[npc.target].Center - npc.Center).ToRotation();
                float speed = 0.07f;

                if (rotationLocked)
                    speed *= 2;

                if (twistTarget == 1)
                    npc.rotation += Helper.CompareAngle(targetRot, npc.rotation) * speed;
                if (twistTarget == -1)
                    npc.rotation += Helper.CompareAngle(targetRot + 3.14f, npc.rotation) * speed;
            }
            else
                npc.rotation = 0;
        }

        #endregion AI

        #region Networking
        public override void SendExtraAI(System.IO.BinaryWriter writer)
        {
            writer.Write(favoriteCrystal);
            writer.Write(altAttack);
        }

        public override void ReceiveExtraAI(System.IO.BinaryReader reader)
        {
            favoriteCrystal = reader.ReadInt32();
            altAttack = reader.ReadBoolean();
        }
        #endregion Networking

        private int IconFrame = 0;
        private int IconFrameCounter = 0;

        public void DrawOnMap(SpriteBatch spriteBatch, Vector2 center, float scale, Color color)
        {
            if (IconFrameCounter++ >= 5) { IconFrame++; IconFrameCounter = 0; }
            if (IconFrame > 3) IconFrame = 0;

            Texture2D tex = GetTexture(Texture + "_Head_Boss");
            spriteBatch.Draw(tex, center, new Rectangle(0, IconFrame * 30, 30, 30), color, npc.rotation, Vector2.One * 15, scale, 0, 0);
        }
    }
}