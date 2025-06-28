using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CheckIntegerSum : MonoBehaviour
{
    public InputField input;

    private int[] GetArrayFromInputField(string input)
    {
        try
        {
            string[] parts = input.Split(',');
            return parts.Select(int.Parse).ToArray();
        }
        catch
        {
            return new int[0];
        }

    }

    public void Check2Nums()
    {
        bool b = HaveTwoNumberSumZero();
        input.text = b.ToString();
    }

    public void Check3Nums()
    {
        bool b = HaveThreeNumberSumZero();
        input.text = b.ToString();
    }

    public void Check2and3Nums()
    {
        bool b1 = HaveTwoNumberSumZero();
        bool b2 = HaveThreeNumberSumZero();

        input.text = b1 && b2 ? "true" : "false";
    }

    private bool HaveTwoNumberSumZero()
    {
        int[] nums = GetArrayFromInputField(input.text);
        HashSet<int> seen = new HashSet<int>();

        foreach (int num in nums)
        {
            if (seen.Contains(-num))
            {
                return true;
            }
            seen.Add(num);
        }

        return false;
    }

    private bool HaveThreeNumberSumZero()
    {
        int[] nums = GetArrayFromInputField(input.text);
        int len = nums.Length;

        for (int i = 0; i < len - 2; i++)
        {
            HashSet<int> seen = new HashSet<int>();
            int currentTarget = -nums[i];

            for (int j = i + 1; j < len; j++)
            {
                if (seen.Contains(currentTarget - nums[j]))
                {
                    return true;
                }
                seen.Add(nums[j]);
            }
        }

        return false;
    }

    private bool HaveTwoNumberSumZeroMethod2()
    {
        int[] nums = GetArrayFromInputField(input.text);
        int len = nums.Length;

        for (int i = 0; i < len / 2; i++)
        {
            int front = nums[i];
            int back = nums[len - 1 - i];

            for (int j = i + 1; j < len - i; j++)
            {
                if (j != len - 1 - i && front + nums[j] == 0)
                {
                    input.text = "true";
                    return true;
                }

                if (j != i && back + nums[j] == 0)
                {
                    input.text = "true";
                    return true;
                }
            }
        }

        input.text = "false";
        return false;
    }
}
