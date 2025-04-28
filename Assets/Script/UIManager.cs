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
    [Header("UI��ʾ���ݶ���")]
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
    [Header("UI�������")]
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
        string temp = "��" + GameData.layer.ToString() + "��\n";
        switch (GameData.layer) {
            case 1:
                temp += "ħŮ����֮��";
                break;
            case 2:
                temp += "�����˵�����֮��";
                break;
            case 3:
                temp += "�����̻�";
                break;
            case 4:
                temp += "���Ե�פ����";
                break;
            case 5:
                temp += "���س�";
                break;
            case 6:
                temp += "�����߹㳡";
                break;
        }
        layerStat.text = temp;
    }
    void updateCompleteStatSlide() {
        //���½����� ����������
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
                    if (GameData.isDeathBuff) offsetText.text += "����buff������";
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
                    gridName.text = "�ϰ���";
                    gridStat.text = "�޷�ͨ��";
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
                        " \nԤ���˺�: ???";
                } else {
                    gridStat.text =gridMonster.atk + "/" + gridMonster.def + "/" + gridMonster.hp + 
                        " \nԤ���˺�:" + cDamage.ToString();
                }
                string abilitys = "";
                if (gridMonster.isLostmind) abilitys = abilitys + "ħ�� ";
                if (gridMonster.isCrack) abilitys = abilitys + "���� ";
                if (gridMonster.isFirmness) abilitys = abilitys + "�ᶨ ";
                if (gridMonster.isStalk) {
                    abilitys = abilitys + "׷��" + gridMonster.stalkTurn +" ";
                }
                if (gridMonster.isCorruptionOne) abilitys = abilitys + "��ʴ1 ";
                if (gridMonster.isCorruptionTwo) abilitys = abilitys + "��ʴ2 ";
                if (gridMonster.isCorruptionThree) abilitys = abilitys + "��ʴ3 ";
                if (gridMonster.isBoss) abilitys = abilitys + "ͷĿ ";
                monsterAbilityStat.text = abilitys;
                return;
            }
            monsterAbilityStat.text = "";
            if (gridInMaped.type == Grid.GridType.BOTTLE) {
                switch (gridInMaped.stat) {
                    case 1:
                        gridName.text = "СѪƿ";
                        gridStat.text = "ʰ���Իָ�" + 
                            ((GridBottle)gridInMaped).healingPoints.ToString() +"��Ѫ��";
                        break;
                    case 2:
                        gridName.text = "��Ѫƿ";
                        gridStat.text = "ʰ���Իָ�" + 
                            ((GridBottle)gridInMaped).healingPoints.ToString() + "��Ѫ��";
                        break;
                    case 3:
                        gridName.text = "��Ѫƿ";
                        gridStat.text = "ʰ���Իָ�" + 
                            ((GridBottle)gridInMaped).healingPoints.ToString() + "��Ѫ��";
                        break;
                }
            }
            if (gridInMaped.type == Grid.GridType.GEM) {
                switch (gridInMaped.stat) {
                    case 1:
                        gridName.text = "������ʯ";
                        gridStat.text = "ʰ���Լ�"+((GridGem)gridInMaped).AddSum + "�㹥����";
                        break;
                    case 2:
                        gridName.text = "������ʯ";
                        gridStat.text = "ʰ���Լ�" + ((GridGem)gridInMaped).AddSum + "�������";
                        break;
                }
            }
            if (gridInMaped.type == Grid.GridType.DOOR) {
                switch (gridInMaped.stat) {
                    case 1:
                        gridName.text = "��������ͭ��";
                        gridStat.text = "һ����ͭԿ���Խ���";
                        break;
                    case 2:
                        gridName.text = "�����İ�����";
                        gridStat.text = "һ�Ѱ���Կ���Խ���";
                        break;
                    case 3:
                        gridName.text = "�����Ļƽ���";
                        gridStat.text = "һ�ѻƽ�Կ���Խ���";
                        break;
                }
            }
            if (gridInMaped.type == Grid.GridType.KEY) {
                switch (gridInMaped.stat) {
                    case 1:
                        gridName.text = "��ͭԿ��";
                        gridStat.text = "һ����ͭԿ��";
                        break;
                    case 2:
                        gridName.text = "����Կ��";
                        gridStat.text = "һ�Ѱ���Կ��";
                        break;
                    case 3:
                        gridName.text = "�ƽ�Կ��";
                        gridStat.text = "һ�ѻƽ�Կ��";
                        break;
                }
            }
            if (gridInMaped.type == GridType.SHOP) {
                GridShop gridShop = (GridShop)gridInMaped;
                gridName.text = "�̵�";
                gridStat.text = gridShop.itemGiveOut + " " + gridShop.itemGiveOutNum + "��\n" +
                    gridShop.itemExchangeFor + " " + gridShop.itemExchangeForNum + "��";
            }
            if (gridInMaped.type == GridType.NPC) {
                gridName.text = "������";
                gridStat.text = "���������ʲô������";
            }
            if (gridInMaped.type == GridType.EVENT) {
                switch (((GridEvent)gridInMaped).eventType) {
                    case GridEvent.EventType.SOULARROW:
                        gridName.text = "�л�֮��";
                        gridStat.text = "�������ֱ������һ������";
                        break;
                    case GridEvent.EventType.SOULGATE:
                        gridName.text = "���֮��";
                        gridStat.text = "������԰�һ����ǰ�ŵĹ���ͺ����ٺ������齻��";
                        break;
                    case GridEvent.EventType.CORRUPTIONROOTSAVE:
                        gridName.text = "��ֳ֮��";
                        gridStat.text = "����Դ����㵱ǰ��һ��Ѫ��";
                        break;
                    case GridEvent.EventType.CORRUPTIONROOTLOAD:
                        gridName.text = "��ֳ֮��";
                        gridStat.text = "�����ȡ��" + ((GridEvent)gridInMaped).hpSave +"Ѫ�����ݻ��������";
                        break;
                }
            }
            if (gridInMaped.type == GridType.BARRIER) {
                gridName.text = "�ϰ���";
                gridStat.text = "�޷�ͨ��";
            }
            if (gridInMaped.type == GridType.PORTAL) {
                gridName.text = "������";
                gridStat.text = "����Դ��͵���һ��";
            }
        }
    }
    [ContextMenu("�л�UIģʽ")]
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
        // ����Ч��
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration) {
            fadeCanvasGroup.alpha = Mathf.Lerp(0f,1f,elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        fadeCanvasGroup.alpha = 1f; // ȷ����ȫ���
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
    [ContextMenu("������STATE")]
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
        //ִ�������Ƶ�ѡ��ʱЧ��
        //�׶ο�ʼʱ��Ч��
        if (GameData.IsTarotEquip(this.GetComponent<TarotManager>().TarotToNum("Magician"))) {
            //ħ��ʦ
            //��������µ�����󣬻��һ��Կ��1
            Debug.Log("ħ��ʦ���������һ��Կ��1");
            GameData.key1++;
        }
        if (GameData.IsTarotEquip(this.GetComponent<TarotManager>().TarotToNum("Tower"))) {
            //��
            //��1����2�������һ���ƶ������㽫Ҫ����һ���µ�����ʱ�����̻��һ��Կ��1��һ��Կ��2
            Debug.Log("�����������һ��Կ��1��һ��Կ��2");
            GameData.key1++;
            GameData.key2++;
        }
        if (GameData.IsTarotEquip(this.GetComponent<TarotManager>().TarotToNum("Lovers"))) {
            //����
            //�������һ���µ�����󣬻��һ��Կ��1��7�㹥������7���������700������ֵ
            Debug.Log("���˴��������һ��Կ��1��7�㹥������7���������700������ֵ");
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
