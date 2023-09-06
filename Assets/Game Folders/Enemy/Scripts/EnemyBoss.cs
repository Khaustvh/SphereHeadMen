using UnityEngine;

public sealed class EnemyBoss : MonoBehaviour
{
    [Header("Enemy Boss info")]
    [SerializeField] private Transform enemyBoss;
    [SerializeField] private MeshRenderer planeMesh;
    [SerializeField] private TextMesh textHitPoints;

    private Animator _bossAnim;
    private Collider _bossCol;
    private SkinnedMeshRenderer _bossMesh;

    private Transform _player;
    private Collider _col;
    private Rigidbody _rb;
    private Transform _tr;
    private PlayerManager _playerManager;
    
    private short _hitPoints;
    private bool _activeMove;
    
    public float Speed { get; set; }
    public float BossSpeed { get; set; }
    
    private void Start()
    {
        _player = GameObject.Find("Player").transform;
        _rb = GetComponent<Rigidbody>();
        _col = GetComponent<Collider>();
        _tr = GetComponent<Transform>();
        
        _bossAnim = enemyBoss.GetComponent<Animator>();
        _bossCol = enemyBoss.GetComponent<Collider>();
        _bossMesh = enemyBoss.GetChild(1).GetComponent<SkinnedMeshRenderer>();
        _playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
    }

    public void BossMove()
    {
        _bossAnim.Play("MoveBoss");
        _activeMove = true;
    }

    private void FixedUpdate()
    {
        if (_activeMove)
        {
            enemyBoss.LookAt(_player.position);
            enemyBoss.position = Vector3.MoveTowards(enemyBoss.position, 
                _player.position, BossSpeed * Time.fixedDeltaTime);
        }
    }

    public void EnableBoss(short hitPointsAverageValue, Vector3 position)
    {
        _tr.position = position;
        
        _hitPoints = (short)Random.Range(hitPointsAverageValue / 1.5f, hitPointsAverageValue * 1.5f);
        textHitPoints.text = _hitPoints.ToString();
        
        _rb.velocity = Vector3.back * Speed * Time.fixedDeltaTime;

        _col.enabled = true;
        _bossCol.enabled = true;
        planeMesh.enabled = true;
        _bossMesh.enabled = true;
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _rb.velocity = Vector3.zero;
            _col.enabled = false;

            _playerManager.StartCoroutine("TakeBoss");
            //Player ready to fight
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (_activeMove && other.collider.CompareTag("Player"))
        {
            _activeMove = false;
            _bossAnim.Play("PanchBoss");
            _playerManager.PlayerContactWithTheEnemy();
        }
    }
}
