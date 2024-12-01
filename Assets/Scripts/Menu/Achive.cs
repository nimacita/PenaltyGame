using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Achive : MonoBehaviour
{
    public enum AchiveType
    {
        goals = 0,
    }

    [Header("Main Settings")]
    [SerializeField] private AchiveType achiveType;
    [SerializeField] private string achiveName;
    [SerializeField] private int reward;

    [Header("Walk Metrs")]
    [SerializeField] private int neededGoals;

    [Header("Components")]
    [SerializeField] private Button claimBtn;
    [SerializeField] private GameObject claimedImg;
    [SerializeField] private TMPro.TMP_Text btnTxt;
    [SerializeField] private TMPro.TMP_Text rewardTxt;
    [SerializeField] private TMPro.TMP_Text nameTxt;
    [SerializeField] private TMPro.TMP_Text achiveTxt;
    [SerializeField] private Slider progressSlider;


    void Start()
    {
        UpdateAchiveView();

        claimBtn.onClick.AddListener(ClaimClick);
    }


    void Update()
    {
        UpdateAchiveView();
    }

    private void UpdateAchiveView()
    {
        rewardTxt.text = $"{reward}";
        nameTxt.text = $"{achiveName}";


        if (achiveType == AchiveType.goals)
        {
            achiveTxt.text = $"Score {neededGoals} goals";
            progressSlider.value = (float)GameSettings.instance.CurrentGoals / (float)neededGoals;

            if (GameSettings.instance.CurrentGoals >= neededGoals)
            {
                if (!GameSettings.instance.IsAchiveClaimed(achiveName)) claimBtn.interactable = true;
            }
            else
            {
                claimBtn.interactable = false;
            }
        }

        if (GameSettings.instance.IsAchiveClaimed(achiveName))
        {
            //собрана
            claimBtn.interactable = false;
            claimedImg.SetActive(true);
            btnTxt.text = "Claimed";
        }
        else
        {
            btnTxt.text = "Claim";
            //не собрана
            claimedImg.SetActive(false);
        }
    }

    private void ClaimClick()
    {
        if (!GameSettings.instance.IsAchiveClaimed(achiveName))
        {
            GameSettings.instance.Coins += reward;
            SoundController.instance.PlayAddCoins();
            GameSettings.instance.SetAchiveClaimed(achiveName);
        }
    }
}
