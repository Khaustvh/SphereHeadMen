using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy info")] 
    [SerializeField] private SkinnedMeshRenderer stickmanMesh;
    [SerializeField] private MeshRenderer standMesh;
    
    private Rigidbody _rb;
    private Collider _col;
    private Transform _tr;
    private static bool _enemyMove;
    
    public float Speed { private get; set; }
    
    private short _hitPoints;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _col = GetComponent<Collider>();
        _tr = GetComponent<Transform>();
    }

    public void EnableEnemy(short hitPointsAverageValue, Vector3 point)
    {
        _tr.position = point;
        _hitPoints = (short)Random.Range(hitPointsAverageValue / 2, hitPointsAverageValue * 2);
        
        standMesh.enabled = true;
        stickmanMesh.enabled = true;
        _col.enabled = true;
        _enemyMove = true;
    }
    
    private void FixedUpdate()
    {
        if (_enemyMove)
        {
            _rb.velocity = Vector3.back * Speed * Time.fixedDeltaTime;
        }
    }
}
