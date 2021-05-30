using UnityEngine;
using UnityEngine.UI;
using Unity.Notifications.Android;

public class MobileNotificationManager : Singleton<MobileNotificationManager>
{
#if UNITY_ANDROID
    public void SendNotificationAndroid()
    {
        var defaultNotificationChannel = new AndroidNotificationChannel()
        {
            Id = "channel_id",
            Name = "Prompt Channel",
            Importance = Importance.Default,
            Description = "Reminder notifications",
        };

        AndroidNotificationCenter.RegisterNotificationChannel(defaultNotificationChannel);

        var notification = new AndroidNotification();
        notification.Title = "找到古生物";
        notification.Text = "有古生物正在附近，趕快尋找看看吧！";
        notification.LargeIcon = "channel_icon";
        notification.FireTime = System.DateTime.Now.AddSeconds(1);

        var identifier = AndroidNotificationCenter.SendNotification(notification, "channel_id");

        if (AndroidNotificationCenter.CheckScheduledNotificationStatus(identifier) == NotificationStatus.Scheduled)
        {
            // Replace the currently scheduled notification with a new notification.
            AndroidNotificationCenter.UpdateScheduledNotification(identifier, notification, "channel_id");
        }
        else if (AndroidNotificationCenter.CheckScheduledNotificationStatus(identifier) == NotificationStatus.Delivered)
        {
            //Remove the notification from the status bar
            AndroidNotificationCenter.CancelNotification(identifier);
        }
        else if (AndroidNotificationCenter.CheckScheduledNotificationStatus(identifier) == NotificationStatus.Unknown)
        {
            AndroidNotificationCenter.SendNotification(notification, "channel_id");
        }
    }
#endif
    /*
    void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            SendNotificationAndroid();
        }
    }
    */
}
