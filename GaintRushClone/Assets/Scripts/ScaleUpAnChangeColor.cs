using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
public class ScaleUpAnChangeColor : MonoBehaviour
{
    [SerializeField] private string targetTag;
    [SerializeField] private Transform blockParent; 
    //public string TargetTag { get { return targetTag; } set { targetTag = value; } }
    [SerializeField] private string redTag;
    [SerializeField] private string greenTag;
    [SerializeField] private string yellowTag;
    [SerializeField] private float scale›ncreaserFactor;
    [SerializeField] private float scale›ncreaserFactorMax;
    [SerializeField] private float scale›ncreaserFactorMin;
    [SerializeField] private float scaleModiferDecrease;
    [SerializeField] private float scaleTime;
    [SerializeField] private float minIntesnity;
    [SerializeField] private float max›ntesnity;
    [SerializeField] private float colorChangeTime=0.1f;
    [SerializeField] private float targetLight›ntensity = 0.1f;
    [SerializeField] private float targetLight›ntensityIncreaseBy = 0.1f;
    [SerializeField] private float lightSpeed = 0.1f;
    [SerializeField] private Color red;
    [SerializeField] private Color green;
    [SerializeField] private Color yellow;
    [SerializeField] private Color blue;
    [SerializeField] private LayerMask punchBlckLayer;
    [SerializeField] private Light light; 
    [SerializeField] private ParticleSystem  collosionParticle;

    [SerializeField] private float power = 10f;
    [SerializeField] private float radius = 5.0f;
    [SerializeField] private float upwardsModifier = 5.0f;
    [SerializeField] private float boxCastDistance = 5.0f;

    [SerializeField] private float shakeIntensity = 5.0f;
    [SerializeField] private float shakeTime = 0.2f;

    [SerializeField] private Vector3 halfExtents; 
  
    [SerializeField] private Vector3 explosionPos;

    private SkinnedMeshRenderer skinnedMeshRenderer;
    public event EventHandler<int> OnColidedWithStickmanIncreaseLevelNumber;

    private Material material;
    private Player player;
    [SerializeField] private bool shoulPunchTheWall;
    private bool anitmateColor = false;
    private Color targetColor;
    private ParticleSystem.MainModule mainModule;
    private LevelText levelText;
    private void Awake()
    {
        levelText = GetComponent<LevelText>();
        player = GetComponent<Player>();
        skinnedMeshRenderer = transform.GetChild(0).GetComponent<SkinnedMeshRenderer>();
        material = skinnedMeshRenderer.material;
    }
    private void Start()
    {
        mainModule = collosionParticle.GetComponent<ParticleSystem>().main;
    }

    private void FixedUpdate()
    {

        if (shoulPunchTheWall)
        {
            RaycastHit[] result = new RaycastHit[25];
            RaycastHit[] hits = Physics.BoxCastAll(transform.position, halfExtents, transform.forward, Quaternion.identity, boxCastDistance, punchBlckLayer);
            for (int i = 0; i < hits.Length; i++)
            {
                Destroy(hits[i].transform.gameObject);
            }
            
        }
    }
 
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag(targetTag))
        {
            targetColor = material.color;
            light.color = targetColor;
            LerpLightIntensity();
            Vector3 myScale= transform.localScale;
            PlayCollisionParticle(material.color);
            transform.DOScale(myScale * scale›ncreaserFactor, scaleTime).OnComplete(() =>
            {
                //collosionParticle.gameObject.SetActive(false);
            });
         
            OnColidedWithStickmanIncreaseLevelNumber?.Invoke(this, 1);
            Destroy(other.gameObject);
        }
       
        else if(other.transform.CompareTag("punchBox"))
        {
            PunchWall punchWall= other.transform.GetComponent<PunchWall>();

            if (punchWall != null)
            {
                if (punchWall.GetLevelNumber() <= levelText.GetLevelNumber())
                {
                    shoulPunchTheWall = true;
                    player.PlayPunchAnimation();
                    CamerController.instance.ShakeCamera(shakeIntensity, shakeTime, StopBoxCastingAction);
                    StartCoroutine(ApplyExplosion());
                }
                
            }
            
        }
       
        else
        { 
            
            if (!other.transform.CompareTag("colorWall"))
            {
                Vector3 myScale = transform.localScale;
                //scale›ncreaserFactor -= scaleModiferDecrease;
                //ClamScaleIncreaseFactorf();
                if (transform.localScale.x >= scale›ncreaserFactorMin)
                {
                    transform.DOScale(myScale * scaleModiferDecrease, scaleTime);
                }
                OnColidedWithStickmanIncreaseLevelNumber?.Invoke(this, -1);
                Destroy(other.gameObject);
            }
         
        }
    }
    private void StopBoxCastingAction()
    {
        shoulPunchTheWall = false;
    }
    private void ClamScaleIncreaseFactorf()
    {
        scale›ncreaserFactor = Mathf.Clamp(scale›ncreaserFactor, scale›ncreaserFactorMin, scale›ncreaserFactorMax);
    }
    public void SetTargetTagAndColor(string tag,Color color)
    {
        targetTag = tag;
        targetColor = color;        
        light.color = targetColor;
        anitmateColor = true;
        material.DOColor(color, colorChangeTime);
        PlayCollisionParticle(material.color);
        LerpLightIntensity();
      
    }
    private void LerpLightIntensity()
    {
        
        light.DOIntensity(targetLight›ntensity, lightSpeed).OnComplete(() =>
        {
            light.intensity = 0;
        });
        targetLight›ntensity += targetLight›ntensityIncreaseBy;
        light.range += targetLight›ntensityIncreaseBy;
    }
   
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
    public IEnumerator  ApplyExplosion()
    {
        yield return null;
        Vector3 explosionPos = transform.position;
        Collider[] colliders = new Collider[48];
        int boxes = Physics.OverlapSphereNonAlloc(explosionPos,radius,colliders,punchBlckLayer);
        print("Length" + boxes);
        for (int i = 0; i < boxes; i++)
        {
           

            Transform targetBox = colliders[i].transform;
            if (targetBox != null) 
            {
                Rigidbody rb = targetBox.GetComponent<Rigidbody>();
                rb.isKinematic = false;
                if (rb != null)
                    rb.AddExplosionForce(power, explosionPos, radius);
            }
      
            //Camera.main.transform.DOShakePosition(0.1f,2);
        }
      
        
    }
    private void PlayCollisionParticle(Color color)
    {
        //collosionParticle.gameObject.SetActive(true);

        mainModule.startColor = color;

    }
}
