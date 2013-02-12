[Setup]
AppName=.Net Download Manager
AppVersion=1.3
LicenseFile=MyDownloader.App\bin\Release\License.txt
AppPublisher=Nathan Adams
AppPublisherURL=http://code.google.com/u/112375326098483542471/
DefaultDirName={pf}\.Net Download Manager
AppReadmeFile=MyDownloader.App\bin\Release\readme.txt
InfoBeforeFile=MyDownloader.App\bin\Release\readme.txt
DefaultGroupName=.Net Download Manager

[Files]
Source: "MyDownloader.App\bin\Release\ICSharpCode.SharpZipLib.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "MyDownloader.App\bin\Release\MyDownloader.App.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "MyDownloader.App\bin\Release\MyDownloader.Core.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "MyDownloader.App\bin\Release\MyDownloader.Extension.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "MyDownloader.App\bin\Release\MyDownloader.Spider.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "MyDownloader.App\bin\Release\TabStrip.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "MyDownloader.App\bin\Release\sqlite3.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "MyDownloader.App\bin\Release\INIFileParser.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "MyDownloader.App\bin\Release\System.Data.SQLite.dll"; DestDir: "{app}"; Flags: ignoreversion

[ThirdParty]
UseRelativePaths=True

[Icons]
Name: "{userstartmenu}\.Net Download Manager\Uninstall DNDM"; Filename: "{uninstallexe}"
Name: "{userstartmenu}\.Net Download Manager\.Net Download Manager"; Filename: "{app}\MyDownloader.App.exe"
