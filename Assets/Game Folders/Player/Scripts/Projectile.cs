using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private ParticleSystem bloodEffect;
    
    private Transform _playerTransform;
    private Transform _tr;
    private Rigidbody _rb;
    private Collider _col;
    private MeshRenderer _mesh;
    
    private float _time;
    private bool _active;
    
    public float LifeTime { private get; set; }
    public float Speed { private get; set; }

    private void Start()
    {
        _playerTransform = GameObject.Find("Player").transform;
        _tr = GetComponent<Transform>();
        _rb = GetComponent<Rigidbody>();
        _col = GetComponent<Collider>();
        _mesh = GetComponent<MeshRenderer>();
    }

    public void EnableProjectile()
    {
        _time = Time.time + LifeTime;
        _tr.position = new Vector3(_playerTransform.position.x, 1f, _playerTransform.position.z + 0.75f);
        
        _col.enabled = true;
        _mesh.enabled = true;
        _active = true;
    }
    
    private IEnumerator DisableProjectile()
    {
        _active = false;
        _col.enabled = false;
        _mesh.enabled = false;
        _rb.velocity = Vector3.zero;

        yield return new WaitForSeconds(0.5f);
        _tr.position = Vector3.zero;
    }

    private void FixedUpdate()
    {
        if (_active)
        {
            if (_time <= Time.time) StartCoroutine("DisableProjectile");
            else _rb.velocity = Vector3.forward * Speed * Time.fixedDeltaTime;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        StartCoroutine("DisableProjectile");
        
        if (other.CompareTag("Enemy"))
        {
            bloodEffect.Play();
        }
    }
}
