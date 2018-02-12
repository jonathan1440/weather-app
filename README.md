# weather-app

The goal is to create a Unity web app showing weather data from my house for my senior project.

This repoistory explains how I'm getting the weather data: https://github.com/jonathan1440/Get-Weather-Data

**Using:**

JetBrains Rider 2017.1 173.3994

Unity 2017.3 with support for:
- MonoDevelop / Unity Debugger
- Documentation
- Standard Assets
- Android Build
- iOS Build
- Linux build
- Windows Store .NET Scripting Backend
- Windows Store IL2CPP Scripting Backend
- Vuforia Augmented REality Support
- WebGL Build

**V1.0:**
https://github.com/jonathan1440/weather-app/commit/7a07cb2fae4db602364b313d573d47327f7c26a7

It displays current weather conditions and does some basic calculations based on preceeding weather.

It displays:
- is it raining?
- rain depth
- wind speed
- wind direction
- average wind speed
- average wind direction
- current temperature
- high temp
- low temp
- current time (in simulation)
- current date (in simulation)

**V2.0, WIP:**
I used the following tutorial to start me off on creating a scatterplot.
https://sites.psu.edu/bdssblog/2017/04/06/basic-data-visualization-in-unity-scatterplot-creation/

To-do:
1. Create graph prefab
   - box/mesh outline
   - toggle walk around/rotate graph
   - scale graph
   - drag/reposition graph
   - duplicate graph
   - make graph selectable
2. Work on scatterplot
   - change ball radius
   - change color/color scheme
3. Make graph type level
   - show axes
   - change axes
4. Create new graph
   - from ui-imported CSV file
