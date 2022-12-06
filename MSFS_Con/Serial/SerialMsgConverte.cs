using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSFS_Con.Serial
{
    internal class SerialMsgConverter
    {
        public static UInt32 si_AUTOPILOT_ALTITUDE_LOCK_VAR = 0;
        public static UInt32 si_AUTOPILOT_HEADING_LOCK_DIR = 0;
        const UInt32 ci_ap_step_alt_ft = 100;
        const UInt32 ci_ap_step_heading_deg = 1;
        public class EventData
        {
            public EVENTS events { get; set; }
            public UInt32 value { get; set; }


            public EventData()
            {
                events = EVENTS.EVENTS_UNKNOWN;
                value = 0;
            }
        }

        public static EventData ToEvents(string msg)
        {
            EventData event_data = new EventData();

            if ("AP_SW" == msg)
            {
                event_data.events = EVENTS.AP_MASTER;
            }
            else if ("AT_SW" == msg)
            {
                event_data.events = EVENTS.AUTO_THROTTLE_ARM;
            }
            else if ("ROT1_CW" == msg)
            {
                event_data.events = EVENTS.AP_VS_VAR_INC;
            }
            else if ("ROT1_CCW" == msg)
            {
                event_data.events = EVENTS.AP_VS_VAR_DEC;
            }
            else if ("ROT1_OFF" == msg)
            {
                event_data.events = EVENTS.AP_VS_SLOT_INDEX_SET;
                event_data.value = 2;
            }
            else if ("ROT1_A" == msg)
            {
                // do nothing
            }
            else if ("ROT1_B" == msg)
            {
                event_data.events = EVENTS.AP_VS_SLOT_INDEX_SET;
                event_data.value = 1;
            }
            else if ("ROT1_AB" == msg)
            {
                event_data.events = EVENTS.EVENTS_UNKNOWN;
            }
            else if ("ROT2_CW" == msg)
            {
                event_data.events = EVENTS.AP_ALT_VAR_SET_ENGLISH;
                UInt32 temp = (UInt32)(si_AUTOPILOT_ALTITUDE_LOCK_VAR + ci_ap_step_alt_ft);
                event_data.value = temp;
                si_AUTOPILOT_ALTITUDE_LOCK_VAR = temp;
            }
            else if ("ROT2_CCW" == msg)
            {
                event_data.events = EVENTS.AP_ALT_VAR_SET_ENGLISH;
                UInt32 temp = (UInt32)((ci_ap_step_alt_ft< si_AUTOPILOT_ALTITUDE_LOCK_VAR)?(si_AUTOPILOT_ALTITUDE_LOCK_VAR - ci_ap_step_alt_ft):0);
                event_data.value = temp;
                si_AUTOPILOT_ALTITUDE_LOCK_VAR = temp;

            }
            else if ("ROT2_OFF" == msg)
            {
                event_data.events = EVENTS.ALTITUDE_SLOT_INDEX_SET;
                event_data.value = 2;
            }
            else if ("ROT2_A" == msg)
            {
                // do nothing
            }
            else if ("ROT2_B" == msg)
            {
                event_data.events = EVENTS.ALTITUDE_SLOT_INDEX_SET;
                event_data.value = 1;
            }
            else if ("ROT2_AB" == msg)
            {
                event_data.events = EVENTS.EVENTS_UNKNOWN;
            }
            else if ("ROT3_CW" == msg)
            {
                event_data.events = EVENTS.HEADING_BUG_SET;
                UInt32 temp = (UInt32)(si_AUTOPILOT_HEADING_LOCK_DIR + ci_ap_step_heading_deg);
                event_data.value = temp;
                si_AUTOPILOT_HEADING_LOCK_DIR = temp;
            }
            else if ("ROT3_CCW" == msg)
            {
                event_data.events = EVENTS.HEADING_BUG_SET;
                UInt32 temp = (UInt32)((ci_ap_step_heading_deg < si_AUTOPILOT_HEADING_LOCK_DIR) ? (si_AUTOPILOT_HEADING_LOCK_DIR - ci_ap_step_heading_deg) : 0);
                event_data.value = temp;
                si_AUTOPILOT_HEADING_LOCK_DIR = temp;
            }
            else if ("ROT3_OFF" == msg)
            {
                event_data.events = EVENTS.HEADING_SLOT_INDEX_SET;
                event_data.value = 2;
            }
            else if ("ROT3_A" == msg)
            {
                // do nothing
            }
            else if ("ROT3_B" == msg)
            {
                event_data.events = EVENTS.HEADING_SLOT_INDEX_SET;
                event_data.value = 1;
            }
            else if ("ROT3_AB" == msg)
            {
                event_data.events = EVENTS.EVENTS_UNKNOWN;
            }
            else if ("ROT4_CW" == msg)
            {
                event_data.events = EVENTS.AP_SPD_VAR_INC;
            }
            else if ("ROT4_CCW" == msg)
            {
                event_data.events = EVENTS.AP_SPD_VAR_DEC;
            }
            else if ("ROT4_OFF" == msg)
            {
                event_data.events = EVENTS.SPEED_SLOT_INDEX_SET;
                event_data.value = 2;
            }
            else if ("ROT4_A" == msg)
            {
                // do nothing
            }
            else if ("ROT4_B" == msg)
            {
                event_data.events = EVENTS.SPEED_SLOT_INDEX_SET;
                event_data.value = 1;
            }
            else if ("ROT4_AB" == msg)
            {
                event_data.events = EVENTS.EVENTS_UNKNOWN;
            }
            return event_data;
        }

        public class VariableData
        {
            public Val simVar { get; set; }
            public String value { get; set; }

            public VariableData()
            {
                simVar = null;
                value = String.Empty;
            }

            public String ToCommand()
            {
                return (null != simVar) ? (simVar.Name + ":" + value) : String.Empty;
            }
        }

        public static VariableData ToVariables(String simVar, String value)
        {
            VariableData variable_data = new VariableData();
            if (simVar == VARIABLES.AUTOPILOT_VS_SLOT_INDEX.Name)
            {
                variable_data.simVar = VARIABLES.AUTOPILOT_VS_SLOT_INDEX;
                variable_data.value = value;
            }
            else if (simVar == VARIABLES.AUTOPILOT_ALTITUDE_SLOT_INDEX.Name)
            {
                variable_data.simVar = VARIABLES.AUTOPILOT_ALTITUDE_SLOT_INDEX;
                variable_data.value = value;
            }
            else if (simVar == VARIABLES.AUTOPILOT_HEADING_SLOT_INDEX.Name)
            {
                variable_data.simVar = VARIABLES.AUTOPILOT_HEADING_SLOT_INDEX;
                variable_data.value = value;
            }
            else if (simVar == VARIABLES.AUTOPILOT_SPEED_SLOT_INDEX.Name)
            {
                variable_data.simVar = VARIABLES.AUTOPILOT_SPEED_SLOT_INDEX;
                variable_data.value = value;
            }
            else if (simVar == VARIABLES.AUTOPILOT_ALTITUDE_LOCK_VAR.Name)
            {
                double temp = double.Parse(value);
                si_AUTOPILOT_ALTITUDE_LOCK_VAR = (UInt32)temp;
            }
            else if (simVar == VARIABLES.AUTOPILOT_HEADING_LOCK_DIR.Name)
            {
                double temp = double.Parse(value);
                si_AUTOPILOT_HEADING_LOCK_DIR = (UInt32)temp;
            }
            else if (simVar == VARIABLES.AUTOPILOT_MASTER.Name)
            {
                variable_data.simVar = VARIABLES.AUTOPILOT_MASTER;
                variable_data.value = value;
            }
            else if (simVar == VARIABLES.AUTOPILOT_THROTTLE_ARM.Name)
            {
                variable_data.simVar = VARIABLES.AUTOPILOT_THROTTLE_ARM;
                variable_data.value = value;
            }
            return variable_data;
        }

    }
}
