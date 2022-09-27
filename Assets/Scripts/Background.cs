using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AutoBattler.Managers;

public class Background : MonoBehaviour, IClickable
{
    public void Click()
    {
        CameraMovement.Instance.IsActive = true;
    }
}
