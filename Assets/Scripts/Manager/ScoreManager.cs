using System;
using Controller;
using UnityEngine;
using UnityEngine.UI;

namespace Manager
{
    public class ScoreManager : MonoBehaviour
    {
        [SerializeField] private Text scoreText;
        [SerializeField] private int _score;

        public int Score
        {
            get => _score;
            set
            {
                if(_score == value) return;
                _score = value;
                scoreText.text = $"Score: {_score}";
            }
        }

        private void Awake()
        {
            scoreText.text = $"Score: {_score}";
        }

        private void OnEnable()
        {
            MatchController.OnTilesPoppedScore += IncreaseScore;
        }

        private void OnDisable()
        {
            MatchController.OnTilesPoppedScore -= IncreaseScore;
        }

        public void IncreaseScore(int score)
        {
            Score += score;
        }
    }
}