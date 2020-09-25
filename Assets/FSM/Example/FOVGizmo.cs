using UnityEngine;

namespace FSM.Example
{
	public class FOVGizmo : MonoBehaviour
	{
		[SerializeField] private float Radius = 10;
		[SerializeField] private float Fov = 100;

		private void OnDrawGizmosSelected()
		{
			FSMHelper.DrawCone(transform, Radius, Fov);
		}
	}
}