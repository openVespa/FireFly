# FireFly
The digital ignition controller, called the FireFly Mini, uses the stock Variable Reluctor (VR) pickup and is the typical retard only system. Also includes (2) K-type thermocouple inputs for Exhaust Gas Temperature (EGT) and Cylinder Head Temperature (CHT) monitoring. The goal is to provide a programmable ignition controller that dynamically adjusts based on EGT and CHT values.  A Windows program allows you to customize ignition curves over USB; log RPM, CHT, and EGT values; and flash the firmware. You can save maps, read the map that is loaded,etc. I've also created an basic android app that displays the RPM, CHT, and EGT values. In addition, the FF mini simulates the popular OBDII ELM327 interface. This allows my old Vespa to have an "OBD II ecu", allowing you to take advantage of the many ELM327 based apps available. My personal favorite is the Torque app.

The GUI was developed using Microsoft Visual C# 2010 and relies heavily on the Generic HID program and contains portions of the WFF GenericHID Communication Library which is (c)2011 Simon Inns - http://www.waitingforfriday.com 

The Eagle schematics and board files are provided, along with the BOM, and the Gerbers. The circuit card was designed to fit into a Polycase LP-11F enclosure.  The design uses an atmega16u2 and the source code was developed in Atmel Studio 6.1.  The USB stack is based on the LUFA USB Library.

http://www.fourwalledcubicle.com/LUFA.php
