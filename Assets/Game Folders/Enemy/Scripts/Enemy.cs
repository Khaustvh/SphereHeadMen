using System.Collections;
using UnityEngine;

public sealed class Enemy : MonoBehaviour
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
    
    public bool EnemyMove { get; set; }
    public float Speed { private get; set; }
    
    private short _hitPoints;
    private short _coinsFromTheEnemy;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _col = GetComponent<Collider>();
        _tr = GetComponent<Transform>();
        _playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        _trueEnemy = GetComponent<Animator>();
    }

    public void EnableEnemy(ref int hitPointsAverageValue, Vector3 position, ref int averageValueOfCoins)
    {
        _tr.position = position;
        
        _hitPoints = (short)Random.Range(hitPointsAverageValue / 2, hitPointsAverageValue * 2);
        _coinsFromTheEnemy = (short)Random.Range(averageValueOfCoins / 2, averageValueOfCoins * 2);
            
        textHitPoints.text = _hitPoints.ToString();
        
        _rb.velocity = Vector3.back * Speed * Time.fixedDeltaTime;
        
        standMesh.enabled = true;
        stickmanMesh.enabled = true;
        _col.enabled = true;
        EnemyMove = true;
    }
    
    private IEnumerator DisableEnemy()
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

    public void Hited(byte damage)
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
    
    void OnTriggerEnter(Collider other)
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
