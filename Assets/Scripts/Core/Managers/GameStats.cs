using System;

namespace Core.Managers
{
    public class GameStats
    {
        public int CurrentLevel { get; private set; }
        public int CurrentExp { get; private set; }
        public int ToLevelUpExp { get; private set; }
        public int CollectedCoins { get; private set; }

        public float GetLevelProgress => (float)CurrentLevel / ToLevelUpExp;

        public event Action OnLevelUp; 
        public event Action OnCoinsUpdate; 

        public GameStats()
        {
            CurrentExp = 0;
            ToLevelUpExp = 10;
            ToLevelUpExp = 10;
            CollectedCoins = 0;
        }

        public void AddExp(int amount)
        {
            CurrentExp += amount;
            while (CurrentExp >= ToLevelUpExp)
            {
                CurrentLevel++;
                CurrentExp -= ToLevelUpExp;
                ToLevelUpExp += 10; // TODO: to global constance or smth like
                OnLevelUp?.Invoke();
            }
        }

        public void AddCoins(int amount)
        {
            CollectedCoins += amount;
            OnCoinsUpdate?.Invoke();
        }
    }
}
