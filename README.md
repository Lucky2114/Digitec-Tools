# Digitec-Tools
Allows you to do stuff with Digitec, that they don't want you to. Also the home of an unofficial Digitec API.

Use the Digitec-Api.dll as an interface to digitec. Currently the Api is very limited. Fell free to contribute. :)


In order to build and run this on your own server, install the Admin SDK credentials json file from your Firebase Console.
https://console.firebase.google.com/u/0/project/your-project-name/settings/serviceaccounts/adminsdk

Click on Generate New Private Key
Save the file somewhere LOCALLY. Don't publish it!
Export the absolute path to the json into the Environment Variable GOOGLE_APPLICATION_CREDENTIALS.

Windows Powershell: $env:GOOGLE_APPLICATION_CREDENTIALS="C:\Users\username\Downloads\service-account-file.json"
Login and out of your pc.
