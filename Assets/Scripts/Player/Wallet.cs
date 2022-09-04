using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Wallet : MonoBehaviour
{
    [SerializeField] private Text creditTxt;
    [SerializeField] private int credits;

    public int Credits => credits;

    private void Update()
    {
        creditTxt.text = "" + credits;

    }

    public void AddCredits(int creditAmount)
    {
        credits += creditAmount;
    }

    public void RemoveCredits(int creditAmount)
    {
        credits -= creditAmount;
    }
}
