import serial
ser = serial.Serial('/dev/ttyGS0', 115200, timeout=2)


#a = ser.read(1)
line = []
x = 0

while 1:
    #while ser.in_waiting:
        input: str = ""
        value: int
        index: int

        ser.read_until(expected=b'@')
        input += ser.read_until(expected=b'/').decode()
        index = ser.read_until(expected=b'=').decode()
        data = ser.read_until(expected=b'$').decode()

        print("input:" + input[:-1] + " index:" + index[:-1] + " data:" + data[:-1])

        x = x ^ 1
        if x == 1:
            ser.write(b"@465/1$")
        else:
            ser.write(b"@465/0$")
            
#        c: bytes = ser.read(1)
#        d: str = c.decode()
#        line.append(d)#
#
#        if d == '$':
#            print(''.join(line))
#            line = []
            

ser.close()
