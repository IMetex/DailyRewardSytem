using DG.Tweening;
using RewardSystem.Card;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RewardSystem.UI
{
    public class UIPageController : MonoBehaviour
    {
        [Header("Button References")]
        [SerializeField] private Button collectBtn;
        [SerializeField] private Button rewardBtn;

        [Header("Page References")] 
        [SerializeField] private GameObject unlockGamePage;
        [SerializeField] private GameObject dailyRewardPage;
        
        [Header("Text References")] 
        [SerializeField] private TMP_Text afterStreakText;
        [SerializeField] private TMP_Text beforeStreakText;
        
        [Header("Unlock Text Reference")]
        [SerializeField] private TMP_Text rewardText;
        [Header("Button Transform Reference")]
        [SerializeField] private RectTransform rewardButton;
        
        private DailyStreakManager _dailyStreakManager;
        private DailyCardSpawner _dailyCardSpawner;

        private void Awake()
        {
            _dailyStreakManager = GetComponent<DailyStreakManager>();
            _dailyCardSpawner = GetComponent<DailyCardSpawner>();
        }

        public void OpenStreakPage()
        {
            unlockGamePage.SetActive(true);
            dailyRewardPage.SetActive(false);
            TextAnimation();
        }

        private void OnEnable()
        {
            collectBtn.onClick.AddListener(CollectButtonPressed);
            rewardBtn.onClick.AddListener(RewardButtonPressed);
        }

        private void OnDisable()
        {
            collectBtn.onClick.RemoveListener(CollectButtonPressed);
            rewardBtn.onClick.RemoveListener(RewardButtonPressed);
        }

        private void RewardButtonPressed()
        {
            dailyRewardPage.SetActive(false);
            unlockGamePage.SetActive(false);
        }

        private void CollectButtonPressed()
        {
            dailyRewardPage.SetActive(true);
            unlockGamePage.SetActive(false);
            
            NewTextUnlock();
            _dailyCardSpawner.OpenCard();
        }

        private void TextAnimation()
        {
            beforeStreakText.DOFade(0f, 2f);
            beforeStreakText.rectTransform.DOLocalMoveY(1100f, 2f);
            afterStreakText.DOFade(1f, 2f);
            afterStreakText.rectTransform.DOLocalMoveY(700f, 2f).SetEase(Ease.OutBack);
            
            beforeStreakText.text = (_dailyStreakManager.StreakCount - 1).ToString();
            afterStreakText.text = _dailyStreakManager.StreakCount.ToString();
        }
        
        private void NewTextUnlock()
        {
            var startFontSize = rewardText.fontSize;
            var endFontSize = 65;
            DOTween.To(() => startFontSize, x
                    => rewardText.fontSize = x, endFontSize, 1f)
                .OnComplete(() => rewardButton.DOMoveY(100, 2f).SetEase(Ease.InOutBack));

        }
    }
}