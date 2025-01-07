using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    [Header("Loading Bar")]
    [SerializeField] Image _process;
    [SerializeField] float _loadingDuration;

    [Header("Logo")]
    [SerializeField] RectTransform _logo;
    [SerializeField] Vector2 _hidenPos;
    [SerializeField] Vector2 _targetPos;

    [Header("Loading Text")]
    [SerializeField] Text _loadingTxt;
    String[] _loadingTxtStatus;

    private async void Start()
    {
        await Task.Delay(1000);

        _loadingTxtStatus = new string[3] { "LOADING.", "LOADING..", "LOADING..." };

        AnimatingLogo();
        AnimateLoadingBar();
        AnimateText();
    }

    private async void AnimateText()
    {
        int idx = 0;
        float timer = 0;

        while(timer < 0.9f * _loadingDuration)
        {
            _loadingTxt.text = _loadingTxtStatus[idx];
            timer += Time.deltaTime + (0.2f * _loadingDuration);
            idx = (idx + 1) % 3;
            await Task.Delay((int)(0.2f * _loadingDuration * 1000f));
        }
        await Task.Yield();
    }

    public void AnimatingLogo()
    {
        _logo.DOAnchorPos(_targetPos, _loadingDuration * 0.8f, false).SetEase(Ease.OutBack);
        _logo.DOScale(0, _loadingDuration * 0.3f).SetEase(Ease.InBack).SetDelay(_loadingDuration*0.6f);
    }

    private async void AnimateLoadingBar()
    {
        _process.fillAmount = 0;

        AsyncOperation loading = SceneManager.LoadSceneAsync(1); 
        loading.allowSceneActivation = false;

        float timer = 0;

        while(timer < _loadingDuration)
        {
            timer += Time.deltaTime;
            _process.fillAmount = timer / _loadingDuration;
            await Task.Yield();
        }

        _process.fillAmount = 1;
        loading.allowSceneActivation = true;
    }
}