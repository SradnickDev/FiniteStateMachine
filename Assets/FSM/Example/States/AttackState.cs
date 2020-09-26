using FSM.Example.Player;
using UnityEngine;

namespace FSM.Example.States
{
	[System.Serializable]
	public class AttackState : State
	{
		[SerializeField] private Weapon Weapon = null;
		[SerializeField] private float AttackDistance = 2f;
		[SerializeField] private float FieldOfView = 2f;
		[SerializeField] private LayerMask ObstacleMask = new LayerMask();
		[SerializeField] private float HitTestThickness = 0.5f;

		//----------------------------------------------------------------
		public override void OnEnter()
		{
			//No target available ? start moving again and find a target
			if (Context.CurrentTarget == null)
			{
				//Note: never tested if changing a state in OnEnter works
				//Hit me up if not
				GoToNextState(nameof(MovementState));
			}
		}

		//----------------------------------------------------------------
		public override void OnUpdate()
		{
			if (Context.CurrentTarget == null) return;

			FollowTarget();
			ShootTarget();
		}

		//----------------------------------------------------------------
		private void FollowTarget()
		{
			Context.OwnerAgent.SetDestination(Context.CurrentTarget.transform.position);
			Context.OwnerAgent.isStopped = Context.OwnerAgent.remainingDistance <= AttackDistance / 2f;

			if (Context.OwnerAgent.remainingDistance <= AttackDistance / 2f)
			{
				var direction = (Context.CurrentTarget.transform.position - Context.Owner.transform.position).normalized;
				var lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
				Context.Owner.transform.rotation = lookRotation;
			}
		}

		//----------------------------------------------------------------
		private void ShootTarget()
		{
			var isVisible = FSMHelper.IsInsideConeSphereHitTest(Context.Owner.transform,
					Context.CurrentTarget.transform,
					AttackDistance,
					FieldOfView,
					ObstacleMask,
					HitTestThickness);
			if (isVisible)
			{
				Weapon.Fire();
			}
		}

		//----------------------------------------------------------------
		public override void DrawGizmos()
		{
			FSMHelper.DrawCone(Context.Owner.transform, AttackDistance, FieldOfView);
			if (Context.CurrentTarget != null)
			{
				FSMHelper.DrawSphereCast(Context.Owner.transform, Context.CurrentTarget.transform, HitTestThickness);
			}
		}

		//----------------------------------------------------------------
		public override void OnLeave()
		{
			Context.OwnerAgent.isStopped = false;
			Context.CurrentTarget = null;
		}
	}
}