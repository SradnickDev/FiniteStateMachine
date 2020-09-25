namespace FSM
{
	[System.Serializable]
	public abstract class Decision
	{
		public StateMachine StateMachine { get; set; }
		public abstract bool IsValid(Context context);
	}
}