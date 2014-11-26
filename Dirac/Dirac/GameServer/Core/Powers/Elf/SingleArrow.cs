using System;
using System.Linq;
using System.Collections.Generic;
using Dirac.Logging;

using Dirac.GameServer.Types;
using Dirac.GameServer;
using Dirac.Extensions;
using Dirac.Math;

namespace Dirac.GameServer.Core
{
    public class SimpleArrow : SkillContext
    {
        public SimpleArrow()
        {
        }

        public override void Run()
        {
            Vector3 origin = this.Player.Position;
            origin.y = origin.y + 4f;

            Vector3 vectorDirector = (this.DestinationUserClick - origin).NormalizedCopy;
            vectorDirector.y = 0;
            origin = origin + vectorDirector * 3; //que salga de la punta del bow y no del centro del char.


            //List<Vector3> arrowsDirections = generateVectorsSpectrum(this.VectorDirector, 10f , 3);

            //newVector = new Vector3(this.VectorDirector.x * (float)Math.Cos(i), 0, this.VectorDirector.z * (float)Math.Sin(i));
            Projectile Arrow = new Projectile(this, 60000, origin);
            //newVector = this.VectorDirector.RotatedCopyNorm(i * 10);

            Arrow.TranslateSpeed = 1f;
            Arrow.TimeOut = 2000;

            Arrow.LaunchToPosition(this.DestinationUserClick);

            /*Executor.Execute(500, () =>
            {
                Arrow.LaunchToPosition(this.DestinationUserClick); //delay de 1seg para q coincida con el movimiento del bow q tira la flecha
            });*/

            Arrow.OnCollision = ((hittedActor) =>
            {
                if (hittedActor.ActorType != ActorType.Monster)
                    return;

                if (Arrow.IsAlreadyDestroyed) //check para q no pegue 2 veces con userdata desde el phsx callback antes de ser dispose. interesting.
                    return;
                /*this.Player.World.BroadcastIfRevealed(new PlayEffectMessage()
                {
                    ActorId = Arrow.DynamicID,
                    EffectOpcode = EffectOpcode.IceArrowExplosion, //ribbon trail
                    Position = Arrow.Position
                }, this.Player);*/
                //Logging.LogManager.DefaultLogger.TraceWithColor(System.Drawing.Color.Black, "Projectile Hitted " + hittedActor.ToString());
                Arrow.Destroy();
                this.WeaponDamage(hittedActor, DamageType.Physical);
            });

            /*this.Player.World.BroadcastIfRevealed(new PlayEffectMessage()
            {
                ActorId = Arrow.DynamicID,
                EffectOpcode = EffectOpcode.SimpleArrow, //ribbon trail
            }, this.Player);*/

            



            /*Projectile Arrow2 = new Projectile(this, 60000, origin);
            Quaternion q = Quaternion.IDENTITY;
            Vector3 dir = this.VectorDirector;
            Arrow2.LaunchDefaultVector();
            Arrow2.OnCollision = ((hit) =>
            {
                if (Arrow2.IsAlreadyDestroyed) //check para q no pegue 2 veces con userdata desde el phsx callback antes de ser dispose. interesting.
                    return;
                Arrow2.Destroy();
                WeaponDamage(hit, DamageType.Physical);
            });*/

        }

        public void OnCollision()
        {
            /*SpawnEffect(99572, new Vector3D(hit.Position.X, hit.Position.Y, hit.Position.Z + 5f)); // impact effect (fix height)
            WeaponDamage(hit, ScriptFormula(1), DamageType.Arcane);

            if (Rune_D > 0)
            {
                GeneratePrimaryResource(ScriptFormula(16));
            }

            if (Rune_C > 0)
            {
                if (Rand.NextDouble() < ScriptFormula(12))
                {
                    //this is actually how i think it should work, pierce first target, if addition targets behind enemy, will continue to do damage to them as well.
                }
                projectile.Destroy();
            }
            else
                projectile.Destroy();*/

        }
    }
}
