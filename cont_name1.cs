using System;
using System.Collections.Generic;

public class Solution {
    public long[] FindXSum(int[] nums, int k, int x) {
        int n = nums.Length;
        long[] ans = new long[n - k + 1];
        Dictionary<int, int> count = new Dictionary<int, int>();
        SortedSet<Pair> top = new SortedSet<Pair>();
        SortedSet<Pair> bot = new SortedSet<Pair>();
        long windowSum = 0;
        int ansIndex = 0;

        Action<int, int> update = (num, freq) => {
            if (count.ContainsKey(num) && count[num] > 0) {
                Pair oldPair = new Pair(count[num], num);
                if (bot.Contains(oldPair)) {
                    bot.Remove(oldPair);
                } else if (top.Contains(oldPair)) {
                    top.Remove(oldPair);
                    windowSum -= (long)num * count[num];
                }
            }
            if (!count.ContainsKey(num)) count[num] = 0;
            count[num] += freq;
            if (count[num] > 0) {
                Pair newPair = new Pair(count[num], num);
                bot.Add(newPair);
            }
        };

        for (int i = 0; i < n; ++i) {
            update(nums[i], 1);
            if (i >= k) {
                update(nums[i - k], -1);
            }
            // Fill top to size x
            while (top.Count < x && bot.Count > 0) {
                Pair p = bot.Max;
                bot.Remove(p);
                top.Add(p);
                windowSum += (long)p.Val * p.Freq;
            }
            // Swap if needed
            while (bot.Count > 0 && bot.Max.CompareTo(top.Min) > 0) {
                Pair pb = bot.Max;
                bot.Remove(pb);
                Pair pt = top.Min;
                top.Remove(pt);
                top.Add(pb);
                bot.Add(pt);
                windowSum += (long)pb.Val * pb.Freq - (long)pt.Val * pt.Freq;
            }
            if (i >= k - 1) {
                ans[ansIndex++] = windowSum;
            }
        }

        return ans;
    }

    private class Pair : IComparable<Pair> {
        public int Freq { get; }
        public int Val { get; }

        public Pair(int freq, int val) {
            Freq = freq;
            Val = val;
        }

        public int CompareTo(Pair other) {
            if (Freq != other.Freq) return Freq.CompareTo(other.Freq);
            return Val.CompareTo(other.Val);
        }
    }
}