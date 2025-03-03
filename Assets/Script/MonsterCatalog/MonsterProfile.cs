
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonsterProfile : MonoBehaviour
{
    public string MonsterName;
    public string intro;
    public Sprite sprite;
    public int MonsterIndex;


    void Awake()
    {
        transform.Find("Intro").gameObject.SetActive(false);
    }

    public void UseData(MonsterData monsterData){
        MonsterName = monsterData.name;
        sprite = monsterData.sprite;
        intro = monsterData.intro;

        transform.Find("Name").GetComponent<TextMeshProUGUI>().text = MonsterName;
        transform.Find("Intro").GetComponent<TextMeshProUGUI>().text = intro;
        transform.Find("Sprite").GetComponent<Image>().sprite = sprite;
    }

    public void OnPointerEnter(){
        transform.Find("Intro").gameObject.SetActive(true);
    }

    public void OnPointerExit(){
        transform.Find("Intro").gameObject.SetActive(false);
    }
}
