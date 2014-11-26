using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading;


using Dirac.GameServer.Types;
using Dirac.Store.FileFormats;
using Dirac.GameServer.Core.AI.Brains;
using Dirac.GameServer.Network;
using Dirac.GameServer.Network.Message;
using Dirac.Math;

namespace Dirac.GameServer.Core
{
    public partial class Monster : Actor
    {
        private void updateMovement(TimeSpan elapsed)
        {
            if (!this.IsMoving)
                return;

            if (this.Path == null)
                return;


            Vector3 new_p = this.Path.Advance(elapsed.Ticks, this.TranslateSpeed);


            if (this.Path.IsFirstTrajectorieCallOnceTrick)
            {
                //ojo esto de abajo nose si va.
                ACDTranslateNormalMessage movementMessage = new ACDTranslateNormalMessage
                {
                    ActorId = (int)this.DynamicID,
                    Position = this.Path.CurrentLinearTrajectorie.Destination,
                    Angle = 0,
                    //lookAt = this.Path.CurrentLinearTrajectorie.Destination,
                    TurnImmediately = false,
                    Speed = this.TranslateSpeed, //deberia ser translate speed
                    Field5 = 0,
                };

                this.World.BroadcastIfRevealed(movementMessage, this);
            }
            else
            {
                if (this.Path.HasChangeDirectionInLastStep)
                {
                    ACDTranslateNormalMessage movementMessage = new ACDTranslateNormalMessage
                    {
                        ActorId = (int)this.DynamicID,
                        Position = this.Path.CurrentLinearTrajectorie.Destination,
                        Angle = 0,
                        //lookAt = this.Path.CurrentLinearTrajectorie.Destination,
                        TurnImmediately = false,
                        Speed = this.TranslateSpeed, //deberia ser translate speed
                        Field5 = 0,
                    };

                    this.World.BroadcastIfRevealed(movementMessage, this);
                    //a partir del primer step entra aca
                    //this.RotateTo(Path.CurrDirection);
                }
            }

            this.Position = new_p;

            if (Path.HasReachedPosition)
            {
                this.StopMoving();
            }
        }


    }
}
