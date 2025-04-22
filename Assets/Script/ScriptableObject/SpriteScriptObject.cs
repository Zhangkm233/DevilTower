using System.Collections.Generic;
using UnityEngine;
using static TarotsDataObject;

[CreateAssetMenu(fileName = "SpriteScriptObject",menuName = "ScriptableObjects/SpriteScriptObject",order = 1)]
public class SpriteScriptObject : ScriptableObject
{
    [System.Serializable]
    public class SpriteData
    {
        public Grid.GridType type;
        [SerializeField]
        public Sprite[] sprites;
    }
    public List<SpriteData> spriteData = new List<SpriteData>();
}
