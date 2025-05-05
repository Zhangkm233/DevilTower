using UnityEngine;
using UnityEngine.UI;

public class MenuTarotHoverHandler : MonoBehaviour
{
    public int cardIndex;
    public Text tarotName;
    public Text tarotDescription;
    public Image tarotImage;
    public TarotsDataObject tarotsDataObject;
    private void Start() {
        ChangeSelfData();
    }
    public void ChangeSelfData() {
        this.GetComponent<Image>().sprite = tarotsDataObject.tarotsData[cardIndex].sprite;
    }
    public void ChangeDisplayData() {
        tarotName.text = tarotsDataObject.tarotsData[cardIndex].cardName + "\n" + tarotsDataObject.tarotsData[cardIndex].chineseName;
        tarotDescription.text = "\u00A0\u00A0\u00A0\u00A0" + tarotsDataObject.tarotsData[cardIndex].description + "\n\n\u00A0\u00A0\u00A0\u00A0" + tarotsDataObject.tarotsData[cardIndex].flavorText;
        tarotImage.sprite = tarotsDataObject.tarotsData[cardIndex].sprite;
    }
}
