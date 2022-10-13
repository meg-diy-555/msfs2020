using Microsoft.FlightSimulator.SimConnect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static MSFS_Con.SimConnectProvider;

namespace MSFS_Con
{

    public class Val
    {
        public String Name { get; set; }
        public String Units { get; set; }
        public SIMCONNECT_PERIOD SIMCONNECT_PERIOD { get; set; } = SIMCONNECT_PERIOD.SECOND;
        public SIMCONNECT_DATA_REQUEST_FLAG SIMCONNECT_DATA_REQUEST_FLAG { get; set; } = SIMCONNECT_DATA_REQUEST_FLAG.DEFAULT;
    }

    public class VARIABLES
    {
        public static Val TITLE { get; } = new Val() { Name = "TITLE", Units = null };
        public static Val GEAR_HANDLE_POSITION { get; } = new Val() { Name = "GEAR HANDLE POSITION", Units = "Bool", SIMCONNECT_PERIOD = SIMCONNECT_PERIOD.SIM_FRAME, SIMCONNECT_DATA_REQUEST_FLAG = SIMCONNECT_DATA_REQUEST_FLAG.CHANGED };
        public static Val PLANE_ALTITUDE { get; } = new Val() { Name = "PLANE ALTITUDE", Units = "Feet" };
        public static Val PLANE_HEADING_DEGREES_TRUE { get; } = new Val() { Name = "PLANE HEADING DEGREES TRUE", Units = "Degrees" };
        public static Val PLANE_LATITUDE { get; } = new Val() { Name = "PLANE LATITUDE", Units = "Radians" };
        public static Val PLANE_LONGITUDE { get; } = new Val() { Name = "PLANE LONGITUDE", Units = "Radians" };
        public static Val PLANE_BANK_DEGREES { get; } = new Val() { Name = "PLANE BANK DEGREES", Units = "Radians" };
        public static Val PLANE_PITCH_DEGREES { get; } = new Val() { Name = "PLANE PITCH DEGREES", Units = "Radians" };
        public static Val AILERON_POSITION { get; } = new Val() { Name = "AILERON POSITION", Units = "Position" };
        public static Val FLAP_POSITION_SET { get; } = new Val() { Name = "FLAP POSITION SET", Units = "Position" };
        public static Val RUDDER_POSITION { get; } = new Val() { Name = "RUDDER POSITION", Units = "Position" };
        public static Val GENERAL_ENG_THROTTLE_LEVER_POSITION_1 { get; } = new Val() { Name = "GENERAL ENG THROTTLE LEVER POSITION:1", Units = "Percent" };
        public static Val GENERAL_ENG_THROTTLE_LEVER_POSITION_2 { get; } = new Val() { Name = "GENERAL ENG THROTTLE LEVER POSITION:2", Units = "Percent" };
        public static Val AUTOPILOT_ALTITUDE_SLOT_INDEX { get;} = new Val() { Name = "AUTOPILOT ALTITUDE SLOT INDEX", Units = "Number"};
        public static Val AUTOPILOT_HEADING_SLOT_INDEX { get; } = new Val() { Name = "AUTOPILOT HEADING SLOT INDEX", Units = "Number" };
        public static Val AUTOPILOT_SPEED_SLOT_INDEX { get; } = new Val() { Name = "AUTOPILOT SPEED SLOT INDEX", Units = "Number" };
        public static Val AUTOPILOT_VS_SLOT_INDEX { get; } = new Val() { Name = "AUTOPILOT VS SLOT INDEX", Units = "Number" };


    }
}
