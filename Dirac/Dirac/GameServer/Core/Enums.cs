using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dirac.GameServer.Core
{
    public enum SkillSlot : int
    {
        Invalid_slot = -1,
        Primary = 1,
        Secondary = 2,
        _one = 3,
        _two = 4,
        _three = 5,
        _four = 6,
    }

    public enum SkillOpcode : int
    {
        Invalid_Skill = -1,

        Hand = 0,

        //DK
        Uppercut = 1,
        Cyclone = 2,
        Lunge = 3,
        Slash = 4,
        FallingSlash = 5,
        Raid = 6,
        Impale = 7,
        Twisting = 8,
        Inner = 9,
        DeathStab = 10,
        RagefulBlow = 11,
        Strike_of_Destruction = 12,
        Combo = 13,
        CrescentMoonSlash = 14,
        BloodStorm = 15,
        Defense = 16,

        //DW
        EnergyBall = 101,
        FireBall = 102,
        PowerWave = 103,
        Lightning = 104,
        Teleport = 105,
        Meteorite = 106,
        Ice = 107,
        Poison = 108,
        Flame = 109,
        Twister = 110,
        EvilSpirits = 111,
        Hellfire = 112,
        AquaBeam = 103,
        Cometfall = 114,
        Inferno = 115,
        TeleportAlly = 116,
        SoulBarrier = 117,
        IceStorm = 118,
        Decay = 119,
        Nova = 120,
    }
}
