# _C# Hair Salon_

#### _C#, Nancy and Razor project for Epicodus, 03.04.2016_

#### By _**Jay Whang**_

## Description

_You can add Hair Stylists and with each stylist you can add clients that see them _

## Setup/Installation Requirements

* Git clone https://github.com/jaywhang83/csharp-codeReview3-hairSalon.git
* To view the project you must clone the files to your desktop and in your powershell run 'dnu restore' while in the project folder, after the restore is complete you then run 'dnx kestrel' and type localhost:5004 into your browser.

* Database used:
* CREATE DATABASE hair_salon;
* GO
* USE hair_salon;
* GO
* CREATE TABLE stylists(id INT IDENTITY(1,1), name VARCHAR(255));
* CREATE TABLE clients(id INT IDENTITY(1,1), name VARCHAR(255), stylist_id INT, appointment_date DATE, notes VARCHAR(255));
* GO

## Support and contact details

_Email me at: jaywhang83@gmail.com_


### License

*This software is licensed under the MIT license.*

Copyright (c) 2016 **_Jay Whang_**
