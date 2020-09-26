using UnityEngine;
using UnityEngine.Events;

namespace FSM.Example.Player
{
	[System.Serializable]
	public class HealthChanged : UnityEvent<float, float>
	{
	}

	public class Health : MonoBehaviour, IDamageable
	{
		public HealthChanged OnHealthChanged;
		[SerializeField] private float RegenerateRate = 2.5f;
		public bool IsAlive => CurrentHealth > 0;
		public float MaxHealth = 1000;
		public float CurrentHealth;
		private States.Player m_lastHit;

		private void Start()
		{
			CurrentHealth = MaxHealth;
			OnHealthChanged.Invoke(CurrentHealth, MaxHealth);
		}

		public void ApplyDamage(float amount, States.Player from)
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
			Regenerate();
		}

		private void Regenerate()
		{
			ApplyHealth(Time.deltaTime * RegenerateRate);
		}

		private void OnDeath()
		{
			GameObject.Destroy(this.gameObject);
		}
	}
}