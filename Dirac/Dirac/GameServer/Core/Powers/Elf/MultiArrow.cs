using System;
using System.Linq;
using System.Collections.Generic;
using Dirac.Logging;
using Dirac.GameServer.Types;
using Dirac.GameServer;
using Dirac.Extensions;
using Dirac.Math;
using Math = Dirac.Math.Vector3;
using Dirac.GameServer.Core;
using Dirac.GameServer.Network;
using Dirac.GameServer.Network.Message;

namespace Dirac.GameServer.Core
{
    public class MultiArrow : SkillContext
    {
        public MultiArrow()
        {

        }
        public override void Run()
        {
            Vector3 origin = this.Player.Position;
            float heigh = 3f;
            origin.y = origin.y + heigh;

            List<Vector3> arrowsDirections = generateVectorsSpectrum(this.VectorDirector, 10f , 3);

            foreach (var arrowdir in arrowsDirections)
            {
                //newVector = new Vector3(this.VectorDirector.x * (float)Math.Cos(i), 0, this.VectorDirector.z * (float)Math.Sin(i));
                Projectile Arrow = new Projectile(this, 60000, origin);
                //newVector = this.VectorDirector.RotatedCopyNorm(i * 10);
                //Arrow.TranslateSpeed = 1f; //default
                Arrow.LaunchToVector(arrowdir);

                Arrow.OnCollision = ((hittedActor) =>
                {
                    if (Arrow.IsAlreadyDestroyed) //check para q no pegue 2 veces con userdata desde el phsx callback antes de ser dispose. interesting.
                        return;

                    Arrow.Destroy();
                    WeaponDamage(hittedActor, DamageType.Physical);
                });
                
            }

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

        public List<Vector3> generateVectorsSpectrum(Vector3 centralDirection, float sightAngle ,int count)
        {
            List<Vector3> retlist = new List<Vector3>();
            /*float angleSteps = (sightAngle / (count - 1));
            for (int i = 0; i < count; i++)
            {
                Vector3 Dir = new Vector3(centralDirection.x, centralDirection.y, centralDirection.z);
                Vector3 newDir = Dir.RotatedCopyNorm(new Degree(-(sightAngle / 2) + i * angleSteps), Vector3.UNIT_Y);
                retlist.Add(newDir);
            }*/
            return retlist;
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
