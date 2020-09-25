using FSM.Example.States;

namespace FSM
{
	[System.Serializable]
	public class OnLowHealth : Decision
	{
		public float MinHealth = 100;

		public override bool IsValid(Context context)
		{
			if (context.Health <= MinHealth && StateMachine.CurrentState != nameof(RunAndHide))
			{
				StateMachine.ChangeState(nameof(RunAndHide));
				return true;
			}

			return false;
		}
	}
}