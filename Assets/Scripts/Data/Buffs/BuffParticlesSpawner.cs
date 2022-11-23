using System.Collections.Generic;
using AutoBattler.Data.Units;
using UnityEngine;
using AutoBattler.EventManagers;
using AutoBattler.UnitsContainers.Containers.Field;

namespace AutoBattler.Data.Buffs
{
    public class BuffParticlesSpawner : MonoBehaviour
    {
        [Header("Target buffs")]
        [SerializeField] private List<Buff> buffs;

        [Header("Particles")]
        [SerializeField] private GameObject applyBuffParticlesPrefab;
        [SerializeField] private GameObject removeBuffParticlesPrefab;

        [Header("Parameters")]
        [SerializeField] private MemberFieldContainer fieldContainer;
        [SerializeField] private string foregroundLayerName = "Foreground";
        [SerializeField] private Vector3 applyBuffParticlesOffset;
        [SerializeField] private Vector3 removeBuffParticlesOffset;

        private BaseUnit[,] units;

        private void Awake()
        {
            BuffsEventManager.OnAppliedBuffsForUnit += InstantiateApplyBuffParticlesOnUnit;
            BuffsEventManager.OnRemovedBuffsFromUnit += InstantiateRemoveBuffParticlesOnUnit;
            BuffsEventManager.OnBuffLevelIncreased += InstantiateApplyBuffParticlesOnUnits;
            BuffsEventManager.OnBuffLevelDecreased += InstantiateRemoveBuffParticlesOnUnits;
        }

        private void OnDestroy()
        {
            BuffsEventManager.OnAppliedBuffsForUnit -= InstantiateApplyBuffParticlesOnUnit;
            BuffsEventManager.OnRemovedBuffsFromUnit -= InstantiateRemoveBuffParticlesOnUnit;
            BuffsEventManager.OnBuffLevelIncreased = InstantiateApplyBuffParticlesOnUnits;
            BuffsEventManager.OnBuffLevelDecreased -= InstantiateRemoveBuffParticlesOnUnits;
        }

        private void InstantiateApplyBuffParticlesOnUnit(BaseUnit unit)
        {
            InstantiateBuffParticles(
                    applyBuffParticlesPrefab,
                    unit.transform.position + applyBuffParticlesOffset
                    );
        }

        private void InstantiateRemoveBuffParticlesOnUnit(BaseUnit unit)
        {
            InstantiateBuffParticles(
                    removeBuffParticlesPrefab,
                    unit.transform.position + removeBuffParticlesOffset
                    );
        }

        private void InstantiateApplyBuffParticlesOnUnits(Buff buff)
        {
            if (!buffs.Contains(buff))
                return;

            InstantiateBuffParticlesOnUnits(applyBuffParticlesPrefab, applyBuffParticlesOffset);
        }

        private void InstantiateRemoveBuffParticlesOnUnits(Buff buff)
        {
            if (!buffs.Contains(buff))
                return;

            InstantiateBuffParticlesOnUnits(removeBuffParticlesPrefab, removeBuffParticlesOffset);
        }

        private void InstantiateBuffParticlesOnUnits(GameObject buffParticlesPrefab, Vector3 spawnPosition)
        {
            if (units == null)
                units = fieldContainer.GetArmy();

            foreach (BaseUnit unit in units)
            {
                if (unit == null)
                    continue;

                InstantiateBuffParticles(
                    buffParticlesPrefab,
                    unit.transform.position + spawnPosition
                    );
            }
        }

        private void InstantiateBuffParticles(GameObject buffParticlesPrefab, Vector2 spawnPosition)
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
