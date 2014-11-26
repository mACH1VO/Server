using System;
using Dirac.GameServer.Core;


namespace Dirac.GameServer.Core.AI.Actions
{
	/// <summary>
	/// Abstract atomary action of AI
	/// </summary>
	public abstract class AIAction : IUpdateable
    {
        #region Private Members
        protected Monster m_owner;
        protected Actor _target;
        #endregion

        protected AIAction(Monster owner)
		{
            UpdateTimerDelay = 500;
            Delay = new TickTimer(UpdateTimerDelay);
            m_owner = owner;
		}

        public TickTimer Delay { get; private set; }

        public int UpdateTimerDelay { get; private set; }

		/// <summary>
		/// Owner of the action
		/// </summary>
        public Monster Owner
		{
            get { return m_owner; }
		}

        /// <summary>
        /// Target if any
        /// </summary>
        public Actor Target
        {
            get { return _target; }
            set { _target = value; }
        }

		/// <summary>
		/// Start a new Action
		/// </summary>
		/// <returns></returns>
		public abstract void Start();

		/// <summary>
		/// Update
		/// </summary>
		/// <returns></returns>
		public abstract void Update(TimeSpan elapsed);

		/// <summary>
		/// Stop (usually called before switching to another Action)
		/// </summary>
		/// <returns></returns>
		public abstract void Stop();
	}
}