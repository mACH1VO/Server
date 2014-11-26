using System.Text;
using System;
using System.Collections.Generic;
using Dirac.GameServer.Core;

namespace Dirac.GameServer.Network.Message
{
    public class AvailableSkillListMessage : GameMessage
    {
        public List<SkillOpcode> availableSkillList;
        public AvailableSkillListMessage()
            : base(Opcodes.AvailableSkillListMessage)
        {
            this.Consumer = Consumers.Player;
            this.availableSkillList = new List<SkillOpcode>();
        }

        public override void Parse(GameBitBuffer buffer)
        {
            throw new NotImplementedException();
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(4, this.availableSkillList.Count);

            for (int i = 0; i < this.availableSkillList.Count; i++)
            {
                SkillOpcode skillOpcode = this.availableSkillList[i];
                buffer.WriteInt(32, (int)skillOpcode);
            }
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("AvailableSkillListMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            //b.Append(' ', pad); b.AppendLine("SNOSkill: 0x" + SNOSkill.ToString("X8"));
            //b.Append(' ', pad); b.AppendLine("RuneIndex: 0x" + RuneIndex.ToString("X8") + " (" + RuneIndex + ")");
            //b.Append(' ', pad); b.AppendLine("SkillIndex: 0x" + SkillIndex.ToString("X8") + " (" + SkillIndex + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}
