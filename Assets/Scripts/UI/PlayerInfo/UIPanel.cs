using UnityEngine;
using AutoBattler.Managers;

namespace AutoBattler.UI.PlayerInfo
{
    public class UIPanel : MonoBehaviour
    {
        public void MouseEnter() => CameraMovement.Instance.IsOnUI = true;
        public void MouseExit() => CameraMovement.Instance.IsOnUI = false;
    }
}