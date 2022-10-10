import serial
from config import Config
import threading
import queue
import time


class MySerial:

    def __init__(self):
        self.serial = serial.Serial(
            Config.com_name, Config.com_baudrate, timeout=Config.com_timeout)
        self.is_enable = False
        self.queue = queue.Queue()


    def __delete__(self):
        self.is_enable = False
        self.thread.join()

    def start(self):
        # print("starting thread...")
        self.is_enable = True
        self.thread = threading.Thread(target=self.loop)
        self.thread.start()
        # print("thread started...")
        return self

    def end(self):
        # print("ending thread")
        self.is_enable = False

    def send_str(self, text: str):
        # self.queue.put(text)
        self.serial.write(text.encode('utf-8'))
        

    def loop(self):
        #a = ser.read(1)
        line = []
        # print("reading serial...")

        while self.is_enable:
            bytes = self.serial.read_all()
            if len(bytes) > 0:
                print('Serial:' , bytes.decode('utf-8'))
            else:
                time.sleep(0.100)

        self.serial.close()
