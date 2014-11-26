using System;
using Dirac.Math;
using Dirac.GameServer.Core.AI.Actions;

namespace Dirac.GameServer.Core.AI.Brains
{
	/// <summary>
	/// The interface to any brain (usually belonging to an NPC)
	/// A brain is a finite automaton with a queue of actions
	/// </summary>
	public interface IBrain : IDisposable 
	{
		/// <summary>
		/// Current state of the brain
		/// </summary>
		BrainState State { get; }

		/// <summary>
		/// Default state of the brain
		/// </summary>
		BrainState DefaultState { get; }

		/// <summary>
		/// Aggressive brains actively seek for combat Action
		/// </summary>
		bool IsAggressive
		{
			get;
			set;
		}


        /// <summary>
        /// The AIAction that is currently being executed
        /// </summary>
        AIAction CurrentAction { get; }

		/// <summary>
		/// Current Running state
		/// </summary>
		/// <value>if false, Brain will not update</value>
		bool IsRunning { get; }

		/// <summary>
		/// The origin location to which this Brain will always want to go back to (if any)
		/// </summary>
		Vector3 SourcePoint { get; set; }

		void EnterDefaultState();

		void StopCurrentAction();
	}
}