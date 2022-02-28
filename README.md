# <img src = https://walkingforrochester.org/wp-content/uploads/2020/08/Favicon_512x512.png width = 64/> Walking For Rochester  <img src = https://walkingforrochester.org/wp-content/uploads/2020/08/Favicon_512x512.png width = 64/>

An app for Walking for Rochester organization and for them to keep track of litters that had been pick in the area.

# Introduction
Welcome to the Walking for Rochester app! This app is created by the RIT/NTID Mobile Application Development students as a part of the capstone class project for Walking for Rochester and two students kept working on it as co-op credit/ job over winter break.


Walking for Rochester's Mission Statement: 
>Walking For Rochester organizes neighborhoods to pick up litter within their community. I am Walking For Rochester, are you?

@2020/2021 NTID/RIT Mobile Application Development. All Rights Reserved.

# Feature
<p algin="center">
<img src = img/Splashscreen.png alt= 'splash Screen' logo width=95><img src = img/thickBorder.png alt= hamburgerIcon logo width=100>&nbsp;<img src = img/map.png alt = 'main feature of map' logo width = 100>&nbsp;&nbsp;<img src = img/Details.png alt= 'Drawer Navigation' logo width=100>&nbsp; &nbsp;<img src = img/leaderboardApp.png alt= leaderboard logo width=100> 
</p>

# Use

## Requirements
 - Xamarin.Forms version above 4.8.0.1687
 - Xamarin.Forms.Maps version above 4.8.0.1687
 - Xamarin.Forms.GoogleMaps version above 3.3.0
 - Xamarin.Essintails version above 1.5.3.2
 - Newtonsoft.Jsom version above 12.0.0
 - MySqlConnector version above 1.2.0
 - Plugin.Toast version 2.2.0
 - Xam.Plugin.Media version above 5.0.0
 - Location permisson All of the time


## Change Google Maps API key
Enable Place API, Maps SDK for Android and Geocoding API in the "APIs and Services" of Google Cloud Services.

Modify the file `AndroidManifest.xml` and its path as follows
```
WalkingForRochester
    WalkingForRochester.Android
        Properties
            AndroidManifest.xml
```

```xml
<meta-data 
    android:name="com.google.android.geo.API_KEY" 
    android:value="Put here your GOOGLE MAPS API KEY" 
/>
```

## Change Hostinger's database 
Modify the file `IDBConnection.cs` and its path as follows
```
WalkingForRochester
    WalkingForRochester
        Services
            MySQL
                IDBConnection.cs
```

```c#
const string DatabaseLocalhost = "Hosting Address";
const string DatabaseUsername  = "Username";
const string DatabasePassword  = "Password";
const string DatabaseName      = "Database Name";
```

## Change Forgot Password Mail
Modify the file `IMailInformation.cs` and its path as follows
```
WalkingForRochester
    WalkingForRochester
        Controls
            IMailInformation.cs
```

```c#
const string SenderMailAddress  = "Sender Email Address";
const string CredentialPassword = "Application-Specific Passwords";
const string Subject            = "Walking for Rochester - CAPTCHA code";
```

# Team infomation
## Project Leader
 - Xiangyu Shi
 - Michelle Olson

## Design Team
 - Xiangyu Shi
 - Michelle Olson
 - Colton Bailiff
 - Harsh Mathur

## Development Team
 - Xiangyu Shi
 - Michelle Olson
 - Zhencheng Chen 
 - Chase Rose
 - Quoc Nhan
