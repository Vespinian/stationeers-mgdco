#!/bin/bash
dotnet build
echo Copying dll to Stationeers directory
cp bin/Debug/net48/MoreGasDisplayConsoleOptions.dll '/mnt/sofia/SteamLibrary/steamapps/compatdata/544550/pfx/drive_c/users/steamuser/Documents/My Games/Stationeers/mods/MoreGasDisplayConsoleOptions/'
echo Copying pdb to Stationeers directory
cp bin/Debug/net48/MoreGasDisplayConsoleOptions.pdb '/mnt/sofia/SteamLibrary/steamapps/compatdata/544550/pfx/drive_c/users/steamuser/Documents/My Games/Stationeers/mods/MoreGasDisplayConsoleOptions/'
echo Copying GameData to Stationeers directory
cp -r GameData '/mnt/sofia/SteamLibrary/steamapps/compatdata/544550/pfx/drive_c/users/steamuser/Documents/My Games/Stationeers/mods/MoreGasDisplayConsoleOptions/'
echo Copying About to Stationeers directory
cp -r About '/mnt/sofia/SteamLibrary/steamapps/compatdata/544550/pfx/drive_c/users/steamuser/Documents/My Games/Stationeers/mods/MoreGasDisplayConsoleOptions/'
