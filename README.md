# Robot-Arm
Using Unity ML-agents to control platform to balance ball.
neural network inferences are sent to arduino which controls servos that control x box controller.

## Requirements
- Unity
- [Unity ML-agents] (https://unity.com/products/machine-learning-agents)

## Basic Steps
Train model with CLI using trainer_config.yaml found in configs/

 `mlagents-learn configs/trainer_config.yaml --run_id="Run_1"`

# Results
[Adapted it to play pong](https://github.com/elmojesus/robot-arm-pong)

[Demo](https://youtu.be/-WaiAsnxeas)

![controller](supercoolvideo.gif)
![Ball](Unity-Ball.gif)
![BallClose](Unity-BallClose.gif)

[Inspiration, and servo rig design from littlefrenchkev. All code is original](https://www.littlefrenchkev.com/xbox-controller-arm)
