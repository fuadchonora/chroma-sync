[Setup]
AppName=Chroma Sync
AppVersion=1.1.0
WizardStyle=modern
DefaultDirName={autopf32}\ASUS\CHROMASYNC
DefaultGroupName=Chroma Sync
UninstallDisplayName=Chroma Sync
UninstallDisplayIcon={app}\ChromaSync.exe
Compression=lzma2
SolidCompression=yes
OutputDir=..\bin\Setup\
OutputBaseFilename=ChromaSyncSetup
PrivilegesRequired=admin
CloseApplications=force
AppPublisher=ASUSTeK Computer Inc.

[Files]
Source: "..\bin\Release\netcoreapp3.1\win-x86\publish\**"; DestDir: "{app}"; Flags: recursesubdirs ignoreversion

[Run]
Filename: "{sys}\sc.exe"; Parameters: "create ChromaSync start=auto binpath=""{app}\ChromaSync.exe"""; StatusMsg: "Installing Chroma Sync Service..."; Flags: runhidden
Filename: "{sys}\sc.exe"; Parameters: "sc config ChromaSync depend=asComSvc/LightingService"; StatusMsg: "Configuring Chroma Sync Service..."; Flags: runhidden
Filename: "{sys}\sc.exe"; Parameters: "start ChromaSync"; StatusMsg: "Starting Chroma Sync Service..."; Flags: runhidden

[UninstallRun]
Filename: "{sys}\sc.exe"; Parameters: "stop ChromaSync"; StatusMsg: "Stopping Chroma Sync Service..."; Flags: runhidden
Filename: "{sys}\sc.exe"; Parameters: "delete ChromaSync"; StatusMsg: "Uninstalling Chroma Sync Service..."; Flags: runhidden
