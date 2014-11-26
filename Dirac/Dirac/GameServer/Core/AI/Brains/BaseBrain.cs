using System;
using Dirac.Math;
using Dirac.GameServer.Core.AI;
using Dirac.GameServer.Core.AI.Actions;
using Dirac.GameServer.Core.AI.Actions.Movement;

namespace Dirac.GameServer.Core.AI.Brains
{
	/// <summary>
	/// The default class for monsters' AI
	/// </summary>
	public class BaseBrain : IBrain , IUpdateable
	{
        #region Members

        private AIAction _currentAction;

		private BrainState _state;

        private BrainState _defaultState;

        private Monster _owner;

        private Vector3 _SourcePoint;

        private bool _isAggressive;

        private bool _isRunning;

        #endregion

        #region Constructors

        public BaseBrain(Monster owner, BrainState startState = BrainState.Roam)
        {
            this._owner = owner;
            this._defaultState = BrainState.Roam;
            this._state = startState;
            
        }

		#endregion

		#region Properties
        public Monster Owner
		{
			get { return _owner; }
		}

		public AIAction CurrentAction
		{
			get { return _currentAction; }
		}

        public BrainState State
        {
            get { return _state; }
        }

		/// <summary>
		/// The State to fall back to when nothing else is up.
		/// </summary>
		public BrainState DefaultState
		{
			get { return _defaultState; }
		}

		public bool IsRunning
		{
			get { return _isRunning; }
		}

		/// <summary>
		/// The point of attraction where we took off when we started with the
		/// last action
		/// </summary>
		public Vector3 SourcePoint
		{
			get { return _SourcePoint; }
			set { _SourcePoint = value; }
		}

		public bool IsAggressive
		{
			get { return _isAggressive; }
			set { _isAggressive = value; }
		}

		#endregion

		/// <summary>
		/// Updates the AIAction by calling Perform. Called every tick by the Map
		/// </summary>
		/// <param name="dt">not used</param>
        public virtual void Update(TimeSpan elapsed)
		{
			if (!IsRunning)
				return;

            // update current Action if any
            if (_currentAction == null)
            {
                //sino esta haciendo nada, hacer algo...
                _currentAction = _getActionFromState(_state);
                _currentAction.Start();
            }
            else
            {
                _currentAction.Update(elapsed);
            }
		}

        public void EnterState(BrainState state, Actor target)
        {
            Logging.LogManager.DefaultLogger.Trace(this.Owner.DynamicID.ToString() + " " + state.ToString());
            if (this._currentAction != null)
            {
                //dejar de hacer lo que esta haciendo actualmente
                _currentAction.Stop();
            }
            _state = state;

            _currentAction = _getActionFromState(_state);
            _currentAction.Start();

            _currentAction.Target = target;
        }

        public void EnterState(BrainState state)
        {
            Logging.LogManager.DefaultLogger.Trace(this.Owner.DynamicID.ToString() + " " + state.ToString());
            if (this._currentAction != null)
            {
                _currentAction.Stop();
            }

            _state = state;

            _currentAction = _getActionFromState(_state);
            _currentAction.Start();

            _currentAction.Target = null; //for roam for example.
        }

		public void EnterDefaultState()
		{
            _state = _defaultState;
		}

		public virtual void Start()
		{
			_isRunning = true;
		}

        public virtual void Stop()
		{
			_isRunning = false;

			StopCurrentAction();
		}

		public void StopCurrentAction()
		{
			if (_currentAction != null)
			{
				_currentAction.Stop();
			}
			_currentAction = null;
		}

		#region Events Handlers
		public virtual void OnEnterCombat()
		{
		}

		public virtual void OnLeaveCombat()
		{
		}

        public virtual void OnHeal(Actor healer, Actor healed, int amtHealed)
		{
		}

		public virtual void OnDamageReceived()
		{
		}

		public virtual void OnDamageDealt()
		{
		}

        public virtual void OnDebuff(Actor caster)
		{
		}

        public virtual void OnKilled(Actor killerActor, Actor victimActor)
		{
		}

		public virtual void OnDeath()
		{
		}

		public virtual void OnCombatTargetOutOfRange()
		{
		}

		#endregion

		public void Dispose()
		{
			_owner = null;
		}

        private AIAction _getActionFromState(BrainState state)
        {
            AIAction result;

            switch (state)
            {
                case BrainState.Combat:
                    {
                        result = new AI.Actions.States.AICombatAction(this.Owner);
                        break;
                    }
                case BrainState.Dead:
                    {
                        result = new AI.Actions.States.AIDeadAction(this.Owner);
                        break;
                    }
                case BrainState.Evade:
                    {
                        result = new AI.Actions.States.AIEvadeAction(this.Owner);
                        break;
                    }
                case BrainState.Follow:
                    {
                        result = new AI.Actions.States.AIFollowAction(this.Owner);
                        break;
                    }
                case BrainState.Guard:
                    {
                        result = new AI.Actions.States.AIGuardMasterAction(this.Owner);
                        break;
                    }
                case BrainState.Idle:
                    {
                        result = new AI.Actions.States.AIIdleAction(this.Owner);
                        break;
                    }
                case BrainState.Roam:
                    {
                        result = new AI.Actions.States.AIRoamAction(this.Owner);
                        break;
                    }
                default: throw new Exception("dont have that state to generate a new action");
            }
            return result;
        }
	}
}