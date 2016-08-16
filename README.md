## ![](https://raw.githubusercontent.com/jbravobr/Xamarin.Plugins/master/mobilefirst.png) MobileFirst Plugin for Xamarin Forms

Plugin designed for connections with MFP servers (IBM Mobile Platform First). With it you can make calls Procedures, send logs and connect / disconnect to the Push Notifications service
### Setup
* Available on NuGet: https://www.nuget.org/packages/Plugin.MobileFirst/ [![NuGet](https://img.shields.io/nuget/v/Plugin.MobileFirst.svg?label=NuGet)](https://www.nuget.org/packages/Plugin.MobileFirst/)
* Install into your PCL project and Client projects.
* **Install IBM Mobile First SDK Component into your Client projects** [IBM MobileFirst SDK ](https://components.xamarin.com/view/ibm-worklight)

Built with C# 6 features, you must be running VS 2015 or Xamarin Studio to compile. **NuGets of course work everywhere!**

## Follow Me
* Twitter: [@jbravobr](http://twitter.com/jbravobr)
* Blog: [Stx.blog.br](http://stx.blog.br)

Build Status: [![Build status](https://ci.appveyor.com/api/projects/status/github/jbravobr/Xamarin.Plugins?branch=master&svg=true)](https://ci.appveyor.com/project/jbravobr/xamarin-plugins)

## What is this?
This is my PCL Plugin from IBM Mobile First Platform to Xamarin Forms

## Contribute
My Plugins for Xamarin are completely open source and I encourage and accept pull requests! So please help out in any ways you can:

1. Report Bugs: Open an Issue
2. Submit Feature Requests: Open an Issue
3. Fix a Bug or Add Feature: Send a Pull Request
4. Create your Own Plugin : [Learn How](https://github.com/xamarin/plugins)

**Platform Support**

|Platform|Supported|Version|
| ------------------- | :-----------: | :------------------: |
|Xamarin.iOS|Yes|iOS 6+|
|Xamarin.iOS Unified|Yes|iOS 6+|
|Xamarin.Android|Yes|API 10+|
|Windows Phone Silverlight|No||
|Windows Phone RT|No||
|Windows Store RT|No||
|Windows 10 UWP|No||
|Xamarin.Mac|No||


### API Usage

Call **CrossMobileFirst.Current** from any project or PCL to gain access to APIs.


**Init (iOS)**
```csharp
/// <summary>
/// Init Method for iOS
/// </summary>
void Init();
```

**Init (Android)**
```csharp
/// <summary>
/// Init Method for Android
/// </summary>
/// <param name="activity">Is the Android App Activity</param>
void Init(object activity);
```

**ConnectAsync**
```csharp
/// <summary>
/// Make a new async connection with the MFP Server
/// </summary>
/// <returns>WorlightResult</returns>
Task<WorklightResult> ConnectAsync();
```

**RestInvokeAsync**
```csharp
/// <summary>
/// Make a async Rest Invoke to a procedure from the MFP Server
/// </summary>
/// <param name="adapterName">The Name of the Adapter</param>
/// <param name="adapterProcedureName">The Name of the Procedure</param>
/// <param name="methodType">The Type of HTTP Verb</param>
/// <returns>WorlightResult</returns>
Task<WorklightResult> RestInvokeAsync(string adapterName, string adapterProcedureName, string methodType);
```

**InvokeAsync**
```csharp
/// <summary>
/// Make a async call to a procedure from the MFP Server
/// </summary>
/// <param name="adapterName">The Adapter name</param>
/// <param name="adapterProcedureName">The Procedure name</param>
/// <returns>WorlightResult</returns>
Task<WorklightResult> InvokeAsync(string adapterName, string adapterProcedureName);
```

**SendActivityAsync**
```csharp
/// <summary>
/// Use this to send logs and other data to the MFP Server
/// </summary>
/// <param name="data">Data to be sent to the MFP server</param>
/// <returns>WorlightResult</returns>
Task<WorklightResult> SendActivityAsync(string data);
```

**SubscribeAsync**
```csharp
/// <summary>
/// Do a new Subscription on the Push Notification server
/// </summary>
/// <returns>WorlightResult</returns>
Task<WorklightResult> SubscribeAsync();
```

**UnSubscribeAsync**
```csharp
/// <summary>
/// Undo a existing subscription on the Push Notification server
/// </summary>
/// <returns></returns>
Task<WorklightResult> UnSubscribeAsync();
```


### **IMPORTANT**
Android:
YOU NEED TO INITIALIZE THE PLUGIN FROM YOUR MAIN ACTIVITY AS THAT:

```csharp
CrossMobileFirst.Current.Init(this);
```

iOS:
YOU NEED TO INITIALIZE THE PLUGIN FORM YOUR AppDelegate.cs AS THAT:

```csharp
CrossMobileFirst.Current.Init();
```

**DO NOT FORGET TO INSTALL THE IBM MOBILEFIRST SDK (LINK ABOVE) INTO YOUR CLIENTS PROJECTS**

#### CONFIGURATIONS OF MFP SERVER

You will have to configurate correctly the **Worklight Properties File**.
At **ANDROID** you will have to put at **Assets** Directory under the name **wlclient.properties**.
At **iOS** you will have to put at the **ROOT Directory of your Application** under the name **worklight.plist** 


#### Contributors
* [jbravobr](https://github.com/jbravobr)

Thanks!

#### License
Licensed under MIT see License file.
