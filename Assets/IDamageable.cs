using FSM.Example.States;

public interface IDamageable
{
	void ApplyDamage(float amount, Player from);
	void ApplyHealth(float amount);
}