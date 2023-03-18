using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;
using TMPro;
using UnityEngine.UI;

public class PlayFabManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject playFabBtn;
    public GameObject loginScreen;
    public GameObject usernamePopup;
    public TextMeshProUGUI messageText;
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public TMP_InputField usernameInput;
    public List<Button> buttonList;

    //leader board 
    public GameObject leaderBoardBtn;
    public GameObject leaderBoardPopUp;
    public GameObject LeaderBoardRowTemplate;
    public GameObject PlayerRank;
    public Transform rowsParent;

    public static bool isLoggedIn = false;
    public static bool isLoading = false;
    public static string playFabId;
    public static string username;

    public static PlayFabManager instance;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }
        instance = this;   
    }

    void Update()
    {
        playFabBtn.SetActive(!isLoggedIn);
        leaderBoardBtn.SetActive(isLoggedIn);
        foreach(Button button in buttonList)
        {
            button.interactable = !isLoading;
        }
    }

    public void OpenLoginScreen()
    {
        loginScreen.SetActive(true);
    }

    public void OnRegister()
    {
        if (String.IsNullOrEmpty(emailInput.text) || String.IsNullOrEmpty(passwordInput.text))
        {
            StartCoroutine(DisplayMessage("Invalid Inputs!"));
            return;
        }
        if (passwordInput.text.Length <6)
        {
            StartCoroutine(DisplayMessage("Password must be at least 6 characters!"));
            return;
        }
        var request = new RegisterPlayFabUserRequest
        {
            Email = emailInput.text,
            Password = passwordInput.text,
            RequireBothUsernameAndEmail = false
        };
        isLoading = true;
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnError);
        messageText.text = "processing...";
    }

    private void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        usernamePopup.SetActive(true);
        //StartCoroutine(DisplayMessage("Registered and logged in!"));
        //isLoggedIn = true;
        //StartCoroutine(WaitAndClose(2f));
    }

    public void OnLogin()
    {
        if (String.IsNullOrEmpty(emailInput.text) || String.IsNullOrEmpty(passwordInput.text))
        {
            StartCoroutine(DisplayMessage("Invalid Inputs!"));
            return;
        }
        var request = new LoginWithEmailAddressRequest
        {
            Email = emailInput.text,
            Password = passwordInput.text,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true,
                GetUserAccountInfo = true
            }
        };
        isLoading = true;
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnError);
        messageText.text = "processing...";
    }

    void OnLoginSuccess(LoginResult result)
    {
        isLoggedIn = true;
        string name = null;
        if(result.InfoResultPayload != null)
        {
            name = result.InfoResultPayload.PlayerProfile.DisplayName;
            playFabId = result.InfoResultPayload.AccountInfo.PlayFabId;
        }
        if(name == null)
        {
            //ask for name
            usernamePopup.SetActive(true);
        }
        else
        {
            username = name;
            StartCoroutine(DisplayMessage("Logged in succesfully"));
            Debug.Log("Successful login/ account create!");
            isLoggedIn = true;
            StartCoroutine(WaitAndClose(2f));
        }
    }

    public void SubmitUsername()
    {
        if (String.IsNullOrEmpty(usernameInput.text))
        {
            StartCoroutine(DisplayMessage("Invalid Inputs!"));
            return;
        }
        // Set up the update request
        UpdateUserTitleDisplayNameRequest request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = usernameInput.text
        };
        isLoading = true;
        // Update the user's display name
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnUsernameSuccess, OnUsernameFailure);
    }

    private void OnUsernameSuccess(UpdateUserTitleDisplayNameResult result)
    {
        // Display name update was successful
        username = result.DisplayName;
        StartCoroutine(DisplayMessage("Logged in successfully!"));
        Debug.Log("Logged in successfully.");
        isLoggedIn = true;
        usernamePopup.SetActive(false);
        StartCoroutine(WaitAndClose(2f));
    }

    private void OnUsernameFailure(PlayFabError error)
    {
        // Display name update failed
        Debug.LogError("Failed to add username: " + error.ErrorMessage);
        // Delete the player to undo the registration process
        StartCoroutine(DisplayMessage("Failed to add username: " + error.ErrorMessage));
        isLoading = false;
    }

    public void OnForgetPassword()
    {
        if (String.IsNullOrEmpty(emailInput.text))
        {
            StartCoroutine(DisplayMessage("Please enter your email address!"));
            return;
        }
        var request = new SendAccountRecoveryEmailRequest
        {
            Email = emailInput.text,
            TitleId = "795FA"
        };
        isLoading = true;
        PlayFabClientAPI.SendAccountRecoveryEmail(request, OnSendRecoveryEmail, OnError);
        messageText.text = "processing...";
    }

    private void OnSendRecoveryEmail(SendAccountRecoveryEmailResult result)
    {
        isLoading = false;
        StartCoroutine(DisplayMessage("Password reset mail is sent!"));
    }

    void OnError(PlayFabError error)
    {
        StartCoroutine(DisplayMessage(error.ErrorMessage));
        Debug.Log(error.GenerateErrorReport());
        isLoading = false;
    }

    public IEnumerator DisplayMessage(string msg, float seconds=5f)
    {
        messageText.text = msg;
        yield return new WaitForSeconds(seconds);
        messageText.text = null;
    }

    public IEnumerator WaitAndClose(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        isLoading = false;
        loginScreen.SetActive(false);
    }

    public void SendLeaderBoard(int star)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = "StarScore",
                    Value = star
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderBoardUpdate, OnError);
    }

    public void OnLeaderBoardUpdate(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("leaderboard updated");
    }

    public void GetLeaderboard()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = "StarScore",
            StartPosition = 0,
            MaxResultsCount = 10
        };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderBoardGet, OnError);
    }

    private void OnLeaderBoardGet(GetLeaderboardResult result)
    {
        Debug.Log("Leaderboard retreived");
        foreach(var item in result.Leaderboard)
        {
            Debug.Log(item.Position + " " + item.PlayFabId + " " + item.StatValue);
            GameObject row = Instantiate(LeaderBoardRowTemplate, rowsParent);
            TextMeshProUGUI[] texts = row.GetComponentsInChildren<TextMeshProUGUI>();
            texts[0].text = (item.Position +1).ToString();
            texts[1].text = item.DisplayName==null? item.PlayFabId.ToString() : item.DisplayName;
            texts[2].text = item.StatValue.ToString();
            row.SetActive(true);
        }

        var request = new GetLeaderboardAroundPlayerRequest
        {
            StatisticName = "StarScore",
            MaxResultsCount = 1
        };

        PlayFabClientAPI.GetLeaderboardAroundPlayer(request, OnGetPlayerLeaderboardSuccess, OnError);
    }

    private void OnGetPlayerLeaderboardSuccess(GetLeaderboardAroundPlayerResult result)
    {
        TextMeshProUGUI[] texts = PlayerRank.GetComponentsInChildren<TextMeshProUGUI>();
        texts[1].text = username;
        foreach (var items in result.Leaderboard)
        {
            if (items.PlayFabId == playFabId)
            {
                texts[0].text = (items.Position + 1).ToString();
                texts[1].text = items.DisplayName == null ? items.PlayFabId.ToString() : items.DisplayName;
                texts[2].text = items.StatValue.ToString();
            }
        }
    }

    //public void SaveGameData(int day, double money, int totalStars, int totalCustomerCount)
    //{
    //    var request =
    //}

    public void OnLeaderBoardBtn()
    {
        GetLeaderboard();
        leaderBoardPopUp.SetActive(true);
    }

    public void OnCloseLeaderBoard()
    {
        leaderBoardPopUp.SetActive(false);
        if (rowsParent.childCount > 1)
        {
            for (int i = rowsParent.childCount - 1; i >= 1; i--)
            {
                //Debug.Log(pastMealWindow.transform.GetChild(i));
                GameObject.Destroy(rowsParent.GetChild(i).gameObject);
            }
        }
    }

    public void OnContinueAsGuest()
    {
        isLoading = true;
        StartCoroutine(DisplayMessage("Continuing as guest, progress will not be saved."));
        isLoading = false;
        StartCoroutine(WaitAndClose(2f));
    }
}
