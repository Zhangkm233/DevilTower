using UnityEngine;

public class BackGroundManager : MonoBehaviour
{
    public Sprite[] backGroundSprites;
    public void UpdateSprite() {
        if(GameData.layer < 0 || GameData.layer >= backGroundSprites.Length) {
            Debug.LogError("Invalid layer index: " + GameData.layer);
            return;
        }
        this.GetComponent<SpriteRenderer>().sprite = backGroundSprites[GameData.layer - 1];
    }
}
