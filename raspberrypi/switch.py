import RPi.GPIO as GPIO
from enum import Enum
import time
from gpiozero import LED


class Switch:
    def __init__(self):
        pass


class RotaryEncoder(Switch):
    def __init__(self, pin_id_rot_a: int, pin_id_rot_b: int, event_rot_sw_cw, event_rot_sw_ccw, is_silent=True):
        self.pin_id_rot_a = pin_id_rot_a
        self.pin_id_rot_b = pin_id_rot_b
        self.event_rot_sw_cw = event_rot_sw_cw
        self.event_rot_sw_ccw = event_rot_sw_ccw
        self.is_silent = is_silent
        # initialize GPIO pins
        GPIO.setmode(GPIO.BCM)
        GPIO.setup(pin_id_rot_a, GPIO.IN, pull_up_down=GPIO.PUD_UP)
        GPIO.add_event_detect(pin_id_rot_a, GPIO.FALLING,
                              callback=self._event_callback_rot_sw_falling, bouncetime=100)
        GPIO.setup(pin_id_rot_b, GPIO.IN, pull_up_down=GPIO.PUD_UP)
        GPIO.add_event_detect(pin_id_rot_b, GPIO.FALLING,
                              callback=self._event_callback_rot_sw_falling, bouncetime=100)

    def __delete__(self):
        # release evets and GPIO pins
        pins = [self.pin_id_rot_a, self.pin_id_rot_b]
        GPIO.setmode(GPIO.BCM)
        GPIO.remove_event_detect(pins)
        GPIO.cleanup(pins)

    def _event_callback_rot_sw_falling(self, gpio_pin):
        time.sleep(0.001)  # Chattering prevention
        is_on_sw_a = GPIO.input(self.pin_id_rot_a)
        is_on_sw_b = GPIO.input(self.pin_id_rot_b)
        if (not is_on_sw_a and not is_on_sw_b):
            pass
        elif (self.pin_id_rot_a == gpio_pin and is_on_sw_a) or (self.pin_id_rot_b == gpio_pin and is_on_sw_b):
            pass
        elif (self.pin_id_rot_a == gpio_pin and is_on_sw_b):
            if not self.is_silent:
                print("[1] gpio_pin:", str(gpio_pin), ", is_on_sw_a:",
                      str(is_on_sw_a), ", is_on_sw_b:", str(is_on_sw_b))
            self.event_rot_sw_cw()
        elif (self.pin_id_rot_b == gpio_pin and not is_on_sw_a):
            if not self.is_silent:
                print("[2] gpio_pin:", str(gpio_pin), ", is_on_sw_a:",
                      str(is_on_sw_a), ", is_on_sw_b:", str(is_on_sw_b))
            self.event_rot_sw_cw()
        elif (self.pin_id_rot_a == gpio_pin and not is_on_sw_b):
            if not self.is_silent:
                print("[3] gpio_pin:", str(gpio_pin), ", is_on_sw_a:",
                      str(is_on_sw_a), ", is_on_sw_b:", str(is_on_sw_b))
            self.event_rot_sw_ccw()
        elif (self.pin_id_rot_b == gpio_pin and is_on_sw_a):
            if not self.is_silent:
                print("[4] gpio_pin:", str(gpio_pin), ", is_on_sw_a:",
                      str(is_on_sw_a), ", is_on_sw_b:", str(is_on_sw_b))
            self.event_rot_sw_ccw()


class PushSwitch(Switch):
    def __init__(self, pin_id_rot_sw: int, event_rot_sw_pushed):
        # initialize GPIO pins
        self.pin_id_rot_sw = pin_id_rot_sw
        self.event_rot_sw_pushed = event_rot_sw_pushed
        GPIO.setmode(GPIO.BCM)
        GPIO.setup(pin_id_rot_sw, GPIO.IN, pull_up_down=GPIO.PUD_UP)
        GPIO.add_event_detect(pin_id_rot_sw, GPIO.FALLING,
                              callback=self._event_callback_rot_pushsw_falling, bouncetime=500)

    def _event_callback_rot_pushsw_falling(self, gpio_pin):
        self.event_rot_sw_pushed()

    def __delete__(self):
        # release evets and GPIO pins
        GPIO.setmode(GPIO.BCM)
        GPIO.remove_event_detect(self.pin_id_rot_sw)
        GPIO.cleanup(self.pin_id_rot_sw)


class RotaryEncoderaWithPushSwitchLED(Switch):
    class State (Enum):
        OFF = 1
        ON_A = 2
        ON_B = 3
        ON_AB = 4

    def __init__(self, pin_id_rot_a: int, pin_id_rot_b: int, pin_id_rot_sw: int, pin_id_led_1: int, pin_id_led_2: int):

        self.rotaryencoder = RotaryEncoder(
            pin_id_rot_a, pin_id_rot_b, self.event_rot_sw_cw, self.event_rot_sw_ccw)
        self.pushswitch = PushSwitch(pin_id_rot_sw, self.event_rot_sw_pushed)
        GPIO.setmode(GPIO.BCM)
        self.e_rot_sw_state = self.State.OFF
        self.pin_id_led_1 = pin_id_led_1
        self.pin_id_led_2 = pin_id_led_2
        self.led_a = LED(pin_id_led_1)
        self.led_b = LED(pin_id_led_2)
        self.led_a.off()
        self.led_b.off()

    def __delete__(self):
        GPIO.setmode(GPIO.BCM)
        GPIO.cleanup([self.pin_id_led_1, self.pin_id_led_2])

    def set_state(self, state: State):
        self.e_rot_sw_state = state
        if self.State.OFF == state:
            self.led_a.off()
            self.led_b.off()
        elif self.State.ON_A == state:
            self.led_a.on()
            self.led_b.off()
        elif self.State.ON_B == state:
            self.led_a.off()
            self.led_b.on()
        elif self.State.ON_AB == state:
            self.led_a.on()
            self.led_b.on()

    def get_state(self) -> State:
        return self.e_rot_sw_state

    def event_rot_sw_cw(self):
        # delegate ... please implement in sub class.
        pass

    def event_rot_sw_ccw(self):
        # delegate ... please implement in sub class.
        pass

    def event_rot_sw_pushed(self):
        # delegate ... please implement in sub class.
        pass
