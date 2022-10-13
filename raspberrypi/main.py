
import RPi.GPIO as GPIO
import time
from myserial import MySerial
from switch import RotaryEncoderaWithPushSwitchLED
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
            Config.pin_id_led_1_b
        )
        self.serial = serial
        self.is_silent = is_silent

    def event_rot_sw_cw(self):
        if not self.is_silent: print("cw")
        self.serial.send_str(Config.text_rot_1_cw)
        pass

    def event_rot_sw_ccw(self):
        if not self.is_silent: print("ccw")
        self.serial.send_str(Config.text_rot_1_ccw)
        pass

    def event_rot_sw_pushed(self):
        # sample
        e_state = super().get_state()
        if RotaryEncoderaWithPushSwitchLED.State.OFF == e_state:
            self.serial.send_str(Config.text_rot_1_state_off)
            if not self.is_silent: print(Config.text_rot_1_state_off)
            # super().set_state(RotaryEncoderaWithPushSwitchLED.State.ON_A)
        elif RotaryEncoderaWithPushSwitchLED.State.ON_A == e_state:
            self.serial.send_str(Config.text_rot_1_state_a)
            if not self.is_silent: print(Config.text_rot_1_state_a)
            # super().set_state(RotaryEncoderaWithPushSwitchLED.State.ON_B)
        elif RotaryEncoderaWithPushSwitchLED.State.ON_B == e_state:
            self.serial.send_str(Config.text_rot_1_state_b)
            if not self.is_silent: print(Config.text_rot_1_state_b)
            # super().set_state(RotaryEncoderaWithPushSwitchLED.State.ON_AB)
        elif RotaryEncoderaWithPushSwitchLED.State.ON_AB == e_state:
            self.serial.send_str(Config.text_rot_1_state_ab)
            if not self.is_silent: print(Config.text_rot_1_state_ab)
            # super().set_state(RotaryEncoderaWithPushSwitchLED.State.OFF)
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
    sw1 = RotaryEncoderaWithPushSwitchLED_1(serial, is_silent=False)

    # test
    sw1.set_state(RotaryEncoderaWithPushSwitchLED.State.ON_AB)
    time.sleep(1)
    sw1.set_state(RotaryEncoderaWithPushSwitchLED.State.ON_A)


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
