import customtkinter
import cv2
import imutils
from imutils.video import VideoStream
from PIL import Image, ImageTk


box_width = 0
box_height = 0



def camera():
        vid = cv2.VideoCapture(0)

        ret,frame = vid.read()

        cv2.imshow("Camera",frame)

        cv2.destroyAllWindows()

        cv2.imwrite("temp.jpg",frame)


camera()

customtkinter.set_appearance_mode("dark")
customtkinter.set_default_color_theme("dark-blue")



root = customtkinter.CTk()
root.geometry("1024x768")
root.title("Configuration")

frame_bg = customtkinter.CTkFrame(master=root)
frame_bg.pack(pady = 20 ,padx = 50,fill="both",expand = True)

#image_frame = customtkinter.CTkFrame(master=frame_bg)
#image_frame.pack(pady = 40,padx=50)

image_to_edit = customtkinter.CTkImage(Image.open("temp.jpg"), size=(640, 480))

#button = customtkinter.CTkButton(master=root, text="",)
#button.place(relx=0.5, rely=0.5, anchor=customtkinter.CENTER)

label = customtkinter.CTkLabel(master = frame_bg,text = " Callibration System")
label.pack(pady= 12, padx=10)

image_label = customtkinter.CTkLabel(master=frame_bg,image=image_to_edit,text="Settings",compound="topp")
image_label.pack(padx = 100)


root.mainloop()