import gymnasium as gym
import numpy as np
from gymnasium import spaces
import UdpComms as U
import time


# print(spaces.Box(low=0,high=3,shape=(3,),dtype=np.float32).sample())


class UnityEnv(gym.Env):
    metadata = {"render_mode": ["None"]}

    def __init__(self):
        super(UnityEnv, self).__init__()
        self.sock = U.UdpComms(udpIP="127.0.0.1", portTX=8000, portRX=8001, enableRX=True, suppressWarnings=True)

        self.action_space = spaces.Box(low=-1, high=1, shape=(3,), dtype=np.float_)

        self.observation_space = spaces.Box(low=-np.inf, high=np.inf, shape=(9,), dtype=np.float32)

    def step(self, action):

        arraytemp = action
        num1= arraytemp[0]
        num2 = arraytemp[1]
        num3 = arraytemp[2]

        actionsString = str(num1) + ',' + str(num2) + ',' + str(num3)

        #print(actionsString)

        self.sock.SendData(str(actionsString))
        time.sleep(0.02)
        self.reward = 0
        data = self.sock.ReadReceivedData()

        if data is not None:
            tempData = data.split()
            totalData = list(np.float_(tempData))
            # print(totalData)
            observation = [totalData[0], totalData[1], totalData[2], totalData[3], totalData[4], totalData[5],
                           totalData[6], totalData[7], totalData[8]]

            reward = totalData[9]

            done = totalData[10]
            self.reward = reward
        else:
            observation = [0,0,0,0,0,0,0,0,0]
            reward = 0
            done = 0

            self.reward = reward

        if done == 1:
            self.done = True
        elif done == 0:
            self.done = False
        # print(done)
        info = {"finished": self.done}
        turncated = False

        return observation, self.reward, self.done, turncated, info

    def reset(self, seed=None, options=None):
        # cube_x , cube_y , delta_ball_x , delta_ball_y, delta_floor
        self.sock.SendData("Reset")
        time.sleep(0.02)

        data = self.sock.ReadReceivedData()

        self.done = False

        if data is not None:
            obs = data.split()
            totalData = list(np.float_(obs))
            observation = [totalData[0], totalData[1], totalData[2], totalData[3], totalData[4], totalData[5],
                           totalData[6], totalData[7], totalData[8]]

        else:
            observation = [0, 0, 0, 0, 0, 0, 0, 0, 0]

        info = {}
        return observation, info
