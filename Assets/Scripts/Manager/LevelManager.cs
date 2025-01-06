using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] RectTransform _boardTransform;

        private void Start()
        {
            _boardTransform.localScale = Vector3.zero;
            _boardTransform.DOScale(1, 1).SetEase(Ease.OutBounce).SetDelay(1);
        }
    }
}
