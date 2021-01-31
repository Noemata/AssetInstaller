# AssetInstaller

Asset installation utility.

Description: 

A utility application that installs the content of an embedded zip file resource to a user specified destination folder.

## Building
* In MainWindow.xaml.cs adjust string constants ProductName, ProductVersion, HelpMessage, ContainerFolder and ResourceName to your requirements.
* Set ContainerFolder to null if you do not need to lock extraction to a particular folder target.
* Replace the zip file in the Resources folder with your own.
* Publish with "Produce single file" option to create self contained executable with your embedded resource zip archive.

## Usage
* Select Help ? for usage details.
* You will be prompted to install .Net 5 if it is not present.
* Unblock this executable if it is downloaded from a trusted source.

Note: This is a WIP POC that was created mostly to determine how to use UWP APIs within a .Net 5 WPF app.

## Credits
* UI ideas: https://github.com/oleg-shilo/wixsharp
* IInitializeWithWindow lib: https://github.com/mveril
* Package handling ideas: https://github.com/colinkiama/UWP-Package-Installer , https://github.com/UWPX/UWPX-Installer

## Screenshot
![Screenshot](https://github.com/Noemata/AssetInstaller/raw/master/Screenshot.png)

## Publish Settings
![Screenshot](https://github.com/Noemata/AssetInstaller/raw/master/PublishSettings.png)
