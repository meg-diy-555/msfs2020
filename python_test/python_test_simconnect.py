from SimConnect import *

def my_event_trigger(ae: AircraftEvents, event_name: str) -> None:
    event_to_trigger = ae.find(event_name)
    if event_to_trigger is not None:
        event_to_trigger()


def my_event_trigger_with_arg1(ae: AircraftEvents, event_name: str, val: int) -> None:
    event_to_trigger = ae.find(event_name)
    if event_to_trigger is not None:
        event_to_trigger(val)


sm = SimConnect()
aq = AircraftRequests(sm, _time=2000)
ae = AircraftEvents(sm)

while(True):
    print('input " " or q,a,w,s,e,d,r,f,h,1,2,3,4,5,6,7,8,9 to control AP or 0 to exit')
    input_str = input()

    if ' ' == input_str: # engage autopilot
        my_event_trigger(ae, "AP_MASTER")
        my_event_trigger(ae, "AUTO_THROTTLE_ARM")

    # increase or decrease autopilot reference speed
    if 'q' == input_str:
        my_event_trigger(ae, "AP_SPD_VAR_INC")

    if 'a' == input_str:
        my_event_trigger(ae, "AP_SPD_VAR_DEC")

    if 'Z' == input_str: # not work
        my_event_trigger_with_arg1(ae, "AP_RPM_SLOT_INDEX_SET", 2)
    if 'z' == input_str: # not work
        my_event_trigger_with_arg1(ae, "AP_RPM_SLOT_INDEX_SET", 1)

    # increase or decrease autopilot reference heading (direction)
    if 'w' == input_str:
        my_event_trigger(ae, "HEADING_BUG_DEC")

    if 's' == input_str:
        my_event_trigger(ae, "HEADING_BUG_INC")

    if 'X' == input_str: # not work
        my_event_trigger_with_arg1(ae, "HEADING_SLOT_INDEX_SET",2)

    if 'x' == input_str: # not work
        my_event_trigger_with_arg1(ae, "HEADING_SLOT_INDEX_SET",1)

    # increase or decrease autopilot reference altitude
    if 'e' == input_str:
        my_event_trigger(ae, "AP_ALT_VAR_INC")

    if 'd' == input_str:
        my_event_trigger(ae, "AP_ALT_VAR_DEC")

    if 'C' == input_str: # not work
        my_event_trigger_with_arg1(ae, "ALTITUDE_SLOT_INDEX_SET",2)
    if 'c' == input_str: # not work
        my_event_trigger_with_arg1(ae, "ALTITUDE_SLOT_INDEX_SET",1)

    # increase or decrease autopilot reference vertical altitude
    if 'r' == input_str:
        my_event_trigger(ae, "AP_VS_VAR_INC")

    if 'f' == input_str:
        my_event_trigger(ae, "AP_VS_VAR_DEC")

    if 'V' == input_str: # not work
        my_event_trigger_with_arg1(ae, "AP_VS_SLOT_INDEX_SET",2)
    if 'v' == input_str: # not work
        my_event_trigger_with_arg1(ae, "AP_VS_SLOT_INDEX_SET",1)

    if 'h' == input_str:
        my_event_trigger(ae, "FLIGHT_LEVEL_CHANGE")

    # set autopilot reference altitude
    if '1' == input_str:
        my_event_trigger_with_arg1(ae, "AP_ALT_VAR_SET_ENGLISH", 1000)
    if '2' == input_str:
        my_event_trigger_with_arg1(ae, "AP_ALT_VAR_SET_ENGLISH", 2000)
    if '3' == input_str:
        my_event_trigger_with_arg1(ae, "AP_ALT_VAR_SET_ENGLISH", 3000)
    if '4' == input_str:
        my_event_trigger_with_arg1(ae, "AP_ALT_VAR_SET_ENGLISH", 4000)
    if '5' == input_str:
        my_event_trigger_with_arg1(ae, "AP_ALT_VAR_SET_ENGLISH", 5000)
    if '6' == input_str:
        my_event_trigger_with_arg1(ae, "AP_ALT_VAR_SET_ENGLISH", 6000)
    if '7' == input_str:
        my_event_trigger_with_arg1(ae, "AP_ALT_VAR_SET_ENGLISH", 7000)
    if '8' == input_str:
        my_event_trigger_with_arg1(ae, "AP_ALT_VAR_SET_ENGLISH", 8000)
    if '9' == input_str:
        my_event_trigger_with_arg1(ae, "AP_ALT_VAR_SET_ENGLISH", 9000)

    if 'u' == input_str:  # not work?
        my_event_trigger(ae, "AP_AVIONICS_MANAGED_ON")

    # exit
    if '0' == input_str:
        break

sm.exit()
quit()
