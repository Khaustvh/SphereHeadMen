using System.Collections;
using UnityEngine;

public sealed class Enemy : MonoBehaviour //Enemy life cycle
{
    [Header("Enemy info")] 
    [SerializeField] private SkinnedMeshRenderer stickmanMesh;
    [SerializeField] private MeshRenderer standMesh;
    [SerializeField] private TextMesh textHitPoints;
    [SerializeField] private float timeForAnimationDead;
    
    private Rigidbody _rb;
    private Collider _col;
    private Transform _tr;
    private PlayerManager _playerManager;
    private Animator _trueEnemy;
    
    public bool EnemyMove { get; private set; }
    public float Speed { private get; set; }
    
    private int _hitPoints;
    private int _coinsFromTheEnemy;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _col = GetComponent<Collider>();
        _tr = GetComponent<Transform>();
        _playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        _trueEnemy = GetComponent<Animator>();
    }

    public void EnableEnemy(ref int hitPointsAverageValue, Vector3 position, ref int averageValueOfCoins) //Spawn enemy
    {
        _tr.position = position;
        
        _hitPoints = Random.Range(hitPointsAverageValue / 2, hitPointsAverageValue * 2);
        _coinsFromTheEnemy = Random.Range(averageValueOfCoins / 2, averageValueOfCoins * 2);
            
        textHitPoints.text = _hitPoints.ToString();
        
        _rb.velocity = Vector3.back * Speed * Time.fixedDeltaTime;
        
        standMesh.enabled = true;
        stickmanMesh.enabled = true;
        _col.enabled = true;
        EnemyMove = true;
    }
    
    private IEnumerator DisableEnemy() //Disable enemy
    {
        textHitPoints.text = "";
        _col.enabled = false;
        
        _trueEnemy.Play("EnemyDeadAnimation");

        yield return new WaitForSeconds(timeForAnimationDead);
        
        standMesh.enabled = false;
        stickmanMesh.enabled = false;
        EnemyMove = false;
        
        _rb.velocity = Vector3.zero;
        _tr.position = Vector3.zero;
        
        _trueEnemy.Play("Nan");
    }

    public void Hited(byte damage) //Enemy hit
    {
        _hitPoints -= damage;

        if (_hitPoints <= 0)
        {
            StartCoroutine("DisableEnemy");
            _playerManager.AddCoins(_coinsFromTheEnemy);
            
            return;
        }

        textHitPoints.text = _hitPoints.ToString();
    }
    
    void OnTriggerEnter(Collider other) //Collisions wish object (player)
    {
        if (other.CompareTag("Player"))
        {
            _playerManager.PlayerContactWithTheEnemy();
        }
        else if (other.CompareTag("Finish"))
        {
            StartCoroutine("DisableEnemy");
        }
    }
}
