using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Photon.Pun;
public class CaballeroOscuroUlti : Ultimate
{
    public int ultimateDamage;
    public float timeToRemoveDarkMark;

    private static readonly object castingUltimate = new object();

    private bool casting = false;

    protected override IEnumerator castUltimate()
    {
        lock (castingUltimate)
        {
            if (CanCastUltimate())
            {
                casting = true;

                UnitBehaviour unit = GetComponent<UnitBehaviour>();
                unit.StopAllCoroutines();
                Animator anim = GetComponent<Animator>();

                bool attack = anim.GetBool("Attack");
                bool running = anim.GetBool("Running");

                anim.SetBool("Attack", true);
                anim.SetBool("Running", false);

                GetComponent<PhotonView>().RPC("applyDarkMarkToEnemies", RpcTarget.All, unit.typeOfEnemy);
                GetComponent<PhotonView>().RPC("ResetEnergy", RpcTarget.All);

                yield return new WaitForSeconds(unit.getAnimDuration());

                unit.GoToNextState();

                yield return new WaitForSeconds(timeToRemoveDarkMark);

                GetComponent<PhotonView>().RPC("removeDarkMarkFromEnemies", RpcTarget.All, unit.typeOfEnemy);

                casting = false;
            }
        }
    }

    [PunRPC]
    private void applyDarkMarkToEnemies(string typeOfEnemy)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(typeOfEnemy);
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] != null && enemies[i].GetComponent<UnitBehaviour>().getCurrentHealth() > 0)
            {
                enemies[i].GetComponent<SpriteRenderer>().color = new UnityEngine.Color(0.2f, 0f, 0.4f, 1f);
            } 
        }
    }

    [PunRPC]
    private void removeDarkMarkFromEnemies(string typeOfEnemy)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(typeOfEnemy);
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] != null && enemies[i].GetComponent<UnitBehaviour>().getCurrentHealth() > 0 && 
                enemies[i].GetComponent<SpriteRenderer>().color == new UnityEngine.Color(0.2f, 0f, 0.4f, 1f))
            {
                enemies[i].GetComponent<UnitBehaviour>().takeDamage(ultimateDamage);
                enemies[i].GetComponent<SpriteRenderer>().color = new UnityEngine.Color(1f, 1f, 1f, 1f);
            }
        }
    }

    protected override bool CanCastUltimate()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(GetComponent<UnitBehaviour>().typeOfEnemy);

        bool canCast = false;

        for (int i = 0; i < enemies.Length && !canCast; i++)
        {
            canCast = enemies[i] != null && enemies[i].GetComponent<UnitBehaviour>().getCurrentHealth() > 0 &&
                enemies[i].GetComponent<SpriteRenderer>().color != new UnityEngine.Color(0.2f, 0f, 0.4f, 1f);
        }
        return canCast;
    }

    protected override void TryCastUltimate()
    {
        if (energy >= maxEnergy && !casting)
        {
            StartCoroutine(castUltimate());
        }
    }
}
