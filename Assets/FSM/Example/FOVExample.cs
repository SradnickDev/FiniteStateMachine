using UnityEngine;

namespace FSM.Example
{
	public class FOVExample : MonoBehaviour
	{
		[SerializeField] private float Radius = 10;
		[SerializeField] private float Fov = 100;
		[SerializeField] private float HitTestRadius = 1;
		[SerializeField] private GameObject Target;
		[SerializeField] private LayerMask ObstacleMask;

		private bool valid;

		void Update()
		{
			valid = FSMHelper.IsInsideConeSphereHitTest(transform, Target.transform, Radius, Fov,
														ObstacleMask,
														HitTestRadius);
		}

		private void OnDrawGizmosSelected()
		{
			FSMHelper.DrawCone(transform, Radius, Fov);
			Gizmos.color = valid ? Color.green : Color.red;
			FSMHelper.DrawSphereCast(transform, Target.transform, HitTestRadius);
		}
	}
}