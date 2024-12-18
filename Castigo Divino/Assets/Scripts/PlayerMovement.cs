using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] public float speed;
    private GameMaster gm;
    private EnemyLevel enemyLevel;

    private Rigidbody2D playerRb;
    private Vector2 moveInput;
    private Animator playerAnimator;
    private SpriteRenderer spriteRenderer;
    private SpriteRenderer GunSpriteRenderer;
    private AudioManager audioManager;
    private NextStage nextStage; 
   [SerializeField] private BossMachine boss;
    private Portals portals; 
    [SerializeField] private TransicionEscena transicion;

    [Header("Rotacion")]
    private Vector3 targetRotation;
    public Transform player;

    [Header("Dash Settings")]
    [SerializeField] float dashSpeed = 25f;
    [SerializeField] float dashDuration = 0.25f;
    [SerializeField] float dashCoolDown = 1f;
    [SerializeField] private GameObject dashEffect;
    [SerializeField] private GameObject Gun;
    bool isDashing;
    bool canDash;
    [SerializeField] private bool dashEnabled = true;

    [Header("Special Attack")]
    public bool canSpecialAttack;
    private PowerOfGod powerOfGod;
    [SerializeField] private float attackRadius = 40f; 
    [SerializeField] private int attackDamage = 50; 

    [Header("Stairs")]
    public float stairHeightOffset = 0.2f; // Ajusta la altura cuando el jugador sube o baja escaleras horizontales
    private int stairSortingOrderAdjustment = 1; 
    private bool onStairs = false;
    private int originalSortingOrder = 2;
    private BoxCollider2D playerCollider;
    private enum StairDirection { Right, Left }
    private StairDirection currentStairDirection;

    [Header("Others")]
    public ManagerData managerData;
    [SerializeField] private CamTransition camTransition;

    [Header("ShockWave")]
    public Material _material;
    private static int _waveDistanceFromCenter = Shader.PropertyToID("_WaveDistanceFromCenter");
    [SerializeField] private float _shockWaveTime = 0.75f;
    private Coroutine _shockWaveCorutine;
    public GameObject shockWavePrefab;
    [SerializeField] GameObject rippleEffect;
    private void Awake()
    {
        // Inicializa el AudioManager
        audioManager = GameObject.FindGameObjectWithTag("Audio")?.GetComponent<AudioManager>();
        if (audioManager == null)
        {
            //.LogError("No se encontró un AudioManager en la escena.");
        }

        _material = GetComponent<SpriteRenderer>().material;
    }

    void Start()
    {
        enemyLevel = FindObjectOfType<EnemyLevel>();
        //transicion = FindObjectOfType<TransicionEscena>();
        powerOfGod = FindObjectOfType<PowerOfGod>();
        portals = FindObjectOfType<Portals>();
        managerData = ManagerData.Instance;
        if (managerData == null)
        {
            //Debug.LogError("No se encontró ManagerData en la escena.");
            return;
        }

        // Carga la velocidad desde ManagerData, que debería haber cargado desde PlayerPrefs
        speed = managerData.speed;

        string sceneName = SceneManager.GetActiveScene().name;
        gm = GameObject.FindGameObjectWithTag("GM")?.GetComponent<GameMaster>();

        if (gm != null && gm.lastCheckpoint != Vector2.zero)
        {
            transform.position = gm.lastCheckpoint;
        } else
        {
            transform.position = new Vector2(2400, -1125); 
            //Debug.Log("gm nulo");
        }

        if (sceneName == "PacificZone" || sceneName == "EnemyLevel")
        {
            transform.position = new Vector2(520, -180); 
            //Debug.Log("ZONA PACIFICATION");
        }

        playerRb = GetComponent<Rigidbody2D>();

        nextStage = FindObjectOfType<NextStage>();
        playerRb = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        GunSpriteRenderer = Gun?.GetComponent<SpriteRenderer>();
        playerCollider = GetComponent<BoxCollider2D>();
        canDash = true;
        canSpecialAttack = false;
        // Verifica si los componentes clave están asignados
        if (playerRb == null || playerAnimator == null || spriteRenderer == null || GunSpriteRenderer == null)
        {
            //Debug.LogError("Uno o más componentes no están asignados correctamente.");
        }

        if (boss == null)
        {
            //Debug.LogError("No se encontró un componente BossMachine en el jugador.");
        }
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveInput = new Vector2(moveX, moveY).normalized;

        // Actualizar animaciones de movimiento
        if (playerAnimator != null)
        {
            playerAnimator.SetFloat("Speed", moveInput.sqrMagnitude);  // Si hay movimiento o no
        }


       
        targetRotation = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
       

        if (Mathf.Abs(targetRotation.x) > Mathf.Abs(targetRotation.y))
        {
            // El mouse está más hacia la izquierda o derecha
            if (targetRotation.x < 0)
            {
                // El mouse está a la izquierda del jugador
                playerAnimator.SetFloat("Horizontal", -1); 
                playerAnimator.SetFloat("Vertical", 0);  
                GunSpriteRenderer.sortingOrder = spriteRenderer.sortingOrder + 1;
            }
            else
            {
                // El mouse está a la derecha del jugador    
                playerAnimator.SetFloat("Horizontal", 1);   
                playerAnimator.SetFloat("Vertical", 0);  
                GunSpriteRenderer.sortingOrder = spriteRenderer.sortingOrder + 1;
            }
        }
        else
        {
            // El mouse está más hacia arriba o abajo
            if (targetRotation.y < 0)
            {
                // El mouse está por debajo del jugador
                playerAnimator.SetFloat("Vertical", -1);  
                playerAnimator.SetFloat("Horizontal", 0);  
                GunSpriteRenderer.sortingOrder = spriteRenderer.sortingOrder + 1;
            }
            else
            {
                // El mouse está por encima del jugador    
                playerAnimator.SetFloat("Vertical", 1);  
                playerAnimator.SetFloat("Horizontal", 0); 
                GunSpriteRenderer.sortingOrder = spriteRenderer.sortingOrder - 1;
            }
        }

        if (moveInput.sqrMagnitude == 0)
        {
           
            if (targetRotation.y > 0)
            {
                GunSpriteRenderer.sortingOrder = spriteRenderer.sortingOrder - 1;  
                
            }
            else
            {
                GunSpriteRenderer.sortingOrder = spriteRenderer.sortingOrder + 1;  
             
            }
        }

        if (isDashing)
        {
            return;
        }


         if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
         {
            if (dashEnabled)
            {
              StartCoroutine(Dash());
            }
            else
            {
              Debug.Log("No hay dash");
            }
         }

         if (Input.GetKey(KeyCode.F) && canSpecialAttack)
         {
            ActivateSpecialAttack();
         }
    }

    private void FixedUpdate()
    {
        if (playerRb == null)
        {
            //Debug.LogError("playerRb no está asignado.");
            return;
        }

        if (isDashing)
        {
            playerRb.velocity = moveInput * dashSpeed;
          //  Instantiate(dashEffect, transform.position, Quaternion.identity);
        }
        else
        {
            playerRb.MovePosition(playerRb.position + moveInput * speed * Time.fixedDeltaTime);
        }

        if (onStairs)
        {
           OnStairs();
        }
        else
        {
           /* playerRb.MovePosition(playerRb.position + moveInput * speed * Time.fixedDeltaTime);
            playerCollider.enabled = true;*/
        }
    }

    private IEnumerator Dash()
    {
        //Debug.Log("dashing");
        audioManager.playSound(audioManager.dash);
        canDash = false;
        isDashing = true;

        playerRb.velocity = moveInput * dashSpeed; // Aplica la velocidad del dash
        Instantiate(dashEffect, transform.position, Quaternion.identity);

        yield return new WaitForSeconds(dashDuration);

        playerRb.velocity = Vector2.zero;
        isDashing = false;

        yield return new WaitForSeconds(dashCoolDown);
        canDash = true;
    }

    private void ActivateSpecialAttack()
    {
        CameraMovement.Instance.MoveCamera(12, 10, 5f);
        audioManager.playSound(audioManager.powerOfGod);
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRadius);
        Instantiate(rippleEffect, transform.position, Quaternion.identity);

        // Instanciar el prefab de la onda de choque
        /* GameObject shockWaveInstance = Instantiate(shockWavePrefab, transform.position, Quaternion.identity);

         // Obtener el material del objeto instanciado
         Material instanceMaterial = shockWaveInstance.GetComponent<SpriteRenderer>().material;*/

        // Llamar a la corutina con el material del prefab instanciado
        // StartCoroutine(ShockWave(instanceMaterial, -0.1f, 1f));
        foreach (Collider2D enemy in hitEnemies)
        {
            // Aplicar daño a cada enemigo dentro del área
            enemy.GetComponent<BaseEnemy>()?.TakeDamage(10, BulletType.GodPower);
        }
 
        //Debug.Log("Ataque especial");
  
        canSpecialAttack = false;
        ManagerData.Instance.ResetCurrentPower();
        powerOfGod.currentPower = 0;
        powerOfGod.UpdatePowerUpBar(0);
        // Destruir la onda de choque después de un tiempo
       // Destroy(shockWaveInstance, _shockWaveTime);
    }
  /*  private IEnumerator ShockWave(Material material, float startPos, float endPos)
    {
        material.SetFloat(_waveDistanceFromCenter, startPos);
        float elapsedTime = 0f;

        while (elapsedTime < _shockWaveTime)
        {
            elapsedTime += Time.deltaTime;
            float lerpedAmount = Mathf.Lerp(startPos, endPos, elapsedTime / _shockWaveTime);
            material.SetFloat(_waveDistanceFromCenter, lerpedAmount);
            yield return null;
        }
    }*/
  /*  private void CallShockWave(GameObject shockWaveInstance)
    {
        _shockWaveCorutine = StartCoroutine(shockWave(shockWaveInstance, -0.1f, 1f));
    }


    private IEnumerator shockWave(GameObject shockWaveInstance, float startPos, float endPos)
    {
        Material shockWaveMaterial = shockWaveInstance.GetComponent<SpriteRenderer>().material;
        shockWaveMaterial.SetFloat(_waveDistanceFromCenter, startPos);
        float lerpedAmount = 0f;
        float elapsedTime = 0f;
        while (elapsedTime < _shockWaveTime)
        {
            elapsedTime += Time.deltaTime;
            lerpedAmount = Mathf.Lerp(startPos, endPos, elapsedTime / _shockWaveTime);
            shockWaveMaterial.SetFloat(_waveDistanceFromCenter, lerpedAmount);
            yield return null;
        }
        Destroy(shockWaveInstance);  // Destruye el prefab solo al finalizar la animación
    }*/
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("entrada"))
        {
            GameEvents.ClosedDoor();
        }
        if(other.gameObject.CompareTag("entrada2"))
        {
            GameEvents.ClosedDoor();
            if (!enemyLevel.comenzarOleada && enemyLevel.contadorOleadas == 0)
            {
                enemyLevel.comenzarOleada = true; 
            }
        }
        if (other.gameObject.CompareTag("entradaBoss"))
        {
            //Debug.Log("Activando jefe");
            if (boss != null)
            {
                GameEvents.ClosedDoor();
                StartCoroutine(camTransition.SwitchPriorityBoss());
                Destroy(other.gameObject);
            }
        }
        if (other.gameObject.CompareTag("entradaShrine"))
        {
           StartCoroutine(camTransition.SwitchPriorityShrine());
           Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("Retorno"))
        {
            transicion.SiguienteNivel("PacificZone");
            gm.lastCheckpoint = Vector2.zero;
        }

        if (other.gameObject.CompareTag("StairsLeft") || other.gameObject.CompareTag("StairsRight"))
        {
            onStairs = true;
            if (other.gameObject.CompareTag("StairsLeft"))
            {
                currentStairDirection = StairDirection.Left;
            }
            else 
            {
                currentStairDirection = StairDirection.Right;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("StairsLeft") || other.gameObject.CompareTag("StairsRight"))
        {
            onStairs = false; 
            spriteRenderer.sortingOrder = originalSortingOrder; 
         //   playerCollider.enabled = true;
            playerRb.velocity = Vector2.zero;
            //   playerRb.bodyType = RigidbodyType2D.Dynamic;
            canDash = true;
        }
    }

    private void OnStairs()
    {
        //  playerCollider.enabled = false;
        //  playerRb.bodyType = RigidbodyType2D.Kinematic;
        canDash = false;

        if (currentStairDirection == StairDirection.Left)
        {
            if (Input.GetKey(KeyCode.A))
            {
                transform.position += new Vector3(moveInput.x, stairHeightOffset, 0) * Time.deltaTime * speed;
                spriteRenderer.sortingOrder = originalSortingOrder + stairSortingOrderAdjustment;
            }
            else if (Input.GetKey(KeyCode.D)) // Mover a la izquierda en la escalera
            {
                transform.position += new Vector3(moveInput.x, -stairHeightOffset, 0) * Time.deltaTime * speed;
                spriteRenderer.sortingOrder = originalSortingOrder - stairSortingOrderAdjustment;
            }

        }
        else if (currentStairDirection == StairDirection.Right) 
        {
            if (Input.GetKey(KeyCode.D))
            {
                transform.position += new Vector3(moveInput.x, stairHeightOffset, 0) * Time.deltaTime * speed;
                spriteRenderer.sortingOrder = originalSortingOrder + stairSortingOrderAdjustment;
            }
            else if (Input.GetKey(KeyCode.A)) // Mover a la izquierda en la escalera
            {
                transform.position += new Vector3(moveInput.x, -stairHeightOffset, 0) * Time.deltaTime * speed;
                spriteRenderer.sortingOrder = originalSortingOrder - stairSortingOrderAdjustment;
            }

        }
        
    }
}
