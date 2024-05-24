using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class FightAnimation : MonoBehaviour
{
    public CameraSwitcher camswitch;
    public PlayerMovement Movement;
    private Animator Animator;
    public GameObject Character;
    public int AddStrengthAfterVictory;
    public int EnemyStrength;
    public GameObject NewPositionEnemy;
    public GameObject NewPositionCharacter;
    public TextArena textarena;

    private Vector3  saveCharTf;
    private Vector3  saveEnemyTf;
    private bool readyToChoose = true;
    private float Cooldown1 = 2.0f;
    private float Cooldown2 = 3.0f;
    private bool died = false;
    private bool died2 = false;



    // Start is called before the first frame update
    void Start()
    {
        Animator = GetComponent<Animator>();
    }

    // Update is called once per frame

    private void OnTriggerEnter(Collider collision)
    {
        saveCharTf = collision.gameObject.transform.position;
        saveEnemyTf = gameObject.transform.position;

        Movement.StopMove();
        camswitch.SwitchCamera();

        AIpatrol patrol = gameObject.GetComponent<AIpatrol>();

        if (patrol != null) {
            patrol.SwitchMove(NewPositionEnemy.transform);
        }
        gameObject.transform.position = NewPositionEnemy.transform.position;
        gameObject.transform.LookAt(NewPositionCharacter.transform.position);
        collision.gameObject.transform.position = NewPositionCharacter.transform.position;
        
    }
    

    private void OnTriggerStay(Collider other)
    {
        CharacterHealth EnemyHealth = gameObject.GetComponent<CharacterHealth>();
        CharacterHealth CharacterHealth = Character.GetComponent<CharacterHealth>();
        CharacterStrength CharacterStrength = Character.GetComponent<CharacterStrength>();

        if(Animator != null)
        {
            if ((EnemyHealth.GetHealthCount() != 0) && (CharacterHealth.GetHealthCount() != 0) && readyToChoose)
            {

                if(Input.GetKeyDown(KeyCode.U))
                {
                    StartCoroutine(PerformActionsWithDelay(EnemyHealth, CharacterHealth, CharacterStrength.GetStrengthCount()*Random.Range(8, 12), EnemyStrength * Random.Range(8, 12))); // damage to enemy, damage to character
                }
                else if(Input.GetKeyDown(KeyCode.I))
                {
                    StartCoroutine(PerformActionsWithDelay(EnemyHealth, CharacterHealth, CharacterStrength.GetStrengthCount()*Random.Range(5, 15), EnemyStrength * Random.Range(8, 12))); // damage to enemy, damage to character
                }
                else if(Input.GetKeyDown(KeyCode.O))
                {
                    StartCoroutine(PerformActionsWithDelay(EnemyHealth, CharacterHealth, CharacterStrength.GetStrengthCount()*Random.Range(1, 20), EnemyStrength * Random.Range(8, 12))); // damage to enemy, damage to character
                }

            }

            if(EnemyHealth.GetHealthCount() == 0)
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

               
                    Character.gameObject.transform.position = saveCharTf;

                    Movement.StartMove();
                }
                

            }
            else if(CharacterHealth.GetHealthCount() == 0)
            {
                // restart game
                Movement.StartMove();
 
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
        Animator.SetTrigger("GetHit");

        // Wait for 2 seconds
        yield return new WaitForSeconds(2);

         if ((enemyHealth.GetHealthCount() != 0))
         {
              // Second set of actions
            characterHealth.TakeDamage(characterDamage);
            Animator.SetTrigger("Attack");

         }

        Invoke(nameof(ResetChoose), Cooldown1);
    }



}
