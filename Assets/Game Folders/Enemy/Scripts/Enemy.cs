using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy info")] 
    [SerializeField] private SkinnedMeshRenderer stickmanMesh;
    [SerializeField] private MeshRenderer standMesh;
    [SerializeField] private TextMesh textHitPoints;
    
    private Rigidbody _rb;
    private Collider _col;
    private Transform _tr;
    private PlayerManager _playerManager;
    
    public bool EnemyMove { get; set; }
    public float Speed { private get; set; }
    
    private short _hitPoints;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _col = GetComponent<Collider>();
        _tr = GetComponent<Transform>();
        _playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
    }

    public void EnableEnemy(short hitPointsAverageValue, Vector3 point)
    {
        _tr.position = point;
        
        _hitPoints = (short)Random.Range(hitPointsAverageValue / 2, hitPointsAverageValue * 2);
        textHitPoints.text = _hitPoints.ToString();
        
        _rb.velocity = Vector3.back * Speed * Time.fixedDeltaTime;
        
        standMesh.enabled = true;
        stickmanMesh.enabled = true;
        _col.enabled = true;
        EnemyMove = true;
    }
    
    private void DisableEnemy()
    {
        textHitPoints.text = "";
        
        _rb.velocity = Vector3.zero;
        _tr.position = Vector3.zero;
        
        standMesh.enabled = false;
        stickmanMesh.enabled = false;
        _col.enabled = false;
        EnemyMove = false;
    }

    public void Hited(byte damage)
    {
        _hitPoints -= damage;

        if (_hitPoints <= 0)
        {
            DisableEnemy();
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
    }
}
