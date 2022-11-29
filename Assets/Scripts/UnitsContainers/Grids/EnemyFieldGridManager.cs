using UnityEngine;
using AutoBattler.Data.Units;
using AutoBattler.UnitsContainers.Enums;

namespace AutoBattler.UnitsContainers.Grids
{
    public class EnemyFieldGridManager : GridManager
    {
        protected override TileStatus GetCurrentTileStatus(Vector2Int index) => TileStatus.Closed;

        public void ChangeTileStatus(Vector2Int index, TileStatus tileStatus) => tiles[index.x, index.y].SetTileStatus(tileStatus);

        public void SpawnUnits(BaseUnit[,] units, Transform unitsContainer, bool isAttackSoundMuted)
        {
            Vector3 newUnitPosition;
            BaseUnit newUnit;

            for (int i = 0; i < units.GetLength(0); ++i)
            {
                for (int j = 0; j < units.GetLength(1); ++j)
                {
                    if (units[i, j] == null)
                        continue;

                    ChangeTileStatus(new Vector2Int(i, j), TileStatus.Opened);
                    newUnitPosition = tiles[i, j].transform.position;

                    newUnit = Instantiate(units[i, j], newUnitPosition, Quaternion.identity);
                    newUnit.transform.SetParent(unitsContainer);
                    newUnit.EnterEnemyMode();
                    newUnit.ShowHealthBar();
                    newUnit.IsAttackSoundMuted = isAttackSoundMuted;

                    units[i, j] = newUnit;
                }
            }
        }
    }
}
