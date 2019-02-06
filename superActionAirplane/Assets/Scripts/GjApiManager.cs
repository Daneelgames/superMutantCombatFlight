using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameJolt;
using GameJolt.UI;
using GameJolt.API;


public class GjApiManager : MonoBehaviour
{

    int scoreValue = 0;
    string scoreText = "";
    int tableID = 0;
    string extraData = "";

    public GameObject signInButton;
    public GameJolt.UI.Controllers.SignInWindow signInWindow;

    public void CancelSignIn()
    {
        GameManager.instance.SetCanStartGame(true);
        signInButton.SetActive(true);
        signInWindow.Dismiss(false);
    }

    public void SaveScore(int newScore, string newName)
    {
        scoreValue = newScore;
        scoreText = scoreValue.ToString();

        if (GameJoltAPI.Instance.CurrentUser != null)
        {
            Scores.Add(scoreValue, scoreText, tableID, extraData, (bool success) =>
            {
                Debug.Log(string.Format("Score Add {0}.", success ? "Successful" : "Failed"));
            });
        }
        else
        {
            Scores.Add(scoreValue, scoreText, newName, tableID, extraData, (bool success) => {
                Debug.Log(string.Format("Score Add {0}.", success ? "Successful" : "Failed"));
            });
        }
    }

    public void SubmitSignIn()
    {
        signInWindow.Submit();
    }
}