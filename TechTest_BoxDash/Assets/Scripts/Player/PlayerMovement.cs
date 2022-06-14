using System.Collections;
using UnityEngine;
using Mirror;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private Camera playerCamera;

    [Header("Materials")]
    public Material defaultMaterial;
    public Material damagedMaterial;
    
    [Header("Variables")]
    [SyncVar]
    public float movementSpeed = 10;
    [SyncVar]
    public float dashDistance = 15f;
    [SyncVar]
    public float damageTime = 3f;
    
    private bool isDashing;
    private bool isUndying;
    
    private Rigidbody _rigidbody;
    private Renderer _playerRenderer;
    private PlayerStats _playerStats;
    
    private void Awake()
    {
        //Caching Components 
        _rigidbody = GetComponent<Rigidbody>();
        _playerRenderer = GetComponent<Renderer>();
        _playerStats = GetComponent<PlayerStats>();
    }

    private void Start()
    {
        //Check another players Camera
        if (!isLocalPlayer)
        {
            playerCamera.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        if (!isDashing)
        {
            Move(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            Dash();
        }

        CheckHeight();
    }

    private void Move(float horizontalAxis, float verticalAxis)
    {
        float horizontal = horizontalAxis * movementSpeed * Time.deltaTime;
        transform.Translate(horizontal, 0, 0);
		
        float vertical = verticalAxis * movementSpeed * Time.deltaTime;
        transform.Translate(0, 0, vertical);
    }

    private void CheckHeight()
    {
        if (transform.position.y <= -2)
        {
            Transform newPos = NetworkManager.singleton.GetStartPosition();
            transform.position = newPos.position;
            transform.rotation = newPos.rotation;
        }
    }

    private void Dash()
    {
        // Getting Mouse Button from Input Manager (changeable)
        // Also (if (Input.GetMouseButtonDown(0)), but with InputManager)
        if (Input.GetButtonDown("LMB"))
        {
            StartCoroutine("DashCoroutine");
        }  
    }

    //If collision with Player - Damage
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Player") && isDashing && other.gameObject.GetComponent<PlayerMovement>() && !other.gameObject.GetComponent<PlayerMovement>().isUndying)
        {
            CmdDashDamage(other.gameObject);
        }
    }

    #region CheckForDamage

    [ClientRpc] //Additional Look for Client
    void RpcDashDamage(GameObject obj)
    {
        obj.GetComponent<PlayerMovement>().StartCoroutine("DamageOfDash");
        _playerStats.CheckScore(1);
    }
    
    [Command] //Look for Command Server
    void CmdDashDamage(GameObject obj)
    {
        RpcDashDamage (obj);    
    }

    private IEnumerator DamageOfDash()
    {
        _playerRenderer.material = damagedMaterial;
        isUndying = true;
        yield return new WaitForSeconds(damageTime);
        isUndying = false;
        _playerRenderer.material = defaultMaterial;
    }
    
    #endregion

    private IEnumerator DashCoroutine()
    {
        isDashing = true;
        _rigidbody.useGravity = false;
        _rigidbody.AddForce(transform.forward * dashDistance, ForceMode.Impulse);
        yield return new WaitForSeconds(1f);
        _rigidbody.useGravity = true;
        _rigidbody.velocity = new Vector3(0f, 0f, 0f);
        isDashing = false;
    }
}
