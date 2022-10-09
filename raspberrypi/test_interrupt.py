import time
import RPi.GPIO as GPIO
from enum import Enum

pin_id_rot_a = 14
pin_id_rot_b = 15
pin_id_rot_sw = 18
pin_id_led_1 = 23
pin_id_led_2 = 24

class State (Enum):
    OFF = 1
    ON_1 = 2
    ON_2 = 3

e_rot_sw_state = State.OFF

def event_callback_rot_sw_falling(gpio_pin):
    time.sleep(0.001) # Chattering prevention
    # print('-----')
    is_on_sw1 = GPIO.input(pin_id_rot_a)
    is_on_sw2 = GPIO.input(pin_id_rot_b)
    # if is_on_sw1:
    #     print('[', ('x' if pin_id_sw1 == gpio_pin else ' '), 'sw1 ON')
    # else:
    #     print('[', ('o' if pin_id_sw1 == gpio_pin else ' '), 'sw1 OFF')
    # if is_on_sw2:
    #     print('[', ('x' if pin_id_sw2 == gpio_pin else ' '), 'sw2 ON')
    # else:
    #     print('[', ('o' if pin_id_sw2 == gpio_pin else ' '), 'sw2 OFF')

    if ( not is_on_sw1 and not is_on_sw2):
        # print ('indistinguishable')
        pass
    elif ( pin_id_rot_a == gpio_pin and is_on_sw1) or ( pin_id_rot_b == gpio_pin and is_on_sw2):
        # print ('chattering')
        pass
    elif ( pin_id_rot_a == gpio_pin and is_on_sw2) or ( pin_id_rot_b == gpio_pin and not is_on_sw1):
        print ('CCW')
    elif ( pin_id_rot_a == gpio_pin and not is_on_sw2) or ( pin_id_rot_b == gpio_pin and is_on_sw1):
        print ('CW')
    # print('-----')

def event_callback_rot_pushsw_falling(gpio_pin):
    global e_rot_sw_state
    time.sleep(0.1) # Chattering prevention
    if State.OFF == e_rot_sw_state:
        GPIO.output(pin_id_led_1, GPIO.HIGH)
        GPIO.output(pin_id_led_2, GPIO.LOW)
        e_rot_sw_state = State.ON_1
    elif State.ON_1 == e_rot_sw_state:
        GPIO.output(pin_id_led_1, GPIO.LOW)
        GPIO.output(pin_id_led_2, GPIO.HIGH)
        e_rot_sw_state = State.ON_2
    elif State.ON_2 == e_rot_sw_state:
        GPIO.output(pin_id_led_1, GPIO.LOW)
        GPIO.output(pin_id_led_2, GPIO.LOW)
        e_rot_sw_state = State.OFF

    print('rot sw pushed.')

GPIO.setmode(GPIO.BCM)
GPIO.setup(pin_id_rot_a, GPIO.IN, pull_up_down=GPIO.PUD_UP)
GPIO.add_event_detect(pin_id_rot_a, GPIO.FALLING, callback=event_callback_rot_sw_falling, bouncetime=10)
GPIO.setup(pin_id_rot_b, GPIO.IN, pull_up_down=GPIO.PUD_UP)
GPIO.add_event_detect(pin_id_rot_b, GPIO.FALLING, callback=event_callback_rot_sw_falling, bouncetime=10)

GPIO.setup(pin_id_rot_sw, GPIO.IN, pull_up_down=GPIO.PUD_UP)
GPIO.add_event_detect(pin_id_rot_sw, GPIO.FALLING, callback=event_callback_rot_pushsw_falling, bouncetime=500)

GPIO.setup(pin_id_led_1, GPIO.OUT)
GPIO.setup(pin_id_led_2, GPIO.OUT)

GPIO.output(pin_id_led_1, GPIO.LOW)
GPIO.output(pin_id_led_2, GPIO.LOW)

try:
    while True:
        time.sleep(1)
except KeyboardInterrupt:
    print ('exit')
    GPIO.remove_event_detect(pin_id_rot_a)
    GPIO.remove_event_detect(pin_id_rot_b)
    GPIO.cleanup()


# while True:
#     GPIO.wait_for_edge(pin_id_sw1, GPIO.FALLING)
#     is_on_sw1 = GPIO.input(pin_id_sw1)
#     time.sleep(0.3)
