using System.Collections.Generic;
using UnityEngine;

namespace FSM.Example.States
{
	public class Player : MonoBehaviour
	{
		private void Start()
		{
			PlayerTracker.Instance.Add(this);
		}

		private void OnDestroy()
		{
			PlayerTracker.Instance.Remove(this);
		}
	}

	public class PlayerTracker : MonoBehaviour
	{
		public static PlayerTracker Instance;

		public List<Player> Players { get; private set; } = new List<Player>();

		//----------------------------------------------------------------
		private void Awake()
		{
			Instance = this;
		}

		//----------------------------------------------------------------
		public IEnumerable<Player> GetValidAllies(Player source)
		{
			foreach (var player in Players)
			{
				if (player != null && player.CompareTag(source.tag) && player != source)
				{
					yield return player;
				}
			}
		}

		//----------------------------------------------------------------
		public IEnumerable<Player> GetValidEnemies(Player source)
		{
			foreach (var player in Players)
			{
				if (player != null && !player.CompareTag(source.tag) && player != source)
				{
					yield return player;
				}
			}
		}

		//----------------------------------------------------------------
		public void Add(Player newPlayer)
		{
			if (!Players.Contains(newPlayer))
			{
				Players.Add(newPlayer);
			}
		}

		//----------------------------------------------------------------
		public void Remove(Player player) => Players.Remove(player);
	}

	[System.Serializable]
	public class MovementState : State
	{
		private const int MinPathPointDistance = 3;

		[SerializeField] private List<Transform> PathPoints = new List<Transform>();
		[SerializeField] private float TargetDetectionRange = 10;
		[SerializeField] private float TargetSearchInterval = 0.5f;

		private int m_nextPathIdx = 0;
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
			var distanceToPoint =
				Vector3.Distance(Context.Owner.transform.position,
								 PathPoints[m_nextPathIdx].position);
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
			PathPoints.FindNearest(Context.Owner.transform.position, out m_nextPathIdx,
								   TargetDetectionRange);
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
				target = Context.AvailableTargets.FindNearest(Context.Owner.transform.position,
															  out _);
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