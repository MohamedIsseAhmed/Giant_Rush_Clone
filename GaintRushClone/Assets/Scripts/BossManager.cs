using System;
using UnityEngine;

public class BossManager : MonoBehaviour,IDamageable
{
    [SerializeField] private Animator animator;
    [SerializeField] private float boxTimer = 0;
    [SerializeField] private float boxTimerMax = 1;
    [SerializeField] private float healthAmount = 1;
    [SerializeField] private float forceFloat = 1;
    [SerializeField] private Vector3 forceV ;
    [SerializeField] private Vector3 explosionPos ;
    [SerializeField] private float lerpSpeed = 1;
    [SerializeField] private Transform targetOnlastHit; 
    [SerializeField] private float healthAmountMax = 4;
    [SerializeField] private ForceMode forceMode;
    [SerializeField] private Collider mainCollider; 
    public event EventHandler KickTheBossEvent;
    public event EventHandler OnEnemyDie;
    public static BossManager instance;
    private bool goTolastHitPoint = false;
    private Rigidbody rigidBody;
    private Collider[] childCollider;
    private Rigidbody[] childBodies;
    private void Awake()
    {
        instance = this;
        healthAmount= healthAmountMax;
        boxTimer = boxTimerMax;
        rigidBody = GetComponent<Rigidbody>();    
    }
    void Update()
    {
        if (GameManager.instance.IsFightingStarted())
        {
            FightWithBoss(GameManager.instance.GetFightinPoint(), GameManager.instance.GetFightinPointRotation());

        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            //GetComponent<Animator>().enabled = false;
            //ApplyForce(transform, explosionPos,forceV, 10);
           
        }
    }
    private void Start()
    {
        childCollider = GetComponentsInChildren<Collider>(true);
        childBodies = GetComponentsInChildren<Rigidbody>();
        DoRagdoll(false);
    }
    
    private void ApplyForce(Transform root,Vector3 forcePos, Vector3 forcedir, float radius)
    {
        foreach (Transform child in root)
        {
            if (child.TryGetComponent<Rigidbody>(out Rigidbody childBody))
            {
                childBody.AddForceAtPosition(forcedir*forceFloat, forcePos);
            }
            ApplyForce(child, forcePos, forcedir, radius);
        }
        
    }
    private void OnTriggerEnter(Collider other)
    {
     
    }
    private void FightWithBoss(Vector3 fightingPoint, Quaternion _fightingRotation)
    {
        animator.SetFloat("BlendSpeed", 0);
        animator.SetTrigger("StartBoxing");
       
        boxTimer -= Time.deltaTime;
        if (boxTimer <= 0 && !goTolastHitPoint)
        {
            boxTimer = boxTimerMax;
            animator.SetTrigger("PunchTheBoss");
        }
       
    }
    
    public void TakeDame(float dameAmount)
    {
      
   
        healthAmount -= dameAmount;
        healthAmount = Mathf.Clamp(healthAmount, 0, healthAmountMax);
        if (healthAmount == 1)
        {
            KickTheBossEvent?.Invoke(this, EventArgs.Empty);
        }
        if (healthAmount <= 0)
        {
            animator.SetTrigger("Die");
           
            OnEnemyDie?.Invoke(this, EventArgs.Empty);
            DoRagdoll(true);
            ApplyForce(transform, explosionPos, forceV, 10);
        }
        else
        {
            animator.SetTrigger("takeHit");
           
        }
       
    }
    public void GoToTargetOnLastHit()
    {
        goTolastHitPoint = true;
    }
    private void DoRagdoll(bool isEnabled)
    {
      
        foreach(var col in childCollider)
        {
            if(col.TryGetComponent(out Damager d))
            {
                continue;
            }
            col.enabled = isEnabled;
            
        }
        foreach (var rb in childBodies)
        {
            rb.isKinematic=!isEnabled;
        }
        mainCollider.enabled = !isEnabled;
        rigidBody.isKinematic = !isEnabled;
        animator.enabled = !isEnabled;
    }
}
