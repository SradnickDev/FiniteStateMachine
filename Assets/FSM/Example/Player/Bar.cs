using UnityEngine;

namespace FSM.Example.Player
{
	public class Bar : MonoBehaviour
	{
		[SerializeField] private SpriteRenderer FillBar = null;
		[SerializeField] private float MaxX = 300f;

		public void OnValueChanged(float currentHealth, float maxHealth)
		{
			var percentage = currentHealth / maxHealth;
			percentage = Mathf.Clamp01(percentage);

			var scale = FillBar.transform.localScale;
			scale.x = MaxX * percentage;
			FillBar.transform.localScale = scale;
		}
	}
}