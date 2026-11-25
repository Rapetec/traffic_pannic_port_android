using UnityEngine;

public class AnimatedLoadingBar : MonoBehaviour
{
	private PackedSprite sprite;

	private float timer;

	private float maxTime = 0.2f;

	private int count;

	private int maxAnimations;

	private void Start()
	{
		sprite = GetComponent<PackedSprite>();
		maxAnimations = sprite.animations.Length;
	}

	private void Update()
	{
		timer += Time.deltaTime;
		if (timer >= maxTime)
		{
			timer = 0f;
			count++;
			if (count == maxAnimations)
			{
				count = 0;
			}
			sprite.PlayAnim(count);
		}
	}
}
