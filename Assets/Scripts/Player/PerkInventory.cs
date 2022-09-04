using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkInventory : MonoBehaviour
{
    [SerializeField] private GameObject speedPerkImg;
    [SerializeField] private GameObject healthPerkImg;
    [SerializeField] private GameObject dmgPerkImg;

    private Wallet _wallet;

    private HealthSystem _healthSystem;
    private PlayerMovement _playerMovement;
    private Gun _gun;

    private int _price = 250;

    private bool _healthPerk;
    private bool _speedPerk;
    private bool _dmgPerk;

    private void Awake()
    {
        _wallet = GetComponent<Wallet>();
        _healthSystem = GetComponent<HealthSystem>();
        _playerMovement = GetComponent<PlayerMovement>();
        _gun = GetComponent<Gun>();
    }

    public void AddPerk(int perk)
    {
        if (perk == 0 && _wallet.Credits >= _price && !_healthPerk)
        {
            healthPerkImg.SetActive(true);
            _healthPerk = true;
            _healthSystem.AddPerk();
            _wallet.RemoveCredits(_price);
        }

        if (perk == 1 && _wallet.Credits >= _price && !_speedPerk)
        {
            speedPerkImg.SetActive(true);
            _speedPerk = true;
            _playerMovement.AddPerk();
            _wallet.RemoveCredits(_price);
        }

        if (perk == 2 && _wallet.Credits >= _price && !_dmgPerk)
        {
            dmgPerkImg.SetActive(true);
            _dmgPerk = true;
            _gun.AddPerk();
            _wallet.RemoveCredits(_price);
        }
    }
}
