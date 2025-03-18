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
        ReadDialog(1);
    }
    public enum sentenceState
    {
        TEXT, EVENT, END,
    };
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
    }

    public void ReadDialog(int dialogNumber) {
        ResetDialog();
        readingDialogNumber = dialogNumber;
        dialogTitle = Application.streamingAssetsPath + "/dialog" + GameData.layer + " " + readingDialogNumber + ".txt";
        Debug.Log(dialogTitle);
        string[] lines = File.ReadAllLines(dialogTitle);
        for (int i = 0;i < lines.Length;i++) {
            sentenceStates.Add((sentenceState)Enum.Parse(typeof(sentenceState),lines[i].Split(';')[0]));
            sentenceNames.Add(lines[i].Split(";")[1]);
            sentenceTexts.Add(lines[i].Split(";")[2]);
        }
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
            //干一些事情 然后跳过这个event
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

    [ContextMenu("快进对话")]
    public void FastForwardNextSentence() {
        if (this.GetComponent<UIManager>().State != UIState.DIALOG) return;
        if (isFastForwarding == false) return;
        if (HandleWithSentence()) return;
        dialogName.text = sentenceNames[sentenceNumber];
        dialogText.text = sentenceTexts[sentenceNumber];
        StartCoroutine(FastForwardText());
    }
    public void NextSentence() {
        if (this.GetComponent<UIManager>().State != UIState.DIALOG) return;
        if (isFastForwarding == true) StopAllCoroutines();
        if (HandleWithSentence()) return;
        dialogName.text = sentenceNames[sentenceNumber];
        StartCoroutine(ShowText(sentenceTexts[sentenceNumber]));
    }
    IEnumerator ShowText(string fullText) {
        isTyping = true;
        string currentText = "";
        for (int i = 0;i <= fullText.Length;i++) {
            currentText = fullText.Substring(0,i); // 截取从0到i的字符
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
