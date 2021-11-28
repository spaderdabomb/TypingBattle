using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentData 
{
    public string opponentName;
    public int opponentRating;

    public int charactersTyped;
    public int errorsTyped;
    public float currentWPM;
    public float currentAccuracy;
    public float netWPM;

    public bool wonMatch;

    public OpponentData(string _opponentName, int _opponentRating)
    {
        opponentName = _opponentName;
        opponentRating = _opponentRating;

        charactersTyped = 0;
        errorsTyped = 0;
        currentWPM = 0;
        currentAccuracy = 0;
        netWPM = 0;

        wonMatch = false;
    }
}
