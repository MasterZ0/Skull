﻿using System;
using System.Collections;
using UnityEngine;

// Responsável por guardar os dados base e receber dano
public abstract class Enemy : PooledObject, IDamageable {
    [Header("Enemy Config")]
    [SerializeField] protected EnemyAttributes enemyAttributes;
    [SerializeField] protected SkinnedMeshRenderer[] skinnedMeshRenderers;

    [Header(" - Prefabs")]
    [SerializeField] protected PooledObject deathFx;
    [SerializeField] protected PooledObject disappearFx;

    public EnemyState enemyState { get; protected set; }
    public EnemyAttributes EnemyAttributes { get => enemyAttributes; }
    public bool IsDead { get; private set; }

    private Material[] defaultMaterials;
    private Material hitMaterial;

    private int currentLife;

    public static event Action<Enemy> OnEnemyDeath = delegate { };
    public event Action OnTakeDamage = delegate { };

    protected virtual void Awake() {
        hitMaterial = Resources.Load<Material>(Constants.Path.HIT);

        defaultMaterials = new Material[skinnedMeshRenderers.Length];
        for (int i = 0; i < skinnedMeshRenderers.Length; i++) {
            defaultMaterials[i] = skinnedMeshRenderers[i].material;
        }
    }
    protected override void OnEnablePooledObject() {
        currentLife = enemyAttributes.maxLife;
    }
    public virtual bool TakeDamage(Damage damage) {
        currentLife -= damage.value;
        if(currentLife <= 0) {
            KillEnemy();
            return true;
        }
        StartCoroutine(HitMaterial());
        return false;
    }
    private void KillEnemy() {
        IsDead = true;
        gameObject.layer = Constants.Layer.INVINCIBLE;
        deathFx.SpawnObject(transform.position, transform.rotation);
        OnEnemyDeath.Invoke(this);
        EnemyDeath();
        StartCoroutine(DestroyEnemy());
    }
    protected abstract void EnemyDeath();   // Animação de morte, desabilitar rb, coroutines, etc...    
   
    private IEnumerator HitMaterial() {
        for (int i = 0; i < skinnedMeshRenderers.Length; i++) {
            skinnedMeshRenderers[i].material = hitMaterial;
        }
        yield return new WaitForSeconds(.1f);

        for (int i = 0; i < skinnedMeshRenderers.Length; i++) {
            skinnedMeshRenderers[i].material = defaultMaterials[i];
        }
    }
    private IEnumerator DestroyEnemy() {
        yield return new WaitForSeconds(2);
        disappearFx.SpawnObject(transform.position, transform.rotation);
        DesactivePooledObject();
    }
}