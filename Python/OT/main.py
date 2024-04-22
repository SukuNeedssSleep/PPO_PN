from collections import deque
from imutils.video import VideoStream
import numpy as np
import argparse
import cv2
import imutils
import time
import UdpComms as U
import socket



ap = argparse.ArgumentParser()
ap.add_argument("-v", "--videosrc", help="path to the {optional} video file")
ap.add_argument("-b", "--buffer", type=int, default=64, help="max buffer size")
args = vars(ap.parse_args())

# Detect the lower and upper boundaries of the colour bring used,  using imutils
# These need to be calibrated as per our env
blueLower = (85, 68, 0) # 85 156 24 , 101 147 24, 106 236 78 95 146 36
blueUpper = (225, 225, 225) # 125 225 225, 109 225 136 ,144

captureWidth = 640
captureHeight = 480


x_offset = 0
y_offset = 0

x_offset_end = 0
y_offset_end = 0

rect_start_point = (x_offset,y_offset)
rect_end_point = (x_offset_end,y_offset_end)
rect_colour = (0,0,225)
rect_thickness = 4




KNOWN_DISTANCE = 0
KNOWN_WIDTH = 0

data = [0, 0, 0,0,0]
# no of points to track per second
points = deque(maxlen=args["buffer"])

if not args.get("videosrc", False):
    vs = VideoStream(src=0).start()

else:
    vs = cv2.VideoCapture(args["videosrc"])



time.sleep(2.0)

# Communication
sock = U.UdpComms(udpIP="127.0.0.1", portTX=8000, portRX=8001, enableRX=True, suppressWarnings=True)


# Functions
def smooth(x):
    temp = int(x / 10)
    smooth_x = temp * 10
    return smooth_x


def distance_to_camera(knownWidth, focalLength, perWidth):
    return (knownWidth * focalLength) / perWidth


def sync(x,id):
    if id == 0:
        return x - x_offset
    elif id == 1:
        return x  - y_offset





# Main Logic

while True:
    frame = vs.read()

    frame = frame[1] if args.get("videosrc", False) else frame

    # When reading from video and we have no frame then it marks the end of the video
    if frame is None:
        break

    # Actual Tracking bit
    frame = imutils.resize(frame, width=captureWidth)
    blurred = cv2.GaussianBlur(frame, (11, 11), 0)  # removes background noise
    hsv = cv2.cvtColor(blurred, cv2.COLOR_BGR2HSV)  # converts RGB colour scale to black and white so tracking easier

    # Construct a mask around the color blue
    mask = cv2.inRange(hsv, blueLower, blueUpper)

    # Erosion: A pixel in the original image (either 1 or 0) will be considered 1 only
    # if all the pixels under the kernel are 1, otherwise, it is eroded (made to zero).

    mask = cv2.erode(mask, None, iterations=2)

    # Dilation: A pixel element in the original image is ‘1’ if at least one pixel under the kernel is ‘1'
    # Increases white space
    mask = cv2.dilate(mask, None, iterations=2)

    # Contour: Basically smooth edge that we detect must have same intensity
    cnts = cv2.findContours(mask.copy(), cv2.RETR_EXTERNAL, cv2.CHAIN_APPROX_SIMPLE)

    cnts = imutils.grab_contours(cnts)

    center = None

    cv2.rectangle(frame,rect_start_point,rect_end_point,rect_colour,rect_thickness)

    if len(cnts) > 0:
        # contour a curve joining all the continuous points (along the boundary), having same color or intensity

        # from the array of contours c represents the max one  where key specifies what we are looking for
        c = max(cnts, key=cv2.contourArea)

        # Get the bounding rectangle of the contour
        x1, y1, w1, h1 = cv2.boundingRect(c)

        # Draw the rectangle on the original frame
        cv2.rectangle(frame, (x1, y1), (x1 + w1, y1 + h1), (0, 255, 0), 2)
        # Calculate the depth of the object using the width of the rectangle
        # Assuming that the object is a sphere with a known radius of 10 cm
        # Using the formula: depth = focal_length * real_width / pixel_width
        # Assuming that the focal length is 700 pixels
        focal_length = 700
        real_width = 10
        depth = focal_length * real_width / w1

        # Display the depth value on the frame
        cv2.putText(frame, f"Depth: {depth:.2f} cm", (x1, y1 - 10), cv2.FONT_HERSHEY_SIMPLEX, 0.5, (0, 255, 0), 1)

        # minEnclosingCircle returns  coordinate of center(x,y) and also returns a radius 
        ((x, y), radius) = cv2.minEnclosingCircle(c)

        M = cv2.moments(c)
        center = (int(M["m10"] / M["m00"]), int(M["m01"] / M["m00"]))

        if radius > 10:
            # draw the circle and centroid on the frame,
            # then update the list of tracked points
            cv2.putText(frame, str(x) + ',' + str(y), (50, 50), cv2.FONT_HERSHEY_PLAIN, 1, (0, 255, 255), 2, cv2.LINE_4)
            cv2.circle(frame, (int(x), int(y)), int(radius),
                       (0, 255, 255), 2)
           # cv2.circle(frame, center, 5, (0, 0, 255), -1)


            data[0] = x
            data[1] = captureHeight - y
            data[2] = smooth(depth)

            data[3] = x_offset_end - x_offset
            data[4] = y_offset_end - y_offset

            data[0] = sync(data[0],0)
           # data[1] = sync(data[1],1)

            cv2.putText(frame, 'Data WRT BoX',(50, 400) ,cv2.FONT_HERSHEY_PLAIN, 1, (0, 255, 255), 2, cv2.LINE_4)
            cv2.putText(frame, str(data[0]) + ',' + str(data[1]), (50, 420), cv2.FONT_HERSHEY_PLAIN, 1, (0, 255, 255), 2, cv2.LINE_4)
            print(str(data))
            sock.SendData(str(data))


            # update the points queue
    points.appendleft(center)

    # loop over the set of tracked points
    for i in range(1, len(points)):
        # if either of the tracked points are None, ignore them
        if points[i - 1] is None or points[i] is None:
            continue
            # otherwise, compute the thickness of the line and draw the connecting lines
        thickness = int(np.sqrt(args["buffer"] / float(i + 1)) * 2.5)
        cv2.line(frame, points[i - 1], points[i], (0, 0, 255), thickness)
        # show the frame to our screen
    cv2.imshow("Frame", frame)
    key = cv2.waitKey(1) & 0xFF
    # if the 'q' key is pressed, stop the loop
    if key == ord("q"):
        break
    # if we are not using a video file, stop the camera video stream
if not args.get("video", False):
    vs.stop()
    # otherwise, release the camera
else:
    vs.release()
    # close all windows

cv2.destroyAllWindows()
