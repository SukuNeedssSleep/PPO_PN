from stable_baselines3.common.evaluation import evaluate_policy
from stable_baselines3 import PPO
from unityenvnine import UnityEnv

model = PPO.load('../BallCode/BallData/SavedModel/5.2/2030000.zip')

env = UnityEnv()


mean_reward ,_ = evaluate_policy(model,env,n_eval_episodes=100)