using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class SceneLoader : MonoBehaviour
{
    [SerializeField] private Image fadeImage;

    public const float FadeTime = 0.5f;
    private bool _isLoadingScene;

    private Sequence _sequence;

    public void SetActiveFade(bool value)
    {
        if (_isLoadingScene) return;
        _sequence.Kill();
        _sequence = DOTween.Sequence()
            .Append(fadeImage.DOFade(value ? 1 : 0, FadeTime))
            .SetEase(Ease.InOutCubic)
            .SetUpdate(true);
    }

    private void OnDestroy()
    {
        _sequence.Kill();
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(nameof(Load), sceneName);
    }

    private IEnumerator Load(string sceneName)
    {
        SetActiveFade(true);
        _isLoadingScene = true;
        
        Time.timeScale = 1;
        yield return new WaitForSeconds(FadeTime);
        yield return SceneManager.LoadSceneAsync(sceneName);
        
        _isLoadingScene = false;
        SetActiveFade(false);
    }
}