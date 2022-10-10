using Microsoft.FlightSimulator.SimConnect;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace MSFS_Con
{
    internal class SimVars
    {
        private static String[,] vals =
        {
            // arg1 : Variable name, arg2 : Unit name
            // If arg2 is null to be string value.
            { "TITLE", null },
            { "GEAR HANDLE POSITION", "Bool" },
            //{ "AUTOPILOT AIRSPEED HOLD VAR",  "Knots" }, // It can't set a value.
            //{ "AUTOPILOT ALTITUDE LOCK VAR", "Feet" },
            //{ "AUTOPILOT HEADING LOCK DIR", "Degrees" },
            { "PLANE ALTITUDE", "Feet" },
            { "PLANE HEADING DEGREES TRUE", "Degrees" },
            { "PLANE LATITUDE", "Radians" },
            { "PLANE LONGITUDE", "Radians" },
            { "PLANE BANK DEGREES", "Radians" },
            { "PLANE PITCH DEGREES", "Radians" },
            { "AILERON POSITION", "Position" },
            { "ELEVATOR POSITION", "Position" },
            { "FLAP POSITION SET", "Position" },
            { "RUDDER POSITION", "Position" },
            { "GENERAL ENG THROTTLE LEVER POSITION:1",  "Percent" },
            { "GENERAL ENG THROTTLE LEVER POSITION:2", "Percent" },


        };

        public UInt32 Definition { get; set; }
        public UInt32 Request { get; set; }
        public String SimVar { get; set; }
        public String Units { get; set; }
        public SIMCONNECT_DATATYPE SIMCONNECT_DATATYPE { get; set; }

        public static List<SimVars> getVariables()
        {
            UInt32 uDef = 0;
            UInt32 uReq = 0;

            List<SimVars> list = new List<SimVars>();
            for (int i = 0; i < vals.GetLength(0); i++)
            {
                SimVars v = new SimVars();
                v.Definition = uDef++;
                v.Request = uReq++;
                v.SimVar = vals[i, 0];
                v.Units = vals[i, 1];
                if (v.Units == null)
                {
                    v.SIMCONNECT_DATATYPE = SIMCONNECT_DATATYPE.STRING256;
                }
                else
                {
                    v.SIMCONNECT_DATATYPE = SIMCONNECT_DATATYPE.FLOAT64;
                }
                list.Add(v);
            }
            return list;
        }
    }
}
