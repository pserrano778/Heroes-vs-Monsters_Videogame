using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Photon.Pun;
public class CaballeroOscuroUlti : Ultimate
{
    public int ultimateDamage;
    public float timeToRemoveDarkMark;

    // Lock to avoid two Dark Knights cast the ultimate at the same time
    private static readonly object castingUltimate = new object();

    private bool casting = false;

    protected override IEnumerator castUltimate()
    {
        // Lock the object
        lock (castingUltimate)
        {
            // See if he can cast the uiltimate
            if (CanCastUltimate())
            {
                // Set status to casting
                casting = true;

                // Get the rigidBody and the velocity
                Rigidbody2D body2d = GetComponent<Rigidbody2D>();
                Vector2 velocity = body2d.velocity;

                // Get the unit
                UnitBehaviour unit = GetComponent<UnitBehaviour>();

                // Stop all unit coroutines 
                unit.StopAllCoroutines();

                // Get the animator
                Animator anim = GetComponent<Animator>();

                // Store the current status
                bool attack = anim.GetBool("Attack");
                bool running = anim.GetBool("Running");

                // Set the new status to attack
                anim.SetBool("Attack", true);
                anim.SetBool("Running", false);

                // Set the velocity to 0
                body2d.velocity = new Vector2(0, 0);

                // Apply Dark Mark using RPC
                GetComponent<PhotonView>().RPC("applyDarkMarkToEnemies", RpcTarget.All, unit.typeOfEnemy);

                // Reset energy using RPC
                GetComponent<PhotonView>().RPC("ResetEnergy", RpcTarget.All);

                // Wait until the animation has finished
                yield return new WaitForSeconds(unit.getAnimDuration());

                // Return to the previous status
                anim.SetBool("Attack", attack);
                anim.SetBool("Running", running);
                body2d.velocity = velocity;
                unit.GoToNextState();

                // Wait some time to remove the marks
                yield return new WaitForSeconds(timeToRemoveDarkMark);

                // Remove the marks using rpc
                GetComponent<PhotonView>().RPC("removeDarkMarkFromEnemies", RpcTarget.All, unit.typeOfEnemy);

                // He has finished the ultimate cast
                casting = false;
            }
        }
    }

    [PunRPC]
    private void applyDarkMarkToEnemies(string typeOfEnemy)
    {
        // Get all enemies
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(typeOfEnemy);
        for (int i = 0; i < enemies.Length; i++)
        {
            // Apply the mark if the dont have any other buff/debuff
            if (enemies[i] != null && enemies[i].GetComponent<UnitBehaviour>().getCurrentHealth() > 0
                && enemies[i].GetComponent<SpriteRenderer>().color == new UnityEngine.Color(1f, 1f, 1f, 1f))
            {
                // Apply the mark
                enemies[i].GetComponent<SpriteRenderer>().color = new UnityEngine.Color(0.2f, 0f, 0.4f, 1f);
            } 
        }
    }

    [PunRPC]
    private void removeDarkMarkFromEnemies(string typeOfEnemy)
    {
        // Get all enemies
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(typeOfEnemy);
        for (int i = 0; i < enemies.Length; i++)
        {
            // Remove the mark if they have it applied
            if (enemies[i] != null && enemies[i].GetComponent<UnitBehaviour>().getCurrentHealth() > 0 && 
                enemies[i].GetComponent<SpriteRenderer>().color == new UnityEngine.Color(0.2f, 0f, 0.4f, 1f))
            {
                // Do the dark mark damage
                enemies[i].GetComponent<UnitBehaviour>().takeDamage(ultimateDamage);
                enemies[i].GetComponent<SpriteRenderer>().color = new UnityEngine.Color(1f, 1f, 1f, 1f);
            }
        }
    }

    protected override bool CanCastUltimate()
    {
        // Get all enemies
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(GetComponent<UnitBehaviour>().typeOfEnemy);

        bool canCast = false;

        // Chek enemies
        for (int i = 0; i < enemies.Length && !canCast; i++)
        {
            // Dark knigth will be able to cast his ultimate if at least one enemy dont have it applied yet
            canCast = enemies[i] != null && enemies[i].GetComponent<UnitBehaviour>().getCurrentHealth() > 0 &&
                enemies[i].GetComponent<SpriteRenderer>().color != new UnityEngine.Color(0.2f, 0f, 0.4f, 1f);
        }
        return canCast;
    }

    protected override void TryCastUltimate()
    {
        // Check if he has enough energy and is not casting
        if (energy >= maxEnergy && !casting)
        {
            // Cast the ultimate using a coroutine
            StartCoroutine(castUltimate());
        }
    }
}
