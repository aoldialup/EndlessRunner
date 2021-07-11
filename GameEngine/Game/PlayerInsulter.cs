namespace WalmartMario
{
    class PlayerInsulter
    {
        private const string HIGH_SCORE_BEAT_INSULT = "Your new high score is pitifully low";
        private const string HIGH_SCORE_NOT_BEAT_INSULT = "You demonstrate a great deal of\nincompetence, maybe try modifying the\ncode to give yourself an advantage.";

        public string GetInsult(PlayerStats playerStats)
        {
            if (playerStats.highScoreBeaten)
            {
                return HIGH_SCORE_BEAT_INSULT;
            }
            else
            {
                return HIGH_SCORE_NOT_BEAT_INSULT;
            }
        }
    }
}