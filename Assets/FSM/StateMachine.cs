using System;
using System.Collections.Generic;
using System.Linq;
using FSM.Example.States;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace FSM
{
	[Serializable]
	public class OnStateChanged : UnityEvent<string>
	{
	}

	[RequireComponent(typeof(Player))]
	[RequireComponent(typeof(NavMeshAgent))]
	public abstract class StateMachine : MonoBehaviour
	{
		//just to display what state is currently active
		//can also be used to check if in a specific state
		[ReadOnly] public string CurrentState;
		[ReadOnly] public string LastDecisionMade;

		protected string StartState => States.Count > 0 ? States[0].Name() : "";
		[SerializeField] private bool LogVerbose = false;

		[TypeFilter("GetFilteredTypeList")]
		[SerializeReference] private List<State> States = new List<State>();

		public IEnumerable<Type> GetFilteredTypeList()
		{
			var retVal = typeof(State).Assembly.GetTypes()
									  .Where(x => !x.IsAbstract)
									  .Where(x => typeof(State).IsAssignableFrom(x));

			return retVal;
		}

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
			Context = new Context
			{
					Owner = GetComponent<Player>(),
					OwnerAgent = GetComponent<NavMeshAgent>()
			};

			foreach (var state in States)
			{
				AddState(state);
			}

			OnInitialize();

			if (!string.IsNullOrEmpty(StartState))
			{
				Active = true;
				ChangeState(StartState);
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
				m_previousState = m_currentState;
				m_previousState?.OnLeave();

				m_currentState = m_states[newState];
				m_currentState.OnEnter();
				CurrentState = newState;
				FSMHelper.Log(LogVerbose, $"State changed from {m_previousState.Name()} to {m_currentState.Name()}");
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
				state.Setup(this);
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
				UpdateDecisions();
			}
		}

		/// <summary>
		/// Update every decision.
		/// </summary>
		private void UpdateDecisions()
		{
			if (m_currentState.Decisions == null) return;

			foreach (var decision in m_currentState.Decisions)
			{
				if (decision.IsValid(Context))
				{
					LastDecisionMade = decision.Name();
					FSMHelper.Log(LogVerbose, $"Decisions validated : {LastDecisionMade}");
				}
			}
		}

		private void OnDrawGizmos()
		{
			if (m_currentState != null)
			{
				m_currentState.DrawGizmos();
			}
		}
	}
}