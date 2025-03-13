using UnityEngine;

public class TarotManager : MonoBehaviour
{
    public void UnlockTarot(int index) {
        GameData.tarotUnlock[index] = true;
    }
    public void LockTarot(int index) {
        GameData.tarotUnlock[index] = false;
    }
}
