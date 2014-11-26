using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dirac.Math;

namespace Dirac.GameServer.Core
{
    public class MUPath
    {
        private List<Vector3> _points = new List<Vector3>();
        private List<LinearTrajectorie> _linearTrajectories = new List<LinearTrajectorie>();
        private int currentLinearTrajectorieIndex;
        private bool int_for_Trick_call_one_time = true;

        public LinearTrajectorie CurrentLinearTrajectorie;
        public Vector3 CurrDirection { get; set; }

        public float CurrentTotalLen { get; set; }
        public float CurrentLocalLen { get; set; }
        public float TotalLen { get; set; }
        public int LinearTrajectoriesCount { get { return this._linearTrajectories.Count; } }

        public Boolean HasReachedPosition
        {
            get { return this.CurrentTotalLen >= this.TotalLen; }
        }
        public Boolean HasChangeDirectionInLastStep { get; set; }
        public Boolean IsFirstTrajectorieCallOnceTrick
        {
            get
            {
                if (this.int_for_Trick_call_one_time == true)
                {
                    this.int_for_Trick_call_one_time = false;
                    return true;
                }
                return false;
            }
        }

        public MUPath() { }
        public MUPath(List<Vector3> points)
        {
            if (points.Count <= 1)
                throw new Exception("points.Count <= 1 in PATH");

            _points = points;

            for (int i = 0; i < points.Count - 1; i++)
            {
                LinearTrajectorie lt = new LinearTrajectorie(points[i], points[i + 1]);
                this._linearTrajectories.Add(lt);
                this.TotalLen = this.TotalLen + lt.PathLen;
            }
            this.CurrentLinearTrajectorie = this._linearTrajectories[0];
            this.currentLinearTrajectorieIndex = 0;
            this.HasChangeDirectionInLastStep = true; //default
        }


        public Vector3 Advance(long ticks, float translateSpeed)
        {
            if (this.CurrentLinearTrajectorie == null)
                return Vector3.Zero;

            Vector3 retPosition = this.CurrentLinearTrajectorie.Advance(ticks, translateSpeed);
            this.CurrDirection = this.CurrentLinearTrajectorie.Direction;
            if (this.CurrentLinearTrajectorie.HasReachedPosition)
            {
                this.HasChangeDirectionInLastStep = true;
                if (currentLinearTrajectorieIndex + 1 < this._linearTrajectories.Count)
                {
                    //si tiene un trajectrorie adelante, entoncesa avanza al siguiente y le pega el len que se paso
                    float LargoQueSePaso = this.CurrentLinearTrajectorie.CurrentLen - this.CurrentLinearTrajectorie.PathLen;
                    this.CurrentTotalLen = this.CurrentTotalLen + this.CurrentLinearTrajectorie.PathLen + LargoQueSePaso;
                    this.CurrentLinearTrajectorie = this._linearTrajectories[++currentLinearTrajectorieIndex];
                    this.CurrentLinearTrajectorie.CurrentLen = LargoQueSePaso;
                    retPosition = this.CurrentLinearTrajectorie.V0 + this.CurrentLinearTrajectorie.Versor * this.CurrentLinearTrajectorie.CurrentLen;

                }
                else
                {
                    //no trajectorie adelante, llega al final y listo
                    retPosition = this.CurrentLinearTrajectorie.Destination;
                    this.CurrentTotalLen = this.TotalLen;
                }
            }
            else
            {
                this.HasChangeDirectionInLastStep = false;
            }
            return retPosition;
        }

    }
}
