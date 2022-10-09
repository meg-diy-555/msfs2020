
import RPi.GPIO as GPIO
import time
from myserial import MySerial
from switch import RotaryEncoderaWithPushSwitchLED
from config import Config


class RotaryEncoderaWithPushSwitchLED_1 (RotaryEncoderaWithPushSwitchLED):
    def __init__(self, serial: MySerial):
        super().__init__(
            Config.pin_id_rot_a,
            Config.pin_id_rot_b,
            Config.pin_id_rot_sw,
            Config.pin_id_led_1,
            Config.pin_id_led_2
        )
        self.serial = serial

    def event_rot_sw_cw(self):
        print("cw")
        self.serial.send("cw")
        pass

    def event_rot_sw_ccw(self):
        print("ccw")
        self.serial.send("ccw")
        pass

    def event_rot_sw_pushed(self):
        # sample
        print('rot sw pushed.')
        e_state = super().get_state()
        if RotaryEncoderaWithPushSwitchLED.State.OFF == e_state:
            super().set_state(RotaryEncoderaWithPushSwitchLED.State.ON_A)
            self.serial.send("Autopilot Master ON Managed")
        elif RotaryEncoderaWithPushSwitchLED.State.ON_A == e_state:
            super().set_state(RotaryEncoderaWithPushSwitchLED.State.ON_B)
            self.serial.send("Autopilot Master ON Directed")
        elif RotaryEncoderaWithPushSwitchLED.State.ON_B == e_state:
            super().set_state(RotaryEncoderaWithPushSwitchLED.State.ON_AB)
            self.serial.send("Autopilot Master ON ???")
        elif RotaryEncoderaWithPushSwitchLED.State.ON_AB == e_state:
            super().set_state(RotaryEncoderaWithPushSwitchLED.State.OFF)
            self.serial.send("Autopilot Master OFF")
        pass


def main():
    serial = MySerial().start()
    sw1 = RotaryEncoderaWithPushSwitchLED_1(serial)

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
