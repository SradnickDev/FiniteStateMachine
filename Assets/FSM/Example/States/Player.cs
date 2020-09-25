using UnityEngine;

namespace FSM.Example.States
{
	public class Player : MonoBehaviour
	{
		private void Start()
		{
			PlayerTracker.Instance.Add(this);
		}

		private void OnDestroy()
		{
			PlayerTracker.Instance.Remove(this);
		}
	}
}