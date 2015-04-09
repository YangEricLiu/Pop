@echo off

ilmerge /target:winexe /targetplatform:v4,C:\Windows\Microsoft.NET\Framework64\v4.0.30319 /out:Simulator.exe GatewaySimulator.exe M2Mqtt.dll Newtonsoft.Json.dll /log:"merge.log"
copy /Y GatewaySimulator.exe.config Simulator.exe.config