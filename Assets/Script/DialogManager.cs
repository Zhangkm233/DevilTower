using System;
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

    public void ReadDialog(int dialogNumber) {
        dialogTitle = Application.streamingAssetsPath + "/dialog" + dialogNumber +".txt";
        //dialogTitle = "Assets/Resources/dialog1.txt";
        Debug.Log(dialogTitle);
        string[] lines = File.ReadAllLines(dialogTitle);
        //Debug.Log(lines[0]);
        //Debug.Log(lines[0].Split(";")[2]);
        for (int i = 0;i < lines.Length;i++) {
            //Debug.Log(i);
            sentenceStates.Add((sentenceState)Enum.Parse(typeof(sentenceState),lines[i].Split(';')[0]));
            sentenceNames.Add(lines[i].Split(";")[1]);
            sentenceTexts.Add(lines[i].Split(";")[2]);
        }
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
        dialogText.text = sentenceTexts[sentenceNumber];
    }
}
