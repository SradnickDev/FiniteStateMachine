using UnityEngine;

namespace FSM.Example.Player
{
	[RequireComponent(typeof(States.Player))]
	public class Weapon : MonoBehaviour
	{
		[SerializeField] private float FireRate = 0.25f;
		[SerializeField] private Bullet Bullet = null;
		[SerializeField] private States.Player Owner = null;
		private float m_nextShot;

		public void Fire()
		{
			if (Time.time > m_nextShot)
			{
				FireBullet(transform.forward);
				m_nextShot = Time.time + FireRate;
			}
		}

		private void FireBullet(Vector3 direction)
		{
			var bullet = Instantiate(Bullet, transform.position + direction + transform.right * 0.5f,
					Quaternion.identity);
			bullet.Initialize(direction, Owner);
		}
	}
}