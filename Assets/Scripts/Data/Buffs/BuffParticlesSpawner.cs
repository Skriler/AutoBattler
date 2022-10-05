using System.Collections.Generic;
using AutoBattler.Data.Units;
using UnityEngine;
using AutoBattler.EventManagers;
using AutoBattler.UnitsContainers.Containers;
using AutoBattler.Data.Players;

namespace AutoBattler.Data.Buffs
{
    public class BuffParticlesSpawner : MonoBehaviour
    {
        [Header("Particles")]
        [SerializeField] private GameObject applyBuffParticlesPrefab;
        [SerializeField] private GameObject removeBuffParticlesPrefab;

        [Header("Parameters")]
        [SerializeField] private PlayerFieldContainer fieldContainer;
        [SerializeField] private string foregroundLayerName = "Foreground";
        [SerializeField] private Vector3 applyBuffParticlesOffset;
        [SerializeField] private Vector3 removeBuffParticlesOffset;

        private List<Buff> buffs;
        private BaseUnit[,] units;

        private void Awake()
        {
            UnitsEventManager.OnUnitAddedOnField += CheckForApplyBuffParticles;
            UnitsEventManager.OnUnitRemovedFromField += CheckForRemoveBuffParticles;
            BuffsEventManager.OnBuffLevelIncreased += InstantiateApplyBuffParticlesForEveryUnit;
            BuffsEventManager.OnBuffLevelDecreased += InstantiateRemoveBuffParticlesForEveryUnit;
        }

        private void OnDestroy()
        {
            UnitsEventManager.OnUnitAddedOnField -= CheckForApplyBuffParticles;
            UnitsEventManager.OnUnitRemovedFromField -= CheckForRemoveBuffParticles;
            BuffsEventManager.OnBuffLevelIncreased = InstantiateApplyBuffParticlesForEveryUnit;
            BuffsEventManager.OnBuffLevelDecreased -= InstantiateRemoveBuffParticlesForEveryUnit;
        }

        private void Start()
        {
            buffs = BuffContainer.Instance.GetBuffs();
        }

        private void CheckForApplyBuffParticles(BaseUnit unit)
        {
            foreach (Buff buff in buffs)
            {
                if (!buff.IsActive())
                    continue;

                InstantiateApplyBuffParticles(
                    applyBuffParticlesPrefab, 
                    unit.transform.position + applyBuffParticlesOffset
                    );
                return;
            }
        }

        private void CheckForRemoveBuffParticles(BaseUnit unit)
        {
            foreach (Buff buff in buffs)
            {
                if (!buff.IsActive())
                    continue;

                InstantiateApplyBuffParticles(
                    removeBuffParticlesPrefab, 
                    unit.transform.position + removeBuffParticlesOffset
                    );
                return;
            }
        }

        private void InstantiateApplyBuffParticlesForEveryUnit(Buff buff)
        {
            if (units == null)
                units = fieldContainer.GetArmy();

            foreach (BaseUnit unit in units)
            {
                if (unit == null)
                    continue;

                InstantiateApplyBuffParticles(
                    applyBuffParticlesPrefab,
                    unit.transform.position + applyBuffParticlesOffset
                    );
            }

        }
        private void InstantiateRemoveBuffParticlesForEveryUnit(Buff buff)
        {
            foreach (BaseUnit unit in units)
            {
                if (unit == null)
                    continue;

                InstantiateApplyBuffParticles(
                    removeBuffParticlesPrefab,
                    unit.transform.position + removeBuffParticlesOffset
                    );
            }
        }


        private void InstantiateApplyBuffParticles(GameObject buffParticlesPrefab, Vector2 spawnPosition)
        {
            GameObject buffParticles = Instantiate(
                buffParticlesPrefab,
                spawnPosition,
                applyBuffParticlesPrefab.transform.rotation
                );

            buffParticles.transform.SetParent(this.gameObject.transform);
            buffParticles.GetComponent<Renderer>().sortingLayerName = foregroundLayerName;
        }
    }
}
