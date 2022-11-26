using System.Collections;
using System;
using UnityEngine;

public class Player : MonoBehaviour,IDamageable
{
    private Rigidbody rigidbody;
    [SerializeField] private float moveSpeed = 8;
    [SerializeField] private float swipeSpeed = 8;
    [SerializeField] private float maxXRange = 0.94f;
    [SerializeField] private float minXRange = -0.94f;
    [SerializeField] private float stopingDistanceFromBoss = 2f;
    [SerializeField] private float boxTimer = 0;
    [SerializeField] private float boxTimerMax = 1f;
    [SerializeField] private float healthBar = 1f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private BossManager bossManager;
    private Camera camera;
    private Animator animator;
    private Vector3 targetPosition;
    private bool swipe = false;
    [SerializeField] private bool startFighting = false;

    [SerializeField] private bool isKickingTheBoss = false;
    [SerializeField] private bool isEnemyDied = false;
    public event EventHandler OnKickingBoss;
    
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        boxTimer = boxTimerMax;
        camera = Camera.main;
        animator.SetFloat("BlendSpeed", 1);
        bossManager.KickTheBossEvent += Ýnstance_KickTheBossEvent;
        bossManager.OnEnemyDie += BossManager_OnEnemyDie;
    }

    private void BossManager_OnEnemyDie(object sender, EventArgs e)
    {
        isEnemyDied = true;
    }

    private void Ýnstance_KickTheBossEvent(object sender, System.EventArgs e)
    {
        
        isKickingTheBoss = true;
        StartCoroutine(KickTheBoss());
    }
    IEnumerator KickTheBoss()
    {
       
        yield return null;
    }
    void Update()
    {
        
        if (GameManager.instance.IsGameOverOver())
        {
            return;
        }
        if (GameManager.instance.IsFightingStarted())
        {
            FightWithBoss(GameManager.instance.GetFightinPoint(), GameManager.instance.GetFightinPointRotation());
            
        }
        if (Input.GetMouseButtonDown(0))
        {
            swipe = true;
        }
        if (Input.GetMouseButton(0) && swipe && !startFighting)
        {
            
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                targetPosition = new Vector3(hit.point.x, transform.position.y, transform.position.z);
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, swipeSpeed * Time.deltaTime);
                Vector3 lookDirection = (targetPosition - transform.position).normalized;
              
            }
          
            ClampXRange();
        }
        if (Input.GetMouseButtonUp(0))
        {
            swipe = false;
        }
        if (!startFighting)
        {
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime, Space.World);
        }
      
        ClampXRange();

    }
    
    private void ClampXRange()
    {
        if (transform.position.x >= maxXRange)
        {
            Vector3 currentPosition = transform.position;
            currentPosition.x = maxXRange;
            currentPosition.y = transform.position.y; 
            currentPosition.z = transform.position.z;
            transform.position = currentPosition;
        }

        else if(transform.position.x < minXRange)
        {
            Vector3 currentPosition = transform.position;
            currentPosition.x = minXRange;
            currentPosition.y = transform.position.y; ;
            currentPosition.z = transform.position.z;
            transform.position = currentPosition;
        }
    }
    private void FightWithBoss(Vector3 fightingPoint,Quaternion _fightingRotation)
    {
        //if (isKickingTheBoss)
        //{
        //    return;
        //}
        animator.SetFloat("BlendSpeed", 0);
        animator.SetTrigger("StartBoxing");
        startFighting = true;
        transform.SetPositionAndRotation(fightingPoint, _fightingRotation);
        boxTimer -= Time.deltaTime;
        if (Input.GetMouseButtonDown(0) && !isEnemyDied)
        {
            if (isEnemyDied)
            {
                return;
            }

            if (isKickingTheBoss )
            {
                animator.SetTrigger("Kick");
              


            }
            else if (boxTimer <= 0)
            {
                boxTimer = boxTimerMax;
              
                animator.SetTrigger("PunchTheBoss");
            }
            
        }
      
       
    }
    public void PlayAnimation(string animationName)
    {
        animator.SetTrigger(animationName);
    }
    public bool IsFighting()
    {
        return startFighting;
    }

    public void TakeDame(float dameAmount)
    {
        healthBar -= dameAmount;
        if (healthBar <= 0)
        {
            animator.SetTrigger("takeHit");
        }
        else
        {
            animator.SetBool("takeHit", true);
        }

    }
    public void ApplySlowMotionAndInformCamera()
    {
        OnKickingBoss?.Invoke(this, EventArgs.Empty);
        SlowMotion.instance.ApplySlowMotion();
    }
}
