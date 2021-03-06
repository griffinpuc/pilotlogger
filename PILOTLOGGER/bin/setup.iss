; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "PilotRC Groundstation"
#define MyAppVersion "1.0.0"
#define MyAppPublisher "Schindler Electronics"
#define MyAppURL "https://github.com/griffinpuc/pilotlogger"
#define MyAppExeName "PILOTLOGGER.exe"

[Setup]
; NOTE: The value of AppId uniquely identifies this application. Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{0711A991-06EA-4027-AC0D-0A59FFAEB49E}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={autopf}\PilotRC Groudstation
DisableProgramGroupPage=yes
LicenseFile=C:\Users\griff\Workspace\pilotlogger\PILOTLOGGER\bin\license.txt
; Uncomment the following line to run in non administrative install mode (install for current user only.)
;PrivilegesRequired=lowest
OutputBaseFilename=mysetup
SetupIconFile=C:\Users\griff\Workspace\pilotlogger\PILOTLOGGER\droneicon.ico
Compression=lzma
SolidCompression=yes
WizardStyle=modern

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "C:\Users\griff\Workspace\pilotlogger\PILOTLOGGER\bin\Release\PILOTLOGGER.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\griff\Workspace\pilotlogger\PILOTLOGGER\bin\Release\AdonisUI.ClassicTheme.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\griff\Workspace\pilotlogger\PILOTLOGGER\bin\Release\AdonisUI.ClassicTheme.xml"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\griff\Workspace\pilotlogger\PILOTLOGGER\bin\Release\AdonisUI.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\griff\Workspace\pilotlogger\PILOTLOGGER\bin\Release\AdonisUI.xml"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\griff\Workspace\pilotlogger\PILOTLOGGER\bin\Release\carbonfiber-vtol.mtl"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\griff\Workspace\pilotlogger\PILOTLOGGER\bin\Release\carbonfiber-vtol.obj"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\griff\Workspace\pilotlogger\PILOTLOGGER\bin\Release\Cyotek.Drawing.BitmapFont.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\griff\Workspace\pilotlogger\PILOTLOGGER\bin\Release\Cyotek.Drawing.BitmapFont.pdb"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\griff\Workspace\pilotlogger\PILOTLOGGER\bin\Release\HelixToolkit.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\griff\Workspace\pilotlogger\PILOTLOGGER\bin\Release\HelixToolkit.Wpf.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\griff\Workspace\pilotlogger\PILOTLOGGER\bin\Release\HelixToolkit.Wpf.SharpDX.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\griff\Workspace\pilotlogger\PILOTLOGGER\bin\Release\HelixToolkit.Wpf.SharpDX.xml"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\griff\Workspace\pilotlogger\PILOTLOGGER\bin\Release\HelixToolkit.Wpf.xml"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\griff\Workspace\pilotlogger\PILOTLOGGER\bin\Release\HelixToolkit.xml"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\griff\Workspace\pilotlogger\PILOTLOGGER\bin\Release\LiveCharts.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\griff\Workspace\pilotlogger\PILOTLOGGER\bin\Release\LiveCharts.Geared.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\griff\Workspace\pilotlogger\PILOTLOGGER\bin\Release\LiveCharts.pdb"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\griff\Workspace\pilotlogger\PILOTLOGGER\bin\Release\LiveCharts.Wpf.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\griff\Workspace\pilotlogger\PILOTLOGGER\bin\Release\LiveCharts.Wpf.pdb"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\griff\Workspace\pilotlogger\PILOTLOGGER\bin\Release\LiveCharts.Wpf.xml"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\griff\Workspace\pilotlogger\PILOTLOGGER\bin\Release\LiveCharts.xml"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\griff\Workspace\pilotlogger\PILOTLOGGER\bin\Release\Microsoft.Expression.Drawing.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\griff\Workspace\pilotlogger\PILOTLOGGER\bin\Release\Microsoft.Expression.Drawing.xml"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\griff\Workspace\pilotlogger\PILOTLOGGER\bin\Release\Microsoft.Maps.MapControl.WPF.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\griff\Workspace\pilotlogger\PILOTLOGGER\bin\Release\Microsoft.Maps.MapControl.WPF.xml"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\griff\Workspace\pilotlogger\PILOTLOGGER\bin\Release\Newtonsoft.Json.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\griff\Workspace\pilotlogger\PILOTLOGGER\bin\Release\Newtonsoft.Json.xml"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\griff\Workspace\pilotlogger\PILOTLOGGER\bin\Release\PILOTLOGGER.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\griff\Workspace\pilotlogger\PILOTLOGGER\bin\Release\PILOTLOGGER.exe.config"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\griff\Workspace\pilotlogger\PILOTLOGGER\bin\Release\PILOTLOGGER.pdb"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\griff\Workspace\pilotlogger\PILOTLOGGER\bin\Release\pilotlogodrone.png"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\griff\Workspace\pilotlogger\PILOTLOGGER\bin\Release\SharpDX.D3DCompiler.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\griff\Workspace\pilotlogger\PILOTLOGGER\bin\Release\SharpDX.D3DCompiler.pdb"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\griff\Workspace\pilotlogger\PILOTLOGGER\bin\Release\SharpDX.D3DCompiler.xml"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\griff\Workspace\pilotlogger\PILOTLOGGER\bin\Release\SharpDX.Direct2D1.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\griff\Workspace\pilotlogger\PILOTLOGGER\bin\Release\SharpDX.Direct2D1.pdb"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\griff\Workspace\pilotlogger\PILOTLOGGER\bin\Release\SharpDX.Direct2D1.xml"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\griff\Workspace\pilotlogger\PILOTLOGGER\bin\Release\SharpDX.Direct3D11.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\griff\Workspace\pilotlogger\PILOTLOGGER\bin\Release\SharpDX.Direct3D11.pdb"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\griff\Workspace\pilotlogger\PILOTLOGGER\bin\Release\SharpDX.Direct3D11.xml"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\griff\Workspace\pilotlogger\PILOTLOGGER\bin\Release\SharpDX.Direct3D9.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\griff\Workspace\pilotlogger\PILOTLOGGER\bin\Release\SharpDX.Direct3D9.pdb"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\griff\Workspace\pilotlogger\PILOTLOGGER\bin\Release\SharpDX.Direct3D9.xml"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\griff\Workspace\pilotlogger\PILOTLOGGER\bin\Release\SharpDX.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\griff\Workspace\pilotlogger\PILOTLOGGER\bin\Release\SharpDX.DXGI.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\griff\Workspace\pilotlogger\PILOTLOGGER\bin\Release\SharpDX.DXGI.pdb"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\griff\Workspace\pilotlogger\PILOTLOGGER\bin\Release\SharpDX.DXGI.xml"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\griff\Workspace\pilotlogger\PILOTLOGGER\bin\Release\SharpDX.Mathematics.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\griff\Workspace\pilotlogger\PILOTLOGGER\bin\Release\SharpDX.Mathematics.pdb"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\griff\Workspace\pilotlogger\PILOTLOGGER\bin\Release\SharpDX.Mathematics.xml"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\griff\Workspace\pilotlogger\PILOTLOGGER\bin\Release\SharpDX.pdb"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\griff\Workspace\pilotlogger\PILOTLOGGER\bin\Release\vtolcolor.mtl"; DestDir: "{userdocs}\PilotRC\models"; Flags: ignoreversion
Source: "C:\Users\griff\Workspace\pilotlogger\PILOTLOGGER\bin\Release\vtolcolor.obj"; DestDir: "{userdocs}\PilotRC\models"; Flags: ignoreversion
Source: "C:\Users\griff\Workspace\pilotlogger\PILOTLOGGER\bin\Release\default.schema"; DestDir: "{userdocs}\PilotRC\schemas"; Flags: ignoreversion
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{autoprograms}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

