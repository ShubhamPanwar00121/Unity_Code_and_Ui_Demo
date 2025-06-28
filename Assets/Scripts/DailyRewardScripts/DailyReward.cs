using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyReward : MonoBehaviour
{
    [SerializeField] private DailyRewardItem weekdayRewardItem;
    [SerializeField] private DailyRewardWeekendItem weekendRewardItem;
    [SerializeField] private Transform weekdayRewardsParent;
    [SerializeField] private List<RewardIcon> rewardIcons;
    [SerializeField] private Transform content, noInternet, loading;
    [SerializeField] private ClaimRewardAnimation claimRewardAnimation;
    private List<RewardData> regularDays = new List<RewardData>();
    private RewardData specialDay7;
    private bool initialized = false;
    private bool clearAllClaimed = false;

    private void OnEnable()
    {
        EventHandlerCustom.AnimateReward += AnimateClaimReward;
    }

    private void OnDisable()
    {
        EventHandlerCustom.AnimateReward -= AnimateClaimReward;
    }

    private IEnumerator Init()
    {
        content.gameObject.SetActive(false);
        loading.gameObject.SetActive(true);
        noInternet.gameObject.SetActive(false);

        if (LoadRewardData())
        {
            yield return StartCoroutine(TimeFetcher.GetOnlineTimestamp(OnServerTimeReceived));
            SetWeekDays();
            SetWeekendDay();

            content.gameObject.SetActive(true);
            loading.gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(false);
        }

        yield return null;
    }

    private void Update()
    {
        if (!initialized)
        {
            if (IsConnectedToInternet())
            {
                initialized = true;
                StartCoroutine(Init());
            }
            else
            {
                content.gameObject.SetActive(false);
                loading.gameObject.SetActive(false);
                noInternet.gameObject.SetActive(true);
            }
        }
    }

    bool LoadRewardData()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("DailyRewardData"); // real game me RC se fetch kar sakte hain data, fail hone par local resource se utha sakte hain
       
        if (jsonFile != null)
        {
            RewardDataList dataList = JsonUtility.FromJson<RewardDataList>(jsonFile.text);
            foreach (var rewardData in dataList.RewardsData)
            {
                if (rewardData.Day == 7)
                {
                    specialDay7 = rewardData;
                }
                else if (rewardData.Day >= 1 && rewardData.Day <= 6)
                {
                    regularDays.Add(rewardData);
                }
            }

            regularDays.Sort((a, b) => a.Day.CompareTo(b.Day));

            return true;
        }
        else
        {
            Debug.LogError("Failed to load reward_data.json from Resources!");
        }

        return false;
    }

    void SetWeekDays()
    {
        foreach (RewardData rewardData in regularDays)
        {
            DailyRewardItem dailyRewardWeekdayItem = Instantiate(weekdayRewardItem, weekdayRewardsParent).GetComponent<DailyRewardItem>();
            dailyRewardWeekdayItem.AddReward(rewardData.Day, rewardData.Rewards[0].RewardName, rewardData.Rewards[0].Quantity, GetRewardSpriteById(rewardData.Rewards[0].iconId));
            
            if (clearAllClaimed) 
                dailyRewardWeekdayItem.ClearClaimed();

            if (rewardData.Day <= DataStorage.UnlockedRewardsThisWeek)
                dailyRewardWeekdayItem.SetAvailable();
        }
    }

    void SetWeekendDay()
    {
        foreach (Reward reward in specialDay7.Rewards)
        {
            weekendRewardItem.AddReward(specialDay7.Day, "specialDay7", reward.Quantity, GetRewardSpriteById(reward.iconId));
        }

        if (clearAllClaimed)
            weekendRewardItem.ClearClaimed();

        if (specialDay7.Day <= DataStorage.UnlockedRewardsThisWeek)
            weekendRewardItem.SetAvailable();
    }

    Sprite GetRewardSpriteById(string id)
    {
        foreach(RewardIcon rewardIcon in rewardIcons)
        {
            if (string.Equals(id,rewardIcon.IconId))
                return rewardIcon.Icon;
        }

        return null;
    }

    void OnServerTimeReceived(long timestamp)
    {
        if (timestamp > DataStorage.DailyRewardTime + DataStorage.DailyRewardNewClaimTime)
        {
            DataStorage.DailyRewardTime = timestamp;
            DataStorage.UnlockedRewardsThisWeek++;

            if (DataStorage.UnlockedRewardsThisWeek > 7 && DataStorage.ClaimedRewardsThisWeek >= 7)
            { 
                DataStorage.UnlockedRewardsThisWeek = 1; 
                DataStorage.ClaimedRewardsThisWeek = 0;
                clearAllClaimed = true;
            }

            else if (DataStorage.UnlockedRewardsThisWeek > 7)
                DataStorage.UnlockedRewardsThisWeek = 7;
        }
    }

    bool IsConnectedToInternet()
    {
        return Application.internetReachability != NetworkReachability.NotReachable;
    }

    void AnimateClaimReward(Sprite reward)
    {
        claimRewardAnimation.gameObject.SetActive(true);
        claimRewardAnimation.SetAnim(reward);
    }
}

[Serializable]
public class RewardDataList
{
    public List<RewardData> RewardsData; // in future agar kisi or day bhi more than one reward dene padenge to json ka format same rahega
}

[Serializable]
public struct RewardData
{
    public int Day;
    public List<Reward> Rewards;
}

[Serializable]
public struct Reward
{
    public string RewardName;
    public int Quantity;
    public string iconId;
}

[Serializable]
public struct RewardIcon
{
    public string IconId;
    public Sprite Icon;
}