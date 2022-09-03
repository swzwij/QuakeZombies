using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkInventory : MonoBehaviour
{
    private Wallet wallet;
    private HealthSystem healthSystem;
    private PlayerMovement playerMovement;

    private int price = 250;

    private void Awake()
    {
        wallet = GetComponent<Wallet>();
        healthSystem = GetComponent<HealthSystem>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    public void AddPerk(int perk)
    {
        if(perk == 0 && wallet.Credits >= price && !healthSystem.hasHealthPerk)
        {
            healthSystem.AddPerk();
            wallet.RemoveCredits(price);
        }
        if(perk == 1 && wallet.Credits >= price && !playerMovement.hasSpeedPerk)
        {
            playerMovement.AddPerk();
            wallet.RemoveCredits(price);
        }
    }
}
