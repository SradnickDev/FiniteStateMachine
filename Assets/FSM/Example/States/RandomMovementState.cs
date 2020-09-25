using UnityEngine;
using System;
using FSM;

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