using System.Collections.Generic;
using UnityEngine;

namespace FSM.Example.States
{
	[System.Serializable]
	public class MovementState : State
	{
		[SerializeField] private List<Transform> PathPoints = new List<Transform>();
		[SerializeField] private float TargetSearchInterval = 0.5f;
		[SerializeField] private float TargetDetectionRange = 10;
		[SerializeField] private float FieldOfView = 180;
		[SerializeField] private LayerMask ObstacleMask = new LayerMask();
		private int m_nextPathIdx = 0;
		private float m_nextSearch;

		//----------------------------------------------------------------
		public override void OnEnter()
		{
			m_nextSearch = 0;
			m_nextPathIdx = 0;

			//find nearest waypoint, maybe if the state will be entered the bot is to far from the old waypoint
			//so find a new one makes more sense
			FindNearestPathPoint();
		}

		//----------------------------------------------------------------
		public override void OnUpdate()
		{
			FollowPath();
			SearchTarget();
		}

#region Logic

		//----------------------------------------------------------------
		private void FollowPath()
		{
			var distanceToPoint =
					Vector3.Distance(Context.Owner.transform.position,
							PathPoints[m_nextPathIdx].position);
			var reachedPoint = distanceToPoint <= Context.OwnerAgent.stoppingDistance;

			if (reachedPoint)
			{
				SetNextPathPoint();
			}

			Context.OwnerAgent.destination = PathPoints[m_nextPathIdx].position;
		}

		//----------------------------------------------------------------
		private void FindNearestPathPoint()
		{
			PathPoints.FindNearest(Context.Owner.transform.position, out m_nextPathIdx);
		}

		//----------------------------------------------------------------
		private void SetNextPathPoint()
		{
			m_nextPathIdx = (m_nextPathIdx + 1) % PathPoints.Count;
		}

		//----------------------------------------------------------------
		private void SearchTarget()
		{
			if (Context.CurrentTarget != null) return;

			Player target = null;

			//interval search
			if (Time.time >= m_nextSearch)
			{
				target = PlayerTracker.Instance.GetValidEnemies(Context.Owner)
									  .FindNearest(Context.Owner.transform.position, out _);
				m_nextSearch = Time.time + TargetSearchInterval;
			}

			if (target == null) return;

			var isValidTarget = FSMHelper.IsInsideConeLineHitTest(Context.Owner.transform,
					target.transform,
					TargetDetectionRange,
					FieldOfView,
					ObstacleMask);
			if (!isValidTarget) return;

			//if valid target is available
			//set target and move to next state

			Context.CurrentTarget = target;
			GoToNextState(nameof(AttackState));
		}

#endregion

		//----------------------------------------------------------------
		public override void DrawGizmos()
		{
			FSMHelper.DrawCone(Context.Owner.transform, TargetDetectionRange, FieldOfView);
		}

		//----------------------------------------------------------------
		public override void OnLeave()
		{
		}
	}
}