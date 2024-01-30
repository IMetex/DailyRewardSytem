using System;
using System.Collections.Generic;
using DG.Tweening;
using RewardSystem.SO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace RewardSystem.Card
{
    public class DailyCardSpawner : MonoBehaviour
    {
        [Header("Prefab Reference")] [SerializeField]
        private GameObject dailyCardPrefab;
        [Header("Parent Reference")] [SerializeField]
        private RectTransform container;
        [Header("List Reference")] 
        [SerializeField] private List<RectTransform> cardTransform;
        [SerializeField] private List<TMP_Text> dailyText;
        [SerializeField] private List<DailyCardData> dailyCardData;
        
        [Header("REF")]
        public RawImage _gameImage;
        public GameObject _gameVideoContainer;
        [SerializeField] private TMP_Text newUnlockText;
        [SerializeField] private RectTransform _videoContainer;
        [SerializeField] private TMP_Text rewardText;
        public RectTransform _button;

        private const byte CardCount = 7;
        private int _currentCardIndex = 0;
        private DailyStreakManager _dailyStreakManager;

        // PlayPref HasKeys
        private const string CountPrefsKey = "CountKey";
        private const string DailyCardDataPrefsKey = "DailyCardDataKey";

        [SerializeField] private VideoPlayer videoPlayer;

        private void Awake()
        {
            _dailyStreakManager = GetComponent<DailyStreakManager>();
        }

        private void Start()
        {
            LoadPlayerPrefs();
            StreakCard();
            PrintDaysOfWeek();
        }

        private void LoadPlayerPrefs()
        {
            dailyCardData = PlayerPrefsExtra.PlayerPrefsExtra
                .GetList<DailyCardData>(DailyCardDataPrefsKey, dailyCardData);
            _currentCardIndex = PlayerPrefs.GetInt(CountPrefsKey, _currentCardIndex);
        }

        private void CreateDailyCard()
        {
            if (cardTransform.Count >= CardCount && dailyCardData.Count >= CardCount)
            {
                for (int i = 0; i < CardCount; i++)
                {
                    var spawnedCard = Instantiate(dailyCardPrefab, cardTransform[i].position,
                        Quaternion.identity, container);
                    var dailyCardComponent = spawnedCard.GetComponent<DailyCard>();
                    dailyCardComponent.LoadData(dailyCardData[i]);
                    dailyCardComponent.SetCardData(dailyCardData[i]);
                }
            }
        }

        private void StreakCard()
        {
            if (_dailyStreakManager.IsStreak)
            {
                CreateDailyCard();
                OpenNextDailyCard();
                NewTextUnlock();
            }
            else if (_dailyStreakManager.IsStreakLose)
            {
                RemoveLostStreakOpenCards();
                CreateDailyCard();
                ResetCount();
                OpenNextDailyCard();
                NewTextUnlock();
            }

            SaveCount();
        }

        private void RemoveLostStreakOpenCards()
        {
            for (int i = _currentCardIndex - 1; i >= 0; i--)
            {
                dailyCardData?.RemoveAt(i);
            }

            PlayerPrefsExtra.PlayerPrefsExtra.SetList(DailyCardDataPrefsKey, dailyCardData);
        }

        private void SaveCount()
        {
            PlayerPrefs.SetInt(CountPrefsKey, _currentCardIndex);
            PlayerPrefs.Save();
        }

        private void OpenNextDailyCard()
        {
            var selectedCard = container.GetChild(_currentCardIndex).gameObject.GetComponent<DailyCard>();
            _gameImage.texture = selectedCard.GameRawImage.texture;
            videoPlayer.clip = selectedCard.GameVideoClip;
            videoPlayer.targetTexture = selectedCard.GameRawImage.texture as RenderTexture;
            videoPlayer.Play();
        }

        public void OpenCard()
        {
            var selectedCard = container.GetChild(_currentCardIndex).gameObject.GetComponent<DailyCard>();
            selectedCard.OpenDailyCard();
            _currentCardIndex++;
            SaveCount();
        }

        private void ResetCount()
        {
            _currentCardIndex = 0;
        }

        private void PrintDaysOfWeek()
        {
            for (int i = 0; i < CardCount; i++)
            {
                DateTime day = _dailyStreakManager.LastLoginDate.AddDays(i);

                if (_dailyStreakManager.IsStreakLose)
                {
                    dailyText[i].text = day.ToString("ddd");
                }
                else
                {
                    int newIndex = (i + _currentCardIndex) % CardCount;
                    dailyText[newIndex].text = day.ToString("ddd");
                }
            }
        }

        private void NewTextUnlock()
        {
            var startFontSize = newUnlockText.fontSize;
            var endFontSize = 100f;
            DOTween.To(() => startFontSize, x
                => newUnlockText.fontSize = x, endFontSize, 3f);
            newUnlockText.DOFade(0, 4f)
                .OnComplete(VideoContainer);
        }

        private void VideoContainer()
        {
            _gameVideoContainer.SetActive(true);
            var defaultSizeDelta = _videoContainer.sizeDelta;
            _videoContainer.DOSizeDelta(defaultSizeDelta, 0.4f)
                .From(new Vector2(1170, 2532))
                .OnComplete(() => _button.DOMoveY(50, 1f)
                    .SetEase(Ease.InOutBack).SetDelay(1f));
        }
    }
}