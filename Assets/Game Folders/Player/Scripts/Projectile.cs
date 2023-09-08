using System.Collections;
using UnityEngine;

public sealed class Projectile : MonoBehaviour //Projectile life cycle
{
    [SerializeField] private ParticleSystem bloodEffect;
    
    private Transform _playerTr;
    private Transform _tr;
    private Rigidbody _rb;
    private Collider _col;
    private MeshRenderer _mesh;
    
    public bool Active { get; private set; }
    public byte Damage { private get; set; }
    public float LifeTime { private get; set; }
    public float Speed { private get; set; }

    private void Start()
    {
        _playerTr = GameObject.Find("Player").transform;
        _tr = GetComponent<Transform>();
        _rb = GetComponent<Rigidbody>();
        _col = GetComponent<Collider>();
        _mesh = GetComponent<MeshRenderer>();
    }

    public void EnableProjectile() //Spawn projectile
    {
        _tr.position = new Vector3(_playerTr.position.x, 1f, _playerTr.position.z + 0.75f);
        _rb.velocity = Vector3.forward * Speed * Time.fixedDeltaTime;
        
        _col.enabled = true;
        _mesh.enabled = true;
        Active = true;
        
        StartCoroutine("CountDisableProjectile");
    }

    IEnumerator CountDisableProjectile() //Projectile lifetime
    {
        yield return new WaitForSeconds(LifeTime);
        if (Active) StartCoroutine("DisableProjectile");
    }
    
    private IEnumerator DisableProjectile() //Disable projectile
    {
        Active = false;
        _col.enabled = false;
        _mesh.enabled = false;
        _rb.velocity = Vector3.zero;

        yield return new WaitForSeconds(0.01f);
        _tr.position = Vector3.zero;
    }
    
    void OnTriggerEnter(Collider other) //Collisions wish object
    {
        if (other.CompareTag("Enemy"))
        {
            bloodEffect.Play();
            other.GetComponent<Enemy>().Hited(Damage);
        }
        else if (other.CompareTag("Boss"))
        {
            bloodEffect.Play();
            other.GetComponentInParent<EnemyBoss>().Hited(Damage);
        }
        
        StartCoroutine("DisableProjectile");
    }
}
