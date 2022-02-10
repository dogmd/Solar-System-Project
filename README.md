# Solar System Simulator
This is a project that uses N body Newtonian gravity and NASA data to simulate the solar system starting at 04/01/2021. It is written using Unity Version 2020.3.3f1.

## Compile / Run Instructions
These instructions assume you have Unity 2020.3.3f1 and UnityHub installed.

To compile and run yourself:
1. Open Unity Hub, and on the projects tab click "Add" and select the source code directory.
2. Select Unity Version 2020.3.3f1 on the project and start it. The first boot usually takes a while.
3. In the Project window of the Unity Editor, double click Assets/Scenes/HorizonsSolarSystem
4. Click the play button at the top center of the screen to run the game in the Unity Editor. You can also create an executable with File -> Build

Alternatively, a prebuilt executable for 64-bit Windows systems can be found [here](https://drive.google.com/file/d/1oq-o7nmQed8Trt3mhksvh1ZMKdY4D_M6/view?usp=sharing)
## Controls
Hold tab to pull up the settings menu, and left-click drag the mouse to rotate camera around the selected object. Use the scrollwheel to zoom in and out, holding shift makes zooming slower, ctrl faster.
## Screenshots
![](screenshot.png?raw=true)
![](sreenshot_settings.png?raw=true)

## Credits
 - Velocity, positon, rotational period, axis obliquity data pulled from [https://ssd.jpl.nasa.gov/horizons.cgi](https://ssd.jpl.nasa.gov/horizons.cgi)
 - 3D Models from [https://solarsystem.nasa.gov/resources/all](https://solarsystem.nasa.gov/resources/all/?order=pub_date+desc&per_page=50&page=0&search=&condition_1=1%3Ais_in_resource_list&fs=&fc=324&ft=&dp=&category=324)
 - [GLTFUtility](https://github.com/Siccity/GLTFUtility) used to import models.
- [Vector3d](https://github.com/sldsmkd/vector3d) used for high precision simulation.
- [Real Stars Skybox Lite](https://assetstore.unity.com/packages/3d/environments/sci-fi/real-stars-skybox-lite-116333)
