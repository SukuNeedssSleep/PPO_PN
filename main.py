import os
import time
from stable_baselines3 import PPO
from stable_baselines3.common.monitor import Monitor
from stable_baselines3.common.callbacks import BaseCallback
from HumanEnv import HUMANENV

env = HUMANENV()
env = Monitor(env)

models_dir = f"Ai/SavedModel/{int(time.time())}"
logdir = f"Ai/Logs/{int(time.time())}"

if not os.path.exists(models_dir):
    os.makedirs(models_dir)

if not os.path.exists(logdir):
    os.makedirs(logdir)


class TrainAndLoggingCallback(BaseCallback):

    def __init__(self, check_freq, save_path, verbose=1):
        super(TrainAndLoggingCallback, self).__init__(verbose)
        self.check_freq = check_freq
        self.save_path = save_path

    def _init_callback(self):
        if self.save_path is not None:
            os.makedirs(self.save_path, exist_ok=True)

    def _on_step(self):
        if self.n_calls % self.check_freq == 0:
            model_path = os.path.join(self.save_path, 'best_model_{}'.format(self.n_calls))
            self.model.save(model_path)

        return True


callback = TrainAndLoggingCallback(check_freq=1000, save_path=models_dir)

model = PPO("MlpPolicy", env, verbose=1, tensorboard_log=logdir, learning_rate=0.0001, ent_coef=0.05)
env.reset()
TIMESTEPS = 10000

for i in range(1, 100000000000000000):
    model.learn(total_timesteps=TIMESTEPS, reset_num_timesteps=False, tb_log_name="PPO")
    model.save(f"{models_dir}/{TIMESTEPS * i}")

env.close()
