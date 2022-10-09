import serial
from config import Config
import threading
import queue


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

    def send(self, text: str):
        # self.queue.put(text)
        self.serial.write(text.encode('utf-8'))
        

    def loop(self):
        #a = ser.read(1)
        line = []
        # print("reading serial...")

        while self.is_enable:
            # print("reading serial...")
            # while ser.in_waiting:
            input: str = ""
            value: int
            index: int

            self.serial.read_until(expected=b'@')
            input += self.serial.read_until(expected=b'/').decode()
            index = self.serial.read_until(expected=b'=').decode()
            data = self.serial.read_until(expected=b'$').decode()

            print("input:" + input[:-1] + " index:" +
                  index[:-1] + " data:" + data[:-1])

        self.serial.close()
        # print("thread end")
