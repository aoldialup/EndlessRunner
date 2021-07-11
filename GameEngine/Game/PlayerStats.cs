namespace WalmartMario
{
    struct PlayerStats
    {
        public const int MAX_HEALTH = 3;
        public const int MIN_HEALTH = 1;

        public int currentScore;
        public int highScore;
        public bool highScoreBeaten;

        public float scoreMultiplier;
        public int killsNeededCount;
        public int killStreak;

        public int killCount;

        public int health;

        public bool isDead;
    }
}
