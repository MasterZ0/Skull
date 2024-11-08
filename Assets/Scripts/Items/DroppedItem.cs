﻿using AdventureGame.Audio;
using AdventureGame.BattleSystem;
using AdventureGame.Data;
using AdventureGame.Items.Data;
using AdventureGame.ObjectPooling;
using AdventureGame.Shared.ExtensionMethods;
using Sirenix.OdinInspector;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
using Random = UnityEngine.Random;

namespace AdventureGame.Items
{
    public class DroppedItem : MonoBehaviour
    {
        [Title("Drop Item")]
        [SerializeField] private Rigidbody rigidbod;
        [SerializeField] private Collider triggerCollider;
        [SerializeField] private Transform modelTransform;

        protected ItemReference item;
        private GameObject model;

        private const string FadeOut = "FadeOut";

        protected GeneralSettings Settings => GameSettings.General;

        protected virtual void OnEnable()
        {
            triggerCollider.gameObject.SetActive(true);
        }

        private void OnDisable()
        {
            model.ReturnToPool();
        }

        #region Public
        public void SetItem(ItemReference itemData)
        {
            //GameSettings
            item = itemData;
            model = ObjectPool.SpawnPooledObject(itemData.Instance.model, modelTransform.position, modelTransform.rotation, modelTransform);

            Pop();
        }

        private void Pop()
        {
            float zxMaxVelocity = Settings.DropItemZXMaxVelocity;
            Vector2 yVelocity = Settings.DropItemYVelocityRange;

            rigidbod.velocity = new Vector3()
            {
                x = Random.Range(-zxMaxVelocity, zxMaxVelocity),
                y = yVelocity.RandomRange(),
                z = Random.Range(-zxMaxVelocity, zxMaxVelocity)
            };
        }

        public void OnFadeOutEnd() => this.ReturnToPool(); // TODO
        #endregion

        private void OnTriggerEnter(Collider col)
        {
            if (col.attachedRigidbody && col.attachedRigidbody.TryGetComponent(out IBattleEntity entity))
            {
                bool successful = item.Instance switch
                {
                    StatusItemData statusItem => TryRestore(entity, statusItem),
                    _ => TryCollect(entity)
                };

                if (successful)
                {
                    Transform collectFX = item.Instance.collectFX;
                    if (collectFX)
                    {
                        ObjectPool.SpawnPooledObject(collectFX, entity.Center.position, entity.Center.rotation, entity.Center);
                    }

                    triggerCollider.gameObject.SetActive(false);
                    this.ReturnToPool();
                }
            }
        }

        private bool TryRestore(IBattleEntity entity, StatusItemData statusItem)
        {
            if (entity is IStatusOwner statusOwner)
            {
                return statusOwner.Restore(statusItem.attributePoint, statusItem.restoreValue);
            }
            return false;
        }

        private bool TryCollect(IBattleEntity entity)
        {
            if (entity is IInventoryOwner inventoryOwner)
            {
                return inventoryOwner.AddItem(item);
            }
            return false;
        }
    }
}