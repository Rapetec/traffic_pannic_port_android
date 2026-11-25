using System.Collections;
using NeonPlay;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	private const int MAX_CLIPS_PLAYED_IN_A_FRAME = 3;

	private bool quitting;

	public VehicleController[] allowedVehicles;

	public LaneController[] lanes;

	public Camera mainTrafficCamera;

	private bool gameOver = true;

	private bool trafficJam;

	private bool playAudio = true;

	private bool bringInGameOver;

	public int playsUntilHiscore = 1;

	public int score;

	public int damageScore;

	public int cash;

	private float _damage;

	private bool stop;

	private float timeSinceStop;

	public float acceleration;

	public float stoppingDistance;

	public float maxWaitTime;

	public float costPerDamage;

	public float maxResetTimer = 5f;

	public float minResetTimer = 1f;

	private float resetTimer;

	public static int tipFrequency = 6;

	public AudioClip lightsGoClick;

	public AudioClip lightsStopClick;

	public AudioClip accelerationSoundCar;

	public AudioClip accelerationSoundSports;

	public AudioClip accelerationSoundTruck;

	public AudioClip skid1Sound;

	public AudioClip skid2Sound;

	public AudioClip skid3Sound;

	public AudioClip crash1Sound;

	public AudioClip crash2Sound;

	public AudioClip crash3Sound;

	public AudioClip crash4Sound;

	public AudioClip beepBeepSound;

	public AudioClip beeeeeeepSound;

	public AudioClip sirenPolice;

	public AudioClip sirenAmbulance;

	public AudioClip sirenFire;

	public AudioClip alarm1;

	public AudioClip alarm2;

	public AudioClip alarm3;

	public AudioClip alarm4;

	public AudioClip alarm5;

	public AudioClip explosion;

	public AudioClip cannon;

	public AudioClip fireworks;

	public AudioClip maxPoints;

	public UIButton mainButton;

	public UIStateToggleBtn trafficLights;

	private bool allowTouch = true;

	public bool playerTruck;

	public bool playerBus;

	public SpriteText gameScoreText;

	public SpriteText gameTotalCashText;

	public SpriteText hiscoreMessage;

	public SpriteText hiscoreValue;

	public Light mainSceneLight;

	private Color startColor;

	public Color afterExplosionColor = new Color(0.34509805f, 0.16862746f, 0.015686275f);

	public bool hadExplosion;

	private float lightChangeStart;

	public GameObject pausebutton;

	public GameObject scorebackground;

	public GameObject damageText;

	public GameObject damageTextBase;

	public UIPanelBase loadingPanel;

	public PackedSprite gameOverTitleSprite;

	public int clipsPlayedThisFrame;

	public int best;

	public int bestDamage;

	public bool betterScore;

	public bool betterDamage;

	public SpecialEffectsManager sfxManager;

	public VehicleController taxi;

	public VehicleController trailer;

	public VehicleController fuelTanker;

	public VehicleController italiano;

	public VehicleController dassporta;

	public VehicleController tank;

	public VehicleController policeCar;

	public VehicleController ambulance;

	public VehicleController fireTruck;

	private int noEmergencyVehicles;

	public static int replays;

	public ArrayList flames = new ArrayList();

	public LaneScore laneScore1;

	public LaneScore laneScore2;

	public LaneScore laneScore3;

	private bool animateCamera;

	private bool notificationShown;

	public float damage
	{
		get
		{
			return _damage;
		}
		set
		{
			_damage = value;
		}
	}

	public bool Stop
	{
		set
		{
			stop = value;
		}
	}

	public bool GameOver
	{
		get
		{
			return gameOver;
		}
		set
		{
			gameOver = value;
			pausebutton.active = !value;
			if (!bringInGameOver)
			{
				damageText.active = value;
				damageTextBase.active = value;
			}
		}
	}

	public bool TrafficJam
	{
		get
		{
			return trafficJam;
		}
		set
		{
			trafficJam = value;
		}
	}

	public void AwardDamage(float d)
	{
		if (SingletonBase<TrafficShopItemHandler>.Instance.InsuranceFraud)
		{
			d *= 2f;
		}
		damage += d;
	}

	public void Restart()
	{
		score = 0;
		gameOver = false;
		trafficJam = false;
		stop = true;
		timeSinceStop = 0f;
		noEmergencyVehicles = 0;
		damageText.active = false;
		damageTextBase.active = false;
	}

	private void Start()
	{
		Restart();
		Time.timeScale = 1f;
		SingletonBase<TextManager>.Instance.HideContinueText();
		SingletonBase<CameraManager>.Instance.CollisionObjects.Clear();
		best = PlayerPrefs.GetInt("best");
		bestDamage = PlayerPrefs.GetInt("bestDamage");
		resetTimer = maxResetTimer;
		GameNav.Instance.gm = this;
		replays++;
		if (!PurchaseManager.Instance.PurchaseMade)
		{
			ADManager.SetHidden(false);
		}
		gameTotalCashText.Text = CurrencyManager.GetCurrency().ToString("N0");
		LoadBuildings();
		if (mainSceneLight == null)
		{
			mainSceneLight = GameObject.Find("Directional light").GetComponent<Light>();
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape) && !quitting)
		{
			quitting = true;
			Debug.Log("MAYBE");
			Time.timeScale = 1f;
			GameNav.Instance.BringInLoading();
			SingletonBase<AchievementStorage>.Instance.ReportAccumulations();
			Application.LoadLevelAsync("Menu");
		}
		if (!notificationShown)
		{
			ShowNotification();
			notificationShown = true;
		}
		if (!gameOver)
		{
			if (mainButton.controlState == UIButton.CONTROL_STATE.ACTIVE && allowTouch)
			{
				allowTouch = false;
				NotificationManager.Instance.DismissTrafficTip();
				stop = !stop;
				trafficLights.SetToggleState(Mathf.Abs(trafficLights.GetStateIndex(trafficLights.StateName) - 1));
				if (stop)
				{
					base.GetComponent<AudioSource>().PlayOneShot(lightsGoClick);
					timeSinceStop = Time.timeSinceLevelLoad;
				}
				else
				{
					base.GetComponent<AudioSource>().PlayOneShot(lightsStopClick);
				}
			}
			else if (Input.GetMouseButtonUp(0))
			{
				allowTouch = true;
			}
		}
		else
		{
			if (!animateCamera)
			{
				trafficLights.gameObject.active = false;
				SingletonBase<AchievementStorage>.Instance.GameOvers++;
				animateCamera = true;
				CameraAnimations();
			}
			SingletonBase<CameraManager>.Instance.UpdateAverageCollisionLocation();
			if (resetTimer < maxResetTimer - minResetTimer && !bringInGameOver)
			{
				SingletonBase<TextManager>.Instance.UpdateContinueText();
			}
			resetTimer -= Time.deltaTime;
			if (mainButton.controlState == UIButton.CONTROL_STATE.ACTIVE && resetTimer < maxResetTimer - minResetTimer && !bringInGameOver)
			{
				GameObject obj = damageText;
				bool active = false;
				damageTextBase.active = active;
				obj.active = active;
				SingletonBase<TextManager>.Instance.HideContinueText();
				bringInGameOver = true;
				resetTimer = 0f;
				if (betterScore)
				{
					PlayerPrefs.SetInt("best", best);
				}
				if (betterDamage)
				{
					PlayerPrefs.SetInt("bestDamage", bestDamage);
				}
				ReportDamageScoreAchievements();
				if (NotificationManager.Instance.FirstJam && trafficJam)
				{
					replays = 0;
					NotificationManager.Instance.ShowTrafficJam();
				}
				else if (NotificationManager.Instance.FirstCrash && !trafficJam)
				{
					replays = 0;
					NotificationManager.Instance.ShowCrash();
				}
				else if (betterScore)
				{
					Debug.Log("BETTER SCORE");
					GameNav.Instance.BringInGameOver();
				}
				else if (betterDamage)
				{
					Debug.Log("BETTER DMG");
					GameNav.Instance.BringInGameOver();
				}
				else
				{
					GameNav.Instance.BringInGameOver();
				}
				if (CurrencyManager.GetCurrency() <= 10000000)
				{
					int currency = CurrencyManager.GetCurrency();
					cash = (score / 100 + (int)(Mathf.Sqrt(damageScore) / 10f)) * 10;
					if (currency + cash > 10000000)
					{
						cash = 10000000 - currency;
					}
					CurrencyManager.AddCurrency(cash);
				}
				SingletonBase<ChallengeData>.Instance.ReportLeaderBoardScores(score, damageScore);
				SingletonBase<TextManager>.Instance.CalculateGameOver(score, damageScore, cash);
				gameTotalCashText.Text = CurrencyManager.GetCurrency().ToString("N0");
			}
		}
		damageScore = (int)(damage * costPerDamage) * 10;
		damageText.GetComponent<SpriteText>().Text = "$" + damageScore.ToString("N0");
		clipsPlayedThisFrame = 0;
		if (score > best)
		{
			if (!betterScore)
			{
				Debug.Log("NEW HI SCORE");
			}
			betterScore = true;
			best = score;
		}
		if (damageScore > bestDamage)
		{
			if (!betterDamage)
			{
				Debug.Log("NEW HI DAMAGE");
			}
			betterDamage = true;
			bestDamage = damageScore;
		}
		if (hadExplosion)
		{
			float t = Mathf.Clamp01((Time.timeSinceLevelLoad - lightChangeStart) / 3f);
			mainSceneLight.color = Color.Lerp(startColor, afterExplosionColor, t);
		}
	}

	public void DoneExplosion()
	{
		if (!hadExplosion)
		{
			startColor = mainSceneLight.color;
			hadExplosion = true;
			lightChangeStart = Time.timeSinceLevelLoad;
		}
	}

	private void ShowNotification()
	{
		if (AppController.Instance.FirstTime)
		{
			AppController.Instance.FirstTime = false;
			PlayerPrefs.SetInt("FirstTime", 1);
			NotificationManager.Instance.Show(true);
		}
		else
		{
			NotificationManager.Instance.Show(false);
		}
	}

	public void AwardPoints(int points, int lane, bool hi)
	{
		if (gameOver)
		{
			return;
		}
		if (SingletonBase<TrafficShopItemHandler>.Instance.InsuranceFraud)
		{
			points *= 2;
		}
		switch (lane)
		{
		case 1:
			laneScore1.NewScore(points, hi);
			break;
		case 2:
			laneScore2.NewScore(points, hi);
			break;
		default:
			laneScore3.NewScore(points, hi);
			break;
		}
		score += points;
		int length = score.ToString().Length;
		gameScoreText.Text = string.Empty;
		if (length < 6)
		{
			for (int i = 0; i < 6 - length; i++)
			{
				gameScoreText.Text += '0';
			}
		}
		gameScoreText.Text += score;
	}

	public bool ShouldStop(LaneController lane)
	{
		if (gameOver)
		{
			return trafficJam;
		}
		if (lane.playerControlled)
		{
			return stop;
		}
		return noEmergencyVehicles > 0;
	}

	public SOUND GetAlarm()
	{
		return (SOUND)Random.Range(17, 21);
	}

	public void PlayCrash(float severity)
	{
		int num = (int)(severity / 100f);
		if (num > 3)
		{
			num = 3;
		}
		float num2 = severity / 100f;
		if (num2 > 1f)
		{
			num2 = 1f;
		}
		SOUND sound = (SOUND)Random.Range(8, 11);
		PlaySound(sound, true, num2);
	}

	public void PlaySkid()
	{
		if (!gameOver && Time.timeSinceLevelLoad - timeSinceStop < 0.25f)
		{
			SOUND sound = (SOUND)Random.Range(5, 7);
			PlaySound(sound);
		}
	}

	public void PlayAccelerate(string name)
	{
		if (!gameOver)
		{
			SOUND sound = SOUND.ACCELERATE_CAR;
			if (name == "truck")
			{
				sound = SOUND.ACCELERATE_TRUCK;
			}
			if (name == "sports")
			{
				sound = SOUND.ACCELERATE_SPORTS;
			}
			PlaySound(sound, Random.Range(0.8f, 0.9f));
		}
	}

	public void PlayAlarm()
	{
		SOUND sound = (SOUND)Random.Range(17, 21);
		PlaySound(sound, 0.15f);
	}

	public void PlayTankShoot()
	{
		SOUND sound = SOUND.CANNON;
		PlaySound(sound);
	}

	public void PlayFireworks()
	{
		SOUND sound = SOUND.FIREWORKS;
		PlaySound(sound);
	}

	public void PlayMaxPoints()
	{
		SOUND sound = SOUND.MAX_POINTS;
		PlaySound(sound);
	}

	public bool PlaySound(SOUND sound)
	{
		return PlaySound(sound, 1f);
	}

	public bool PlaySound(SOUND sound, float volume)
	{
		bool oneShot = true;
		return PlaySound(sound, oneShot, volume);
	}

	public bool PlaySound(SOUND sound, bool oneShot, float volume)
	{
		if (AudioPlayedThisFrame())
		{
			return false;
		}
		AudioClip audioClip = GetAudioClip(sound);
		if (audioClip == null)
		{
			return false;
		}
		if (playAudio)
		{
			playAudio = false;
			StartCoroutine(WaitToPlay(0.1f));
			base.GetComponent<AudioSource>().PlayOneShot(audioClip, volume);
		}
		return true;
	}

	private IEnumerator WaitToPlay(float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
		playAudio = true;
	}

	public AudioClip GetAudioClip(SOUND sound)
	{
		switch (sound)
		{
		case SOUND.LIGHTS_GO:
			return lightsGoClick;
		case SOUND.LIGHTS_STOP:
			return lightsStopClick;
		case SOUND.ACCELERATE_CAR:
			return accelerationSoundCar;
		case SOUND.ACCELERATE_SPORTS:
			return accelerationSoundSports;
		case SOUND.ACCELERATE_TRUCK:
			return accelerationSoundTruck;
		case SOUND.SKID_1:
			return skid1Sound;
		case SOUND.SKID_2:
			return skid2Sound;
		case SOUND.SKID_3:
			return skid3Sound;
		case SOUND.CRASH_1:
			return crash1Sound;
		case SOUND.CRASH_2:
			return crash2Sound;
		case SOUND.CRASH_3:
			return crash3Sound;
		case SOUND.CRASH_4:
			return crash4Sound;
		case SOUND.BEEP_BEEP:
			return beepBeepSound;
		case SOUND.BEEEEEEEP:
			return beeeeeeepSound;
		case SOUND.SIREN_POLICE:
			return sirenPolice;
		case SOUND.SIREN_AMBULANCE:
			return sirenAmbulance;
		case SOUND.SIREN_FIRE:
			return sirenFire;
		case SOUND.ALARM_1:
			return alarm1;
		case SOUND.ALARM_2:
			return alarm2;
		case SOUND.ALARM_3:
			return alarm3;
		case SOUND.ALARM_4:
			return alarm4;
		case SOUND.ALARM_5:
			return alarm5;
		case SOUND.EXPLOSION:
			return explosion;
		case SOUND.CANNON:
			return cannon;
		case SOUND.FIREWORKS:
			return fireworks;
		case SOUND.MAX_POINTS:
			return maxPoints;
		default:
			return null;
		}
	}

	private bool AudioPlayedThisFrame()
	{
		if (clipsPlayedThisFrame + 1 >= 3)
		{
			return true;
		}
		clipsPlayedThisFrame++;
		return false;
	}

	private void PlayAgain()
	{
		GameNav.Instance.Retry();
	}

	public VehicleController GetVehicle()
	{
		return allowedVehicles[Random.Range(0, allowedVehicles.Length)];
	}

	public void AddEmergencyVehicleInTriggerZone()
	{
		noEmergencyVehicles++;
	}

	public void RemoveEmergencyVehicleFromTriggerZone()
	{
		if (noEmergencyVehicles > 0)
		{
			noEmergencyVehicles--;
		}
	}

	private void LoadBuildings()
	{
		int num = 4;
		for (int i = 0; i < num; i++)
		{
			if (SingletonBase<TrafficShopItemHandler>.Instance.IsBuildingUnlocked(i) && SingletonBase<TrafficShopItemHandler>.Instance.IsBuildingSelected(i))
			{
				Object.Instantiate(Resources.Load("Buildings/Upgrades/" + i));
			}
			else
			{
				Object.Instantiate(Resources.Load("Buildings/Default/" + i));
			}
		}
	}

	private void CameraAnimations()
	{
		mainTrafficCamera.GetComponent<Animation>().Play();
	}

	private void ReportDamageScoreAchievements()
	{
		if (bestDamage >= 25000)
		{
			SingletonBase<ChallengeData>.Instance.ReportAchievement(ChallengeData.AchievementIDDamage1);
		}
		if (bestDamage >= 100000)
		{
			SingletonBase<ChallengeData>.Instance.ReportAchievement(ChallengeData.AchievementIDDamage2);
		}
		if (bestDamage >= 500000)
		{
			SingletonBase<ChallengeData>.Instance.ReportAchievement(ChallengeData.AchievementIDDamage3);
		}
		if (bestDamage >= 1000000)
		{
			SingletonBase<ChallengeData>.Instance.ReportAchievement(ChallengeData.AchievementIDDamage4);
		}
		if (bestDamage >= 3000000)
		{
			SingletonBase<ChallengeData>.Instance.ReportAchievement(ChallengeData.AchievementIDDamage5);
		}
		if (best >= 1000)
		{
			SingletonBase<ChallengeData>.Instance.ReportAchievement(ChallengeData.AchievementIDScore1);
		}
		if (best >= 5000)
		{
			SingletonBase<ChallengeData>.Instance.ReportAchievement(ChallengeData.AchievementIDScore2);
		}
		if (best >= 25000)
		{
			SingletonBase<ChallengeData>.Instance.ReportAchievement(ChallengeData.AchievementIDScore3);
		}
		if (best >= 50000)
		{
			SingletonBase<ChallengeData>.Instance.ReportAchievement(ChallengeData.AchievementIDScore4);
		}
		if (best >= 100000)
		{
			SingletonBase<ChallengeData>.Instance.ReportAchievement(ChallengeData.AchievementIDScore5);
		}
	}
}
