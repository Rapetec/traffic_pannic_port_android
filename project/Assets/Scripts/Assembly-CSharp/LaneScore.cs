using UnityEngine;

public class LaneScore : MonoBehaviour
{
	private enum TextState
	{
		None = 0,
		Reset = 1,
		Float = 2
	}

	public float fadeRate = 1f;

	public float riseSpeed = 1f;

	private Transform cachedTrans;

	private SpriteText scoreText;

	private Vector3 startPosition;

	private float transparency = 1f;

	private int currentScore;

	private bool _isHi;

	private TextState state;

	private GameManager gameManager;

	private void Start()
	{
		GameObject gameObject = GameObject.Find("Game Manager");
		gameManager = gameObject.GetComponent<GameManager>();
		cachedTrans = base.transform;
		startPosition = cachedTrans.position;
		scoreText = GetComponent<SpriteText>();
		scoreText.Text = string.Empty;
	}

	private void Update()
	{
		switch (state)
		{
		case TextState.None:
			break;
		case TextState.Reset:
			transparency = 1f;
			cachedTrans.position = startPosition;
			state = TextState.Float;
			break;
		case TextState.Float:
			cachedTrans.position += new Vector3(0f, riseSpeed, 0f);
			transparency -= 0.01f * fadeRate;
			if (transparency <= 0f)
			{
				state = TextState.None;
			}
			if (_isHi)
			{
				scoreText.color = new Color(255f, 0f, 0f, transparency);
			}
			else
			{
				scoreText.color = new Color(255f, 255f, 255f, transparency);
			}
			scoreText.Text = currentScore.ToString();
			break;
		}
	}

	public void NewScore(int score, bool hi)
	{
		_isHi = hi;
		currentScore = score;
		state = TextState.Reset;
		if (hi)
		{
			gameManager.PlayMaxPoints();
		}
	}
}
