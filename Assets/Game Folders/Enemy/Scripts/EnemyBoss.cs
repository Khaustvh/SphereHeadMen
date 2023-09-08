using UnityEngine;

public sealed class EnemyBoss : MonoBehaviour //Boss life cycle
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
    
    private int _hitPoints;
    private int _coinsFromTheBoss;
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

    public void BossMove() //Start boss movement
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

    public void Hited(byte damage) //Boss hit
    {
        _hitPoints -= damage;

        if (_hitPoints <= 0 && _activeMove)
        {
            textHitPoints.text = "";
            
            _bossCol.enabled = false;
            _activeMove = false;
            
            _bossAnim.Play("DeadBoss");
            _bossAnim.applyRootMotion = true;

            _playerManager.LevelAccess();
            _playerManager.AddCoins(_coinsFromTheBoss);
            
            return;
        }

        textHitPoints.text = _hitPoints.ToString();
    }

    public void EnableBoss(ref int hitPointsAverageValue, Vector3 position, ref int averageValueOfCoins) //Spawn boss
    {
        _tr.position = position;
        
        _hitPoints = (short)Random.Range(hitPointsAverageValue, hitPointsAverageValue * 1.5f);
        _coinsFromTheBoss = (short)Random.Range(averageValueOfCoins, averageValueOfCoins * 2f);
        textHitPoints.text = _hitPoints.ToString();
        
        _rb.velocity = Vector3.back * Speed * Time.fixedDeltaTime;

        _col.enabled = true;
        _bossCol.enabled = true;
        planeMesh.enabled = true;
        _bossMesh.enabled = true;
    }
    
    void OnTriggerEnter(Collider other) //Collisions wish object (player)
    {
        if (!_activeMove && other.CompareTag("Player")) //Player ready to fight
        {
            _rb.velocity = Vector3.zero;
            _col.enabled = false;

            _playerManager.StartCoroutine("TakeBoss");
        }
        else if (_activeMove && other.CompareTag("Player")) //The player lost
        {
            _bossCol.enabled = false;
            _activeMove = false;
            
            _bossAnim.Play("PanchBoss");
            _playerManager.PlayerContactWithTheEnemy();
        }
    }
}
