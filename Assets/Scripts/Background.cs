using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour, IClickable
{
    public void Click()
    {
        CameraMovement.Instance.IsActive = true;
    }
}
