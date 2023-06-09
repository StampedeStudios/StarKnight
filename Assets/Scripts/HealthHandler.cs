using System.Collections.Generic;
using UnityEngine;

public class HealthHandler : MonoBehaviour, IHittableInterface
{
    [Tooltip("Vita del possessore")]
    public int Health = 100;

    public List<AudioClip> deathExplosion;

    private StatsHandler statsHandler = null;

    private void Awake()
    {
        statsHandler = GetComponent<StatsHandler>();
    }

    private void Start()
    {
        if (statsHandler)
            statsHandler.SetupHealth(Health, Health);
    }

    public void DealDamage(int damage)
    {
        // sottraggo il danno subito
        Health -= damage;

        if (statsHandler)
            statsHandler.UpdateHealth(Health);

        // controllo se la vita è finita
        if (Health <= 0)
            Die();
    }

    private void Die()
    {
        // comunico lo stato di morte su tutti i componenti del possesore
        foreach (var item in GetComponents<IDeathIterface>())
        {
            item.OnDeathEvent();
        }

        // suono di esplosione
        AudioSource.PlayClipAtPoint(deathExplosion[Random.Range(0, deathExplosion.Count - 1)], transform.position);

        // recupero l' animatore ed eseguo l' animazione dedicata 
        Animator anim = GetComponent<Animator>();
        if (anim)
            anim.SetBool("isDead", true);
        else
            Destroy(gameObject);

        Destroy(this);
    }

}
