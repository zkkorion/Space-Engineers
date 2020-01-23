using Sandbox.Game;
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
    class HydroGeneratorControl : MyGridProgram
    {
        public void Program()
        {
            Runtime.UpdateFrequency = UpdateFrequency.Update100;
        }
        public void Main(string argument, UpdateType updateSource)
        {
            int minValue = 30;
            int maxValue = 90;
            char[] delimiterChars = { ',', '.', ':', ';', '\\', '|', '/', '-', '–'};
            if(!string.IsNullOrEmpty(argument))
            {
                string[] range = argument.Split(delimiterChars);
                bool minVS = int.TryParse(range[0], out minValue);
                bool maxVS = int.TryParse(range[1], out maxValue);
                if(!minVS || !maxVS)
                    Echo("Wrong value of argument. Need format: 10-20 (beatwen number can be: , . : ; \\ | / - –). Range will be set to default");
            }
            List<IMyBatteryBlock> batteries = new List<IMyBatteryBlock>();
            List<IMyTerminalBlock> engines = new List<IMyTerminalBlock>();
            IMyBlockGroup group = GridTerminalSystem.GetBlockGroupWithName("HydrogenEngines");
            if (group == null)
            {
                Echo("Group not found");
                return;
            }
            GridTerminalSystem.GetBlocksOfType(batteries);
            group.GetBlocks(engines);
            float totalMaxCharge = 0, curTotalCharge = 0, curChargeValue;
            foreach (IMyBatteryBlock battery in batteries)
            {
                totalMaxCharge += battery.MaxStoredPower;
                curTotalCharge += battery.CurrentStoredPower;
            }
            curChargeValue = curTotalCharge / totalMaxCharge * 100;
            if (curChargeValue < minValue)
                foreach (IMyTerminalBlock engine in engines)
                    engine.ApplyAction("OnOff_On");
            if (curChargeValue > maxValue)
                foreach (IMyTerminalBlock engine in engines)
                    engine.ApplyAction("OnOff_Off");
        }
    }
}