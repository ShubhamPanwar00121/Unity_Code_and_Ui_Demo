using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

[Serializable]
public class WorldTime
{
    public string dateTime;
}

public static class EventHandlerCustom
{
    public static event Action<Sprite> AnimateReward;
    public static event Action<string,int,int,Sprite> PlayerScoreUpdate;

    public static void CallAnimateReward(Sprite sprite)
    {
        AnimateReward?.Invoke(sprite);
    }

    public static void CallPlayerScoreUpdate(string playerName,int score,int rank,Sprite rankBg)
    {
        PlayerScoreUpdate?.Invoke(playerName,score,rank,rankBg);
    }
}

public static class DataStorage
{
    public const string DAILY_REWARD_LAST_REWARD_TIME = "DAILY_REWARD_LAST_REWARD_TIME";
    public const string THIS_WEEK_CLAIMED_REWARDS = "THIS_WEEK_CLAIMED_REWARDS";
    public const string THIS_WEEK_UNLOCKED_REWARDS = "THIS_WEEK_UNLOCKED_REWARDS";
    public const string DAILY_REWARD_NEW_CLAIM_TIME = "DAILY_REWARD_NEW_CLAIM_TIME";

    public static long DailyRewardTime
    {
        get => long.Parse(PlayerPrefs.GetString(DAILY_REWARD_LAST_REWARD_TIME, "0"));
        set => PlayerPrefs.SetString(DAILY_REWARD_LAST_REWARD_TIME, value.ToString());
    }

    public static int ClaimedRewardsThisWeek
    {
        get => PlayerPrefs.GetInt(THIS_WEEK_CLAIMED_REWARDS, 0);
        set => PlayerPrefs.SetInt(THIS_WEEK_CLAIMED_REWARDS, value);
    }

    public static int DailyRewardNewClaimTime
    {
        get => PlayerPrefs.GetInt(DAILY_REWARD_NEW_CLAIM_TIME, 86400);
        set => PlayerPrefs.SetInt(DAILY_REWARD_NEW_CLAIM_TIME, value);
    }

    public static int UnlockedRewardsThisWeek
    {
        get => PlayerPrefs.GetInt(THIS_WEEK_UNLOCKED_REWARDS, 0);
        set => PlayerPrefs.SetInt(THIS_WEEK_UNLOCKED_REWARDS, value);
    }
}

public static class TimeFetcher
{
    public static IEnumerator GetOnlineTimestamp(Action<long> onResult)
    {
        using (UnityWebRequest www = UnityWebRequest.Get("https://timeapi.io/api/Time/current/zone?timeZone=Etc/UTC")) // sometime server is down so please try again after some time
        {
            float timeoutSeconds = 3f;
            float elapsedTime = 0f;
            var operation = www.SendWebRequest();

            while (!operation.isDone && elapsedTime < timeoutSeconds)
            {
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            if (!operation.isDone)
            {
                Debug.LogWarning("Request timed out. I did not have my own API so I am sending local time on public API fail");

                long fallbackTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                onResult?.Invoke(fallbackTimestamp);
                yield break;
            }

            if (www.result == UnityWebRequest.Result.Success)
            {
                try
                {
                    string json = www.downloadHandler.text;
                    WorldTime data = JsonUtility.FromJson<WorldTime>(json);
                    DateTime serverTime = DateTime.Parse(data.dateTime).ToUniversalTime();
                    long unixTimestamp = new DateTimeOffset(serverTime).ToUnixTimeSeconds();
                    onResult?.Invoke(unixTimestamp);
                    Debug.Log("Got current time from server.");
                }
                catch (Exception e)
                {
                    Debug.LogWarning("Parsing error: " + e.Message);
                    long fallbackTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                    onResult?.Invoke(fallbackTimestamp);
                }
            }
            else
            {
                Debug.LogWarning("Request failed: " + www.error);
                long fallbackTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                onResult?.Invoke(fallbackTimestamp);
            }
        }
    }
}

public static class GetRandoms
{
    private static string[] randomNames = {
    "Fast", "Carrot", "Car", "Bunny", "Rocket",
    "Bubble", "Zebra", "Cactus", "Waffle", "Penguin",
    "Jelly", "Pickle", "Noodle", "Marble", "Fluffy",
    "Drizzle", "Snail", "Echo", "Giggle", "Toast",
    "Whisker", "Taco", "Muffin", "Pancake", "Sprinkle",
    "Gadget", "Banana", "Pebble", "Doodle", "Fudge",
    "Pumpkin", "Slinky", "Tornado", "Daisy", "Oreo",
    "Cricket", "Twinkle", "Lemon", "Mocha", "Biscuit",
    "Glitch", "Fizzy", "Twirl", "Wiggle", "S'more",
    "Tinsel", "Skippy", "Jumper", "Pudding", "Marshmallow",
    "Snuggle", "Peanut", "Cloud", "Toffee", "Scooter",
    "Froggy", "Blizzard", "Tumble", "Pebbles", "Gummy",
    "Wobble", "Cocoa", "Rascal", "Blip", "Zigzag",
    "Snappy", "Wafflecone", "Zoodle", "Popcorn", "Dumpling",
    "Bongo", "Churro", "Whistle", "Clumsy", "SlinkyPop",
    "Hiccup", "Crumpet", "Boogie", "Flicker", "Tickle",
    "Yoyo", "Bloop", "Syrup", "Ketchup", "Mochi",
    "Sprout", "Doodlebug", "Zappy", "Wizzle", "Scooby",
    "Macaroni", "Blinky", "Crispy", "Scooch", "Fiddle",
    "Gizmo", "Jellybean", "Plop", "Bubbles", "Sparkle"
};

    public static string GetRandomName()
    {
        int index1 = UnityEngine.Random.Range(0, randomNames.Length);
        int index2 = UnityEngine.Random.Range(0, randomNames.Length);

        return randomNames[index1] + randomNames[index2] + UnityEngine.Random.Range(0,100).ToString();
    }

    public static int GetRandomScore()
    {
        return UnityEngine.Random.Range(10, 10000);
    }
}