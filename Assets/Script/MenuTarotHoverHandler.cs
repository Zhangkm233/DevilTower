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
        tarotDescription.text = tarotsDataObject.tarotsData[cardIndex].description;
        tarotImage.sprite = tarotsDataObject.tarotsData[cardIndex].sprite;
    }
}
