using System;
using System.Linq;
using Dirac.Logging;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using Dirac.GameServer.Network;
using Dirac.GameServer.Network.Message;

namespace Dirac.GameServer.Core
{
    public class PowerSet
    {
        public Dictionary<SkillSlot, SkillOpcode> SkillSlots { get; set; } 

        public PowerSet(Player player)
        {
            this.SkillSlots = new Dictionary<SkillSlot, SkillOpcode>();

            //load from DB
            this.SkillSlots.Add(SkillSlot.Primary, SkillOpcode.Hand);
            this.SkillSlots.Add(SkillSlot.Secondary, SkillOpcode.Hand);

            this.SkillSlots.Add(SkillSlot._one, SkillOpcode.Hand);
            this.SkillSlots.Add(SkillSlot._two, SkillOpcode.Hand);
            this.SkillSlots.Add(SkillSlot._three, SkillOpcode.Hand);
            this.SkillSlots.Add(SkillSlot._four, SkillOpcode.Hand);


            /*if (this.ActivePowers.ContainsKey(PowerSlot.Power_1))
                this.HotbarButtonData[0].SNOPower = (int)this.ActivePowers[PowerSlot.Power_1];
            else
                this.HotbarButtonData[0].SNOPower = (int)PowerOpcode.Transparent;

            if (this.ActivePowers.ContainsKey(PowerSlot.Power_2))
                this.HotbarButtonData[1].SNOPower = (int)this.ActivePowers[PowerSlot.Power_2];
            else
                this.HotbarButtonData[1].SNOPower = (int)PowerOpcode.Transparent;

            if (this.ActivePowers.ContainsKey(PowerSlot.Power_3))
                this.HotbarButtonData[2].SNOPower = (int)this.ActivePowers[PowerSlot.Power_3];
            else
                this.HotbarButtonData[2].SNOPower = (int)PowerOpcode.Transparent;

            if (this.ActivePowers.ContainsKey(PowerSlot.Power_4))
                this.HotbarButtonData[3].SNOPower = (int)this.ActivePowers[PowerSlot.Power_4];
            else
                this.HotbarButtonData[3].SNOPower = (int)PowerOpcode.Transparent;

            if (this.ActivePowers.ContainsKey(PowerSlot.Primary))
                this.HotbarButtonData[4].SNOPower = (int)this.ActivePowers[PowerSlot.Primary];
            else
                this.HotbarButtonData[4].SNOPower = (int)PowerOpcode.Transparent;

            if (this.ActivePowers.ContainsKey(PowerSlot.Secondary))
                this.HotbarButtonData[5].SNOPower = (int)this.ActivePowers[PowerSlot.Secondary];
            else
                this.HotbarButtonData[5].SNOPower = (int)PowerOpcode.Transparent;*/
        }

        public void SwitchUpdatePower(int oldSNOPower, int SNOPower, int SNORune)
        {
            /*for (int i = 0; i < this.HotbarButtonData.Length; i++)
            {
                if (this.HotbarButtonData[i].SNOPower == oldSNOPower)
                {
                    Logging.LogManager.DefaultLogger.Trace("PowerSet: SwitchUpdatePower OldPower {0} NewPower {1}", oldSNOPower, SNOPower);
                    this.HotbarButtonData[i].SNOPower = SNOPower;
                    return;
                }
            }*/
        }

        public void UpdateSkills(int hotBarIndex, int SNOSkill, int SNORune)
        {
            Logging.LogManager.DefaultLogger.Trace("Update index {0} power {1} rune {2}", hotBarIndex, SNOSkill, SNORune);
            //save to db inmediatly?
        }

        

        
    }

    
}
