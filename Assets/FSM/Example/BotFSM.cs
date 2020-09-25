namespace FSM.Example
{
	public class BotFSM : StateMachine
	{
		protected override void OnInitialize()
		{
			//keep it simple without any inspector stuff
			//just get all states that are on this gameobject and add it
			var states = GetComponents<State>();
			foreach (var state in states)
			{
				AddState(state.Setup(this));
			}
		}
	}
}