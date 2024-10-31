using UnityEngine;
using UnityEngine.Android;
using Unity.Notifications.Android;
using System;

public class AndroidNotifications : MonoBehaviour
{
    //Authorization request

    public void RequestAuthorization()
    {
        if (!Permission.HasUserAuthorizedPermission("android.permission.POST_NOTIFICATIONS"))
        {
            Permission.RequestUserPermission("android.permission.POST_NOTIFICATIONS");
        }
    }

    // create notification channel

    public void RegisterNotificationChannels()
    {
        var channel = new AndroidNotificationChannel
        {
            Id = "default_channel",
            Name = "Default Channel",
            Importance = Importance.Default,
            Description = "GuildForge Notifications"
        };


        AndroidNotificationCenter.RegisterNotificationChannel(channel);
    }

    // Setup notification Template

    public void SendNotification(string title, string text, DateTime time)
    {
        var notification = new AndroidNotification();
        notification.Title = title;
        notification.Text = text;
        notification.FireTime = time;
        notification.SmallIcon = "icon";
        notification.LargeIcon = "logo";

        AndroidNotificationCenter.SendNotification(notification, "default_channel");
    }
}
