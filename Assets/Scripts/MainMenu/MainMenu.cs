using UnityEngine;
using Zenject;

namespace MainMenu
{
     [DisallowMultipleComponent]
     public class MainMenu : MonoBehaviour
     {
          [Inject] private SceneLoader _sceneLoader;

          public void StartGame()
          {
               _sceneLoader.LoadScene("Game");
          }
     }
}
