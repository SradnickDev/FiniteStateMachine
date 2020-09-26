using System;
using UnityEngine;
using UnityEngine.AI;

namespace FSM.Example.States
{
	[Serializable]
	public class RunAndHide : State
	{
		[SerializeField] private LayerMask ObstacleMask = new LayerMask();
		[SerializeField] private float ScanRadius = 100;
		private Collider[] m_hitResults;
		private GameObject m_targetToHide;
		private Vector3 m_hidePosition;

		public override void OnEnter()
		{
			m_hitResults = new Collider[10];
			m_targetToHide = null;
			m_hidePosition = Vector3.zero;
			Context.OwnerAgent.speed *= 2;
		}

		public override void OnUpdate()
		{
			if (m_hidePosition == Vector3.zero)
			{
				FindObstacle();
				CalculateHidePosition();
				Context.OwnerAgent.SetDestination(m_hidePosition);
			}

			if (m_hidePosition != Vector3.zero &&
				Context.OwnerAgent.remainingDistance <= Context.OwnerAgent.stoppingDistance)
			{
				GoToNextState(nameof(MovementState));
			}
		}

		private void FindObstacle()
		{
			var currentPos = Context.Owner.transform.position;
			var hits = Physics.OverlapSphereNonAlloc(currentPos, ScanRadius, m_hitResults, ObstacleMask);

			if (hits > 0)
			{
				var target = m_hitResults.AnyOne();
				m_targetToHide = target.gameObject;
			}
		}

		private void CalculateHidePosition()
		{
			if (NavMesh.SamplePosition(m_targetToHide.transform.position, out var hit, 4, NavMesh.AllAreas))
			{
				m_hidePosition = hit.position;
			}
		}

		public override void OnLeave()
		{
			Context.OwnerAgent.speed /= 2;
		}
	}
}