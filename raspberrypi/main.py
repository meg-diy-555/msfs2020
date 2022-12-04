
import RPi.GPIO as GPIO
import time
from myserial import MySerial
from switch import RotaryEncoderaWithPushSwitchLED, PushSwitch, PushSwitchWithLED
from config import Config


sw1 = None
sw2 = None
sw3 = None
sw4 = None


class RotaryEncoderaWithPushSwitchLED_1 (RotaryEncoderaWithPushSwitchLED):
    def __init__(self, serial: MySerial, is_silent=True):
        super().__init__(
            Config.pin_id_rot_1_a,
            Config.pin_id_rot_1_b,
            Config.pin_id_rot_1_push_sw,
            Config.pin_id_led_1_a,
            Config.pin_id_led_1_b,
            self.event_rot_sw_cw,
            self.event_rot_sw_ccw,
            self.event_rot_sw_pushed
        )
        self.serial = serial
        self.is_silent = is_silent

    def event_rot_sw_cw(self):
        if not self.is_silent: print(Config.text_rot_1_cw)
        self.serial.send_str(Config.text_rot_1_cw)
        pass

    def event_rot_sw_ccw(self):
        if not self.is_silent: print(Config.text_rot_1_ccw)
        self.serial.send_str(Config.text_rot_1_ccw)
        pass

    def event_rot_sw_pushed(self):
        e_state = super().get_state()
        if RotaryEncoderaWithPushSwitchLED.State.OFF == e_state:
            self.serial.send_str(Config.text_rot_1_state_off)
            if not self.is_silent: print(Config.text_rot_1_state_off)
        elif RotaryEncoderaWithPushSwitchLED.State.ON_A == e_state:
            self.serial.send_str(Config.text_rot_1_state_a)
            if not self.is_silent: print(Config.text_rot_1_state_a)
        elif RotaryEncoderaWithPushSwitchLED.State.ON_B == e_state:
            self.serial.send_str(Config.text_rot_1_state_b)
            if not self.is_silent: print(Config.text_rot_1_state_b)
        elif RotaryEncoderaWithPushSwitchLED.State.ON_AB == e_state:
            self.serial.send_str(Config.text_rot_1_state_ab)
            if not self.is_silent: print(Config.text_rot_1_state_ab)
        pass

class RotaryEncoderaWithPushSwitchLED_2 (RotaryEncoderaWithPushSwitchLED):
    def __init__(self, serial: MySerial, is_silent=True):
        super().__init__(
            Config.pin_id_rot_2_a,
            Config.pin_id_rot_2_b,
            Config.pin_id_rot_2_push_sw,
            Config.pin_id_led_2_a,
            Config.pin_id_led_2_b,
            self.event_rot_sw_cw,
            self.event_rot_sw_ccw,
            self.event_rot_sw_pushed
        )
        self.serial = serial
        self.is_silent = is_silent

    def event_rot_sw_cw(self):
        if not self.is_silent: print(Config.text_rot_2_cw)
        self.serial.send_str(Config.text_rot_2_cw)
        pass

    def event_rot_sw_ccw(self):
        if not self.is_silent: print(Config.text_rot_2_ccw)
        self.serial.send_str(Config.text_rot_2_ccw)
        pass

    def event_rot_sw_pushed(self):
        e_state = super().get_state()
        if RotaryEncoderaWithPushSwitchLED.State.OFF == e_state:
            self.serial.send_str(Config.text_rot_2_state_off)
            if not self.is_silent: print(Config.text_rot_2_state_off)
        elif RotaryEncoderaWithPushSwitchLED.State.ON_A == e_state:
            self.serial.send_str(Config.text_rot_2_state_a)
            if not self.is_silent: print(Config.text_rot_2_state_a)
        elif RotaryEncoderaWithPushSwitchLED.State.ON_B == e_state:
            self.serial.send_str(Config.text_rot_2_state_b)
            if not self.is_silent: print(Config.text_rot_2_state_b)
        elif RotaryEncoderaWithPushSwitchLED.State.ON_AB == e_state:
            self.serial.send_str(Config.text_rot_2_state_ab)
            if not self.is_silent: print(Config.text_rot_2_state_ab)
        pass


class RotaryEncoderaWithPushSwitchLED_3 (RotaryEncoderaWithPushSwitchLED):
    def __init__(self, serial: MySerial, is_silent=True):
        super().__init__(
            Config.pin_id_rot_3_a,
            Config.pin_id_rot_3_b,
            Config.pin_id_rot_3_push_sw,
            Config.pin_id_led_3_a,
            Config.pin_id_led_3_b,
            self.event_rot_sw_cw,
            self.event_rot_sw_ccw,
            self.event_rot_sw_pushed
        )
        self.serial = serial
        self.is_silent = is_silent

    def event_rot_sw_cw(self):
        if not self.is_silent: print(Config.text_rot_3_cw)
        self.serial.send_str(Config.text_rot_3_cw)
        pass

    def event_rot_sw_ccw(self):
        if not self.is_silent: print(Config.text_rot_3_ccw)
        self.serial.send_str(Config.text_rot_3_ccw)
        pass

    def event_rot_sw_pushed(self):
        e_state = super().get_state()
        if RotaryEncoderaWithPushSwitchLED.State.OFF == e_state:
            self.serial.send_str(Config.text_rot_3_state_off)
            if not self.is_silent: print(Config.text_rot_3_state_off)
        elif RotaryEncoderaWithPushSwitchLED.State.ON_A == e_state:
            self.serial.send_str(Config.text_rot_3_state_a)
            if not self.is_silent: print(Config.text_rot_3_state_a)
        elif RotaryEncoderaWithPushSwitchLED.State.ON_B == e_state:
            self.serial.send_str(Config.text_rot_3_state_b)
            if not self.is_silent: print(Config.text_rot_3_state_b)
        elif RotaryEncoderaWithPushSwitchLED.State.ON_AB == e_state:
            self.serial.send_str(Config.text_rot_3_state_ab)
            if not self.is_silent: print(Config.text_rot_3_state_ab)
        pass


class RotaryEncoderaWithPushSwitchLED_4 (RotaryEncoderaWithPushSwitchLED):
    def __init__(self, serial: MySerial, is_silent=True):
        super().__init__(
            Config.pin_id_rot_4_a,
            Config.pin_id_rot_4_b,
            Config.pin_id_rot_4_push_sw,
            Config.pin_id_led_4_a,
            Config.pin_id_led_4_b,
            self.event_rot_sw_cw,
            self.event_rot_sw_ccw,
            self.event_rot_sw_pushed
        )
        self.serial = serial
        self.is_silent = is_silent

    def event_rot_sw_cw(self):
        if not self.is_silent: print(Config.text_rot_4_cw)
        self.serial.send_str(Config.text_rot_4_cw)
        pass

    def event_rot_sw_ccw(self):
        if not self.is_silent: print(Config.text_rot_4_ccw)
        self.serial.send_str(Config.text_rot_4_ccw)
        pass

    def event_rot_sw_pushed(self):
        e_state = super().get_state()
        if RotaryEncoderaWithPushSwitchLED.State.OFF == e_state:
            self.serial.send_str(Config.text_rot_4_state_off)
            if not self.is_silent: print(Config.text_rot_4_state_off)
        elif RotaryEncoderaWithPushSwitchLED.State.ON_A == e_state:
            self.serial.send_str(Config.text_rot_4_state_a)
            if not self.is_silent: print(Config.text_rot_4_state_a)
        elif RotaryEncoderaWithPushSwitchLED.State.ON_B == e_state:
            self.serial.send_str(Config.text_rot_4_state_b)
            if not self.is_silent: print(Config.text_rot_4_state_b)
        elif RotaryEncoderaWithPushSwitchLED.State.ON_AB == e_state:
            self.serial.send_str(Config.text_rot_4_state_ab)
            if not self.is_silent: print(Config.text_rot_4_state_ab)
        pass



class PushSwitch_1 (PushSwitch):
    def __init__(self, serial: MySerial, is_silent=True):
        super().__init__(Config.pin_id_rot_1_pull_sw, self.event_sw_pushed, bouncetime=100)
        self.serial = serial
        self.is_silent = is_silent

    def event_sw_pushed(self):
        if not self.is_silent: print("ROT1_PULL_SW")
        self.serial.send_str(Config.text_rot_1_state_off)
        pass

class PushSwitch_2 (PushSwitch):
    def __init__(self, serial: MySerial, is_silent=True):
        super().__init__(Config.pin_id_rot_2_pull_sw, self.event_sw_pushed, bouncetime=100)
        self.serial = serial
        self.is_silent = is_silent

    def event_sw_pushed(self):
        if not self.is_silent: print("ROT2_PULL_SW")
        self.serial.send_str(Config.text_rot_2_state_off)
        pass

class PushSwitch_3 (PushSwitch):
    def __init__(self, serial: MySerial, is_silent=True):
        super().__init__(Config.pin_id_rot_3_pull_sw, self.event_sw_pushed, bouncetime=100)
        self.serial = serial
        self.is_silent = is_silent

    def event_sw_pushed(self):
        if not self.is_silent: print("ROT3_PULL_SW")
        self.serial.send_str(Config.text_rot_1_state_off)
        pass

class PushSwitch_4 (PushSwitch):
    def __init__(self, serial: MySerial, is_silent=True):
        super().__init__(Config.pin_id_rot_4_pull_sw, self.event_sw_pushed, bouncetime=100)
        self.serial = serial
        self.is_silent = is_silent

    def event_sw_pushed(self):
        if not self.is_silent: print("ROT4_PULL_SW")
        self.serial.send_str(Config.text_rot_1_state_off)
        pass

class PushSwitch_AP (PushSwitchWithLED):
    def __init__(self, serial: MySerial, is_silent=True):
        super().__init__(Config.pin_id_ap_sw, Config.pin_id_ap_led, self.event_sw_pushed, bouncetime=100)
        self.serial = serial
        self.is_silent = is_silent

    def event_sw_pushed(self):
        if not self.is_silent: print(Config.text_ap_sw)
        self.serial.send_str(Config.text_ap_sw)
        pass

class PushSwitch_AT (PushSwitchWithLED):
    def __init__(self, serial: MySerial, is_silent=True):
        super().__init__(Config.pin_id_at_sw, Config.pin_id_at_led, self.event_sw_pushed, bouncetime=100)
        self.serial = serial
        self.is_silent = is_silent

    def event_sw_pushed(self):
        if not self.is_silent: print(Config.text_at_sw)
        self.serial.send_str(Config.text_at_sw)
        pass

def serial_received(msg : str):
    global sw1
    print(msg)
    if(msg == "AUTOPILOT SPEED SLOT INDEX:1\r\n"):
        sw1.set_state(RotaryEncoderaWithPushSwitchLED.State.ON_B)
    elif(msg == "AUTOPILOT SPEED SLOT INDEX:2\r\n"):
        sw1.set_state(RotaryEncoderaWithPushSwitchLED.State.ON_A)



def main():
    global sw1
    serial = MySerial(serial_received).start()
    rot_sw1 = RotaryEncoderaWithPushSwitchLED_1(serial, is_silent=False)
    pull_sw1 = PushSwitch_1(serial, is_silent=False)
    rot_sw2 = RotaryEncoderaWithPushSwitchLED_2(serial, is_silent=False)
    pull_sw2 = PushSwitch_2(serial, is_silent=False)
    rot_sw3 = RotaryEncoderaWithPushSwitchLED_3(serial, is_silent=False)
    pull_sw3 = PushSwitch_3(serial, is_silent=False)
    rot_sw4 = RotaryEncoderaWithPushSwitchLED_4(serial, is_silent=False)
    pull_sw4 = PushSwitch_4(serial, is_silent=False)

    ap_sw = PushSwitch_AP(serial, is_silent=False)
    at_sw = PushSwitch_AT(serial, is_silent=False)

    # test
    rot_sw1.set_state(RotaryEncoderaWithPushSwitchLED.State.ON_A)
    rot_sw2.set_state(RotaryEncoderaWithPushSwitchLED.State.ON_A)
    rot_sw3.set_state(RotaryEncoderaWithPushSwitchLED.State.ON_A)
    rot_sw4.set_state(RotaryEncoderaWithPushSwitchLED.State.ON_A)
    time.sleep(1)
    rot_sw1.set_state(RotaryEncoderaWithPushSwitchLED.State.ON_B)
    rot_sw2.set_state(RotaryEncoderaWithPushSwitchLED.State.ON_B)
    rot_sw3.set_state(RotaryEncoderaWithPushSwitchLED.State.ON_B)
    rot_sw4.set_state(RotaryEncoderaWithPushSwitchLED.State.ON_B)
    time.sleep(1)
    rot_sw1.set_state(RotaryEncoderaWithPushSwitchLED.State.ON_AB)
    rot_sw2.set_state(RotaryEncoderaWithPushSwitchLED.State.ON_AB)
    rot_sw3.set_state(RotaryEncoderaWithPushSwitchLED.State.ON_AB)
    rot_sw4.set_state(RotaryEncoderaWithPushSwitchLED.State.ON_AB)
    ap_sw.set_state(PushSwitchWithLED.State.ON)
    at_sw.set_state(PushSwitchWithLED.State.ON)
    time.sleep(1)
    # ap_sw.set_state(PushSwitchWithLED.State.OFF)
    # at_sw.set_state(PushSwitchWithLED.State.OFF)


    try:
        print("please input Ctrl-C and wait few seconds to terminalte this program.")
        while True:
            time.sleep(1)
    except KeyboardInterrupt:
        serial.end()
        pass

    print('exit')


if __name__ == "__main__":
    main()
