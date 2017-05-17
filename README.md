![YoApp Logo](https://github.com/rubit0/YoApp/blob/master/assets/App%20Icons/export/splash_icon@2x.png?raw=true)

# YoApp
YoApp is a WhatsApp style messaging suite that covers the whole stack from back-end to mobile-apps, batteries included!  
Learn more from the [Wiki Pages](https://github.com/rubit0/YoApp/wiki).

## Technology stack
YoApp is primarily powered by microsoft open source technologies.

### Backend
* [.NET Core 1.1](https://github.com/dotnet/core)
* [ASP.NET Core](https://github.com/aspnet/Home)
* [EntityFramework Core](https://github.com/aspnet/EntityFramework)
* [OpenIddict](https://github.com/openiddict)
* [SignalR2](https://github.com/SignalR/SignalR)

### Mobile
* [Xamarin Forms](https://github.com/xamarin/Xamarin.Forms)
* [Akavache](https://github.com/akavache)
* [Realm](https://github.com/realm)
* [Google libphonenumbers](https://github.com/googlei18n/libphonenumber)
* [SimpleFormsAuth](https://github.com/rubit0/SimpleFormsAuth)-

## Restoring this Project

### Ending with newer nuget packages
The Chat project is dependent on SignalR Core which has many dependencies to other preview libraries that are already obsolete.  
This means that those libraries aren't anymore available on the preview asp-net contribution nuget feed and you will get warnings about fetching newer packages.  
So far this hasn't turn out to be a problem, but there is always a chance of an breaking API change.

As a temporary fix, I plan to add the obsolete nugets as dlls directly into the project (libs folder).  
SignalR Core is expected to be final around 3Q 2017.  

### Some Projects may not be found
Please make sure to build every single project, clean solution and then restart Visual Studio.