using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using static UIManager;

public class DialogManager : MonoBehaviour
{
    private void Start() {
        //ReadDialog(1);
    }
    public enum sentenceState
    {
        TEXT, EVENT, END,
    };
    public GameObject nameTag;
    public GameObject dialogNameObject;
    public GameObject dialogIllustrationObject;
    public GameObject dialogIllustrationShadowObject;
    public Sprite[] illustrationSprites;
    public Sprite[] tagSprites;
    public Text dialogName;
    public Text dialogText;
    public string dialogTitle;
    public int readingDialogNumber = 1;
    public int sentenceNumber = -1;
    public List<sentenceState> sentenceStates;
    public List<string> sentenceNames = new List<string>();
    public List<string> sentenceTexts = new List<string>();
    public bool isTyping = false;
    public bool isFastForwarding = false;
    public bool isTextRed = false;

    [ContextMenu("READDIALOG")]
    public void ReadDialog() {
        ReadDialog(readingDialogNumber);
    }
    private void Update() {
        if (this.GetComponent<UIManager>().State != UIState.DIALOG) return;
        if (Input.GetKeyDown(KeyCode.Space)) {
            ClickPanel();
        }
        if (Input.GetKeyDown(KeyCode.LeftControl) && isFastForwarding == false) {
            isFastForwarding = true;
            StopAllCoroutines();
            FastForwardNextSentence();
        }
        if (Input.GetKeyUp(KeyCode.LeftControl)) {
            StopAllCoroutines();
            isFastForwarding = false;
        }

    }
    public void ResetDialog() {
        isTyping = false;
        isFastForwarding = false;
        sentenceNames.Clear();
        sentenceTexts.Clear();
        sentenceStates.Clear();
        sentenceNumber = -1;
        dialogText.text = "";
        dialogName.text = "";
    }
    public void ReadDialog(int layer,int dialogNumber) {
        ResetDialog();
        readingDialogNumber = dialogNumber;
        dialogTitle = Application.streamingAssetsPath + "/dialog" + layer + " " + readingDialogNumber + ".txt";
        Debug.Log(dialogTitle);
        string[] lines = File.ReadAllLines(dialogTitle);
        for (int i = 0;i < lines.Length;i++) {
            sentenceStates.Add((sentenceState)Enum.Parse(typeof(sentenceState),lines[i].Split(';')[0]));
            sentenceNames.Add(lines[i].Split(";")[1]);
            sentenceTexts.Add(lines[i].Split(";")[2]);
        }
        NextSentence();
    }
    public void ReadDialog(int dialogNumber) {
        ResetDialog();
        readingDialogNumber = dialogNumber;
        if (dialogNumber == 99) {
            dialogTitle = Application.streamingAssetsPath + "/dialog" + GameData.layer + " BeforeBoss.txt";
        } else if (dialogNumber == 100){
            dialogTitle = Application.streamingAssetsPath + "/dialog" + GameData.layer + " AfterBoss.txt";
        } else {
            dialogTitle = Application.streamingAssetsPath + "/dialog" + GameData.layer + " " + readingDialogNumber + ".txt";
        }
        Debug.Log(dialogTitle);
        string[] lines = File.ReadAllLines(dialogTitle);
        for (int i = 0;i < lines.Length;i++) {
            sentenceStates.Add((sentenceState)Enum.Parse(typeof(sentenceState),lines[i].Split(';')[0]));
            sentenceNames.Add(lines[i].Split(";")[1]);
            sentenceTexts.Add(lines[i].Split(";")[2]);
        }
        NextSentence();
    }
    public void ClickPanel() {
        if (isTyping) {
            StopAllCoroutines();
            dialogText.text = "\u00A0\u00A0\u00A0\u00A0" + sentenceTexts[sentenceNumber];
            isTyping = false;
            return;
        }
        NextSentence();
    }
    public bool HandleWithSentence() {
        if (sentenceStates[sentenceNumber + 1] == sentenceState.EVENT) {
            if (sentenceNames[sentenceNumber + 1] == "GAINTAROT") {
                //Debug.Log("DialogManager尝试UnlockTarot:" + sentenceTexts[sentenceNumber + 1]);
                this.gameObject.GetComponent<TarotManager>().UnlockTarot(sentenceTexts[sentenceNumber + 1]);
            }
            //干一些事情 然后跳过这个event
            sentenceNumber++;
            FastForwardNextSentence();
            return true;
        }
        if (sentenceStates[sentenceNumber + 1] == sentenceState.END) {
            if(sentenceTexts[sentenceNumber + 1] == "GOTAROT") {
                ResetDialog();
                this.GetComponent<UIManager>().GoTarot();
            } else {
                ResetDialog();
                this.GetComponent<UIManager>().GoStat();
                SaveManager.Save(0);
            }
            return true;
        }
        sentenceNumber++;
        return false;
    }
    public void HandleWithName() {
        dialogName.text = sentenceNames[sentenceNumber];
        if (dialogName.text == " ") {
            nameTag.SetActive(false);
            dialogNameObject.SetActive(false);
        } else {
            nameTag.SetActive(true);
            dialogNameObject.SetActive(false);
        }
        isTextRed = (dialogName.text == "??");
        if(isTextRed) {
            dialogText.color = Color.red;
        } else {
            dialogText.color = Color.black;
        }
        //设置nametag的sprite
        //魔女1 铁匠2 半魔人3 修女4 倒吊人5
        Image tagImage = nameTag.GetComponent<Image>();
        tagImage.sprite = dialogName.text switch {
            "魔女" => tagSprites[1],
            "铁匠" => tagSprites[2],
            "半魔人" => tagSprites[3],
            "修女" => tagSprites[4],
            "倒吊人" => tagSprites[5],
            _ => tagSprites[0],
        };
        if (tagImage.sprite == tagSprites[0]) dialogNameObject.SetActive(true);
        tagImage.SetNativeSize();
        //设置立绘的sprite
        dialogIllustrationObject.SetActive(true);
        dialogIllustrationShadowObject.SetActive(true);
        Image illuImage = dialogIllustrationObject.GetComponent<Image>();
        illuImage.sprite = dialogName.text switch {
            "魔女" => illustrationSprites[0],
            "铁匠" => illustrationSprites[1],
            "半魔人" => illustrationSprites[2],
            "修女" => illustrationSprites[3],
            "倒吊人" => illustrationSprites[4],
            "学者" => illustrationSprites[5],
            "月亮圣女" => illustrationSprites[6],
            "太阳圣女" => illustrationSprites[7],
            "月之刃" => illustrationSprites[8],
            "密探" => illustrationSprites[9],
            "十字军" => illustrationSprites[10],
            _ => null,
        };
        if (illuImage.sprite == null) {
            illuImage.gameObject.SetActive(false);
            dialogIllustrationShadowObject.SetActive(false);
        } else {
            dialogIllustrationShadowObject.GetComponent<Image>().sprite = illuImage.sprite;
            illuImage.SetNativeSize(); 
            dialogIllustrationShadowObject.GetComponent<Image>().SetNativeSize();
        }
    }
    [ContextMenu("跳过对话")]
    public void SkipDialog() {
        StopAllCoroutines();
        isTyping = false;
        for (int i = sentenceNumber;i < sentenceStates.Count;i++) {
            //Debug.Log(sentenceStates[i]);
            if (sentenceStates[i] == sentenceState.EVENT) {
                if (sentenceNames[i] == "GAINTAROT") {
                    this.gameObject.GetComponent<TarotManager>().UnlockTarot(sentenceTexts[i]);
                }
            }
            if (sentenceStates[i] == sentenceState.END) {
                if (sentenceTexts[i] == "GOTAROT") {
                    ResetDialog();
                    this.GetComponent<UIManager>().GoTarot();
                    return;
                } else {
                    ResetDialog();
                    SaveManager.Save(0);
                    this.GetComponent<UIManager>().GoStat();
                    return;
                }
            }
        }
        //ResetDialog();
        this.GetComponent <UIManager>().GoStat();
    }
    [ContextMenu("快进对话")]
    public void FastForwardNextSentence() {
        if (this.GetComponent<UIManager>().State != UIState.DIALOG) return;
        if (isFastForwarding == false) return;
        if (HandleWithSentence()) return;
        HandleWithName();
        dialogText.text = "\u00A0\u00A0\u00A0\u00A0" + sentenceTexts[sentenceNumber];
        StartCoroutine(FastForwardText());
    }
    public void NextSentence() {
        if (this.GetComponent<UIManager>().State != UIState.DIALOG) return;
        if (isFastForwarding == true) StopAllCoroutines();
        if (HandleWithSentence()) return;
        HandleWithName();
        StartCoroutine(ShowText(sentenceTexts[sentenceNumber]));
    }
    IEnumerator ShowText(string fullText) {
        isTyping = true;
        string currentText = "";
        for (int i = 0;i <= fullText.Length;i++) {
            currentText = "\u00A0\u00A0\u00A0\u00A0" + fullText.Substring(0,i); // 截取从0到i的字符
            dialogText.text = currentText; // 更新Text组件的内容
            yield return new WaitForSeconds(0.03f); // 等待指定的延迟时间
        }
        isTyping = false;
    }
    IEnumerator FastForwardText() {
        yield return new WaitForSeconds(0.08f);
        FastForwardNextSentence();
    }
}
