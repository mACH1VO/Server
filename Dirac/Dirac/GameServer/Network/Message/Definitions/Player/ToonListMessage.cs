using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dirac.GameServer.Network.Message
{
    public class ToonListMessage : GameMessage
    {
        public ToonData[] ToonList;

        public ToonListMessage() : base(Opcodes.ToonListMessage, Consumers.Login) { }

        public override void Parse(GameBitBuffer buffer)
        {
            this.ToonList = new ToonData[5];
            for (int i = 0; i < this.ToonList.Length; i++)
            {
                this.ToonList[i] = new ToonData();
                this.ToonList[i].Parse(buffer);
            }
        }

        public override void Encode(GameBitBuffer buffer)
        {
            for (int i = 0; i < this.ToonList.Length; i++)
            {
                this.ToonList[i].Encode(buffer);
            }
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("ToonListMessage:");
            b.Append(' ', pad++);
            b.Append(' ', pad);
            b.AppendLine("{");
            for (int i = 0; i < this.ToonList.Length; i++)
            {
                this.ToonList[i].AsText(b, pad + 1);
                b.AppendLine();
            }
            b.Append(' ', pad);
            b.AppendLine("}");
            b.AppendLine();
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}
