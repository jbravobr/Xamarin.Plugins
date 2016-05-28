## More Information

1. [IBM Worklight Foundation home page](http://www.ibm.com/developerworks/mobile/worklight/index.html)
2. [IBM Worklight Foundation Knowledge Center](http://www-01.ibm.com/support/knowledgecenter/SSZH4A_6.2.0/)
3.  The C# API guide is bundled inside the component
4.  The sample Xamarin solution for Android and iOS is bundled in the component

## The IBM Worklight Add-in for Xamarin Studio

A add-in is provided for the Xamarin Studio for IBM Worklight integration. This add-in prepares the development environment by:

 1.  Creating the Worklight Server instance for the installation
 2.  Creating a Worklight project for the Xamarin solution that is active
 3.  Creating a Android native app configuration and a iOS app configuration in the project
 4.  Allows to Start/stop the server as well as open the Worklight Console in the browser

## Pre-requisites for a New Solution

 1.  You need an instance of the IBM Worklight Server on the development machine.  Install Worklight CLI (Command line Interface) from the [ IBM Worklight download page](http://www.ibm.com/developerworks/mobile/worklight/download/cli.html)
 2.  Create a Xamarin Solution
 3.  Add a Android and/or iOS project in the solution
 4.  Add this component to the project 
 5.  Install the IBM Worklight Add-in 
  1.  Right click on the IBM Worklight Component and click on **Open containing folder**
  2.  Add the Add-in (.mpack file) by using the Add-in panel from the **component\addin** folder
 6. Click on **Menu>Tools>IBM Worklight>Start Server** - this creates the Worklight setup
 7. The Worklight SDK needs a property file that contains information on how to connect to the Worklight Server. This information is pre-populated with some data (like the IP address of the server, application name etc) in the Worklight project the add-in created. Add it to the Xamarin Application projects.
  1. Android: Add the < Solution folder>\worklight\< SolutionName>\apps\android< SolutionName>\wlclient.properties file to the Xamarin Android **Assets** folder and set the build action to **AndroidAsset**. (e.g: \Xtest\worklight\Xtest\apps\androidXtest\wlclient.properties)
  2. iOS: Add the < Solution folder>\worklight\< SolutionName>\apps\iOS< SolutionName>\worklight.plist file to the Xamarin iOS **resources** folder and set the build action to **bundleResource** (e.g: \Xtest\worklight\Xtest\apps\iOSXtest\worklight.plist)
 8. To use the JSONStore API, Worklight SDK needs some native files. These need to be added to the project.
   1. Android: Add to the **Assets** folder the files from < Solution folder>\worklight\< SolutionName>\apps\android< SolutionName>/jsonstore/assets . Set the **BuildAction** for these files to **AndroidAsset**.
   1. iOS: No action needed.

**Note:** 

When you add the Worklight Xamarin Component to your project, the following DLLs get referenced in the project

1. Android:   Worklight.Android.dll and Worklight.Xamarin.Android.dll
2.  iOS :  Worklight.iOS.dll and Worklight.Xamarin.iOS.dll

## Sample Application Quickstart

###Pre-requisites
 

1.  You need a instance of the Worklight Server on the development machine.  Install Worklight CLI (Command line Interface) from the [ IBM Worklight download page](http://www.ibm.com/developerworks/mobile/worklight/download/cli.html)
2.  Install the add-in

###Open the samples in Xamarin Studio:

1. Open Xamarin Studio.
2. Create a new Solution and add a project to it
3. Add this component from the component store
4. Double-click on the IBM MobileFirst Component
5. Navigate to the **Samples** tab
3. Open the sample

###Prepare the MobileFirst Server

1.  From the add-in - click on **Start Server** - 1. this command might take some time the first time you run it.
2.  Click on **Open Console** and log into the console, by using the following credentials: username =  admin, and password =  admin
3.  You now see two apps and a SampleHTTPAdapter in the console
4.  Run the app in the simulator/real device


###Configure and run the iOS Sample

1. Right-click the **WorklightSample.iOS** project and select **Set As Startup Project**
2. Expand the **Worklightsample.iOS** project and double-click the file **worklight.plist** to open it in the property value editor.
3. In the property value editor find the entry for "host" and update its value to the "Server host" value.
4. Run the sample project by clicking Xamarin menu **Run > Start Debugging**

###Using JSONStore in the Android Sample
 1. To use the JSONStore API, Worklight SDK needs some native files. These need to be added to the project.
   1. Android: Copy the files under < Solution folder>\worklight\< SolutionName>\apps\android< SolutionName>/jsonstore/assets to the **Assets** folder. Set the **BuildAction** for these files to **AndroidAsset**.
   1. iOS: No action needed.

##Additional Info

###Known Issues

 Add-in
 
 1. Depending on the developer environment, you might get a message that says: "Error: Process Timed out" when you start or stop the server from the add-in. Check whether the server is running as follows:
  1. Open a command prompt or shell window
  2. Type ``` wl status ```
  3. Check that you can see ``` Server worklight is running. Server worklight is listening on port 10080.```

Worklight CLI

1. You might get a message that says: "Cannot find module generator-worklight.". To resolve this error, upgrade the Worklight CLI with the patch, or install the latest version of the CLI. For more information see detailed [discussion on Stackoverflow](http://stackoverflow.com/questions/26136870/is-worklight-cli-installer-broken) .

###Appendix I

Sample Commands for  Worklight CLI.

    wl create-server
    wl create <solutionName>
From within the  &lt;solutionName&gt;  directory

    wl add api <solutionName>Android -e android
    wl add api <solutionName>iOS -e ios
    wl start
    wl build
    wl deploy
    wl stop
    wl status

For a full list of the CLI commands see the topic [Overview of Worklight CLI commands](http://www-01.ibm.com/support/knowledgecenter/SSZH4A_6.2.0/com.ibm.worklight.dev.doc/dev/r_wl_cli_commands.html) in the product documentation.

###Appendix II

To setup a Xamarin development environment with Worklight Studio.

 1. Install Worklight studio from the [IBM website](http://www.ibm.com/developerworks/mobile/worklight/download/studio.html)
 2. Go to the **Servers** tab and Start the server
 3. Click **File>New>Worklight Project** with Project Template = Android;
 4. Click **File>New>Worklight Native API** with Environment = iOS
 5. Right click on **App >Run As> deploy Native API**
 6. Copy the wlclient.properties file and worklight.plist file

