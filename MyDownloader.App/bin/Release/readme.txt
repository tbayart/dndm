.Net Download Manager (DNDM) is a fork of MyDownloader

Original Google code link = http://code.google.com/p/mydownloader/
Original Codeproject link = http://www.codeproject.com/Articles/21053/MyDownloader-A-Multi-thread-C-Segmented-Download-M

I contacted the author for special permission to fork under the Apache license instead of GPL.

Some of the features I have included in this forked release:
+ Fixed support for FTP
(The application would crash if you didn't give it a file name when downloading from FTP)
+ Fixed up the parameter support so you can add downloads via the command line
(This was needed for the following Chrome extension - https://chrome.google.com/webstore/detail/downloaders/lfjamigppmepikjlacjdpgjaiojdjhoj - I'm sure an extension exists for Firefox as well where you can use a custom download manager. See the wiki for more information on intergration.)
+ Upgraded to VS2010/.Net 3.5
+ Create an installer
+ Fixed cookie support 
(Microsoft randomizes the cookie file names now)
+ Added cookie support for Chrome and Firefox (this can be changed in settings)

I highly recommend you check out his original project as it is freaking awesome. You will see "MyDownloader" tags all over this application - the reason is very simple - Guilherme Labigalini deserves most of the credit as all I did was merely added/updated some code and created the installer.

I do want to note a couple of things:
+ This fork's source will always be available
+ The installer that I create will NEVER contain toolbars/greyware/malware (unlike other download managers)
+ Please do not contact Guilherme with questions about DNDM - those should be directed to me adamsna[at]datanethost.net . Questions about MyDownloader should be directed to him, but I should be able to answer some.
+ Please do contact me with bugs/issues - while I can't make any promises at least they can be recorded.