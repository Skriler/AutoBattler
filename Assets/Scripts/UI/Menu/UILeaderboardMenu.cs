using UnityEngine;
using UnityEngine.UI;

namespace AutoBattler.UI.Menu
{
    public class UILeaderboardMenu : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private GameObject leaderboardContent;

        [SerializeField] private InputField MemberId, PlayerScore;
        
        [Header("Parameters")]
        [SerializeField] private int id;

        private void Start()
        {
            
        }
    }
}
