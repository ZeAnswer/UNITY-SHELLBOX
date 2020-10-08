using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEffect : MonoBehaviour
{
    public GameObject explosionParticles;
    private const float MIN_FORCE = 2000f;
    private const float MAX_FORCE = 20000f;

    private static ExplosionEffect instance;
    public static ExplosionEffect GetInstance(){
        return instance;
    }

    void Awake(){
        instance = this;
    }

    public void MakeExplosionEffect(Vector3 pos, float force){
        if (force >= MIN_FORCE){
            float explosion_power = ((force - MIN_FORCE) / (MAX_FORCE-MIN_FORCE)) * 5f;
            GameObject clone = Instantiate(explosionParticles, pos, Quaternion.identity);
            ParticleSystem ps = clone.GetComponent<ParticleSystem>();
            var emissionmod = ps.emission;
            var mainmod = ps.main;
            emissionmod.rateOverTime = explosion_power * 1000f;
            mainmod.startSpeed = explosion_power * 10f;
            ps.Play();
        }
    }
}
