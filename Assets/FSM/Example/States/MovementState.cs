using System.Collections.Generic;
using UnityEngine;

namespace FSM.Example.States
{
	public class MovementState : State
	{
		private const int MinPathPointDistance = 3;

		[SerializeField] private List<Transform> PathPoints = new List<Transform>();
		[SerializeField] private float TargetDetectionRange = 10;
		[SerializeField] private float TargetSearchInterval = 0.5f;

		private int m_nextPathIdx;
		private float m_nextSearch;

		//----------------------------------------------------------------
		public override void OnEnter()
		{
			m_nextSearch = 0;

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
			var distanceToPoint = Vector3.Distance(transform.position, PathPoints[m_nextPathIdx].position);
			var reachedPoint = distanceToPoint <= MinPathPointDistance;

			if (reachedPoint)
			{
				SetNextPathPoint();
			}

			//TODO movement stuff
		}

		//----------------------------------------------------------------
		private void FindNearestPathPoint()
		{
			PathPoints.FindNearest(transform.position, out m_nextPathIdx, TargetDetectionRange);
		}

		//----------------------------------------------------------------
		private void SetNextPathPoint()
		{
			m_nextPathIdx = (m_nextPathIdx + 1) % PathPoints.Count;
		}

		//----------------------------------------------------------------
		private void SearchTarget()
		{
			DummyTarget target = null;

			//interval search
			if (Time.time >= m_nextSearch)
			{
				target = Context.AvailableTargets.FindNearest(transform.position, out _);
				m_nextSearch = Time.time + TargetSearchInterval;
			}

			//TODO validate target
			var isValidTarget = false;
			if (!isValidTarget) return;

			//if valid target is available
			//set target and move to next state

			Context.CurrentTarget = target;
			GoToNextState(nameof(AttackState));
		}

#endregion

		//----------------------------------------------------------------
		public override void OnLeave() { }
	}
}