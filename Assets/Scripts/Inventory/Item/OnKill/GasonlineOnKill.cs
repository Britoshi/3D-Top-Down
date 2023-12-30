using Game;
using Game.Utilities; 
using System.Collections; 
/*
public class GasolineOnKill : OnKillAffectHandler
{
    public float Radius = 3f;
    public LayerMask FriendlyFaction = 10 | 15;
    public LayerMask HostileFaction = 20;
    public override void OnTrigger()
    {
        LifeTimeCountDown = LifeTime;
        sprite = GetComponent<SpriteRenderer>();
        originalColor = sprite.color; 

        Radius += killer.status.Range.GetBaseValue();//.baseValue;
        Radius *= killer.status.Range.GetMultiplier();//attribute.ValuePair.multiplier; 

        StartCoroutine(StartDelayFuse());
    }

    IEnumerator StartDelayFuse()
    {
        yield return new WaitForSeconds(explosionFuseDelay);
        Triggered = true;
        Explode();
    }

    SpriteRenderer sprite;
    Color originalColor;

    public void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    public float LifeTime = .5f;
    public float LifeTimeCountDown;

    public float explosionFuseDelay = 0.2f;

    public void Update()
    {
        if (!Triggered) return;

        if(LifeTimeCountDown <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            float ratio = LifeTimeCountDown / LifeTime;

            sprite.color = new(originalColor.r * ratio, originalColor.g * ratio, originalColor.b * ratio, originalColor.a * ratio);

            transform.localScale = new Vector2(ratio * Radius, ratio * Radius);

            LifeTimeCountDown -= Time.deltaTime;
        }
    }

    void Explode()
    {
        LayerMask targetLayer = HostileFaction;

        if (killer.Faction == EntityFaction.Enemy)
        {
            if (affect.target == AffectTarget.Enemy)
                targetLayer = FriendlyFaction;
        }
        else if(affect.target == AffectTarget.Self)
            targetLayer = FriendlyFaction;


        foreach (var hit in Physics2D.OverlapCircleAll(transform.position, Radius, targetLayer))
        {
            if (affect.target == AffectTarget.Self) 
                killer.status.ApplyResourceAffect(killer, affect.affect, applyOnHit: true); 
            else
            {
                if (hit.TryGetComponent<Entity>(out var enemyEntity))
                {
                    if (hit.Equals(deadVictim)) continue;
                    enemyEntity.status.ApplyResourceAffect(killer, affect.affect, applyOnHit: true);
                }
                else Debug.LogError("Who the fcuk are you?");
            }
        }
    }
}*/