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
    public int sentenceNumber = -1;
    public List<sentenceState> sentenceStates;
    public List<string> sentenceNames = new List<string>();
    public List<string> sentenceTexts = new List<string>();
    public float delay = 0.01F;
    public bool isTyping = false;
    public void ReadDialog(int dialogNumber) {
        dialogTitle = Application.streamingAssetsPath + "/dialog" + dialogNumber +".txt";
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
    public void NextSentence() {
        if (this.GetComponent<UIManager>().State != UIState.DIALOG) return;
        if (sentenceStates[sentenceNumber + 1] == sentenceState.EVENT) {
            //干一些事情 然后跳过这个event
            sentenceNumber++;
            NextSentence();
            return;
        }
        if (sentenceStates[sentenceNumber + 1] == sentenceState.END) {
            sentenceNumber = -1;
            this.GetComponent<UIManager>().GoStat();
            return;
        }
        sentenceNumber++;
        dialogName.text = sentenceNames[sentenceNumber];
        StartCoroutine(ShowText(sentenceTexts[sentenceNumber]));
        //dialogText.text = sentenceTexts[sentenceNumber];
    }
    IEnumerator ShowText(string fullText) {
        isTyping = true;
        string currentText = "";
        for (int i = 0;i <= fullText.Length;i++) {
            currentText = fullText.Substring(0,i); // 截取从0到i的字符
            dialogText.text = currentText; // 更新Text组件的内容
            yield return new WaitForSeconds((float)delay); // 等待指定的延迟时间
        }
        isTyping = false;
    }
}
