using MagicSpells.DataHolders;
using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using static TaleWorlds.Core.ItemObject;

namespace MagicSpells
{
    public static class Utils
    {
        public static EffectData? GetSpellEffectData(Agent agent, string spellName)
        {
            switch (spellName)
            {
                case "Spell Healing Bolt":
                    return new EffectData(agent, "psys_campfire", (affectedAgent) =>
                    {
                        Utils.PrintToMessages("healing bolt healing");
                        Utils.ModAgentHealth(affectedAgent, 10.0f);
                    }, 1.0f);
                case "Spell Healing Aura":
                    return new EffectData(agent, "psys_campfire", (affectedAgent) =>
                    {
                        Utils.PrintToMessages("healing aura healing");
                        Utils.ModAgentHealth(affectedAgent, 10.0f);
                    }, 15.0f, 2.0f);
                case "Spell Mass Healing":
                    return new EffectData(agent, "psys_campfire", (affectedAgent) =>
                    {
                        Utils.PrintToMessages("mass healing");
                    }, 15.0f, 2.0f, 2.0f);
                default:
                    return null;
            }
        }

        public static void ModAgentHealth(Agent agent, float mod)
        {
            if (agent.Health + mod > agent.HealthLimit)
                agent.Health = agent.HealthLimit;
            else
                agent.Health += mod;
        }

        public static bool IsMissionWeaponSpell(MissionWeapon weapon)
        {
            if (weapon.Item.ItemType.Equals(ItemTypeEnum.Thrown) && weapon.Item.Name.ToString().Contains("Spell "))
                return true;
            return false;
        }

        public static bool DoesMissionWeaponHeal(MissionWeapon weapon)
        {
            if (weapon.Item.ItemType.Equals(ItemTypeEnum.Thrown) && weapon.Item.Name.ToString().Contains("Healing"))
                return true;
            return false;
        }

        public static void PrintToMessages(string str, int r, int g, int b)
        {
            float[] newValues = { (float)r / 255.0f, (float)g / 255.0f, (float)b / 255.0f };
            Color col = new(newValues[0], newValues[1], newValues[2]);
            InformationManager.DisplayMessage(new InformationMessage(str, col));
        }
        public static void PrintToMessages(string str)
        {
            InformationManager.DisplayMessage(new InformationMessage(str));
        }

        public static sbyte[] FXBoneIndices = new sbyte[]
        {
            0
        };
    }
}
