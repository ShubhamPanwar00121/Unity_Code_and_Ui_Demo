using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Leaderboard : MonoBehaviour
{
    [SerializeField] private Sprite gold, silver, bronze, normal;
    [SerializeField] private RectTransform playerParent;
    [SerializeField] private PlayerProfileItem playerProfileItem;
    [SerializeField] private ScrollRect scrollRect;

    private PlayerData[] playerDatas;
    private List<PlayerProfileItem> activePlayerItems = new List<PlayerProfileItem>(); // i am using it as object pool
    private float playerHeight, spacing = 60f, topSpacing = 50f, lastScrollPosSnap = 0;
    private int topIndex, bottomIndex;

    private void Start()
    {
        FillDataInArray();
        SpawnInitial();
    }

    private void FillDataInArray()
    {
        playerDatas = new PlayerData[30];
        for (int i = 0; i < playerDatas.Length; i++)
        {
            playerDatas[i] = new PlayerData
            {
                playerName = GetRandoms.GetRandomName(),
                playerScore = GetRandoms.GetRandomScore()
            };
        }

        System.Array.Sort(playerDatas, (a, b) => b.playerScore.CompareTo(a.playerScore));
    }

    private void SpawnInitial()
    {
        PlayerProfileItem p = Instantiate(playerProfileItem, playerParent);
        activePlayerItems.Add(p);
        playerHeight = p.rectTransform.rect.height;

        int toSpawn = Mathf.CeilToInt(Screen.height / (playerHeight + spacing));

        for(int i = 0; i < toSpawn; i++)
        {
            p = Instantiate(playerProfileItem, playerParent);
            activePlayerItems.Add(p);
        }

        InitialItemsPositionSetInScrollView();
    }
    
    private void InitialItemsPositionSetInScrollView()
    {
        RectTransform rect;

        for (int i = 0; i < activePlayerItems.Count; i++)
        {
            rect = activePlayerItems[i].rectTransform;
            rect.anchoredPosition = new Vector2(0, -(topSpacing + i * (playerHeight + spacing) + playerHeight / 2f));

            activePlayerItems[i].SetPlayerProfile(playerDatas[i].playerName, playerDatas[i].playerScore, i + 1, GetPlayerRankBg(i + 1));
        }

        playerParent.anchoredPosition = Vector2.zero;
        topIndex = 0;
        bottomIndex = activePlayerItems.Count - 1;
    }

    private void Update()
    {
        if (playerParent.anchoredPosition.y < 5)
        {
            playerParent.anchoredPosition = Vector2.zero;
        }

        else if (playerParent.anchoredPosition.y > (playerHeight+spacing)*(playerDatas.Length - activePlayerItems.Count + 1))
        {
            playerParent.anchoredPosition = Vector2.up * ((playerHeight + spacing) * (playerDatas.Length - activePlayerItems.Count + 1));
        }

        else if (playerParent.anchoredPosition.y > (lastScrollPosSnap + playerHeight + spacing / 2f) + topSpacing)
        {
            lastScrollPosSnap += (playerHeight + spacing);
            AddNewAtLast();
        }

        else if (playerParent.anchoredPosition.y < lastScrollPosSnap)
        {
            lastScrollPosSnap -= (playerHeight + spacing);
            AddNewAtFirst();
        }
    }

    private void AddNewAtLast()
    {
        if (bottomIndex >= playerDatas.Length-1) return;

        RectTransform lastRect = (RectTransform)playerParent.transform.GetChild(playerParent.transform.childCount - 1);
        RectTransform firstRect = (RectTransform)playerParent.transform.GetChild(0);

        firstRect.anchoredPosition = new Vector2(0, lastRect.anchoredPosition.y - (playerHeight + spacing));
        firstRect.SetAsLastSibling();

        topIndex++;
        bottomIndex++;

        firstRect.GetComponent<PlayerProfileItem>().SetPlayerProfile(playerDatas[bottomIndex].playerName, playerDatas[bottomIndex].playerScore, bottomIndex + 1, GetPlayerRankBg(bottomIndex + 1));
    }

    private void AddNewAtFirst()
    {
        if (topIndex <= 0) return;

        RectTransform lastRect = (RectTransform)playerParent.transform.GetChild(playerParent.transform.childCount - 1);
        RectTransform firstRect = (RectTransform)playerParent.transform.GetChild(0);

        lastRect.anchoredPosition = new Vector2(0, firstRect.anchoredPosition.y + (playerHeight + spacing));
        lastRect.SetAsFirstSibling();

        topIndex--;
        bottomIndex--;

        lastRect.GetComponent<PlayerProfileItem>().SetPlayerProfile(playerDatas[topIndex].playerName, playerDatas[topIndex].playerScore, topIndex + 1, GetPlayerRankBg(topIndex + 1));
    }

    private Sprite GetPlayerRankBg(int rank)
    {
        switch (rank)
        {
            case 1: return gold;
            case 2: return silver;
            case 3: return bronze;
            default: return normal;
        }
    }

    public void UpdatePlayerScore(string playerName, int newScore)
    {
        if (newScore < 0)
        {
            Debug.Log("Invalid score");
            return;
        }

        int index = -1;

        index = System.Array.FindIndex(playerDatas, p => p.playerName == playerName);

        if (index == -1) 
        {
            print("player not found");
            return;
        };

        playerDatas[index].playerScore = newScore;
        System.Array.Sort(playerDatas, (a, b) => b.playerScore.CompareTo(a.playerScore));

        //EventHandlerCustom.CallPlayerScoreUpdate(playerName, newScore, newRank, GetPlayerRankBg(newRank)); // need to rest positions also so i am instantiating again
       
        activePlayerItems.Clear();

        List<GameObject> children = new List<GameObject>();
        for (int i = 0; i < playerParent.transform.childCount; i++)
        {
            children.Add(playerParent.transform.GetChild(i).gameObject);
        }

        foreach (GameObject child in children)
        {
            DestroyImmediate(child);
        }

        SpawnInitial();
    }
}

public struct PlayerData
{
    public string playerName;
    public int playerScore;
}