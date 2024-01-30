using UnityEngine;
using System;
using RewardSystem.UI;
using TMPro;

namespace RewardSystem
{
    public class DailyStreakManager : MonoBehaviour
    {
        [Header("Text Reference")] [SerializeField]
        private TMP_Text streakCountText;

        private const string StreakCountPrefsKey = "DailyStreak";
        private const string LastLoginDateKey = "LastLoginDate";
        private DateTime _lastLoginDate;
        public DateTime LastLoginDate => _lastLoginDate;
        private int _streakCount = 0;
        public int StreakCount => _streakCount;
        
        private bool _isStreak;
        private bool _isStreakLose;
        public bool IsStreak => _isStreak;
        public bool IsStreakLose => _isStreakLose;

        private UIPageController _uıPageController;

        private void Awake()
        {
            _uıPageController = GetComponent<UIPageController>();
        }

        private void Start()
        {
            LoadPlayerData();
            CheckDailyLogin();
        }

        private void CheckDailyLogin()
        {
            DateTime today = DateTime.Today;

            if (today == _lastLoginDate)
            {
                ResetStreakStatus(false, false);
                Debug.Log("Welcome back! Your streak continues.");
            }
            else if (today == _lastLoginDate.AddDays(1))
            {
                _streakCount++;
                ResetStreakStatus(true, false);
                Debug.Log("Congratulations! Your streak is now: " + _streakCount);
            }
            else
            {
                _streakCount = 1;
                ResetStreakStatus(false, true);
                Debug.Log("Oops! Your streak was reset. Start a new one!");
            }

            _lastLoginDate = today;
            SavePlayerData();
        }

        private void LoadPlayerData()
        {
            _streakCount = PlayerPrefs.GetInt(StreakCountPrefsKey, 0);
            string lastLoginDateString = PlayerPrefs.GetString(LastLoginDateKey, DateTime.MinValue.ToString());

            if (DateTime.TryParse(lastLoginDateString, out DateTime loadedDate))
            {
                _lastLoginDate = loadedDate;
            }
            else
            {
                _lastLoginDate = DateTime.MinValue;
            }
        }

        private void ResetStreakStatus(bool isStreak, bool isStreakLose)
        {
            _isStreak = isStreak;
            _isStreakLose = isStreakLose;
            if (isStreak || isStreakLose)
            {
                _uıPageController.OpenStreakPage();
                StreakDisplay();
            }
        }

        private void SavePlayerData()
        {
            PlayerPrefs.SetInt(StreakCountPrefsKey, _streakCount);
            PlayerPrefs.SetString(LastLoginDateKey, _lastLoginDate.ToString());
            PlayerPrefs.Save();
        }

        private void StreakDisplay()
        {
            streakCountText.text = _streakCount.ToString();
        }
    }
}