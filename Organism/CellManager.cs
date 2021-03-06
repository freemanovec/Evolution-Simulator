﻿using Evolution_Simulator.Positioning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evolution_Simulator.Organism
{
    class CellManager
    {
        public void Tick(CellManagerData data)
        {
            PhaseEliminative(data);
            PhaseMurrative(data);
            PhaseAnimative(data);
            PhaseDigestive(data);
            PhaseProductive(data);
        }

        private void PhaseEliminative(CellManagerData data)
        {
            CheckAndKill(data);
        }
        private void PhaseAnimative(CellManagerData data)
        {
            Move(data);
        }
        private void PhaseDigestive(CellManagerData data)
        {
            Eat(data);
        }
        private void PhaseProductive(CellManagerData data)
        {

        }
        private void PhaseMurrative(CellManagerData data)
        {
            Mate(data);
        }
        
        private bool Mate(CellManagerData data)
        {
            Cell mate = GetMatingCell(data);
            if (mate != null)
                return MateInternal(data.CellCurrent, mate, data.Map);
            return false;
        }
        private bool MateInternal(Cell parent0, Cell parent1, Map map)
        {
            if (
                !HasEnergy(parent0, parent0.EnergyNeededToReproduce).Item1 ||
                !HasEnergy(parent1, parent1.EnergyNeededToReproduce).Item1
            )
            {
                return false;
            }
            DrainEnergy(parent0, parent0.EnergyNeededToReproduce);
            DrainEnergy(parent1, parent1.EnergyNeededToReproduce);
            Cell child = new Cell(parent0.Position);
            map.AddCell(child);
            return true;
        }
        private void CheckAndKill(CellManagerData data)
        {
            Cell cell = data.CellCurrent;
            double needed = cell.EnergyNeededToTick;
            if (!HasEnergy(cell, needed).Item1)
                Kill(cell, data.Map, "Not enough energy to survive");
            else
                DrainEnergy(cell, needed);
        }
        private bool Kill(Cell cell, Map parentMap, string reason = "Unknown cause")
        {
            bool success = parentMap.RemoveCell(cell);
            if (success)
            {
                Logger.Logger.Log("Death: " + cell + ", cause: " + reason);
            }
            return success;
        }
        private void Move(CellManagerData data)
        {
            Cell cell = data.CellCurrent;
            if(!HasEnergy(cell, cell.EnergyNeededToMove).Item1)
            {
                return;
            }
            Vector2? newPosition = GetBestPosition(data);
            if (newPosition == null)
                return;

            Logger.Logger.Log(cell + " Moving from " + cell.Position + " to " + newPosition);

            DrainEnergy(cell, cell.EnergyNeededToMove);
            data.Map.MoveCell(cell, (Vector2)newPosition);
        }
        private Vector2? GetBestPosition(CellManagerData data)
        {
            Vector2? bestPosition = null;
            double biasBest = -1;
            double biasMate = 40d;
            double biasFoodPoint = 55d;
            for(int i = 0; i < 8; i++)
            {
                double biasCurrent = 0;
                Tile tileInspected = data.TilesSurrounding[i];
                if (tileInspected == null)
                    continue;
                Cell mate = GetMatingCell(data, i);
                if (mate != null &&
                    HasEnergy(data.CellCurrent, data.CellCurrent.EnergyNeededToReproduce).Item1 &&
                    HasEnergy(mate, mate.EnergyNeededToReproduce).Item1)
                {
                    biasCurrent += biasMate;
                }
                double biasFoodCurrent = tileInspected.Food * biasFoodPoint;
                biasCurrent += biasFoodCurrent;
                if(biasCurrent > biasBest)
                {
                    biasBest = biasCurrent;
                    bestPosition = tileInspected.Position;
                }
            }
            return bestPosition;
        }
        private Cell GetMatingCell(CellManagerData data, int index=8)
        {
            List<Cell> cellsOnTile = data.CellsSurrounding[index];
            foreach(Cell cell in cellsOnTile)
            {
                if (HasEnergy(cell, cell.EnergyNeededToReproduce).Item1)
                    return cell;
            }
            return null;
        }
        private Cell GetMatingCellSurrounding(CellManagerData data)
        {
            List<Cell>[] cellsTiles = data.CellsSurrounding;
            for(int i = 0; i < cellsTiles.Length; i++)
            {
                Cell cell = GetMatingCell(data, i);
                if (cell != null)
                    return cell;
            }
            return null;
        }
        private void Eat(CellManagerData data)
        {
            Tile tile = data.TileCurrent;
            Cell cell = data.CellCurrent;

            double foodAvailable = tile.Food;
            double foodCapacity = cell.EnergyCapacity / cell.RatioFoodEnergy;
            double foodConsumable = foodCapacity - (cell.Energy / cell.RatioFoodEnergy);
            double foodToEat = (foodConsumable >= foodAvailable) ? foodAvailable : foodConsumable;
            foodToEat = (foodToEat > cell.MaxFoodEatenOnTick) ? cell.MaxFoodEatenOnTick : foodToEat;

            Logger.Logger.Log(cell + " Eating " + foodToEat);
            Logger.Logger.Log(cell + " Tile_Before: " + tile.Food);
            tile.Food -= foodToEat;
            Logger.Logger.Log(cell + " Tile_After: " + tile.Food);

            Logger.Logger.Log(cell + " Cell_Before: " + cell.Energy);
            AddEnergy(cell, foodToEat * cell.RatioFoodEnergy);
            Logger.Logger.Log(cell + " Cell_After: " + cell.Energy);
        }
        private Tuple<bool, double> HasEnergy(Cell cell, double level)
        {
            double energy = cell.Energy;
            double final = energy - level;
            return final > 0 ? new Tuple<bool, double>(true, final) : new Tuple<bool, double>(false, final);
        }
        private bool DrainEnergy(Cell cell, double level)
        {
            Logger.Logger.Log(cell + " Draining energy: From: " + cell.Energy);
            Logger.Logger.Log(cell + " Draining energy: With: " + level);
            if (HasEnergy(cell, level).Item1)
            {
                double prev = cell.Energy;
                double fin = prev - level;
                cell.Energy = fin;
                Logger.Logger.Log(cell + " Draining energy: To: " + cell.Energy);
                return true;
            }
            Logger.Logger.Log(cell + " Draining energy: Not enough energy");
            return false;
        }
        private bool AddEnergy(Cell cell, double level)
        {
            double energy = cell.Energy;
            Logger.Logger.Log(cell + " Adding energy: From: " + energy);
            Logger.Logger.Log(cell + " Adding energy: With: " + level);
            double final = energy + level;
            bool success;
            if(final > cell.EnergyCapacity)
            {
                cell.Energy = cell.EnergyCapacity;
                success = false;
            }
            else
            {
                cell.Energy += level;
                success = true;
            }
            Logger.Logger.Log(cell + " Adding energy: To: " + cell.Energy);
            return success;
        }
    }
}
