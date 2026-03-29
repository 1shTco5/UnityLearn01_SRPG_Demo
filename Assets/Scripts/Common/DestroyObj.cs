using UnityEngine;

///<summary>
///自动删除物体
///</summary>
public class DestroyObj : MonoBehaviour
{
    public float timer;

    void Start()
    {
        Destroy(gameObject, timer);
    }
}
