using DG.Tweening;
using RewardSystem.SO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace RewardSystem.Card
{
    public class DailyCard : MonoBehaviour
    {
        [Header("UI References")] 
        [SerializeField] private GameObject background;
        [SerializeField] private GameObject trueIcon;
        [SerializeField] private RectTransform unlockText;
        [SerializeField] private Image gameIcon;
        [SerializeField] private RawImage gameRawImage;
        [SerializeField] private VideoClip gameVideoClip = null;
        public RawImage GameRawImage => gameRawImage;
        public VideoClip GameVideoClip => gameVideoClip;
        
        private int _gameID;
        private string _backgroundPrefsKey;
        private string _trueIconPrefsKey;

        public void LoadData(DailyCardData dailyCardData)
        {
            _backgroundPrefsKey = "BackgroundKey_" + dailyCardData.gameName;
            _trueIconPrefsKey = "TrueIconKey_" + dailyCardData.gameName;

            int backgroundState = PlayerPrefs.GetInt(_backgroundPrefsKey, 1);
            int trueIconState = PlayerPrefs.GetInt(_trueIconPrefsKey, 0);

            background.SetActive(backgroundState == 1);
            trueIcon.SetActive(trueIconState == 1);
        }

        public void SetCardData(DailyCardData dailyCardData)
        {
            gameIcon.sprite = dailyCardData.gameIcon;
            gameRawImage.texture = dailyCardData.gameVideoTexture;
            gameVideoClip = dailyCardData.gameVideoClip;
        }

        public void OpenDailyCard()
        {
            background.SetActive(false);
            trueIcon.SetActive(true);

            unlockText.DOLocalMoveY(30, 2f)
                .SetEase(Ease.InOutBack).OnComplete(() => unlockText.DOLocalMoveY(0, 1f))
                .SetEase(Ease.OutBack);

            SaveCardData();
        }

        private void SaveCardData()
        {
            PlayerPrefs.SetInt(_backgroundPrefsKey, 0);
            PlayerPrefs.SetInt(_trueIconPrefsKey, 1);
            PlayerPrefs.Save();
        }
    }
}