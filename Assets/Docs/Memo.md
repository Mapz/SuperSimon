## 关于暂停

By setting the timeScaling factor to 0, you basically pause your game, since everything time-based will be stopped. Effectively, it results in Time.deltaTime to be constantly 0, and FixedUpdate is not called anymore. (Update and LateUpdate will still be called). If you want to have animations during pause, you have two possibilities:

Use an animator component with Update Mode: Unscaled Time.

If the animations are done through code (like rotation, simple movement or fading) you can use Time.unscaledDeltaTime. Like the name says, this is the unscaled delta value, and thus the same as deltaTime for timeScale = 1.

If you don't use a physics or time based game, you don't need to do anything in special. If your game is purely UI based, you can open a pause panel on click on the pause button. If the pause panel fills out the entire screen (can also be transparent, if you don't want it to look like that), it blocks every underlying UI input, and your game is effectively paused.

## 关于2D像素游戏制作

http://forum.china.unity3d.com/thread-13845-1-1.html