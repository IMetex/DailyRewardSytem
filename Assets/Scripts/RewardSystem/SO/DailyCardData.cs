using UnityEngine;
using UnityEngine.Video;

namespace RewardSystem.SO
{
    [CreateAssetMenu(fileName = "GameName", menuName = "DailyCardCreate", order = 0)]
    public class DailyCardData : UnityEngine.ScriptableObject
    {
        public Sprite gameIcon;
        public RenderTexture gameVideoTexture;
        public VideoClip gameVideoClip;
        public string gameName;
    }
}