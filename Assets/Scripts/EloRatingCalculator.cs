using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EloRatingCalculator
{
    public static float[] CalculateEloChange(float rating1, float rating2, int winnerIndex)  // winnderIndex = 1 if rating1 player won
    {
        float[] eloResults = new float[2];

        float kfactor1 = 25;
        float kfactor2 = 25;
        if (rating1 > 2000) { kfactor1 = 10; }
        if (rating2 > 2000) { kfactor2 = 10; }

        int score1 = 1;
        int score2 = 1;
        if (winnerIndex == 1) { score2 = 0; }
        if (winnerIndex == 0) { score1 = 0; }

        float newRating1 = rating1 + kfactor1 * (score1 - (1f / (1f + Mathf.Pow(10, (rating2 - rating1) / 400))));
        float newRating2 = rating2 + kfactor2 * (score2 - (1f / (1f + Mathf.Pow(10, (rating1 - rating2) / 400))));

        eloResults[0] = newRating1;
        eloResults[1] = newRating2;

        return eloResults;
    }
}
