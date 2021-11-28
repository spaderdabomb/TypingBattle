using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameSceneController : MonoBehaviour
{
    public GameObject gameData;
    public GameObject winMenu;

    GameObject mainUI;
    GameObject usernamesLabel;
    GameObject writingPrompt;
    GameObject writingPromptLabel;
    GameObject writingPromptLabelInvisible;
    GameObject netWPMLabel;
    GameObject WPMLabel;
    GameObject accuracyLabel;
    GameObject[] playerUIArray;

    Text writingPromptLabelText;
    Text writingPromptLabelTextInvisible;

    List<int> errorsCharacterIndices;

    public OpponentData opponentData;

    public string[] usernames;
    string[] englishDict;
    string[] levelWordArray;
    string[] currentLineTextArray;
    string normalMainStr;
    string normalMainStrRich;
    string gameMode;
    int levelWordArraySize;
    int currentLineCharacterIndex;
    public int charactersTyped;
    public int errorsTyped;
    int characterCountLastMove;
    int errorCountLastMove;
    int charactersToWin;
    public float currentWPM;
    public float currentAccuracy;
    public float netWPM;
    float totalTimeElapsed;
    float playerPathProgressStartX;
    float playerPathProgressEndX;
    bool matchFinished;
    public bool wonMatch;

    private void Awake()
    {
        // Initialize game data
        if (GameData.instance == null)
        {
            Instantiate(gameData);
        }
        GameData.instance.Save();
        GameData.instance.ResetData();
    }

    void Start()
    {
        levelWordArraySize = 100;
        levelWordArray = new string[levelWordArraySize];
        currentLineTextArray = new string[3];
        currentLineCharacterIndex = 0;
        errorsTyped = 0;
        characterCountLastMove = 0;
        errorCountLastMove = 0;
        errorsCharacterIndices = new List<int>();
        charactersToWin = GlobalData.charactersToWinArray[GetCurrentRatingIndex()];
        matchFinished = false;
        wonMatch = false;
        opponentData = new OpponentData("Barfsweats", 1001);
        playerPathProgressStartX = 30;
        playerPathProgressEndX = 1730;


        mainUI = Core.FindGameObjectByNameAndTag("MainUI", "MainUI");
        usernamesLabel = GameObject.Find("UsernamesLabel");
        netWPMLabel = GameObject.Find("NetWPMLabel");
        WPMLabel = GameObject.Find("WPMLabel");
        accuracyLabel = GameObject.Find("AccuracyLabel");
        usernames = new string[] { "Spaderdabomb", opponentData.opponentName };
        InitGameScene("Normal", usernames);
    }


    public void InitGameScene(string mode, string[] usernames)
    {
        this.usernames = usernames;
        gameMode = mode;
        if (mode == "Practice")
        {

        }
        else if (mode == "Normal")
        {
            playerUIArray = new GameObject[2];
            GameObject playerUI = Resources.Load<GameObject>("Prefabs/PlayerUI");
            GameObject writingPromptTemp = Resources.Load<GameObject>("Prefabs/WritingPrompt");

            playerUIArray[0] = Instantiate(playerUI, mainUI.transform); 
            playerUIArray[1] = Instantiate(playerUI, mainUI.transform);
            playerUIArray[0].transform.localPosition = new Vector2(0, 190);
            playerUIArray[1].transform.localPosition = new Vector2(0, 50);
            playerUIArray[0].GetComponent<PlayerUIController>().Initialize(1, new Color32(53, 105, 222, 255), usernames[0]);
            playerUIArray[1].GetComponent<PlayerUIController>().Initialize(2, new Color32(233, 95, 89, 255), usernames[1]);

            usernamesLabel.GetComponent<Text>().text = usernames[0] + " vs. " + usernames[1];

            writingPrompt = Instantiate(writingPromptTemp, mainUI.transform);
            InitWordDictionary();
            writingPromptLabel = writingPrompt.transform.Find("WritingPromptLabel").gameObject;
            writingPromptLabelInvisible = writingPrompt.transform.Find("WritingPromptLabelInvisible").gameObject;
            string tempStr = "";
            string tempStrRich = "";
            for (int i = 0; i < levelWordArray.Length; i++)
            {
                if (i != 0) 
                { 
                    tempStr += " ";
                    tempStrRich += " ";
                    tempStr += levelWordArray[i];
                    tempStrRich += levelWordArray[i];
                }
                else
                {
                    tempStr += levelWordArray[i];
                    tempStrRich += ("<color=#" + ColorUtility.ToHtmlStringRGB(GlobalData.blueTextColor) + ">" + 
                                    levelWordArray[i].Substring(0, 1) + "</color>" + levelWordArray[i].Substring(1));
                }
            }
            normalMainStr = tempStr;
            writingPromptLabelText = writingPromptLabel.GetComponent<Text>();
            writingPromptLabelText.text = tempStrRich;
            writingPromptLabelTextInvisible = writingPromptLabelInvisible.GetComponent<Text>();
            writingPromptLabelTextInvisible.text = tempStr;
            Canvas.ForceUpdateCanvases();
            SetNewLineProperties();

        }
        else if (mode == "Battle")
        {
            
        }
    }

    void SetNewLineProperties()
    {
        for (int i = 0; i < 3; i++)
        {
            int startIndex = writingPromptLabelTextInvisible.cachedTextGenerator.lines[i].startCharIdx;
            int endIndex = (i == writingPromptLabelTextInvisible.cachedTextGenerator.lines.Count - 1) ? writingPromptLabelTextInvisible.text.Length
                            : writingPromptLabelTextInvisible.cachedTextGenerator.lines[i + 1].startCharIdx;
            int length = endIndex - startIndex;
            currentLineTextArray[i] = writingPromptLabelTextInvisible.text.Substring(startIndex, length);
        }
    }

    void UpdateRichText()
    {
        normalMainStrRich = "";
        int maxIndexError = 0;
        if (errorsCharacterIndices.Count > 0)
        {
            maxIndexError = errorsCharacterIndices.Max();
        }

        for (int i = 0; i < normalMainStr.Length; i++)
        {
            bool currentCharacterIndexIsErrorIndex = false;
            for (int j = 0; j < errorsCharacterIndices.Count; j++)
            {
                if (i == errorsCharacterIndices[j])
                {
                    currentCharacterIndexIsErrorIndex = true;
                }
            }

            if (currentCharacterIndexIsErrorIndex)
            {
                normalMainStrRich += "<color=#" + ColorUtility.ToHtmlStringRGB(GlobalData.redTextColor) + ">" +
                                     normalMainStr.Substring(i, 1) + "</color>";
            }
            else if (i == currentLineCharacterIndex)
            {
                normalMainStrRich += "<color=#" + ColorUtility.ToHtmlStringRGB(GlobalData.blueTextColor) + ">" +
                                     normalMainStr.Substring(i, 1) + "</color>";
            }
            else if (i > maxIndexError && i > currentLineCharacterIndex)
            {
                normalMainStrRich += normalMainStr.Substring(i, 1);
            }
            else
            {
                normalMainStrRich += "<color=#" + ColorUtility.ToHtmlStringRGB(GlobalData.greenTextColor) + ">" +
                                     normalMainStr.Substring(i, 1) + "</color>";
            }
        }
    }

    void InitWordDictionary()
    {
        TextAsset txt = (TextAsset)Resources.Load("Text/English");
        englishDict = txt.text.Split("\n"[0]);

        for (int i = 0; i < levelWordArraySize; i++)
        {
            int randomInt = (int)UnityEngine.Random.Range(0, englishDict.Length);
            levelWordArray[i] = englishDict[randomInt];
        }
    }

    public void CheckKeyInputToWord(char letter)
    {
        if (!matchFinished)
        {
            if (gameMode == "Practice")
            {

            }
            else if (gameMode == "Normal")
            {
                if (letter == normalMainStr[currentLineCharacterIndex])
                {
                    // Update color of character we're on
                    currentLineCharacterIndex += 1;
                    charactersTyped += 1;
                    UpdateRichText();
                    writingPromptLabelText.text = normalMainStrRich;

                    // For if we've finished a line
                    if (currentLineCharacterIndex >= currentLineTextArray[0].Length)
                    {
                        errorsCharacterIndices.Clear();
                        normalMainStr = normalMainStr.Substring(currentLineCharacterIndex);
                        currentLineCharacterIndex = 0;
                        normalMainStrRich = "<color=#" + ColorUtility.ToHtmlStringRGB(GlobalData.blueTextColor) + ">" +
                                            normalMainStr.Substring(0, 1) + "</color>" + normalMainStr.Substring(1);
                        writingPromptLabelText.text = normalMainStrRich;
                        writingPromptLabelTextInvisible.text = normalMainStr;

                        Canvas.ForceUpdateCanvases();
                        SetNewLineProperties();
                    }
                }
                else if ((int)letter == 8)
                {
                    if (currentLineCharacterIndex > 0)
                    {
                        int index = errorsCharacterIndices.IndexOf(currentLineCharacterIndex - 1);
                        if (index != -1)
                        {
                            errorsCharacterIndices.RemoveAt(index);
                            errorsTyped -= 1;
                        }
                        currentLineCharacterIndex -= 1;
                        charactersTyped -= 1;
                        UpdateRichText();
                    }
                    writingPromptLabelText.text = normalMainStrRich;
                }
                else
                {
                    errorsCharacterIndices.Add(currentLineCharacterIndex);
                    currentLineCharacterIndex += 1;
                    charactersTyped += 1;
                    errorsTyped += 1;
                    UpdateRichText();
                    writingPromptLabelText.text = normalMainStrRich;
                }

                // If space is pressed
                if ((int)letter == 32)
                {
                    MovePlayer();
                }

                // If mid-word and we type enough characters
                if (charactersTyped - errorsTyped >= charactersToWin)
                {
                    MovePlayer();
                }
            }
            else if (gameMode == "Battle")
            {

            }
        }
    }

    void MovePlayer()
    {
        float percentageMove = (((float)charactersTyped - (float)characterCountLastMove) - ((float)errorsTyped - (float)errorCountLastMove)) / (float)charactersToWin;
        playerUIArray[0].GetComponent<PlayerUIController>().MovePlayer(percentageMove);
        RectTransform playerRT = playerUIArray[0].transform.Find("PlayerPathProgress").gameObject.GetComponent<RectTransform>();
        float newWidth = playerRT.rect.width + percentageMove * (playerPathProgressEndX - playerPathProgressStartX);
        playerRT.sizeDelta = new Vector2(newWidth, playerRT.rect.height);
        characterCountLastMove = charactersTyped;
        errorCountLastMove = errorsTyped;
        if (charactersTyped - errorsTyped >= charactersToWin && !matchFinished)
        {
            matchFinished = true;
            MatchFinished();
        }
    }

    public int GetCurrentRatingIndex()
    {
        int returnIndex = 0;
        for (int i = 0; i < GlobalData.charactersToWinArray.Length; i++)
        {
            if (GameData.instance.currentRating > GlobalData.ratingTierArray[i])
            {
                returnIndex += 1;
            }
            else
            {
                break;
            }
        }

        return returnIndex;
    }

    void MatchFinished()
    {
        if (!opponentData.wonMatch)
        {
            wonMatch = true;
        }
        winMenu.GetComponent<WinMenuController>().WinGame(wonMatch);
    }

    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (!matchFinished)
        {
            if (charactersTyped == 0)
            {
                totalTimeElapsed = 0;
            }
            totalTimeElapsed += Time.fixedDeltaTime;

            // WPM
            currentWPM = (float)(60 / 5) * (charactersTyped) / totalTimeElapsed;
            WPMLabel.GetComponent<Text>().text = "WPM: " + currentWPM.ToString("0.0");

            // Accuracy
            currentAccuracy = 0;
            if (charactersTyped == 0)
            {
                currentAccuracy = 100;
            }
            else
            {
                currentAccuracy = (100.0f * (1.0f - ((float)errorsTyped / (float)charactersTyped)));
            }
            accuracyLabel.GetComponent<Text>().text = "Accuracy: " + currentAccuracy.ToString("0.0") + "%";

            // Net WPM
            netWPM = (float)(60 / 5) * ((float)charactersTyped - (float)errorsTyped) / totalTimeElapsed;
            netWPMLabel.GetComponent<Text>().text = "Net WPM: " + netWPM.ToString("0.0");
        }
    }

}
