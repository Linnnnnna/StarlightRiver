﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.UI;
using static Terraria.ModLoader.ModContent;


namespace StarlightRiver.Helpers
{
	public static partial class Helper
    {
        public static void BoostAllDamage(this Player Player, float damage, int crit = 0)
        {
            Player.meleeDamage += damage;
            Player.rangedDamage += damage;
            Player.magicDamage += damage;
            Player.minionDamage += damage;
            Player.thrownDamage += damage;

            Player.thrownCrit += crit;
            Player.rangedCrit += crit;
            Player.meleeCrit += crit;
            Player.magicCrit += crit;
        }

        public static void NewItemSpecific(Vector2 position, Item Item)
        {
            int targetIndex = 400;
            Main.item[400] = new Item(); //Vanilla seems to make sure to set the dummy here, so I will too.

            if (Main.netMode != NetmodeID.MultiplayerClient) //Main.item index finder from vanilla
            {
                for (int j = 0; j < 400; j++)
                {
                    if (!Main.item[j].active && Main.timeItemSlotCannotBeReusedFor[j] == 0)
                    {
                        targetIndex = j;
                        break;
                    }
                }
            }

            if (targetIndex == 400 && Main.netMode != NetmodeID.MultiplayerClient) //some sort of vanilla failsafe if no safe index is found it seems?
            {
                int num2 = 0;
                for (int k = 0; k < 400; k++)
                {
                    if (Main.item[k].spawnTime - Main.timeItemSlotCannotBeReusedFor[k] > num2)
                    {
                        num2 = Main.item[k].spawnTime - Main.timeItemSlotCannotBeReusedFor[k];
                        targetIndex = k;
                    }
                }
            }

            Main.item[targetIndex] = Item;
            Main.item[targetIndex].position = position;

            if (ItemSlot.Options.HighlightNewItems && Item.type >= ItemID.None && !ItemID.Sets.NeverShiny[Item.type]) //vanilla Item highlight system
            {
                Item.newAndShiny = true;
            }
            if (Main.netMode == NetmodeID.Server) //syncing
            {
                NetMessage.SendData(MessageID.SyncItem, -1, -1, null, targetIndex, 0, 0f, 0f, 0, 0, 0);
                Item.FindOwner(Item.whoAmI);
            }
            else if (Main.netMode == NetmodeID.SinglePlayer)
            {
                Item.playerIndexTheItemIsReservedFor = Main.myPlayer;
            }
        }

        public static Item NewItemPerfect(Vector2 position, Vector2 velocity, int type, int stack = 1, bool noBroadcast = false, int prefixGiven = 0, bool noGrabDelay = false, bool reverseLookup = false)
        {
            int targetIndex = 400;
            Main.item[400] = new Item(); //Vanilla seems to make sure to set the dummy here, so I will too.

            if (Main.netMode != NetmodeID.MultiplayerClient) //Main.item index finder from vanilla
            {
                if (reverseLookup)
                {
                    for (int i = 399; i >= 0; i--)
                    {
                        if (!Main.item[i].active && Main.timeItemSlotCannotBeReusedFor[i] == 0)
                        {
                            targetIndex = i;
                            break;
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < 400; j++)
                    {
                        if (!Main.item[j].active && Main.timeItemSlotCannotBeReusedFor[j] == 0)
                        {
                            targetIndex = j;
                            break;
                        }
                    }
                }
            }
            if (targetIndex == 400 && Main.netMode != NetmodeID.MultiplayerClient) //some sort of vanilla failsafe if no safe index is found it seems?
            {
                int num2 = 0;
                for (int k = 0; k < 400; k++)
                {
                    if (Main.item[k].spawnTime - Main.timeItemSlotCannotBeReusedFor[k] > num2)
                    {
                        num2 = Main.item[k].spawnTime - Main.timeItemSlotCannotBeReusedFor[k];
                        targetIndex = k;
                    }
                }
            }
            Main.timeItemSlotCannotBeReusedFor[targetIndex] = 0; //normal stuff
            Item Item = Main.item[targetIndex];
            Item.SetDefaults(type, false);
            Item.Prefix(prefixGiven);
            Item.position = position;
            Item.velocity = velocity;
            Item.active = true;
            Item.spawnTime = 0;
            Item.stack = stack;

            Item.wet = Collision.WetCollision(Item.position, Item.width, Item.height); //not sure what this is, from vanilla

            if (ItemSlot.Options.HighlightNewItems && Item.type >= ItemID.None && !ItemID.Sets.NeverShiny[Item.type]) //vanilla Item highlight system
            {
                Item.newAndShiny = true;
            }
            if (Main.netMode == NetmodeID.Server && !noBroadcast) //syncing
            {
                NetMessage.SendData(MessageID.SyncItem, -1, -1, null, targetIndex, noGrabDelay ? 1 : 0, 0f, 0f, 0, 0, 0);
                Item.FindOwner(Item.whoAmI);
            }
            else if (Main.netMode == NetmodeID.SinglePlayer)
            {
                Item.playerIndexTheItemIsReservedFor = Main.myPlayer;
            }
            return Item;
        }

        public static Texture2D GetPopupTexture(Item Item)
        {
            if (Item.type < Main.maxItemTypes) return Terraria.GameContent.TextureAssets.Item[Item.type];
            else return Request<Texture2D>(Item.ModItem.Texture).Value;
        }

        public static Texture2D GetPopupTexture(int type)
        {
            if (type < Main.maxItemTypes) return Terraria.GameContent.TextureAssets.Item[type];
            else
            {
                Item Item = new Item();
                Item.SetDefaults(type);
                return Request<Texture2D>(Item.ModItem.Texture).Value;
            }
        }
    }
}

