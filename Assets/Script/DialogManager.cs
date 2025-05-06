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
                //Debug.Log("DialogManager����UnlockTarot:" + sentenceTexts[sentenceNumber + 1]);
                this.gameObject.GetComponent<TarotManager>().UnlockTarot(sentenceTexts[sentenceNumber + 1]);
            }
            //��һЩ���� Ȼ���������event
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
        //����nametag��sprite
        //ħŮ1 ����2 ��ħ��3 ��Ů4 ������5
        Image tagImage = nameTag.GetComponent<Image>();
        tagImage.sprite = dialogName.text switch {
            "ħŮ" => tagSprites[1],
            "����" => tagSprites[2],
            "��ħ��" => tagSprites[3],
            "��Ů" => tagSprites[4],
            "������" => tagSprites[5],
            _ => tagSprites[0],
        };
        if (tagImage.sprite == tagSprites[0]) dialogNameObject.SetActive(true);
        tagImage.SetNativeSize();
        //���������sprite
        dialogIllustrationObject.SetActive(true);
        dialogIllustrationShadowObject.SetActive(true);
        Image illuImage = dialogIllustrationObject.GetComponent<Image>();
        illuImage.sprite = dialogName.text switch {
            "ħŮ" => illustrationSprites[0],
            "����" => illustrationSprites[1],
            "��ħ��" => illustrationSprites[2],
            "��Ů" => illustrationSprites[3],
            "������" => illustrationSprites[4],
            "ѧ��" => illustrationSprites[5],
            "����ʥŮ" => illustrationSprites[6],
            "̫��ʥŮ" => illustrationSprites[7],
            "��֮��" => illustrationSprites[8],
            "��̽" => illustrationSprites[9],
            "ʮ�־�" => illustrationSprites[10],
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
    [ContextMenu("�����Ի�")]
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
    [ContextMenu("����Ի�")]
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
            currentText = "\u00A0\u00A0\u00A0\u00A0" + fullText.Substring(0,i); // ��ȡ��0��i���ַ�
            dialogText.text = currentText; // ����Text���������
            yield return new WaitForSeconds(0.03f); // �ȴ�ָ�����ӳ�ʱ��
        }
        isTyping = false;
    }
    IEnumerator FastForwardText() {
        yield return new WaitForSeconds(0.08f);
        FastForwardNextSentence();
    }
}
