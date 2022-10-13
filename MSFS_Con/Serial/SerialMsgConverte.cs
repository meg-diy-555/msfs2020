using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSFS_Con.Serial
{
    internal class SerialMsgConverter
    {
        public class EventData
        {
            public EVENTS events { get; set; }
            public uint value { get; set; }


            public EventData()
            {
                events = EVENTS.EVENTS_UNKNOWN;
                value = 0;
            }
        }

        public static EventData ToEvents(string msg)
        {
            EventData event_data = new EventData();

            if ("ROT1_CW" == msg)
            {
                event_data.events = EVENTS.AP_SPD_VAR_INC;
            }
            else if ("ROT1_CCW" == msg)
            {
                event_data.events = EVENTS.AP_SPD_VAR_DEC;
            }
            else if ("ROT1_OFF" == msg)
            {
                // do nothing
            }
            else if ("ROT1_A" == msg)
            {
                event_data.events = EVENTS.SPEED_SLOT_INDEX_SET;
                event_data.value = 1;
            }
            else if ("ROT1_B" == msg)
            {
                event_data.events = EVENTS.SPEED_SLOT_INDEX_SET;
                event_data.value = 2;
            }
            else if ("ROT1_AB" == msg)
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
            if (simVar == "AUTOPILOT SPEED SLOT INDEX")
            {
                variable_data.simVar = VARIABLES.AUTOPILOT_SPEED_SLOT_INDEX;
                variable_data.value = value;
            }
            return variable_data;
        }
    }
}
