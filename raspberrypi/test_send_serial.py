
import RPi.GPIO as GPIO
import time
from myserial import MySerial
from config import Config



def main():
    serial = MySerial().start()
    serial.send_str(Config.text_rot_1_cw)
    time.sleep(0.100)
    serial.send_str(Config.text_rot_1_ccw)
    time.sleep(0.100)
    serial.send_str(Config.text_rot_1_state_a)
    
if __name__ == "__main__":
    main()
