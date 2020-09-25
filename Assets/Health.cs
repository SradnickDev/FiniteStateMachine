using FSM.Example.States;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class HealthChanged : UnityEvent<float, float> { }

public class Health : MonoBehaviour, IDamageable
{
	public HealthChanged OnHealthChanged;
	public bool IsAlive => CurrentHealth > 0;
	public float MaxHealth = 1000;
	public float CurrentHealth;
	private Player m_lastHit;

	private void Start()
	{
		CurrentHealth = MaxHealth;
		OnHealthChanged.Invoke(CurrentHealth, MaxHealth);
	}

	public void ApplyDamage(float amount, Player from)
	{
		CurrentHealth = Mathf.Clamp(CurrentHealth -= amount, 0, MaxHealth);
		OnHealthChanged.Invoke(CurrentHealth, MaxHealth);

		if (CurrentHealth <= 0)
		{
			OnDeath();
		}
	}

	public void ApplyHealth(float amount)
	{
		CurrentHealth = Mathf.Clamp(CurrentHealth += amount, 0, MaxHealth);

		OnHealthChanged.Invoke(CurrentHealth, MaxHealth);
	}

	private void Update()
	{
		ApplyHealth(Time.deltaTime * 2.5f);
	}

	private void OnDeath()
	{
		GameObject.Destroy(this.gameObject);
	}
}