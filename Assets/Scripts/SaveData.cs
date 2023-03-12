using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    public int day;
    public double money;
    public int totalStars;
    public int totalCustomerCount;

    public SaveData(int day, double money, int totalStars, int totalCustomerCount)
    {
        this.day = day;
        this.money = money;
        this.totalStars = totalStars;
        this.totalCustomerCount = totalCustomerCount;
    }
}
