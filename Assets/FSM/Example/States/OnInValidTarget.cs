using UnityEngine;

namespace FSM.Example.States
{
	[System.Serializable]
	public class OnInValidTarget : Decision
	{
		[SerializeField] private float MaxDistance = 50;

		public override bool IsValid(Context context)
		{
			var isNull = context.CurrentTarget == null;
			var isNotNullButNotAlive = context.CurrentTarget != null &&
									   !context.CurrentTarget.GetComponent<Health>().IsAlive;

			if (isNull || isNotNullButNotAlive)
			{
				context.CurrentTarget = null;
				StateMachine.ChangeState(nameof(MovementState));
				return true;
			}

			var isToFar = Vector3.Distance(context.CurrentTarget.transform.position,
										   context.Owner.transform.position) > MaxDistance;

			if (isToFar)
			{
				context.CurrentTarget = null;
				StateMachine.ChangeState(nameof(RunAndHide));
				return true;
			}

			return false;
		}
	}
}