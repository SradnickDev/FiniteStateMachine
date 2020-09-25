using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace FSM
{
	[Serializable]
	public class OnStateChanged : UnityEvent<string>
	{
	}

	public abstract class StateMachine : MonoBehaviour
	{
		public OnStateChanged StateChanged;

		//just to display what state is currently active
		//can also be used to check if in a specific state
		[ReadOnly] public string CurrentState;
		[ReadOnly] public string LastDecisionMade;

		[SerializeField] protected State StartState;
		[SerializeField] private bool LogVerbose = false;

		/// <summary>
		/// Shared between all states.But owned and created from a StateMachine.
		/// </summary>
		public Context Context { get; private set; }

		public bool Active { get; set; } = false;
		private Dictionary<string, State> m_states;
		private State m_currentState;
		private State m_previousState;

		private void Start() => Initialize();

		/// <summary>
		/// Called on start and call's OnInitialize.
		/// </summary>
		private void Initialize()
		{
			m_states = new Dictionary<string, State>();
			Context = new Context();

			//order is important
			OnInitialize();

			if (StartState != null)
			{
				Active = true;
				ChangeState(StartState.Name());
			}
			else if (StartState == null)
			{
				FSMHelper.Log(LogVerbose, $"Start State is null! Please assign one.");
			}
		}

		/// <summary>
		/// True if in state equals current state, otherwise false.
		/// </summary>
		public bool IsInState(string state)
		{
			return string.Equals(m_currentState.Name(), state);
		}

		/// <summary>
		/// Called on start, should be used to configure the StateMachine before it starts.
		/// </summary>
		protected abstract void OnInitialize();

		/// <summary>
		/// State will be changed immediately.
		/// </summary>
		/// <param name="newState"></param>
		public void ChangeState(string newState)
		{
			if (m_states.ContainsKey(newState) && !string.Equals(CurrentState, newState))
			{
				StateChanged?.Invoke(newState);
				m_previousState?.OnLeave();
				m_previousState = m_currentState;
				m_currentState = m_states[newState];
				m_currentState.OnEnter();
				CurrentState = newState;
				FSMHelper.Log(LogVerbose,
					$"State changed from {m_previousState.Name()} to {m_currentState.Name()}");
			}
		}

		/// <summary>
		/// Add a new state that will be taken care of
		/// </summary>
		/// <param name="state"></param>
		public void AddState(State state)
		{
			if (!m_states.ContainsKey(state.Name()))
			{
				m_states.Add(state.Name(), state);
				FSMHelper.Log(LogVerbose, $"New State added : {state.Name()}");
			}
		}

		/// <summary>
		/// Remove state.If needed ?w
		/// </summary>
		/// <param name="state"></param>
		public void RemoveState(string state)
		{
			if (m_states.ContainsKey(state))
			{
				m_states.Remove(state);
				FSMHelper.Log(LogVerbose, $"State removed : {state}");
			}
		}

		/// <summary>
		/// Unity's Update, should always call UpdateState()
		/// </summary>
		protected virtual void Update() => UpdateState();

		/// <summary>
		/// Updates current state.
		/// </summary>
		protected void UpdateState()
		{
			if (!Active) return;
			if (m_currentState != null)
			{
				m_currentState.OnUpdate();
			}

			UpdateDecisions();
		}

		/// <summary>
		/// Update every decision.
		/// </summary>
		private void UpdateDecisions()
		{
			foreach (var decision in m_currentState.Decisions.Where(decision =>
				decision.IsValid(Context)))
			{
				LastDecisionMade = decision.Name();
				FSMHelper.Log(LogVerbose, $"Decisions validated : {LastDecisionMade}");
			}
		}
	}
}