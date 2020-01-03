# Digitec-Tools
Allows you to do stuff with Digitec, that they don't want you to. Also the home of an unofficial Digitec API.

Use the Digitec-Api.dll as an interface to digitec. Currently the Api is very limited. Fell free to contribute. :)


In order to build and run this on your own server, install the Admin SDK credentials json file from your Firebase Console.
https://console.firebase.google.com/u/0/project/your-project-name/settings/serviceaccounts/adminsdk

Click on Generate New Private Key
Save the file somewhere LOCALLY. Don't publish it!
Export the absolute path to the json into the Environment Variable GOOGLE_APPLICATION_CREDENTIALS.

Windows: Use the GUI. Everything else is buggy as hell. (Login and out afterwards)

The Identity System needs a running MySQL Server.
On Windows use the installer: https://dev.mysql.com/downloads/windows/installer/8.0.html
On Linux: https://raspberry-projects.com/pi/software_utilities/web-servers/mysql

If mysqld for starting the server doesn't work, create a new folder named "data" in the root mysql directory.
