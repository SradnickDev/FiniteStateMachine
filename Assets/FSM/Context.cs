using FSM.Example.States;
using UnityEngine.AI;

namespace FSM
{
	/// <summary>
	/// Store data, that will be shared between the states.
	/// </summary>
	public class Context
	{
		public Player Owner;
		public NavMeshAgent OwnerAgent;

		public Player CurrentTarget;
	}
}