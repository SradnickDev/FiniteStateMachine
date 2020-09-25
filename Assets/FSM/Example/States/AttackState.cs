namespace FSM.Example.States
{
	public class AttackState : State
	{
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
			//TODO Follow Target
			//TODO if target is null/dead or something like this go to MovementState
			//TODO Validate if bot can attack and attack
			//TODO if target is to far, start next state e.g. find nearst obstalce away from target and move there to hide
			//after that start movementstate again
		}

		//----------------------------------------------------------------
		public override void OnLeave() { }
	}
}