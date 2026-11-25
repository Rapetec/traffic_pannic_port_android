using NeonPlay;
using UnityEngine;

public class TextController : MonoBehaviour
{
	public SpriteText GOscore;

	public SpriteText GOdamage;

	public SpriteText GOcash;

	public SpriteText continueText;

	private void Start()
	{
		SingletonBase<TextManager>.Instance.TextController = this;
	}
}
