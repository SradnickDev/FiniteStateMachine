using System;
using System.Linq;
using UnityEngine;

namespace FSM.Example.States
{
	[Serializable]
	public class RunAndHide : State
	{
		[SerializeField] private LayerMask ObstacleMask;
		[SerializeField] private float ScanRadius = 100;
		private Collider[] m_hitResults;
		private GameObject m_targetToHide;
		public override void OnEnter()
		{
			
		}

		public override void OnUpdate()
		{
			FindObstacle();

		}
		

		private void FindObstacle()
		{
			var hits = Physics.OverlapSphereNonAlloc(Context.Owner.transform.position, ScanRadius,
													 m_hitResults, ObstacleMask);
			var target = m_hitResults.ToList().FindNearest(Context.Owner.transform.position, out _);
			m_targetToHide = target.gameObject;
		}
		
	

		public override void OnLeave()
		{
			
		}
	}
}