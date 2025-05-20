using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPos : MonoBehaviour
{
    // xet toa do cua camera
    public void SetPos(float x , float y)
    {
        Vector3 pos = new Vector3((x - 1) / 2, (y - 1) / 2, -10);
        transform.position = pos;
        Camera.main.orthographicSize = x +1;
    }
}
