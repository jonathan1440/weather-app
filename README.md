# weather-app

The goal is to create a Unity web app showing weather data from my house for my senior project. To run the app, download the following to the same directory, and then run the .exe file:
- weather-data-build_Data
- UnityPlayer.dll
- weather-view-build.exe

Everything in this project is my own creation except for the standard asset package from Unity and the CSVReader.cs file that I got from https://bravenewmethod.com/2014/09/13/lightweight-csv-reader-for-unity/

All scripts are stored in `/Assets/Scripts`

This repoistory explains how I collected the weather data I use in this simulation: https://github.com/jonathan1440/Get-Weather-Data

**Using:**

JetBrains Rider 2017.1 173.3994

Unity 2017.3 with support for:
- MonoDevelop / Unity Debugger
- Documentation
- Standard Assets
- Android Build
- Windows Store .NET Scripting Backend
- Vuforia Augmented REality Support

**V1.5:**
https://github.com/jonathan1440/weather-app/commit/c225075fb77f7a57a3d762538f072e52a89d6418

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

**V2.0, WNIP:**
I used the following tutorial to start me off on creating a scatterplot:
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
