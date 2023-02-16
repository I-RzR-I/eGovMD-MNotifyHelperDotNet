> :point_up: From the beginning let's make clear one moment. It is a wrapper for notification service provided by [e-governance agency](https://egov.md/). If you accessed this repository and already installed it or you want or install the package you already contacted the agency and discussed that and obtained the necessary information for clean implementation. :muscle:

<br/>

In curret wrapper are available a few methods like:
* `SendSimpleNotification/Async` -> Send simple notification;
* `Send[Text|Html]NotificationByEmail/Async`;
* `Send[Text|Html]NotificationRequestByEmail/Async`;
* `GetNotificationStatus/Async`;
* `Ping` -> Check if the notification service is alive and if you can use it, available from `MNotifyClientInternal.Instance.Ping()`.

For more details, please check the documentation obtained from the responsible authorities where you can find all the smallest details necessary for the implementation and understanding of the working flow.
<br/>
In dependece of what type of project you have, in you configuration file please provide available configurable parameters.<br/>

<hr/>

**Configure the application settings file**

In case you use `netstandard2.0`, `netstandard2.1`, `net5`, `netcoreapp3.1` in your project find a settings file like `appsettings.json` or `appsettings.env.json` and complete it with the following parameters.
```json
  "RemoteMNotifyOptions": {
    "ServiceClientAddress": "notifications service",
    "ServiceCertificatePath": "service certificate",
    "ServiceCertificatePassword": "password for service certificate"
  }
```

If you have the `app/web.config` file (must common for .net framework projects)
```xml
  <configSections>
    <section name="MNotifyConfigurationSection"
             type="MNotifyHelperDotNet.Helpers.ConfSection.MNotifyConfigurationSection,MNotifyHelperDotNet" />
  </configSections>

  <MNotifyConfigurationSection>
    <RemoteMNotifyOptions>
      <option key="ServiceClientAddress" value="notifications service" />
      <option key="ServiceCertificatePath" value="service certificate" />
      <option key="ServiceCertificatePassword" value="password for service certificate" />
    </RemoteMNotifyOptions>
  </MNotifyConfigurationSection>
```

* `RemoteServiceClientAddress` -> supply with URL to notification service like '<b>https://host.domain/Service.svc</b>';
* `ServiceCertificatePath` -> provide service certificate, the path with filename (PFX certificate);
* `ServiceCertificatePassword` -> provide the password to service certificate specified upper.

<hr/>

**Calling the service**

In case of using the `netstandard2.0+` in your project, after adding configuration data, you must set dependency injection for using functionality. In your project in the file `Startup.cs` add the following part of the code:
```csharp
public void ConfigureServices(IServiceCollection services)
        {
            ...
            
            services.AddNotifyService(Configuration);
            
            ...
        }
```

In the service that will be implemented, inject internal service (`IMNotifyService`).
```csharp
public class Notification
    {
        private readonly IMNotifyService _mNotifyService;

        public Notification(IMNotifyService mNotifyService)
        {
            _mNotifyService = mNotifyService;
        }
        
        ...
    }
```

**NetFramework** <br/>
For invoking service from .net framework, the request is almost the same with a small difference, call an instance of the class(`MNotifyService.Instance`).

Available languages: "ro", "ru", "en". <br/>

For more information about the fields and how to complete them, you can find in integration guide obtained from the service provider.




