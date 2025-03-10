
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonsterProfile : MonoBehaviour
{
    public string MonsterName;
    public string intro;
    public Sprite sprite;
    public string Atk;
    public string Def;
    public string Hp;
    public int MonsterIndex;


    void Awake()
    {
        transform.Find("Intro").gameObject.SetActive(false);
    }

    public void UseData(MonsterData monsterData){
        MonsterName = monsterData.name;
        sprite = monsterData.sprite;
        intro = monsterData.intro;
        Atk = monsterData.atk.ToString();
        Def = monsterData.def.ToString();
        Hp = monsterData.hp.ToString();
        transform.Find("Name").GetComponent<TextMeshProUGUI>().text = MonsterName;
        transform.Find("Intro").GetComponent<TextMeshProUGUI>().text = intro;
        transform.Find("Sprite").GetComponent<Image>().sprite = sprite;
        transform.Find("Atk").GetComponent<TextMeshProUGUI>().text = "¹¥»÷Á¦:" + Atk;
        transform.Find("Def").GetComponent<TextMeshProUGUI>().text = "·ÀÓùÁ¦:" + Def;
        transform.Find("Hp").GetComponent<TextMeshProUGUI>().text = "ÑªÁ¿:" + Hp;
    }

    public void OnPointerEnter(){
        transform.Find("Intro").gameObject.SetActive(true);
    }

    public void OnPointerExit(){
        transform.Find("Intro").gameObject.SetActive(false);
    }
}
