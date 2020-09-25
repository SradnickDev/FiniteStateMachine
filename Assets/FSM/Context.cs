using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
	/// <summary>
	/// Store data, that will be shared between the states.
	/// </summary>
	public class Context
	{
		//TODO palce your stuff that need to be shared between states here
		public float
			Health; // use health component or where ever you store ur health instead of this

		public DummyTarget CurrentTarget;
		public List<DummyTarget> AvailableTargets;
	}

	//TODO delete this and replace it with ur actual target monobehaviour 
	public class DummyTarget : MonoBehaviour { }
}