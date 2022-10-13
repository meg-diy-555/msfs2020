using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSFS_Con
{
    public enum EVENTS
    {
        // If you want, add the values,
        ALTITUDE_SLOT_INDEX_SET,
        HEADING_SLOT_INDEX_SET,
        SPEED_SLOT_INDEX_SET,
        VS_SLOT_INDEX_SET,
        FLAPS_INCR,
        FLAPS_DECR,
        AP_MASTER,
        AP_SPD_VAR_SET,
        AP_ALT_VAR_SET_ENGLISH,
        HEADING_BUG_SET,
        AP_HDG_HOLD,
        AP_ALT_HOLD,
        AP_APR_HOLD,
        AP_VS_HOLD,
        AP_VS_VAR_INC,
        AP_VS_VAR_DEC,
        FLIGHT_LEVEL_CHANGE,
        AP_SPD_VAR_INC,
        AP_SPD_VAR_DEC,
        EVENTS_UNKNOWN
    };
}
