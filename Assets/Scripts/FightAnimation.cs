using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;


public class FightAnimation : MonoBehaviour
{
    public Animator CharAnimator;
    public CameraSwitcher camswitch;
    public PlayerMovement Movement;
    public GameObject Character;
    public int AddStrengthAfterVictory;
    public int EnemyStrength;
    public GameObject NewPositionEnemy;
    public GameObject NewPositionCharacter;
    public TextArena textarena;
    public ThirdPersonCam cam;
    private AnimCharacter animchar;

    private Vector3  saveCharTf;
    private Vector3  saveEnemyTf;
    private bool readyToChoose = true;
    private float Cooldown1 = 2.0f;
    private float Cooldown2 = 4.0f;
    private bool died = false;
    private bool died2 = false;
    private bool animation_done = true;
    private Animator Animator;
    private Transform tfstore;
    private CinemachineFreeLook vcam;



    // Start is called before the first frame update
    void Start()
    {
        Animator = GetComponent<Animator>();
        animchar = Character.GetComponent<AnimCharacter>();
        vcam = cam.GetComponent<CinemachineFreeLook>();
    }


    private void OnTriggerEnter(Collider collision)
    {
        //tfstore = cam.transform;
        //cam.enabled = false;
        cam.SwitchMovement();
        vcam.Follow = null;
        vcam.LookAt = null;
        animchar.Switch();
        saveCharTf = collision.gameObject.transform.position;
        saveEnemyTf = gameObject.transform.position;

        Movement.StopMove();
        camswitch.SwitchCamera();

        AIpatrol patrol = gameObject.GetComponent<AIpatrol>();

        if (patrol != null) {
            patrol.SwitchMove(NewPositionEnemy.transform);
        }
        else 
        {
            Animator.SetTrigger("Wake");
            camswitch.SwitchCamera();

        }
        gameObject.transform.position = NewPositionEnemy.transform.position;
        gameObject.transform.LookAt(NewPositionCharacter.transform.position);
        collision.gameObject.transform.position = NewPositionCharacter.transform.position;
        collision.gameObject.transform.LookAt(NewPositionEnemy.transform.position);
        
    }
    

    private void OnTriggerStay(Collider other)
    {
        CharacterHealth EnemyHealth = gameObject.GetComponent<CharacterHealth>();
        CharacterHealth CharacterHealth = Character.GetComponent<CharacterHealth>();
        CharacterStrength CharacterStrength = Character.GetComponent<CharacterStrength>();
        KeyCount KeyCount = Character.GetComponent<KeyCount>();


        if(Animator != null)
        {
            if ((EnemyHealth.GetHealthCount() != 0) && (CharacterHealth.GetHealthCount() != 0) && readyToChoose)
            {

                if(Input.GetKeyDown(KeyCode.U))
                {
                    animation_done = false;
                    StartCoroutine(PerformActionsWithDelay(EnemyHealth, CharacterHealth, CharacterStrength.GetStrengthCount()*Random.Range(8, 12), EnemyStrength * Random.Range(8, 12))); // damage to enemy, damage to character
                }
                else if(Input.GetKeyDown(KeyCode.I))
                {
                    animation_done = false;
                    StartCoroutine(PerformActionsWithDelay(EnemyHealth, CharacterHealth, CharacterStrength.GetStrengthCount()*Random.Range(5, 15), EnemyStrength * Random.Range(8, 12))); // damage to enemy, damage to character
                }
                else if(Input.GetKeyDown(KeyCode.O))
                {
                    animation_done = false;
                    StartCoroutine(PerformActionsWithDelay(EnemyHealth, CharacterHealth, CharacterStrength.GetStrengthCount()*Random.Range(1, 20), EnemyStrength * Random.Range(8, 12))); // damage to enemy, damage to character
                }

            }

            if(EnemyHealth.GetHealthCount() == 0 && animation_done)
            {

                if (!died) 
                {
                    if(!died2)
                    {
                        died2 = true;
                        Animator.SetTrigger("Die");
                        CharacterStrength.GetStrength(AddStrengthAfterVictory);
                    }
       
                     Invoke(nameof(WaitDie), Cooldown2);
                }

                else
                {
                    gameObject.SetActive(false);


                    camswitch.SwitchCamera();

                    AIpatrol patrol = gameObject.GetComponent<AIpatrol>();

                    if (patrol != null) {
                          camswitch.SwitchCamera();

                    }
                    else {
                        KeyCount.Collect(1);
                    }

               
                    Character.gameObject.transform.position = saveCharTf;

                    Movement.StartMove();
                    cam.SwitchMovement();
                    //cam.transform.position = tfstore.position;
                    //cam.enabled = true;
                    animchar.Switch();
                    vcam.Follow = Character.transform;
                    vcam.LookAt = Character.transform;
                }
                

            }
            if(CharacterHealth.GetHealthCount() == 0 && animation_done)
            {
                if (!died) 
                {
                    if(!died2)
                    {
                        died2 = true;
                        CharAnimator.SetTrigger("Die");
                    }
                    
                    //Invoke(nameof(WaitDie), Cooldown2);
                }
                
 
            }
           
        }
    }

     private void WaitDie()
    {
        died = true;
    }

    private void ResetChoose()
    {
        readyToChoose = true;
        textarena.Turn();
    }

    private IEnumerator PerformActionsWithDelay(CharacterHealth enemyHealth, CharacterHealth characterHealth, int enemyDamage, int characterDamage)
    {
        readyToChoose = false;
        textarena.Turn();
        // First set of actions
        enemyHealth.TakeDamage(enemyDamage);
        CharAnimator.SetTrigger("Attack2");
        yield return new WaitForSeconds(0.2f);
        Animator.SetTrigger("GetHit");


         if ((enemyHealth.GetHealthCount() != 0))
         {
             // Wait for 2 seconds
            yield return new WaitForSeconds(2);
              // Second set of actions
            characterHealth.TakeDamage(characterDamage);
            Animator.SetTrigger("Attack");
            yield return new WaitForSeconds(0.2f);
            CharAnimator.SetTrigger("GetHit");


         }

        Invoke(nameof(ResetChoose), Cooldown1);
        animation_done = true;
    }



}
