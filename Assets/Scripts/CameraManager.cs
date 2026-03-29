using UnityEngine;

public class CameraManager
{
    private Transform cameraTf;
    private Vector3 prePos;

    public CameraManager()
    {
        cameraTf = Camera.main.transform;
        prePos = cameraTf.position;
    }

    public void SetPosition(Vector3 pos)
    {
        pos.z = cameraTf.position.z;
        cameraTf.position = pos;
    }

    public void ResetPosition()
    {
        cameraTf.position = prePos;
    }
}
