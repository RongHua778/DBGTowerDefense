using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour,IDamageable
{

    [SerializeField] private Image _healthBar;
    [SerializeField] private float _initialHealth = 10f;
    [SerializeField] private float _maxHealth = 10f;

    private bool isDie = false;
    public bool IsDie { get => isDie; set => isDie = value; }

    private EnemyAnim _enemyAnim;
    private Enemy _enemy;
    public float MaxHealth { get => _maxHealth; set => _maxHealth = value; }
    private float _currentHealth;
    public float CurrentHealth { get => _currentHealth; 
        set 
        {
            _currentHealth = Mathf.Clamp(value,0,MaxHealth);
            _healthBar.fillAmount = CurrentHealth / MaxHealth;
            if (_currentHealth <= 0)
            {
                StartCoroutine(PlayDieCor());
            }
        } 
    }


    void Start()
    {
        ResetHealth();
        _enemyAnim = GetComponent<EnemyAnim>();
        _enemy = GetComponent<Enemy>();
    }

    public void TakeDamage(float damageReceived)
    {
        if(!IsDie)
            CurrentHealth -= damageReceived;
    }


    public IEnumerator PlayDieCor()
    {
        IsDie = true;
        _enemyAnim.PlayDie();
        _enemy.StopMovement();
        yield return new WaitForSeconds(_enemyAnim.GetCurrentAnimationLenght() + .3f);
        GameEvents.Instance.EnemyDie();
        _enemy.UnspawnThisEnemy();
    }

    public void ResetHealth()
    {
        CurrentHealth = _initialHealth;
        IsDie = false;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyUp(KeyCode.D))
        //{
        //    CurrentHealth -= 5f;
        //}
    }
}
