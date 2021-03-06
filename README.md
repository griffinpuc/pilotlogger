<p align="center">
  <img src="images/newlogo.png">
</p>

**PILOT RC Groundstaton** is a Windows application designed to run with the PILOT RC wireless drone control hardware. The log tool provides automatic csv logging capabilities and real-time serial port monitoring. This software acts as a fully functional base station featuring multiple telemetric monitoring capabilities including live GPS tracking integrated with Bing Maps API.
<br />
<br />

## Download and Install

The latest Windows installer can be downloaded in the [releases section](https://github.com/griffinpuc/pilotlogger/releases) of the repo.
Installing is easy, simply follow the installer.
<br />

## Usage

In order to begin logging, one must specify a data schema and COM port. There is a default data schema installed by default, though one must create a custom schema depending on the implementation. This will not affect the functionality of the application and is only used to assign labels to data columns in the csv.

Refer to the [default.schema](https://github.com/griffinpuc/pilotlogger/blob/master/PILOTLOGGER/bin/Debug/schemas/default.schema) schema to create your own. It is simply a list of values stored in an array format. You can save as many schemas as you'd like. By default, the application will write output into your user Documents folder.
<br />
<br />

<p align="center">
  <img src="images/contentfeatures.png">
</p>

## Hardware and More Information

#### PILOT RC Firmware:
* [Repo Link](https://github.com/mschindler301/TX250)

#### More information on hardware and other products:
* [PILOT RC Specifications](https://www.schindlerelectronics.com/specs)
* [Other Downloads](https://www.schindlerelectronics.com/downloads)
* [PILOT RC Tutorials](https://www.schindlerelectronics.com/getting-started)

#### Collaborators

* [Griffin Puc](https://github.com/griffinpuc), basestation software developer
* [Matthew Schindler](https://github.com/mschindler301), firmware author and field testing
