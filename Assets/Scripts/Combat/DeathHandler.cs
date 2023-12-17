using UnityEngine;

public class DeathHandler : MonoBehaviour
{
    private void OnEnable() {
        Health.OnDeath += SpawnDeathSplatterVFX;
        Health.OnDeath += SpawnDeathParticleVFX;
    }
    private void OnDisable() {
        Health.OnDeath -= SpawnDeathSplatterVFX;
        Health.OnDeath -= SpawnDeathParticleVFX;
    }

    private void SpawnDeathSplatterVFX(Health sender){
        GameObject newDeathSplatter = Instantiate(sender.DeathSplatterVFX, sender.transform.position, RandomRotation());
        SpriteRenderer spriteRenderer = newDeathSplatter.GetComponent<SpriteRenderer>();
        spriteRenderer.color = sender.ColorChanger.DefaultColor;
        newDeathSplatter.transform.SetParent(transform);
    }

    private void SpawnDeathParticleVFX(Health sender){
        GameObject newParticleVFX = Instantiate(sender.DeathParticleVFX, sender.transform.position, Quaternion.identity);
        ParticleSystem.MainModule particleSystem = newParticleVFX.GetComponent<ParticleSystem>().main;
        particleSystem.startColor = sender.ColorChanger.DefaultColor;
    }

    private Quaternion RandomRotation(){
        return Quaternion.Euler(0, 0, Random.Range(0,360));
    }
}
