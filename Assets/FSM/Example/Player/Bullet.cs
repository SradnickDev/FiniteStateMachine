using UnityEngine;

namespace FSM.Example.Player
{
	[RequireComponent(typeof(Rigidbody))]
	public class Bullet : MonoBehaviour
	{
		[SerializeField] private Rigidbody Rigidbody = null;
		[SerializeField] private float Force = 10;
		[SerializeField] private ForceMode ForceMode = ForceMode.Force;
		[SerializeField] private float Damage = 10;
		[SerializeField] private float LifeTime = 2;
		private States.Player m_owner;
		private Vector3 m_movingDirection;

		public void Initialize(Vector3 dir, States.Player from)
		{
			m_owner = from;
			m_movingDirection = dir;
			transform.forward = dir;
			Rigidbody.AddForce(dir * Force, ForceMode);
			Destroy(gameObject, LifeTime);
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.gameObject == m_owner.gameObject) return;

			if (!other.CompareTag(m_owner.tag))
			{
				DealDamage(other);
			}

			Destroy(gameObject);
		}

		private void DealDamage(Component other)
		{
			var damageable = other.GetComponent<IDamageable>();
			damageable?.ApplyDamage(Damage, m_owner);
		}
	}
}