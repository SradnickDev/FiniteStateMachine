using FSM.Example.States;
using UnityEngine;

namespace FSM
{
	[System.Serializable]
	public class OnLowHealth : Decision
	{
		[Range(0, 1f)] public float MinHealthPercentage = 100;
		private Health m_ownerHealth;

		public override bool IsValid(Context context)
		{
			if (m_ownerHealth == null)
			{
				m_ownerHealth = context.Owner.GetComponent<Health>();
			}

			var percentage = m_ownerHealth.CurrentHealth / m_ownerHealth.MaxHealth;

			if (percentage <= MinHealthPercentage &&
				StateMachine.CurrentState != nameof(RunAndHide))
			{
				StateMachine.ChangeState(nameof(RunAndHide));
				return true;
			}

			return false;
		}
	}
}