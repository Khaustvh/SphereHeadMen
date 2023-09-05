using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("Player info")]
    [SerializeField] private Transform player;
    [SerializeField] private float playerSpeed;
    [SerializeField] private float playerAttackSpeed;
    [Header("PlayerControl info")]
    [SerializeField] private Transform cameraOnPlayer;
    [SerializeField] private RectTransform touchPoint;
    [Header("Projectile info")]
    [SerializeField] private byte allNumberProjectile;
    [SerializeField] private GameObject projectileObject;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float projectileLifeTime;
    [Header("Enemy info")] 
    [SerializeField] private byte allNumberEnemy;
    [SerializeField] private short hitPointsAverageValue;
    [SerializeField] private float maxWaitingTimeForNextSpawn;
    [SerializeField] private float enemySpeed;
    [SerializeField] private GameObject enemyObject;
    [SerializeField] private Transform[] spawnEnemyPoints;
    
    private List<Projectile> _listProjectiles;
    private List<Enemy> _listEnemies;

    private bool _spawnEnemyStop;
    
    private void Awake()
    {
        _listProjectiles = new List<Projectile>();
        _listEnemies = new List<Enemy>();
        
        CreateProjectile(allNumberProjectile);
        CreateEnemy(allNumberEnemy);
    }

    private void Start()
    {
        StartCoroutine("Attack");
        StartCoroutine("EnemySpawn");
    }

    private void CreateProjectile(byte number)
    {
        for (byte i = 0; i < number; i++)
        {
            Projectile proj = Instantiate(projectileObject).GetComponent<Projectile>();
            proj.Speed = projectileSpeed;
            proj.LifeTime = projectileLifeTime;
            
            _listProjectiles.Add(proj);
        }
    }
    
    private void CreateEnemy(byte number)
    {
        for (byte i = 0; i < number; i++)
        {
            Enemy enemy = Instantiate(enemyObject).GetComponent<Enemy>();
            enemy.Speed = enemySpeed;
            
            _listEnemies.Add(enemy);
        }
    }

    private IEnumerator EnemySpawn()
    {
        float spawnTime = 0f;
        byte enemyNumber = 0;
        
        while (true)
        {
            yield return new WaitForSeconds(spawnTime);

            if (!_spawnEnemyStop)
            {
                byte enemyCount = (byte)Random.Range(1, 4);

                if (enemyCount == 1)
                {
                    if (enemyNumber == _listEnemies.Count) enemyNumber = 0;
                    
                    byte point = (byte)Random.Range(0, 3);

                    _listEnemies[enemyNumber].EnableEnemy(hitPointsAverageValue, spawnEnemyPoints[point].position);

                    enemyNumber++;
                }
                else if (enemyCount == 2)
                {
                    byte pointBusy = 3;
                    
                    for (byte i = 0; i < 10; i++)
                    {
                        if (enemyNumber == _listEnemies.Count) enemyNumber = 0;
                        
                        byte point = (byte)Random.Range(0, 3);
                        
                        if (pointBusy == 3)
                        {
                            _listEnemies[enemyNumber].EnableEnemy(hitPointsAverageValue, spawnEnemyPoints[point].position);
                            enemyNumber++;
                        }
                        else if (point != pointBusy)
                        {
                            _listEnemies[enemyNumber].EnableEnemy(hitPointsAverageValue, spawnEnemyPoints[point].position);
                            enemyNumber++;
                            break;
                        }
                    }
                }
                else
                {
                    for (byte i = 0; i < 3; i++)
                    {
                        if (enemyNumber == _listEnemies.Count) enemyNumber = 0;
                        
                        _listEnemies[enemyNumber].EnableEnemy(hitPointsAverageValue, spawnEnemyPoints[i].position);
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
            yield return new WaitForSeconds(playerAttackSpeed);

            if (_listProjectiles.Count != 0)
            {
                if (projectileNumber == _listProjectiles.Count) projectileNumber = 0;
            
                _listProjectiles[projectileNumber].EnableProjectile();
                projectileNumber++;
            }
        }
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
                        }
                    }
                    else touchPoint.localPosition = Vector3.zero;
                    
                    break;

                case TouchPhase.Moved:
                    
                    ray = Camera.main.ScreenPointToRay(touch.position);
                    
                    if (Physics.Raycast(ray, out hit, 1f))
                    {
                        if (hit.collider.CompareTag("UI"))
                        {
                            touchPoint.position = hit.point;
                        }
                    }
                    else touchPoint.localPosition = Vector3.zero;

                    break;

                case TouchPhase.Ended:

                    touchPoint.localPosition = Vector3.zero;
                    
                    break;
            }
        }
    }

    private void FixedUpdate()
    {
        if (touchPoint.localPosition.x != 0f)
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
        }
    }
}
