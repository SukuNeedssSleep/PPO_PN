# Import the required libraries
import cv2
import numpy as np

# Create a video capture object
cap = cv2.VideoCapture(0)

# Define the range of the target color in HSV
lower_color = np.array([85, 68, 0])  # lower bound of blue color
upper_color = np.array([225, 225, 255])  # upper bound of blue color

# Loop until the user presses 'q' to quit
while True:
    # Read a frame from the video
    ret, frame = cap.read()
    if not ret:
        break

    # Convert the frame to HSV format
    hsv = cv2.cvtColor(frame, cv2.COLOR_BGR2HSV)

    # Threshold the HSV image to get only the target color
    mask = cv2.inRange(hsv, lower_color, upper_color)

    mask = cv2.erode(mask, None, iterations=2)

    # Dilation: A pixel element in the original image is ‘1’ if at least one pixel under the kernel is ‘1'
    # Increases white space
    mask = cv2.dilate(mask, None, iterations=2)

    # Find the contours of the masked image
    contours, hierarchy = cv2.findContours(mask, cv2.RETR_EXTERNAL, cv2.CHAIN_APPROX_SIMPLE)

    # Loop over the contours
    for c in contours:
        # If the contour area is too small, ignore it
        if cv2.contourArea(c) < 100:
            continue

        # Get the bounding rectangle of the contour
        x, y, w, h = cv2.boundingRect(c)

        # Draw the rectangle on the original frame
        cv2.rectangle(frame, (x, y), (x + w, y + h), (0, 255, 0), 2)

        # Calculate the depth of the object using the width of the rectangle
        # Assuming that the object is a sphere with a known radius of 10 cm
        # Using the formula: depth = focal_length * real_width / pixel_width
        # Assuming that the focal length is 700 pixels
        focal_length = 700
        real_width = 10
        depth = focal_length * real_width / w

        # Display the depth value on the frame
        cv2.putText(frame, f"Depth: {depth:.2f} cm", (x, y - 10), cv2.FONT_HERSHEY_SIMPLEX, 0.5, (0, 255, 0), 1)

    # Display the original frame and the mask
    cv2.imshow("Frame", frame)
    cv2.imshow("Mask", mask)

    # Check if the user presses 'q' to quit
    if cv2.waitKey(1) & 0xFF == ord('q'):
        break

# Release the video capture object and close the windows
cap.release()
cv2.destroyAllWindows()