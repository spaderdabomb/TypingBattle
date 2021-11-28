using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class WinMenuController : MonoBehaviour
{
    [SerializeField] GameSceneController gameSceneController;
    [SerializeField] GameObject winMenuLabel;
    [SerializeField] GameObject playerUsernameLabel;
    [SerializeField] GameObject opponentUsernameLabel;

    [SerializeField] GameObject playerNetWPMLabel;
    [SerializeField] GameObject playerCharactersTypedLabel;
    [SerializeField] GameObject playerMistakesLabel;
    [SerializeField] GameObject playerRatingLabel;

    [SerializeField] GameObject opponentNetWPMLabel;
    [SerializeField] GameObject opponentCharactersTypedLabel;
    [SerializeField] GameObject opponentMistakesLabel;
    [SerializeField] GameObject opponentRatingLabel;


    private void Start()
    {
        
    }

    public void WinGame(bool wonGame)
    {
        gameObject.SetActive(true);

        float[] eloResults;
        float[] eloResultsDiff = new float[2];
        float[] eloResultsDiffAbs = new float[2];
        string player1AddCharacter = "+";
        string player2AddCharacter = "+";
        eloResults = EloRatingCalculator.CalculateEloChange(GameData.instance.currentRating, gameSceneController.opponentData.opponentRating, Convert.ToInt32(wonGame));
        eloResultsDiff[0] = eloResults[0] - GameData.instance.currentRating;
        eloResultsDiff[1] = eloResults[1] - gameSceneController.opponentData.opponentRating;
        eloResultsDiffAbs[0] = Math.Abs(eloResultsDiff[0]);
        eloResultsDiffAbs[1] = Math.Abs(eloResultsDiff[1]);

        // TODO: change and save opponent rating

        string player1ColorStr;
        string player2ColorStr;
        if (eloResultsDiff[0] > 0f)
        {
            player1ColorStr = "#5c9b43";
            player2ColorStr = "#cb2929";
            player1AddCharacter = "+";
            player2AddCharacter = "-";
        }
        else
        {
            player1ColorStr = "#cb2929";
            player2ColorStr = "#5c9b43";
            player1AddCharacter = "-";
            player2AddCharacter = "+";
        }

        playerUsernameLabel.GetComponent<Text>().text = gameSceneController.usernames[0];
        opponentUsernameLabel.GetComponent<Text>().text = gameSceneController.usernames[1];

        playerNetWPMLabel.GetComponent<Text>().text = gameSceneController.netWPM.ToString(".0");
        playerCharactersTypedLabel.GetComponent<Text>().text = (gameSceneController.charactersTyped - gameSceneController.errorsTyped).ToString();
        playerMistakesLabel.GetComponent<Text>().text = gameSceneController.errorsTyped.ToString();
        playerRatingLabel.GetComponent<Text>().text = GameData.instance.currentRating.ToString() + 
                                                      "<color=" + player1ColorStr + "> " + player1AddCharacter + " " + eloResultsDiffAbs[0].ToString("0.") + "</color>";

        opponentNetWPMLabel.GetComponent<Text>().text = gameSceneController.opponentData.netWPM.ToString(".0");
        opponentCharactersTypedLabel.GetComponent<Text>().text = (gameSceneController.opponentData.charactersTyped - gameSceneController.opponentData.errorsTyped).ToString();
        opponentMistakesLabel.GetComponent<Text>().text = gameSceneController.opponentData.errorsTyped.ToString();
        opponentRatingLabel.GetComponent<Text>().text = gameSceneController.opponentData.opponentRating.ToString() +
                                                        "<color=" + player2ColorStr + "> " + player2AddCharacter + " " + eloResultsDiffAbs[1].ToString("0.") + "</color>"; ;

        if (gameSceneController.wonMatch)
        {
            winMenuLabel.GetComponent<Text>().text = "Match won!";
        }
        else
        {
            winMenuLabel.GetComponent<Text>().text = "Match lost";

        }

        GameData.instance.currentRating = eloResults[0];
        GameData.instance.Save();
    }
}
