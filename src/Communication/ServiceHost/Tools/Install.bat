sc stop PopCommunicationService
sc delete PopCommunicationService
sc create PopCommunicationService binpath= "D:\workspace\pop\Pop\src\Communication\ServiceHost\bin\Debug\SE.DSP.Pop.Communication.ServiceHost.exe"
sc start PopCommunicationService