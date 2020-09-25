using System.Collections.Generic;
using UnityEngine;

namespace FSM.Example.States
{
	public class PlayerTracker : MonoBehaviour
	{
		public static PlayerTracker Instance;

		public List<Player> Players { get; private set; } = new List<Player>();

		//----------------------------------------------------------------
		private void Awake()
		{
			Instance = this;
		}

		//----------------------------------------------------------------
		public List<Player> GetValidAllies(Player source)
		{
			var retVal = new List<Player>();
			foreach (var player in Players)
			{
				if (player != null && player.CompareTag(source.tag) && player != source)
				{
					retVal.Add(player);
				}
			}

			return retVal;
		}

		//----------------------------------------------------------------
		public List<Player> GetValidEnemies(Player source)
		{
			var retVal = new List<Player>();
			foreach (var player in Players)
			{
				if (player != null && !player.CompareTag(source.tag) && player != source)
				{
					retVal.Add(player);
				}
			}

			return retVal;
		}

		//----------------------------------------------------------------
		public void Add(Player newPlayer)
		{
			if (!Players.Contains(newPlayer))
			{
				Players.Add(newPlayer);
			}
		}

		//----------------------------------------------------------------
		public void Remove(Player player) => Players.Remove(player);
	}
}