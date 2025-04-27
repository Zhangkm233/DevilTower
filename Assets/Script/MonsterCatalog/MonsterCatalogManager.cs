using System.Collections;
using UnityEngine;

public class MonsterCatalogManager : MonoBehaviour
{
    public GameObject gameManager;
    public GameObject backgroundPanel;
    public GameObject[] monsterProfiles;
    public SpriteScriptObject[] layerSpritess;
    public Sprite[] backgrounds;
    public Sprite[] frames;
    public Sprite[] questionMarks;
    public MonsterDataObject monsterDataObject;
    public bool isCatalogOpen = false;

    private Vector3 OriginPos;

    void Awake()
    {
        backgroundPanel.gameObject.SetActive(false);
        OriginPos = transform.position;
        UpdateMonsterData();
    }

    public void UpdateMonsterData() {
        string txtFilePath = Application.streamingAssetsPath + "/monster" + GameData.layer + ".txt";
        string[] lines = System.IO.File.ReadAllLines(txtFilePath);
        for(int i = 0;i < lines.Length;i++) {
            string[] monsterStat = lines[i].Split(' ');
            monsterDataObject.monsterDataList[i].name =  monsterStat[0];
            monsterDataObject.monsterDataList[i].atk = int.Parse(monsterStat[1]);
            monsterDataObject.monsterDataList[i].def = int.Parse(monsterStat[2]);
            monsterDataObject.monsterDataList[i].hp = int.Parse(monsterStat[3]);
            monsterDataObject.monsterDataList[i].gold = int.Parse(monsterStat[4]); 
            monsterDataObject.monsterDataList[i].abilities = "";
            monsterDataObject.monsterDataList[i].sprite = layerSpritess[GameData.layer - 1].spriteData[3].sprites[i];
            int abilityLength = monsterStat.Length - 5;
            if (abilityLength > 0) {
                for (int j = 0;j < abilityLength;j++) {
                    switch (monsterStat[j + 5]) {
                        case "LOSTMIND":
                            monsterDataObject.monsterDataList[i].abilities += "魔心 ";
                            break;
                        case "CRACK":
                            monsterDataObject.monsterDataList[i].abilities += "碎裂 ";
                            break;
                        case "FIRMNESS":
                            monsterDataObject.monsterDataList[i].abilities += "坚定 ";
                            break;
                        case "STALK":
                            monsterDataObject.monsterDataList[i].abilities += "追猎 ";
                            break;
                        case "CORRPUTIONONE":
                            monsterDataObject.monsterDataList[i].abilities += "腐蚀I ";
                            break;
                        case "CORRPUTIONTWO":
                            monsterDataObject.monsterDataList[i].abilities += "腐蚀II ";
                            break;
                        case "CORRPUTIONTHREE":
                            monsterDataObject.monsterDataList[i].abilities += "腐蚀III ";
                            break;
                        case "BOSS":
                            monsterDataObject.monsterDataList[i].abilities += "头目 ";
                            break;
                    }
                }
            }
        }
        if (monsterDataObject.monsterDataList.Count > lines.Length) {
            for(int j = lines.Length;j < monsterDataObject.monsterDataList.Count;j++) {
                monsterDataObject.monsterDataList[j].name = null;
                monsterDataObject.monsterDataList[j].sprite = questionMarks[GameData.layer - 1];
                monsterDataObject.monsterDataList[j].atk = 0;
                monsterDataObject.monsterDataList[j].def = 0;
                monsterDataObject.monsterDataList[j].hp = 0;
                monsterDataObject.monsterDataList[j].gold = 0;
            }
        }
        //更改显示的数据
        for (int i = 0;i < monsterProfiles.Length;i++) {
            if (monsterProfiles[i].GetComponent<MonsterProfile>() != null) {
                monsterProfiles[i].GetComponent<MonsterProfile>().UseData(monsterDataObject.monsterDataList[i]);
            }
        }
    }

    public void OnButtonClicked(){
        if (isCatalogOpen){
            HideCatalog();
            //gameManager.GetComponent<UIManager>().GoStat();
        } else {
            if (gameManager.GetComponent<UIManager>().State != UIManager.UIState.STAT) return;
            gameManager.GetComponent<UIManager>().GoDictionary();
            ShowCatalog();
        }
    }

    public void ShowCatalog(){
        backgroundPanel.gameObject.SetActive(true);
        isCatalogOpen = true;
        StartCoroutine(ShowCatalogCoroutine(0.3f));
    }
    IEnumerator ShowCatalogCoroutine(float duration){
        float elaspseTime = 0;
        while(elaspseTime <= duration){
            transform.position = Vector3.Lerp(OriginPos,new Vector3(0, 0, 0), elaspseTime / duration);
            elaspseTime += Time.deltaTime;
            yield return null;
        }
        transform.position = new Vector3(0, 0, 0);
    }

    public void HideCatalog(){
        isCatalogOpen = false;
        StartCoroutine(HideCatalogCoroutine(0.3f));
    }

    IEnumerator HideCatalogCoroutine(float duration){
        float elaspseTime = 0;
        while(elaspseTime <= duration){
            transform.position = Vector3.Lerp(new Vector3(0, 0, 0), OriginPos, elaspseTime / duration);
            elaspseTime += Time.deltaTime;
            yield return null;
        }
        transform.position = OriginPos;
        backgroundPanel.gameObject.SetActive(false);
        gameManager.GetComponent<UIManager>().GoStat();
    }
}
