using System.Collections;
using System.Net.NetworkInformation;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Grid;

public class UIManager : MonoBehaviour
{
    public enum UIState {
        STAT,DIALOG,DICTIONARY,SHOP,FORGE,EVENT,TAROT,STANDBY,SETTING,FAIL
    };
    public enum sentenceState
    {
        TEXT, EVENT, END,
    };

    public UIState State = UIState.STAT;
    [Header("UI显示数据对象")]
    public Text goldStat;
    public Text key1Stat;
    public Text key2Stat;
    public Text key3Stat;
    public Text HpStat;
    public Text AtkStat;
    public Text DefStat;
    public Text gridName;
    public Text gridStat;
    public Text monsterAbilityStat;
    public Text completeStat;
    public Text layerStat;
    public Slider completeSlider;
    [Header("UI主体对象")]
    public GameObject forgeMain;
    public GameObject statMain;
    public GameObject dialogMain;
    public GameObject goForgeButton;
    public GameObject shopMain;
    public GameObject buttonMain;
    public GameObject dictionaryMain;
    public GameObject eventMain;
    public GameObject backGroundMain;
    public GameObject tarotMain;
    public GameObject tileMain;
    public GameObject settingMain;
    public GameObject offsetPanel;
    public GameObject failMain;
    [Header("Manager")]
    public GameObject audioManager;
    public GameObject[] haloSlots;

    private float fadeDuration = 0.5f;
    public CanvasGroup fadeCanvasGroup;

    void Start() {
    }
    
    void Update() {
        updatePlayerKeyText();
        updateGridStat();
        updateCompleteStatSlide();
        updateLayerName();
    }
    public void InitializeUI() {
        gridStat.text = " ";
        monsterAbilityStat.text = " ";
        goForgeButton.SetActive(false);
        settingMain.SetActive(false);
        failMain.SetActive(false);
    }

    void updateLayerName() {
        string temp = "第" + GameData.layer.ToString() + "层\n";
        switch (GameData.layer) {
            case 1:
                temp += "魔女隐居之地";
                break;
            case 2:
                temp += "倒吊人的阵枢之座";
                break;
            case 3:
                temp += "月亮教会";
                break;
            case 4:
                temp += "狂猎的驻扎地";
                break;
            case 5:
                temp += "灵魂坟场";
                break;
            case 6:
                temp += "征服者广场";
                break;
        }
        layerStat.text = temp;
    }
    void updateCompleteStatSlide() {
        //更新进度条 更新铁匠铺
        completeStat.text = GameData.eventEncounter.ToString() + "/" + GameData.eventCount.ToString();
        completeSlider.value = (float)GameData.eventEncounter / GameData.eventCount;
    }
    void updatePlayerKeyText() {
        HpStat.text = GameData.playerHp.ToString();
        AtkStat.text = GameData.playerTotalAtk.ToString();
        DefStat.text = GameData.playerTotalDef.ToString();
        key1Stat.text = GameData.key1.ToString();
        key2Stat.text = GameData.key2.ToString();
        key3Stat.text = GameData.key3.ToString();
        goldStat.text = GameData.gold.ToString();
    }
    void updateGridStat() {
        if (this.GetComponent<GameManager>().objectClick == null) {
            gridName.text = " ";
            gridStat.text = " ";
            monsterAbilityStat.text = " ";
            offsetPanel.gameObject.SetActive(false);
        }

        if (this.GetComponent<GameManager>().objectClick != null) {
            if (this.GetComponent<GameManager>().objectClick.CompareTag("statGameObject")) {
                if(this.GetComponent<GameManager>().objectClick.name == "AtkVolume") {
                    offsetPanel.gameObject.SetActive(true);
                    Text offsetText = offsetPanel.transform.GetChild(0).GetComponent<Text>();
                    offsetText.text = GameData.playerAtk.ToString() + "+" + GameData.atkOffsetInt.ToString() + "=" + GameData.playerTotalAtk + "\n";
                    if (GameData.isDeathBuff) offsetText.text += "死神buff适用中";
                }
                if (this.GetComponent<GameManager>().objectClick.name == "DefVolume") {
                    offsetPanel.gameObject.SetActive(true);
                    Text offsetText = offsetPanel.transform.GetChild(0).GetComponent<Text>();
                    offsetText.text = GameData.playerDef.ToString() + "+" + GameData.defOffsetInt.ToString() + "=" + GameData.playerTotalDef + "\n";
                }
            }
            if (this.GetComponent<GameManager>().objectClick.CompareTag("gridGameObject") == false) return;
            GameObject objectClicked = this.GetComponent<GameManager>().objectClick;
            if (GetComponent<GameManager>().GridInMap == null) {
                GridTileManager gridTileManager = objectClicked.GetComponent<GridTileManager>();
                if(gridTileManager.gridType == GridType.BARRIER) {
                    gridName.text = "障碍物";
                    gridStat.text = "无法通过";
                }
                return;
            }
            Grid gridInMaped = GetComponent<GameManager>().GridInMap;
            if (gridInMaped.type != Grid.GridType.EVENT) {
                gridStat.fontSize = 64;
            } else {
                gridStat.fontSize = 48;
            }
            if (gridInMaped.type == Grid.GridType.MONSTER) {
                GridMonster gridMonster = (GridMonster)gridInMaped;
                int cDamage = this.GetComponent<GameManager>().CaculateDamage(gridMonster);
                gridName.text = gridMonster.name;
                if (cDamage == -1) {
                    gridStat.text =gridMonster.atk + "/" + gridMonster.def +"/" + gridMonster.hp + 
                        " \n预计伤害: ???";
                } else {
                    gridStat.text =gridMonster.atk + "/" + gridMonster.def + "/" + gridMonster.hp + 
                        " \n预计伤害:" + cDamage.ToString();
                }
                string abilitys = "";
                if (gridMonster.isLostmind) abilitys = abilitys + "魔心 ";
                if (gridMonster.isCrack) abilitys = abilitys + "碎裂 ";
                if (gridMonster.isFirmness) abilitys = abilitys + "坚定 ";
                if (gridMonster.isStalk) {
                    abilitys = abilitys + "追猎" + gridMonster.stalkTurn +" ";
                }
                if (gridMonster.isCorruptionOne) abilitys = abilitys + "腐蚀1 ";
                if (gridMonster.isCorruptionTwo) abilitys = abilitys + "腐蚀2 ";
                if (gridMonster.isCorruptionThree) abilitys = abilitys + "腐蚀3 ";
                if (gridMonster.isBoss) abilitys = abilitys + "头目 ";
                monsterAbilityStat.text = abilitys;
                return;
            }
            monsterAbilityStat.text = "";
            if (gridInMaped.type == Grid.GridType.BOTTLE) {
                switch (gridInMaped.stat) {
                    case 1:
                        gridName.text = "小血瓶";
                        gridStat.text = "拾起以恢复" + 
                            ((GridBottle)gridInMaped).healingPoints.ToString() +"点血量";
                        break;
                    case 2:
                        gridName.text = "中血瓶";
                        gridStat.text = "拾起以恢复" + 
                            ((GridBottle)gridInMaped).healingPoints.ToString() + "点血量";
                        break;
                    case 3:
                        gridName.text = "大血瓶";
                        gridStat.text = "拾起以恢复" + 
                            ((GridBottle)gridInMaped).healingPoints.ToString() + "点血量";
                        break;
                }
            }
            if (gridInMaped.type == Grid.GridType.GEM) {
                switch (gridInMaped.stat) {
                    case 1:
                        gridName.text = "攻击宝石";
                        gridStat.text = "拾起以加"+((GridGem)gridInMaped).AddSum + "点攻击力";
                        break;
                    case 2:
                        gridName.text = "防御宝石";
                        gridStat.text = "拾起以加" + ((GridGem)gridInMaped).AddSum + "点防御力";
                        break;
                }
            }
            if (gridInMaped.type == Grid.GridType.DOOR) {
                switch (gridInMaped.stat) {
                    case 1:
                        gridName.text = "上锁的青铜门";
                        gridStat.text = "一把青铜钥匙以解锁";
                        break;
                    case 2:
                        gridName.text = "上锁的白银门";
                        gridStat.text = "一把白银钥匙以解锁";
                        break;
                    case 3:
                        gridName.text = "上锁的黄金门";
                        gridStat.text = "一把黄金钥匙以解锁";
                        break;
                }
            }
            if (gridInMaped.type == Grid.GridType.KEY) {
                switch (gridInMaped.stat) {
                    case 1:
                        gridName.text = "青铜钥匙";
                        gridStat.text = "一把青铜钥匙";
                        break;
                    case 2:
                        gridName.text = "白银钥匙";
                        gridStat.text = "一把白银钥匙";
                        break;
                    case 3:
                        gridName.text = "黄金钥匙";
                        gridStat.text = "一把黄金钥匙";
                        break;
                }
            }
            if (gridInMaped.type == GridType.SHOP) {
                GridShop gridShop = (GridShop)gridInMaped;
                gridName.text = "商店";
                gridStat.text = gridShop.itemGiveOut + " " + gridShop.itemGiveOutNum + "个\n" +
                    gridShop.itemExchangeFor + " " + gridShop.itemExchangeForNum + "个";
            }
            if (gridInMaped.type == GridType.NPC) {
                gridName.text = "神秘人";
                gridStat.text = "或许会遇到什么好事呢";
            }
            if (gridInMaped.type == GridType.EVENT) {
                switch (((GridEvent)gridInMaped).eventType) {
                    case GridEvent.EventType.SOULARROW:
                        gridName.text = "拘魂之箭";
                        gridStat.text = "点击可以直接消灭一个怪物";
                        break;
                    case GridEvent.EventType.SOULGATE:
                        gridName.text = "灵魂之门";
                        gridStat.text = "点击可以把一个最前排的怪物和后面再后面的物块交换";
                        break;
                    case GridEvent.EventType.CORRUPTIONROOTSAVE:
                        gridName.text = "腐殖之根";
                        gridStat.text = "点击以存入你当前的一半血量";
                        break;
                    case GridEvent.EventType.CORRUPTIONROOTLOAD:
                        gridName.text = "腐殖之根";
                        gridStat.text = "点击以取出" + ((GridEvent)gridInMaped).hpSave +"血量并摧毁这个格子";
                        break;
                }
            }
            if (gridInMaped.type == GridType.BARRIER) {
                gridName.text = "障碍物";
                gridStat.text = "无法通过";
            }
            if (gridInMaped.type == GridType.PORTAL) {
                gridName.text = "传送门";
                gridStat.text = "点击以传送到下一层";
            }
        }
    }
    [ContextMenu("切换UI模式")]
    public void ChangeState() {
        switch (State) {
            case UIState.STAT:
                GoDialog();
                break;
            case UIState.DIALOG:
                GoStat();
                break;
        }
    }
    public void StartEvent(GridEvent gridEvent,int X,int Y) {
        GoEvent();
        eventMain.GetComponent<EventManager>().eventType = gridEvent.eventType;
        eventMain.GetComponent<EventManager>().mapX = X;
        eventMain.GetComponent<EventManager>().mapY = Y;
    }
    public void StartDialogBeforeBoss() {
        GoDialog();
        this.GetComponent<DialogManager>().ReadDialog(99);
    }
    public void StartDialogAfterBoss() {
        GoDialog();
        this.GetComponent<DialogManager>().ReadDialog(100);
    }
    public void StartDialog(int layer,int dialogStat) {
        GoDialog();
        this.GetComponent<DialogManager>().ReadDialog(layer,dialogStat);
    }
    public void StartDialog(int dialogStat) {
        GoDialog();
        this.GetComponent<DialogManager>().ReadDialog(dialogStat);
    }
    public void StartTrade(GridShop gridShop,int X,int Y) {
        GoShop();
        shopMain.GetComponent<ShopManager>().UpdateShopData(gridShop,X,Y);
    }
    public void StartForge() {
        if(GameData.layer < 2) return;
        GoForge();
        forgeMain.GetComponent<ForgeManager>().initializePrice();
    }
    public void GoDialog() {
        GoState(UIState.DIALOG);
    }
    public void GoStat() {
        GoState(UIState.STAT);
    }
    public void GoShop() {
        GoState(UIState.SHOP);
    }

    public void GoForge() {
        GoState(UIState.FORGE);
    }
    public void GoDictionary() {
        GoState(UIState.DICTIONARY);
    }
    public void GoEvent() {
        GoState(UIState.EVENT);
    }
    public void GoMenu() {
        //GoStat();
        StartCoroutine(FadeAndLoadScene("MainMenu"));
        //SceneManager.LoadScene("MainMenu");
    }
    public IEnumerator FadeAndLoadScene(string sceneTitle) {
        // 渐暗效果
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration) {
            fadeCanvasGroup.alpha = Mathf.Lerp(0f,1f,elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        fadeCanvasGroup.alpha = 1f; // 确保完全变黑
        if (sceneTitle == "Exit") {
            Application.Quit();
        } else {
            SceneManager.LoadScene(sceneTitle);
        }
    }
    public void RestartGame() {
        GoStat();
        gameObject.GetComponent<GameManager>().RestartGame();
    }
    [ContextMenu("塔罗牌STATE")]
    public void GoTarot() {
        GameData.tarotLastEquip = GameData.tarotEquip;
        GoState(UIState.TAROT);
        GameObject[] cards = GameObject.FindGameObjectsWithTag("cardGameObject");
        foreach (GameObject card in cards) {
            card.GetComponent<TarotCard>().CheckIfThisUnlock();
            card.GetComponent<TarotCard>().ClearParticle();
        }
        /*
        foreach (GameObject haloslot in haloSlots) {
            Debug.Log("haloslot");
            haloslot.SetActive(true);
        }*/
    }
    public void GoSetting() {
        GoState(UIState.SETTING);
        audioManager.GetComponent<AudioManager>().InitialVolume();
    }

    public void EndTarot() {
        //执行塔罗牌的选择时效果
        //阶段开始时的效果
        if (GameData.IsTarotEquip(this.GetComponent<TarotManager>().TarotToNum("Magician"))) {
            //魔术师
            //在你进入新的区域后，获得一把钥匙1
            Debug.Log("魔术师触发，获得一把钥匙1");
            GameData.key1++;
        }
        if (GameData.IsTarotEquip(this.GetComponent<TarotManager>().TarotToNum("Tower"))) {
            //塔
            //门1和门2会像怪物一样移动，在你将要进入一个新的区域时，立刻获得一把钥匙1和一把钥匙2
            Debug.Log("塔触发，获得一把钥匙1和一把钥匙2");
            GameData.key1++;
            GameData.key2++;
        }
        if (GameData.IsTarotEquip(this.GetComponent<TarotManager>().TarotToNum("Lovers"))) {
            //恋人
            //在你进入一个新的区域后，获得一把钥匙1，7点攻击力，7点防御力，700点生命值
            Debug.Log("恋人触发，获得一把钥匙1，7点攻击力，7点防御力，700点生命值");
            GameData.key1++;
            GameData.playerAtk += 7;
            GameData.playerDef += 7;
            GameData.playerHp += 700;
        }
        /*
        foreach (GameObject haloslot in haloSlots) {
            Debug.Log("haloslot");
            haloslot.SetActive(false);
        }
        */
        //StopAllCoroutines();
        this.GetComponent<GameManager>().UpdatePlayerOffset();
        GoStat();
        this.GetComponent<GameManager>().LayerChangeTo(GameData.layer + 1,true);
        StartDialog(0,GameData.layer);
    }
    public void GoState(UIState uistate) {
        State = uistate;
        if (uistate == UIState.DIALOG) dialogMain.SetActive(true);
        if (uistate != UIState.DIALOG) dialogMain.SetActive(false);
        if (uistate == UIState.STAT) {
            statMain.SetActive(true);
            buttonMain.SetActive(true);
            dictionaryMain.SetActive(true);
        }
        if (uistate != UIState.STAT) {
            statMain.SetActive(false);
            buttonMain.SetActive(false);
            dictionaryMain.SetActive(false);
        }
        if (uistate == UIState.SHOP) shopMain.SetActive(true);
        if (uistate != UIState.SHOP) shopMain.SetActive(false);
        if (uistate == UIState.FORGE) forgeMain.SetActive(true);
        if (uistate != UIState.FORGE) forgeMain.SetActive(false);
        if (uistate == UIState.DICTIONARY) {
            dictionaryMain.SetActive(true);
        }
        if (uistate == UIState.EVENT) eventMain.SetActive(true);
        if (uistate != UIState.EVENT) eventMain.SetActive(false);
        if (uistate != UIState.TAROT) {
            backGroundMain.SetActive(true);
            tileMain.SetActive(true);
            tarotMain.SetActive(false);
        }
        if (uistate == UIState.TAROT) {
            backGroundMain.SetActive(false);
            tarotMain.SetActive(true);
            tileMain.SetActive(false);
        }
        if (uistate == UIState.SETTING) settingMain.SetActive(true);
        if (uistate != UIState.SETTING) settingMain.SetActive(false);
        if (uistate == UIState.FAIL) failMain.SetActive(true);
        if (uistate != UIState.FAIL) failMain.SetActive(false);
    }
    public void Cheat() {
        GameData.key1++;
        GameData.key2++;
        GameData.key3++;
        GameData.gold += 1000;
        GameData.playerAtk += 100;
    }

    [Header("Pop Number")]
    public GameObject numberPrefab;
    public void PopNumber(int num, Color color, int size = 1, float intensity = 0.015f){
        GameObject number = Instantiate(numberPrefab);

        Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        number.transform.position = new Vector3(position.x, position.y, 0);
        number.transform.localScale = new Vector3(size,size,size);
        number.GetComponent<TextMeshPro>().color = color;
        number.GetComponent<TextMeshPro>().text = num.ToString();

        Vector3 direction = new Vector3(Random.Range(-1f,1f), Random.Range(0,1f), 0).normalized;
        Vector3 velocity = direction * intensity;

        StartCoroutine(NumberJumpCoroutine(number, velocity));
    }
    IEnumerator NumberJumpCoroutine(GameObject number, Vector3 initVelocity){
        float duration = 2.0f;
        float elapsed = 0.0f;
        Vector3 velocity = initVelocity;
        TextMeshPro tmp = number.GetComponent<TextMeshPro>();
        

        while(elapsed <= duration){

            velocity += new Vector3(0,-0.05f,0) * Time.deltaTime;
            number.transform.position += velocity;
            tmp.color = new Color(tmp.color.r, tmp.color.g, tmp.color.b, (duration-elapsed)/duration);

            elapsed += Time.deltaTime;
            yield return null;
        }
        Destroy(number);
    }
}
