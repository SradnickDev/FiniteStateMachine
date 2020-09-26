using System;
using UnityEngine;

namespace FSM.Example.States
{
	[Serializable]
	public class RandomMovementState : State
	{
		[SerializeField] private float MaxRange;
		private Vector3 m_nextDestination;
		public override void OnEnter() { }

		public override void OnUpdate()
		{
		
		}

		public override void OnLeave() { }
	}
}