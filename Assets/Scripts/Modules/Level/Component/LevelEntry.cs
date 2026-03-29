using UnityEngine;

public class LevelEntry : MonoBehaviour
{
    public int levelID; //关卡ID

    public void OnTriggerEnter2D(Collider2D other)
    {
        GameApp.EventCenter.BroadcastEvent(Defines.ShowLevelDescriptionEvent, levelID);

    }

    public void OnTriggerExit2D(Collider2D other)
    {
        GameApp.EventCenter.BroadcastEvent(Defines.HideLevelDescriptionEvent);
    }
}
