from PIL import ImageGrab, Image
import time
from functools import reduce
import signal, os, sys

#returns remaining health as a fraction over 8
#e.g. 8/8 = full heatlh, 0/8 = dead
def determineRemainingHealth(data, eigths, x):
    width = x
    pixel = x*2 + (eigths * 8) - 3
    missing_parts = 0
    while width > 0:
        if data[pixel][1] <= 160 or data[pixel][1] >= 175:      #acceptable rgb values for green
            missing_parts += 1
        else:
            return 8 - missing_parts
        width -= eigths
        pixel -= eigths
    return 0

def isDotaInForeground(screenGrab):
    screenGrab.crop((6, 790, 32, 812)).save("box.jpg")  
    screenGrabCrop = Image.open("box.jpg")
    pixel = screenGrabCrop.getpixel((13, 11))
    sum = pixel[0] + pixel[1] + pixel[2]
    if sum > 165 or sum < 145:
        return False
    else:
        return True

def determineRemainingMana(data, eigths, x):
    width = x
    pixel = x*39 + (eigths * 8) - eigths
    missing_parts = 0
    while width > 0:
        if data[pixel][2] <= 60 or data[pixel][2] >= 90:        #RGB values for blue
            missing_parts += 1
        else:
            return 8 - missing_parts
        width -= eigths
        pixel -= eigths
    return 0

def handler(signum, frame):
    os.remove("data.jpg")
    os.remove("box.jpg")
    sys.exit(0)

def main():
    signal.signal(signal.SIGINT, handler)
    while(True):
        time.sleep(0.5)
        ImageGrab.grab().save("data.jpg", "JPEG")
        screenGrab = Image.open("data.jpg")
        if not isDotaInForeground(screenGrab):
            print("BG")
            continue
        screenGrab.crop((675, 912, 1343, 953)).save("data.jpg")        #The box is a 4-tuple defining the left, upper, right, and lower pixel coordinate.
        screenGrabCrop = Image.open("data.jpg")
        data = screenGrabCrop.getdata()
        x = data.size[0]
        y = data.size[1]
        eigths = int(x/8)

        print("{0} {1}".format(determineRemainingHealth(data, eigths, x), determineRemainingMana(data, eigths, x)))

if __name__ == '__main__':
    main()