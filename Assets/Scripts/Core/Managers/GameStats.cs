using System;

namespace Core.Managers
{
    public class GameStats
    {
        public int CurrentLevel { get; private set; }
        public int CurrentExp { get; private set; }
        public int ToLevelUpExp { get; private set; }
        public int CollectedCoins { get; private set; }

        public float ProgressPercent => (float)CurrentExp / ToLevelUpExp;

        public event Action OnLevelUp; 
        public event Action OnExpUpdate; 
        public event Action OnCoinsUpdate; 

        public GameStats()
        {
            CollectedCoins = 0;
            CurrentLevel = 1;
            CurrentExp = 0;
            ToLevelUpExp = 30;
        }

        public void AddExp(int amount)
        {
            CurrentExp += amount;
            while (CurrentExp >= ToLevelUpExp)
            {
                CurrentLevel++;
                CurrentExp -= ToLevelUpExp;
                ToLevelUpExp += 30; // TODO: to global constance or smth like
                OnLevelUp?.Invoke();
            }
            OnExpUpdate?.Invoke();
        }

        public void AddCoins(int amount)
        {
            CollectedCoins += amount;
            OnCoinsUpdate?.Invoke();
        }
    }
}
