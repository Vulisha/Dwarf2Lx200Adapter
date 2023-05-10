# Dwarf2Lx200Adapter
Adapter to enable LX200 protocol on Dwarf2 smart telescope. 

This is extremly early version that has not been fully tested yet due to clouds. 

This version is intended to be used to be installed directly on dwarf. If you want and know how you can alter IP adresses and build it for your own Windows/Linux PC 


To install this follow this steps. 

1. Download  executables/Dwarf2Lx200Adapter file
2. Connect Dwarf to your local network, turn on photo so you can see stream and detect its IPADDRESS, using tool like Fing or similar. 
3. Using SCP command copy Dwarf2Lx200Adapter executable to usr/bin directory on Dwarf: 
'scp local_path_to_Dwarf2Lx200Adapter root@IPADDRESS/usr/bin/Dwarf2Lx200Adapter
When prompted for password it is 
'rockchip
4. Using ssh command make file executable 
'''ssh root@IPADDRESS
cd ..
chmod +x /usr/bin/Dwarf2Lx200Adapter
'''
5. Start Dwarf2Lx200Adapter with following command
'./usr/bin/Dwarf2Lx200Adapter
7. Now take your app that supports LX200 (skysafary for example)
 - Settings -> Setup -> Scope Type: Meade LX200 Classic, Mount Type -- Equatorial GoTo (German) 
  Connect via WiFi
  IpAddress: IPADDRESS 
  Port Number: 9999
  Check "Set Time&Location" 
  
  
Dwarf should now try to calibrate, watch dwarf2 mobile app  in astro mode to see the status
When calibration is finished, goto should work(not tested) 


Troubleshooting: 
SkySafary has some bad implementation when Setting time and location, so if it fails to connect after ~30 seconds, turn off Set Time & Location for every sunsequent connect. It is only needed on first connect. 
If it fails to connect after few sec it mens you got something wrong. 

Fun facts. 
You can access stream directly by visiting:
http://IPADDRESS:8092/mainstream
http://IPADDRESS:8092/thirdstream



