namespace FSM.Example.Player
{
	public interface IDamageable
	{
		void ApplyDamage(float amount, States.Player from);
		void ApplyHealth(float amount);
	}
}