
import RPi.GPIO as GPIO
import time
from myserial import MySerial
from switch import RotaryEncoderaWithPushSwitchLED
from config import Config


class RotaryEncoderaWithPushSwitchLED_1 (RotaryEncoderaWithPushSwitchLED):
    def __init__(self, serial: MySerial, is_silent=True):
        super().__init__(
            Config.pin_id_rot_1_a,
            Config.pin_id_rot_1_b,
            Config.pin_id_rot_1_sw,
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
            super().set_state(RotaryEncoderaWithPushSwitchLED.State.ON_A)
            self.serial.send_str(Config.text_rot_1_state_a)
            if not self.is_silent: print(Config.text_rot_1_state_a)
        elif RotaryEncoderaWithPushSwitchLED.State.ON_A == e_state:
            super().set_state(RotaryEncoderaWithPushSwitchLED.State.ON_B)
            self.serial.send_str(Config.text_rot_1_state_b)
            if not self.is_silent: print(Config.text_rot_1_state_b)
        elif RotaryEncoderaWithPushSwitchLED.State.ON_B == e_state:
            super().set_state(RotaryEncoderaWithPushSwitchLED.State.ON_AB)
            self.serial.send_str(Config.text_rot_1_state_ab)
            if not self.is_silent: print(Config.text_rot_1_state_ab)
        elif RotaryEncoderaWithPushSwitchLED.State.ON_AB == e_state:
            super().set_state(RotaryEncoderaWithPushSwitchLED.State.OFF)
            self.serial.send_str(Config.text_rot_1_state_off)
            if not self.is_silent: print(Config.text_rot_1_state_off)
        pass


def main():
    serial = MySerial().start()
    sw1 = RotaryEncoderaWithPushSwitchLED_1(serial, is_silent=False)

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
