using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public sealed class PlayerManager : MonoBehaviour
{
    [Header("GameMenu info")] 
    [SerializeField] private float timeForActiveMenuAfterTheFall;
    [SerializeField] private float timeForActiveMenuAfterContactWithTheEnemy;
    [SerializeField] private float timeForVictoryDance;
    [SerializeField] private Text numberOfLevel;
    [SerializeField] private Text numberOfCoins;
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private GameObject failedPanel;
    [Header("Player info")]
    [SerializeField] private Transform player;
    [SerializeField] private float playerSpeed;
    private float _playerAttackSpeed;
    [Header("PlayerControl info")]
    [SerializeField] private Transform cameraOnPlayer;
    [SerializeField] private RectTransform touchPoint;
    [Header("Projectile info")]
    [SerializeField] private byte allNumberProjectile;
    [SerializeField] private GameObject projectileObject;
    private byte _projectileDamage;
    private float _projectileSpeed;
    private float _projectileLifeTime;
    [Header("Enemy info")] 
    private short _coinsFromTheEnemyAverageValue;
    [SerializeField] private byte allNumberEnemy;
    private short _hitPointsEnemyAverageValue;
    [SerializeField] private float maxWaitingTimeForNextSpawn;
    [SerializeField] private float enemySpeed;
    [SerializeField] private GameObject enemyObject;
    [SerializeField] private Transform[] spawnEnemyPoints;
    [Header("Enemy Boss info")]
    private short _coinsFromTheBossAverageValue;
    private short _hitPointsBossAverageValue;
    [SerializeField] private GameObject bossObject;
    [SerializeField] private float timeForEnableBoss;
    [SerializeField] private float timeToStartBoss;
    [SerializeField] private float bossSpeed;

    private Animator _playerAnim;
    
    private List<Projectile> _listProjectiles;
    
    private List<Enemy> _listEnemies;
    private List<Rigidbody> _listRbEnemies;
    
    private EnemyBoss _boss;

    private readonly string _dataKey = "PlayerStatistics";
    private SaveData _data;

    private bool _spawnEnemyStop;
    private bool _attackedStop;
    private bool _playerMoveStop;

    private short _allValueOfCoins;
    
    private void Awake()
    {
        _listProjectiles = new List<Projectile>();
        _listEnemies = new List<Enemy>();
        _listRbEnemies = new List<Rigidbody>();
        _playerAnim = player.GetChild(0).GetComponent<Animator>();

        Load();
        
        CreateProjectile(allNumberProjectile);
        CreateEnemy(allNumberEnemy);
    }

    private void Start()
    {
        StartCoroutine("Attack");
        StartCoroutine("EnemyBossSpawn");
        StartCoroutine("EnemySpawn");
    }

    void Load()
    {
        _data = SaveManager.Load<SaveData>(_dataKey);

        _allValueOfCoins = _data.AllValueOfCoins;
        _playerAttackSpeed = _data.AttackSpeed;
        _projectileDamage = _data.Damage;
        _projectileSpeed = _data.ProjectileSpeed;
        _projectileLifeTime = _data.ProjectileDistance;
        _coinsFromTheEnemyAverageValue = _data.CoinsFromTheEnemyAverageValue;
        _hitPointsEnemyAverageValue = _data.HitPointsEnemyAverageValue;
        _coinsFromTheBossAverageValue = _data.CoinsFromTheBossAverageValue;
        _hitPointsBossAverageValue = _data.HitPointsBossAverageValue;

        numberOfLevel.text = _data.NumberOfLevel.ToString();
        numberOfCoins.text = _allValueOfCoins.ToString();
    }

    void Save()
    {
        SaveManager.Save(_dataKey, _data);
    }

    public void AddCoins(short value)
    {
        _allValueOfCoins += value;
        numberOfCoins.text = _allValueOfCoins.ToString();
    }
    
    public IEnumerator TakeBoss()
    {
        _playerAnim.Play("PreparationPlayer");
        _attackedStop = true;
        _playerMoveStop = true;
        
        yield return new WaitForSeconds(timeToStartBoss);

        _boss.BossMove();
        _playerMoveStop = false;
        _attackedStop = false;
        _playerAnim.Play("ShootingIdlePlayer");
    }
    
    private void CreateProjectile(byte number)
    {
        for (byte i = 0; i < number; i++)
        {
            Projectile proj = Instantiate(projectileObject).GetComponent<Projectile>();
            proj.Speed = _projectileSpeed;
            proj.LifeTime = _projectileLifeTime;
            proj.Damage = _projectileDamage;
            
            _listProjectiles.Add(proj);
        }
    }
    
    private void CreateEnemy(byte number)
    {
        for (byte i = 0; i < number; i++)
        {
            Enemy enemy = Instantiate(enemyObject).GetComponent<Enemy>();
            enemy.Speed = enemySpeed;
            
            _listRbEnemies.Add(enemy.GetComponent<Rigidbody>());
            _listEnemies.Add(enemy);
        }

        _boss = Instantiate(bossObject).GetComponent<EnemyBoss>();
        _boss.Speed = enemySpeed;
        _boss.BossSpeed = bossSpeed;
    }

    private  IEnumerator EnemyBossSpawn()
    {
        yield return new WaitForSeconds(timeForEnableBoss);
        _spawnEnemyStop = true;

        yield return new WaitForSeconds(5f);
        
        _boss.EnableBoss(ref _hitPointsBossAverageValue, spawnEnemyPoints[1].position, ref _coinsFromTheBossAverageValue);
    }

    private IEnumerator EnemySpawn()
    {
        float spawnTime = 0f;
        
        byte enemyNumber = 0;
        
        while (true)
        {
            yield return new WaitForSeconds(spawnTime);

            if (!_spawnEnemyStop && _listEnemies.Count != 0)
            {
                byte enemyCount = (byte)Random.Range(1, 100);
                
                if ( enemyCount <= 70)
                {
                    if (enemyNumber == _listEnemies.Count) enemyNumber = 0;
                    
                    byte point = (byte)Random.Range(0, 3);

                    if (!_listEnemies[enemyNumber].EnemyMove)
                    {
                        _listEnemies[enemyNumber].EnableEnemy(ref _hitPointsEnemyAverageValue, spawnEnemyPoints[point].position, ref _coinsFromTheEnemyAverageValue);

                        enemyNumber++;
                    }
                }
                else if (enemyCount > 70 && enemyCount <= 95)
                {
                    byte pointBusy = 3;
                    
                    for (byte i = 0; i < 10; i++)
                    {
                        if (enemyNumber == _listEnemies.Count) enemyNumber = 0;

                        if (!_listEnemies[enemyNumber].EnemyMove)
                        {
                            byte point = (byte)Random.Range(0, 3);
                        
                            if (pointBusy == 3)
                            {
                                _listEnemies[enemyNumber].EnableEnemy(ref _hitPointsEnemyAverageValue, spawnEnemyPoints[point].position, ref _coinsFromTheEnemyAverageValue);
                                pointBusy = point;
                                enemyNumber++;
                            }
                            else if (point != pointBusy)
                            {
                                _listEnemies[enemyNumber].EnableEnemy(ref _hitPointsEnemyAverageValue, spawnEnemyPoints[point].position, ref _coinsFromTheEnemyAverageValue);
                                enemyNumber++;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    for (byte i = 0; i < 3; i++)
                    {
                        if (enemyNumber == _listEnemies.Count) enemyNumber = 0;
                        
                        _listEnemies[enemyNumber].EnableEnemy(ref _hitPointsEnemyAverageValue, spawnEnemyPoints[i].position, ref _coinsFromTheEnemyAverageValue);
                        enemyNumber++;
                    }
                }

                spawnTime = Random.Range(2f, maxWaitingTimeForNextSpawn);
            }
        }
    }
    
    private IEnumerator Attack()
    {
        byte projectileNumber = 0;
        while (true)
        {
            yield return new WaitForSeconds(_playerAttackSpeed);

            if (!_attackedStop && _listProjectiles.Count != 0)
            {
                if (projectileNumber == _listProjectiles.Count) projectileNumber = 0;

                if (!_listProjectiles[projectileNumber].Active)
                {
                    _listProjectiles[projectileNumber].EnableProjectile();
                    projectileNumber++;
                }
            }
        }
    }

    public void LevelAccess()
    {
        _playerMoveStop = true;
        _attackedStop = true;
        StartCoroutine(ActiveGameMenuAfterVictory(timeForVictoryDance));
        _playerAnim.Play("VictoryDance");
    }
    
    private void Update()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            
            RaycastHit hit;
            
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    
                    Ray ray = Camera.main.ScreenPointToRay(touch.position);
                    
                    if (Physics.Raycast(ray, out hit, 1f))
                    {
                        if (hit.collider.CompareTag("UI"))
                        {
                            touchPoint.position = hit.point;
                            
                            _playerAnim.SetBool("ControlPlayer", true);
                        }
                    }
                    else
                    {
                        touchPoint.localPosition = Vector3.zero;
                        _playerAnim.SetBool("ControlPlayer", false);
                    }
                    
                    _playerAnim.SetFloat("ValueX", touchPoint.localPosition.x);
                    
                    break;

                case TouchPhase.Moved:
                    
                    ray = Camera.main.ScreenPointToRay(touch.position);
                    
                    if (Physics.Raycast(ray, out hit, 1f))
                    {
                        if (hit.collider.CompareTag("UI"))
                        {
                            touchPoint.position = hit.point;
                            _playerAnim.SetBool("ControlPlayer", true);
                        }
                    }
                    else
                    {
                        touchPoint.localPosition = Vector3.zero;
                        _playerAnim.SetBool("ControlPlayer", false);
                    }

                    break;

                case TouchPhase.Ended:

                    touchPoint.localPosition = Vector3.zero;
                    
                    _playerAnim.SetBool("ControlPlayer", false);
                    
                    break;
            }
        }
    }

    private void FixedUpdate()
    {
        if (!_playerMoveStop && touchPoint.localPosition.x != 0f)
        {
            if (touchPoint.localPosition.x > 0)
            {
                player.Translate(Vector3.right * playerSpeed * Time.fixedDeltaTime);
                cameraOnPlayer.Translate(Vector3.right * playerSpeed * Time.fixedDeltaTime);
            }
            else
            {
                player.Translate(Vector3.left * playerSpeed * Time.fixedDeltaTime);
                cameraOnPlayer.Translate(Vector3.left * playerSpeed * Time.fixedDeltaTime);
            }

            if (player.position.x < -3 || player.position.x > 3)
            {
                _playerAnim.Play("FailingPlayer");
                _playerAnim.applyRootMotion = true;
                _spawnEnemyStop = true;
                _attackedStop = true;
                _playerMoveStop = true;

                StartCoroutine(ActiveGameMenuAfterFailed(timeForActiveMenuAfterTheFall));

                if (player.position.x < -3)
                {
                    player.rotation = Quaternion.Euler(0f, -45f, 0f);
                }
                else
                {
                    player.rotation = Quaternion.Euler(0f, 45f, 0f);
                }
            }
        }
    }

    public void PlayerContactWithTheEnemy()
    {
        _playerAnim.Play("PlayerDead");
        _playerAnim.applyRootMotion = true;
        
        _spawnEnemyStop = true;
        _attackedStop = true;
        _playerMoveStop = true;
        
        foreach (var enemyRb in _listRbEnemies)
        {
            enemyRb.velocity = Vector3.zero;
        }
        
        StartCoroutine(ActiveGameMenuAfterFailed(timeForActiveMenuAfterContactWithTheEnemy));
    }
    
    private IEnumerator ActiveGameMenuAfterFailed(float time)
    {
        yield return new WaitForSeconds(time);
        Time.timeScale = 0f;

        _data.AllValueOfCoins = _allValueOfCoins;
        
        Save();
        
        failedPanel.SetActive(true);
        
        //Active Menu Failed Menu
    }
    
    private IEnumerator ActiveGameMenuAfterVictory(float time)
    {
        yield return new WaitForSeconds(time);
        Time.timeScale = 0f;

        _data.AllValueOfCoins = _allValueOfCoins;
        _data.NumberOfLevel++;
        _data.CoinsFromTheEnemyAverageValue += 20;
        _data.HitPointsEnemyAverageValue += 20;
        _data.CoinsFromTheBossAverageValue += 30;
        _data.HitPointsBossAverageValue += 30;
        
        Save();
        
        victoryPanel.SetActive(true);
        
        //Active Menu Victory Menu
    }

}
