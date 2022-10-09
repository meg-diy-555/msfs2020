
import RPi.GPIO as GPIO
import time
from switch import RotaryEncoderaWithPushSwitchLED
from config import Config


class RotaryEncoderaWithPushSwitchLED_1 (RotaryEncoderaWithPushSwitchLED):
    def __init__(self):
        super().__init__(
            Config.pin_id_rot_a,
            Config.pin_id_rot_b,
            Config.pin_id_rot_sw,
            Config.pin_id_led_1,
            Config.pin_id_led_2
        )

    def event_rot_sw_cw(self):
        print("cw")
        pass

    def event_rot_sw_ccw(self):
        print("ccw")
        pass

    def event_rot_sw_pushed(self):
        # sample
        print('rot sw pushed.')
        e_state = super().get_state()
        if RotaryEncoderaWithPushSwitchLED.State.OFF == e_state:
            super().set_state(RotaryEncoderaWithPushSwitchLED.State.ON_A)
        elif RotaryEncoderaWithPushSwitchLED.State.ON_A == e_state:
            super().set_state(RotaryEncoderaWithPushSwitchLED.State.ON_B)
        elif RotaryEncoderaWithPushSwitchLED.State.ON_B == e_state:
            super().set_state(RotaryEncoderaWithPushSwitchLED.State.ON_AB)
        elif RotaryEncoderaWithPushSwitchLED.State.ON_AB == e_state:
            super().set_state(RotaryEncoderaWithPushSwitchLED.State.OFF)
        pass


def main():
    sw1 = RotaryEncoderaWithPushSwitchLED_1()

    try:
        while True:
            time.sleep(1)
    except KeyboardInterrupt:
        print('exit')


if __name__ == "__main__":
    main()
