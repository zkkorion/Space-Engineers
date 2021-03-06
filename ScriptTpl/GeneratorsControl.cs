﻿using Sandbox.Game;
using Sandbox.Game.EntityComponents;
using Sandbox.Common;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.Entities.Blocks;
using SpaceEngineers.Game.ModAPI.Ingame;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System;
using VRage.Game.Components;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Game;
using VRageMath;

namespace IngameScript
{
    class GeneratorsControl : MyGridProgram
    {
        public void Program()
        {
            Runtime.UpdateFrequency = UpdateFrequency.Update100;
        }
        public void Main(string argument, UpdateType updateSource)
        {
            int minValue = 30;
            int maxValue = 90;
            string defaultGroupName = "Generators";
            Echo("Default group name is 'Generators', default min value - 30%, max value - 90%. You can use yourself setting by change it in script.");
            List<IMyBatteryBlock> batteries = new List<IMyBatteryBlock>();
            List<IMyTerminalBlock> generators = new List<IMyTerminalBlock>();
            IMyBlockGroup group = GridTerminalSystem.GetBlockGroupWithName(defaultGroupName);
            if (group == null)
            {
                Echo("Group not found");
                return;
            }
            GridTerminalSystem.GetBlocksOfType(batteries);
            group.GetBlocks(generators);
            float totalMaxCharge = 0, curTotalCharge = 0, curChargeValue;
            foreach (IMyBatteryBlock battery in batteries)
            {
                totalMaxCharge += battery.MaxStoredPower;
                curTotalCharge += battery.CurrentStoredPower;
            }
            curChargeValue = curTotalCharge / totalMaxCharge * 100;
            if (curChargeValue < minValue)
                foreach (IMyTerminalBlock engine in generators)
                    engine.ApplyAction("OnOff_On");
            if (curChargeValue > maxValue)
                foreach (IMyTerminalBlock engine in generators)
                    engine.ApplyAction("OnOff_Off");
        }
    }
}