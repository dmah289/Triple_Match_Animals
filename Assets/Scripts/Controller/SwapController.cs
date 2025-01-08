using System;
using System.Threading.Tasks;
using DG.Tweening;
using Framework;
using Model;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Controller
{
    public class SwapController : Singleton<SwapController>
    {
        [SerializeField] private float _swapDuration = 0.25f;
        [SerializeField] private float _shakeDuration = 0.5f;
        [SerializeField] public bool IsSwapping;

        public static event Func<Tile[], bool> OnTileSwapped;
        
        public async Task SwapProcess(Tile tile1, Tile tile2)
        {
            await Swap(tile1, tile2);

            bool? swappable = OnTileSwapped?.Invoke(new Tile[2] { tile1, tile2 });
            if (swappable == false)
            {
                var sequence = DOTween.Sequence();

                sequence.Join(tile1.IconTransform.DOShakePosition(_shakeDuration, 0.05f, 10, 180, false, true, ShakeRandomnessMode.Harmonic))
                    .Join(tile2.IconTransform.DOShakePosition(_shakeDuration, 0.05f, 10, 180, false, true, ShakeRandomnessMode.Harmonic));

                await sequence.Play().AsyncWaitForCompletion();
                
                await Task.Delay(300);
                
                await Swap(tile1, tile2);
            }
        }

        private async Task Swap(Tile tile1, Tile tile2)
        {
            IsSwapping = true;
            
            SpriteRenderer icon1 = tile1.IconSpriteRenderer;
            SpriteRenderer icon2 = tile2.IconSpriteRenderer;

            Transform icon1Transform = tile1.IconTransform;
            Transform icon2Transform = tile2.IconTransform;

            var sequence = DOTween.Sequence();

            sequence.Join(icon1Transform.DOMove(icon2Transform.position, _swapDuration))
                .Join(icon2Transform.DOMove(icon1Transform.position, _swapDuration));
            
            await sequence.Play().AsyncWaitForCompletion();
            
            icon1Transform.SetParent(tile2.transform);
            icon2Transform.SetParent(tile1.transform);
            
            Item item1 = tile1.Item;
            tile1.UpdateNewIcon(tile2.Item);
            tile2.UpdateNewIcon(item1);

            await Task.Yield();
            
            IsSwapping = false;
        }
    }
}
