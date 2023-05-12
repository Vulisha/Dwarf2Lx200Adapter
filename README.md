# Dwarf2Lx200Adapter
Adapter to enable LX200 protocol on Dwarf2 smart telescope. 

### Version 0.0.1 added ability to configure Dwarf2 IpAddress so adapter can run from Windows  
This is extremly early version that has not been fully tested yet, so expect unexpected. 
This is tutorial for windows, for other OS you will have to use executables/portable and install .Net7 runtime (linux arm,x86,x64, osx... supported) 

1. Turn on your Dwarf2 connect it to same network as your computer, and do calibration.

2. To run this adapter from windows (64bit) first find IpAddress of your Dwarf2(use tool like Fing), download executables/win64/Dwarf2Lx200Adapter.exe

3. Open command prompt in same location where downloaded exe is and run following command 
```
Dwarf2Lx200Adapter.exe IpAddress
```
4. Now take your app that supports LX200 (skysafary for example) and configure it with IpAddress of YOUR *COMPUTER*, not dwarf.
  
  ```
  Settings -> Setup -> Scope Type: Meade LX200 Classic, Mount Type -- Equatorial GoTo (German)   
  Connect via WiFi  
  IpAddress: IPADDRESS 
  Port Number: 9999  
  Check "Set Time&Location" 
  ```
  
  GoTo should work now. 
  
  COM port support also added, you need com0com for that to be able to connect with Stellarium, but stellarium does not seem to send commands to app, so it is not working. If you want to play around just add com port after IpAddress e.g Dwarf2Lx200Adapter.exe 192.168.1.180 COM5
 
### Version 0.0.0 You can brick your device if you are not carefull, you are using this on your own responsibility 
This is extremly early version that has not been fully tested yet due to clouds. 

This version is intended to be used to be installed directly on dwarf. If you want and know how you can alter IP adresses and build it for your own Windows/Linux PC 


To install this follow this steps: 

1. Download  executables/Dwarf2Lx200Adapter file
2. Connect Dwarf to your local network, turn on photo so you can see stream and detect its IPADDRESS, using tool like Fing or similar. 
3. Using SCP command copy Dwarf2Lx200Adapter executable to usr/bin directory on Dwarf:

```
scp local_path_to_Dwarf2Lx200Adapter root@IPADDRESS/usr/bin/Dwarf2Lx200Adapter
``` 
When prompted for password it is ```rockchip```

4. Using ssh command make file executable 
```
ssh root@IPADDRESS
cd ..
chmod +x /usr/bin/Dwarf2Lx200Adapter
```
5. Start Dwarf2Lx200Adapter with following command

```
./usr/bin/Dwarf2Lx200Adapter
```

7. Now take your app that supports LX200 (skysafary for example)
  
  ```
  Settings -> Setup -> Scope Type: Meade LX200 Classic, Mount Type -- Equatorial GoTo (German)   
  Connect via WiFi  
  IpAddress: IPADDRESS   
  Port Number: 9999  
  Check "Set Time&Location" 
  ```
  
Dwarf should now try to calibrate, watch dwarf2 mobile app  in astro mode to see the status
When calibration is finished, goto should work(not tested) 


Troubleshooting: 
SkySafary has some bad implementation when Setting time and location, so if it fails to connect after ~30 seconds, turn off Set Time & Location for every sunsequent connect. It is only needed on first connect. 
If it fails to connect after few sec it mens you got something wrong. 

Fun facts: 
You can access stream directly by visiting:
```
http://IPADDRESS:8092/mainstream
http://IPADDRESS:8092/thirdstream
```


