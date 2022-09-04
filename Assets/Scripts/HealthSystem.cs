using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private float health = 100f;
    private float _maxHealth;
    [SerializeField] private Text healthText;

    [SerializeField] private bool hasHealthTxt;

    [SerializeField] private UnityEvent onHealthAdded = new UnityEvent();
    [SerializeField] private UnityEvent onDamageTaken = new UnityEvent();
    [SerializeField] private UnityEvent onDie = new UnityEvent();
    [SerializeField] private UnityEvent onResurrected = new UnityEvent();

    public float Health => health;
    public float MaxHealth => _maxHealth;
    public bool HasMaxHealth => health >= _maxHealth;

    private bool _isHit;
    [HideInInspector] public bool isDead;

    private void Start()
    {
        InitHealth();
    }

    private void Update()
    {
        if(hasHealthTxt)
            healthText.text = "" + (Mathf.Round(health));
    }

    private void InitHealth()
    {
        _maxHealth = health;
    }

    public void AddHealth(float healthAmount)
    {
        if (isDead || HasMaxHealth) return;

        health += healthAmount;

        onHealthAdded?.Invoke();
    }

    public void Resurrect(float newHealth)
    {
        isDead = false;
        AddHealth(newHealth);
        onResurrected?.Invoke();

    }

    public void TakeDamage(float damage)
    {
        if (isDead || _isHit) return;

        _isHit = true;
        health -= damage;
        onDamageTaken?.Invoke();
        _isHit = false;

        if (health <= 0) Die();
    }

    private void Die()
    {
        health = 0;
        isDead = true;
        onDie?.Invoke();
        print(gameObject.name + " Died");
    }

    public void Kill()
    {
        Die();
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    public void AddPerk()
    {
        _maxHealth *= 1.5f;
        health = _maxHealth;
        print("added perk");
    }
}
