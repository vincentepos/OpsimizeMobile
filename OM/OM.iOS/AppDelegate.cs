﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Firebase.CloudMessaging;
using Foundation;
using UIKit;
using UserNotifications;
using Firebase.Core;
using Xamarin.Essentials;
using Plugin.FirebasePushNotification;
using KeyboardOverlap.Forms.Plugin.iOSUnified;

namespace OM.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate, IUNUserNotificationCenterDelegate, IMessagingDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App());
            KeyboardOverlapRenderer.Init();

            //if (!App.Current.Properties.ContainsKey("license"))
            //{
            //    App.Current.Properties.Add("license", "");
            //}

            //FirebasePushNotificationManager.Initialize(options, true);

            Firebase.Core.App.Configure();

            Messaging.SharedInstance.Delegate = this;

            // Register your app for remote notifications.
            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {

                // For iOS 10 display notification (sent via APNS)
                UNUserNotificationCenter.Current.Delegate = this;

                var authOptions = UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound;
                UNUserNotificationCenter.Current.RequestAuthorization(authOptions, (granted, error) => {
                    Console.WriteLine(granted);
                });
            }
            else
            {
                // iOS 9 or before
                var allNotificationTypes = UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound;
                var settings = UIUserNotificationSettings.GetSettingsForTypes(allNotificationTypes, null);
                UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
            }

            UIApplication.SharedApplication.RegisterForRemoteNotifications();

            return base.FinishedLaunching(app, options);
        }

        [Export("messaging:didReceiveRegistrationToken:")]
        public void DidReceiveRegistrationToken(Messaging messaging, string fcmToken)
        {
            Console.WriteLine($"Firebase registration token: {fcmToken}");

            var token = Messaging.SharedInstance.FcmToken ?? "";
            Console.WriteLine($"FCM token: {token}");
            if (App.Current.Properties.ContainsKey("tkn"))
            {
                App.Current.Properties["tkn"] = token;
            }
            else
            {
                App.Current.Properties.Add("tkn", token);
            }

            // TODO: If necessary send token to application server.
            // Note: This callback is fired at each app startup and whenever a new token is generated.
        }

        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            Messaging.SharedInstance.ApnsToken = deviceToken;
            Console.WriteLine($"Firebase registration token: {deviceToken}");
            //if (deviceToken != null)
            //{
            //    //App.Current.Properties["tokenios"] = deviceToken;
            //    Preferences.Set("my_key", deviceToken.ToString());
            //}
            //else
            //{
            //    //App.Current.Properties["tokenios"] = "Test123";
            //    Preferences.Set("my_key", "Test123");
            //}

            //FirebasePushNotificationManager.DidRegisterRemoteNotifications(deviceToken);
            //Messaging.SharedInstance.ApnsToken = deviceToken;
            //Console.WriteLine($"Firebase registration token: {deviceToken}");
            //CrossFirebasePushNotification.Current.OnTokenRefresh += (s, p) =>
            //{
            //    System.Diagnostics.Debug.WriteLine($"TOKEN : {p.Token}");
            //    //App.Current.Properties["tkn"] = p.Token;
            //    //App.Current.Properties.Add("tkn", p.Token);

            //    //Preferences.Set("my_key", p.Token);
            //};

        }
    }
}
