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
            dialogText.text = sentenceTexts[sentenceNumber];
            isTyping = false;
            return;
        }
        NextSentence();
    }
    public bool HandleWithSentence() {
        if (sentenceStates[sentenceNumber + 1] == sentenceState.EVENT) {
            if (sentenceNames[sentenceNumber + 1] == "UnlockTarot") {
                this.gameObject.GetComponent<TarotManager>().UnlockTarot(sentenceTexts[sentenceNumber + 1]);
            }
            if (sentenceNames[sentenceNumber + 1] == "UnlockMission") {
                this.gameObject.GetComponent<TarotManager>().UnlockMission(sentenceTexts[sentenceNumber + 1]);
            }
            //��һЩ���� Ȼ���������event
            sentenceNumber++;
            FastForwardNextSentence();
            return true;
        }
        if (sentenceStates[sentenceNumber + 1] == sentenceState.END) {
            ResetDialog();
            this.GetComponent<UIManager>().GoStat();
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
    }
    [ContextMenu("�����Ի�")]
    public void SkipDialog() {
        for (int i = sentenceNumber;i < sentenceStates.Count;i++) {
            //Debug.Log(sentenceStates[i]);
            if (sentenceStates[i] == sentenceState.EVENT) {
                if (sentenceNames[i] == "UnlockTarot") {
                    this.gameObject.GetComponent<TarotManager>().UnlockTarot(sentenceTexts[i]);
                }
                if (sentenceNames[i].ToString() == "UnlockMission") {
                    this.gameObject.GetComponent<TarotManager>().UnlockMission(sentenceTexts[i]);
                }
            }
        }
        ResetDialog();
        this.GetComponent <UIManager>().GoStat();
    }
    [ContextMenu("����Ի�")]
    public void FastForwardNextSentence() {
        if (this.GetComponent<UIManager>().State != UIState.DIALOG) return;
        if (isFastForwarding == false) return;
        if (HandleWithSentence()) return;
        HandleWithName();
        dialogText.text = sentenceTexts[sentenceNumber];
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
            currentText = fullText.Substring(0,i); // ��ȡ��0��i���ַ�
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
