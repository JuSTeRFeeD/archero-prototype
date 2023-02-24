using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private Image fadeImage;

    public const float FadeTime = 0.5f;
    private bool _isLoading;
        
    public void SetActiveFade(bool value)
    {
        if (_isLoading) return;
        fadeImage.DOFade(value ? 1 : 0, FadeTime).SetEase(Ease.InOutCubic).SetUpdate(true);
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(nameof(Load), sceneName);
    }

    private IEnumerator Load(string sceneName)
    {
        SetActiveFade(true);
        _isLoading = true;
        
        Time.timeScale = 1;
        yield return new WaitForSeconds(FadeTime);
        yield return SceneManager.LoadSceneAsync(sceneName);
        
        _isLoading = false;
        SetActiveFade(false);
    }
}