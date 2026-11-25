using NeonPlay;

public sealed class CopyScript : SingletonBase<CopyScript>
{
	public struct Feedback
	{
		public string mailAddress;

		public string subject;

		public string body;
	}

	public struct Review
	{
		public string title;

		public string message;

		public string yes;

		public string no;
	}

	public struct ShareEmail
	{
		public string mailAddress;

		public string subject;

		public string body;
	}

	public struct ShareTwitter
	{
		public string loginTitle;

		public string loginMessage;

		public string loginUsername;

		public string loginPassword;

		public string optionsTweet;

		public string damageHiScore1;

		public string damageHiScore2;

		public string pointHiScore1;

		public string pointHiScore2;
	}

	public struct ShareFacebook
	{
		public string optionsTitle;

		public string optionsPost;

		public string damageHiScoreTitle1;

		public string damageHiScoreTitle2;

		public string damageHiScore;

		public string pointHiScoreTitle1;

		public string pointHiScoreTitle2;

		public string pointHiScore;
	}

	public struct Leaderboard
	{
		public string retrieving;

		public string failedToRetrieve;

		public string failedToLogin;

		public string noScoresFound;
	}

	public struct Achievements
	{
		public string retrieving;

		public string failedToRetrieve;

		public string failedToLogin;

		public string noAchievementsFound;
	}

	private Feedback feedback;

	private Review review;

	private ShareEmail email;

	private ShareTwitter twitter;

	private ShareFacebook facebook;

	private Leaderboard leaderboard;

	private Achievements achievements;

	public Feedback GetFeedback
	{
		get
		{
			return feedback;
		}
	}

	public Review GetReview
	{
		get
		{
			return review;
		}
	}

	public ShareEmail GetEmail
	{
		get
		{
			return email;
		}
	}

	public ShareTwitter GetTwitter
	{
		get
		{
			return twitter;
		}
	}

	public ShareFacebook GetFacebook
	{
		get
		{
			return facebook;
		}
	}

	public Leaderboard GetLeaderboard
	{
		get
		{
			return leaderboard;
		}
	}

	public Achievements GetAchievements
	{
		get
		{
			return achievements;
		}
	}

	private CopyScript()
	{
		feedback = default(Feedback);
		email = default(ShareEmail);
		twitter = default(ShareTwitter);
		facebook = default(ShareFacebook);
		leaderboard = default(Leaderboard);
		review = default(Review);
		achievements = default(Achievements);
		feedback.mailAddress = "support@neonplay.com";
		feedback.subject = "Traffic Panic 3D Feedback (iPhone)";
		feedback.body = "Thanks for playing! If you've got any nice things to say, we'd love to hear your comments.\n\nOr, if there's a problem, we're really sorry, but please let us know and we'll try and sort it.";
		review.title = "Submit review";
		review.message = "Would you like to review Traffic Panic 3D? We'd love it if you did!";
		review.yes = "Yes";
		review.no = "No";
		email.mailAddress = string.Empty;
		email.subject = "Play Traffic Panic 3D for iPhone/iPad!";
		email.body = "It's rush hour, but don't panic!\n\nTraffic Panic 3D brings you more decision-making destruction. Will you save the traffic or cause epic collisions?\n\nAvailable now on iPhone and iPad:\n\nhttp://www.bit.ly/TP-3D \n\nLike - http://www.facebook.com/neonplay\nFollow - http://www.twitter.com/neonplay\n";
		twitter.loginTitle = "Twitter";
		twitter.loginMessage = "Enter your Twitter details to tweet!";
		twitter.loginUsername = "username";
		twitter.loginPassword = "password";
		twitter.optionsTweet = "It's rush hour, but don't panic! Traffic Panic 3D brings you more decision making destruction for iPhone/iPad! http://bit.ly/TP-3D";
		twitter.damageHiScore1 = "I just caused ";
		twitter.damageHiScore2 = " worth of damage in Traffic Panic 3D! Cause CARNAGE in this awesome iPhone/iPad game - http://bit.ly/TP-3D";
		twitter.pointHiScore1 = "I just scored ";
		twitter.pointHiScore2 = " in Traffic Panic 3D! Can you handle the RUSH HOUR in this awesome iPhone/iPad game? - http://bit.ly/TP-3D";
		facebook.optionsTitle = "Prepare for rush hour in Traffic Panic 3D for iPhone/iPad";
		facebook.optionsPost = "Traffic Panic 3D brings you more decision making destruction! Keep the traffic flowing OR crash and cause carnage! How will you play? It's a fun and frantic game suitable for all!";
		facebook.pointHiScoreTitle1 = "I just scored ";
		facebook.pointHiScoreTitle2 = " in Traffic Panic 3D for iPhone/iPad!";
		facebook.pointHiScore = "Traffic Panic 3D brings you more decision making destruction! Keep the traffic flowing OR crash and cause carnage! How will you play? It's a fun and frantic game suitable for all!";
		facebook.damageHiScoreTitle1 = "I just caused ";
		facebook.damageHiScoreTitle2 = " worth of damage in Traffic Panic 3D!";
		facebook.damageHiScore = "Traffic Panic 3D brings you more decision making destruction! Keep the traffic flowing OR crash and cause carnage! How will you play? It's a fun and frantic game suitable for all!";
		leaderboard.retrieving = "LOADING SCORES...";
		leaderboard.failedToRetrieve = "FAILED TO LOAD SCORES, CHECK YOUR INTERNET CONNECTION";
		leaderboard.failedToLogin = "FAILED TO LOGIN TO GAMECENTER, PLEASE CHECK YOUR DETAILS";
		leaderboard.noScoresFound = "NO SCORES COULD BE FOUND, PLEASE TRY AGAIN LATER.";
		achievements.retrieving = "LOADING ACHIEVEMENTS...";
		achievements.failedToRetrieve = "FAILED TO LOAD ACHIEVEMENTS, CHECK YOUR INTERNET CONNECTION";
		achievements.failedToLogin = "FAILED TO LOGIN TO GAMECENTER, PLEASE CHECK YOUR DETAILS";
		achievements.noAchievementsFound = "NO ACHIEVEMENTS COULD BE FOUND, PLEASE TRY AGAIN LATER.";
	}
}
