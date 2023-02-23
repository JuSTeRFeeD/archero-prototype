using UnityEngine;

namespace Core.Managers
{
    public class GameStateManager : MonoBehaviour
    {
        public bool IsPaused { get; private set; } = false;

        public void SetPause(bool value)
        {
            IsPaused = value;
            Time.timeScale = value ? 0 : 1;
        }
    }
}
