using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkBuyer : MonoBehaviour
{
    private PerkInventory perkInventory;

    private void Awake()
    {
        perkInventory = GetComponent<PerkInventory>();
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "PerkMachine" && Input.GetKey(KeyCode.E))
        {
            perkInventory.AddPerk(other.gameObject.GetComponent<PerkMachine>().MachineType);
        }
    }
}
