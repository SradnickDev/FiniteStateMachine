using System;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace FSM
{
	[Serializable]
	public abstract class State
	{
		protected StateMachine StateMachine;
		protected Context Context;

		[TypeFilter("GetFilteredTypeList")]
		[SerializeReference] public List<Decision> Decisions;

		public IEnumerable<Type> GetFilteredTypeList()
		{
			var retVal = typeof(Decision).Assembly.GetTypes()
				.Where(x => !x.IsAbstract)
				.Where(x => typeof(Decision).IsAssignableFrom(x));

			return retVal;
		}

		public State Setup(StateMachine stateMachine)
		{
			Decisions.ForEach(decision => decision.StateMachine = stateMachine);
			StateMachine = stateMachine;
			Context = StateMachine.Context;
			return this;
		}

		/// <summary>
		/// Is called if the states will be used.
		/// </summary>
		public abstract void OnEnter();

		/// <summary>
		/// Called while the state is activated.
		/// </summary>
		public abstract void OnUpdate();

		/// <summary>
		/// Called if the state will be left.
		/// </summary>
		public abstract void OnLeave();

		protected void GoToNextState(string typeName)
		{
			StateMachine.ChangeState(typeName);
		}
	}
}