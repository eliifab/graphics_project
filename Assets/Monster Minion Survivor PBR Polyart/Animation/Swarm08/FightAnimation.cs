using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class FightAnimation : MonoBehaviour
{
    public PlayerMovement Movement;
    private Animator Swarm08_Animator;
    public GameObject Character;
    public int AddStrengthAfterVictory;
    public int AttackWeight;

    private bool readyToChoose = true;
    private float Cooldown = 0.2f;


    // Start is called before the first frame update
    void Start()
    {
        Swarm08_Animator = GetComponent<Animator>();
    }

    // Update is called once per frame

    private void OnTriggerEnter(Collider collision)
    {
        //SceneManager.LoadScene("Arena");
        //teleport
        Movement.StopMove();
        //gameObject.transform.Translate(10,0,0);
        //collision.gameObject.transform.Translate(10,0,0);
        
    }
    

    private void OnTriggerStay(Collider other)
    {
        CharacterHealth EnemyHealth = gameObject.GetComponent<CharacterHealth>();
        CharacterHealth CharacterHealth = Character.GetComponent<CharacterHealth>();
        CharacterStrength CharacterStrength = Character.GetComponent<CharacterStrength>();

        if(Swarm08_Animator != null)
        {
            if ((EnemyHealth.GetHealthCount() != 0) && (CharacterHealth.GetHealthCount() != 0) && readyToChoose)
            {

                if(Input.GetKeyDown(KeyCode.U))
                {
                    StartCoroutine(PerformActionsWithDelay(EnemyHealth, CharacterHealth, CharacterStrength.GetStrengthCount()*Random.Range(8, 12), AttackWeight * Random.Range(8, 12))); // damage to enemy, damage to character
                }
                else if(Input.GetKeyDown(KeyCode.I))
                {
                    StartCoroutine(PerformActionsWithDelay(EnemyHealth, CharacterHealth, CharacterStrength.GetStrengthCount()*Random.Range(5, 15), AttackWeight * Random.Range(8, 12))); // damage to enemy, damage to character
                }
                else if(Input.GetKeyDown(KeyCode.O))
                {
                    StartCoroutine(PerformActionsWithDelay(EnemyHealth, CharacterHealth, CharacterStrength.GetStrengthCount()*Random.Range(1, 20), AttackWeight * Random.Range(8, 12))); // damage to enemy, damage to character
                }

            }

            if(EnemyHealth.GetHealthCount() == 0)
            {
                Swarm08_Animator.SetTrigger("Die");

                // wait?

                this.GetComponent<Collider>().enabled = false;

                CharacterStrength.GetStrength(AddStrengthAfterVictory);

                Movement.StartMove();
            }
            else if(CharacterHealth.GetHealthCount() == 0)
            {
                Movement.StartMove();
 
            }
           
        }
    }

     private void ResetChoose()
    {
        readyToChoose = true;
    }

    private IEnumerator PerformActionsWithDelay(CharacterHealth enemyHealth, CharacterHealth characterHealth, int enemyDamage, int characterDamage)
    {
        readyToChoose = false;
        // First set of actions
        enemyHealth.TakeDamage(enemyDamage);
        Swarm08_Animator.SetTrigger("GetHit");

        // Wait for 2 seconds
        yield return new WaitForSeconds(2);

         if ((enemyHealth.GetHealthCount() != 0))
         {
              // Second set of actions
            characterHealth.TakeDamage(characterDamage);
            Swarm08_Animator.SetTrigger("Attack");

         }

         Invoke(nameof(ResetChoose), Cooldown);
    }
}
