using Microsoft.FlightSimulator.SimConnect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static MSFS_Con.SimConnectProvider;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

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
        //AIRCRAFT BRAKE / LANDING GEAR VARIABLES
        public static Val GEAR_HANDLE_POSITION { get; } = new Val() { Name = "GEAR HANDLE POSITION", Units = "Percent", SIMCONNECT_PERIOD = SIMCONNECT_PERIOD.SIM_FRAME, SIMCONNECT_DATA_REQUEST_FLAG = SIMCONNECT_DATA_REQUEST_FLAG.CHANGED };
        public static Val BRAKE_LEFT_POSITION { get; } = new Val() { Name = "BRAKE LEFT POSITION", Units = "Position", SIMCONNECT_PERIOD = SIMCONNECT_PERIOD.SIM_FRAME, SIMCONNECT_DATA_REQUEST_FLAG = SIMCONNECT_DATA_REQUEST_FLAG.CHANGED };
        public static Val BRAKE_RIGHT_POSITION { get; } = new Val() { Name = "BRAKE RIGHT POSITION", Units = "Position", SIMCONNECT_PERIOD = SIMCONNECT_PERIOD.SIM_FRAME, SIMCONNECT_DATA_REQUEST_FLAG = SIMCONNECT_DATA_REQUEST_FLAG.CHANGED };

        //AIRCRAFT MISC VARIABLES
        public static Val PLANE_ALTITUDE { get; } = new Val() { Name = "PLANE ALTITUDE", Units = "Feet" };
        public static Val PLANE_HEADING_DEGREES_TRUE { get; } = new Val() { Name = "PLANE HEADING DEGREES TRUE", Units = "Degrees" };
        public static Val PLANE_LATITUDE { get; } = new Val() { Name = "PLANE LATITUDE", Units = "Radians" };
        public static Val PLANE_LONGITUDE { get; } = new Val() { Name = "PLANE LONGITUDE", Units = "Radians" };
        public static Val PLANE_BANK_DEGREES { get; } = new Val() { Name = "PLANE BANK DEGREES", Units = "Radians" };
        public static Val PLANE_PITCH_DEGREES { get; } = new Val() { Name = "PLANE PITCH DEGREES", Units = "Radians" };
        public static Val ROTATION_ACCELERATION_BODY_X { get; } = new Val() { Name = "ROTATION ACCELERATION BODY X", Units = "Radians" };
        public static Val ROTATION_ACCELERATION_BODY_Y { get; } = new Val() { Name = "ROTATION ACCELERATION BODY Y", Units = "Radians" };
        public static Val ROTATION_ACCELERATION_BODY_Z { get; } = new Val() { Name = "ROTATION ACCELERATION BODY Z", Units = "Radians" };
        public static Val VELOCITY_BODY_X { get; } = new Val() { Name = "VELOCITY BODY X", Units = "Feet" };
        public static Val VELOCITY_BODY_Y { get; } = new Val() { Name = "VELOCITY BODY Y", Units = "Feet" };
        public static Val VELOCITY_BODY_Z { get; } = new Val() { Name = "VELOCITY BODY Z", Units = "Feet" };
        
        // AIRCRAFT AUTOPILOT/ASSISTANT VARIABLES
        public static Val AUTOPILOT_ALTITUDE_SLOT_INDEX { get;} = new Val() { Name = "AUTOPILOT ALTITUDE SLOT INDEX", Units = "Number"};
        public static Val AUTOPILOT_HEADING_SLOT_INDEX { get; } = new Val() { Name = "AUTOPILOT HEADING SLOT INDEX", Units = "Number" };
        public static Val AUTOPILOT_SPEED_SLOT_INDEX { get; } = new Val() { Name = "AUTOPILOT SPEED SLOT INDEX", Units = "Number" };
       
        public static Val AUTOPILOT_VS_SLOT_INDEX { get; } = new Val() { Name = "AUTOPILOT VS SLOT INDEX", Units = "Number" };
        public static Val AUTOPILOT_ALTITUDE_LOCK_VAR { get; } = new Val() { Name = "AUTOPILOT ALTITUDE LOCK VAR", Units = "Feet" };
        public static Val AUTOPILOT_BANK_HOLD_REF { get; } = new Val() { Name = "AUTOPILOT BANK HOLD REF", Units = "Degrees" };
        public static Val AUTOPILOT_FLIGHT_LEVEL_CHANGE { get; } = new Val() { Name = "AUTOPILOT FLIGHT LEVEL CHANGE", Units = "Bool" };
        public static Val AUTOPILOT_HEADING_LOCK_DIR { get; } = new Val() { Name = "AUTOPILOT HEADING LOCK DIR", Units = "Degrees" };
        public static Val AUTOPILOT_HEADING_MANUALLY_TUNABLE { get; } = new Val() { Name = "AUTOPILOT HEADING MANUALLY TUNABLE", Units = "Bool" };
        public static Val AUTOPILOT_THROTTLE_MAX_THRUST { get; } = new Val() { Name = "AUTOPILOT THROTTLE MAX THRUST", Units = "Percent" };
        public static Val AUTOPILOT_VERTICAL_HOLD_VAR { get; } = new Val() { Name = "AUTOPILOT VERTICAL HOLD VAR", Units = "Feet" };
        
        // AIRCRAFT CONTROL VARIABLES
        public static Val AILERON_POSITION { get; } = new Val() { Name = "AILERON POSITION", Units = "Position" };
        public static Val AILERON_TRIM_PCT { get; } = new Val() { Name = "AILERON TRIM PCT", Units = "Percent" };
        public static Val ELEVATOR_POSITION { get; } = new Val() { Name = "ELEVATOR POSITION", Units = "Position" };
        public static Val ELEVATOR_TRIM_POSITION { get; } = new Val() { Name = "ELEVATOR TRIM POSITION", Units = "Radians" };
        public static Val FLAP_POSITION_SET { get; } = new Val() { Name = "FLAP POSITION SET", Units = "Position", SIMCONNECT_PERIOD = SIMCONNECT_PERIOD.SIM_FRAME, SIMCONNECT_DATA_REQUEST_FLAG = SIMCONNECT_DATA_REQUEST_FLAG.CHANGED };
        public static Val FLAPS_HANDLE_INDEX { get; } = new Val() { Name = "FLAPS HANDLE INDEX", Units = "Number", SIMCONNECT_PERIOD = SIMCONNECT_PERIOD.SIM_FRAME, SIMCONNECT_DATA_REQUEST_FLAG = SIMCONNECT_DATA_REQUEST_FLAG.CHANGED };
        public static Val RUDDER_POSITION { get; } = new Val() { Name = "RUDDER POSITION", Units = "Position" };
        public static Val RUDDER_TRIM_PCT { get; } = new Val() { Name = "RUDDER TRIM PCT", Units = "Percent" };
        public static Val SPOILERS_HANDLE_POSITION { get; } = new Val() { Name = "SPOILERS HANDLE POSITION", Units = "Position" };

        // AIRCRAFT FUEL VARIABLES
        //public static Val FUEL_TANK_CENTER_QUANTITY { get; } = new Val() { Name = "FUEL TANK CENTER QUANTITY", Units = "Gallons" };

        // AIRCRAFT ENGINE VARIABLES
        public static Val GENERAL_ENG_THROTTLE_LEVER_POSITION_1 { get; } = new Val() { Name = "GENERAL ENG THROTTLE LEVER POSITION:1", Units = "Percent" };
        public static Val GENERAL_ENG_THROTTLE_LEVER_POSITION_2 { get; } = new Val() { Name = "GENERAL ENG THROTTLE LEVER POSITION:2", Units = "Percent" };
        
    }
}
