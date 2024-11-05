using UnityEngine;
using System;

#if UNITY_ANDROID
using UnityEngine.Android;
using Unity.Notifications.Android;
#endif

public class NotificationManager : MonoBehaviour
{
    [SerializeField] GameState gameState;
    [SerializeField] AndroidNotifications androidNotifications;


    private void Start()
    {
        androidNotifications.RequestAuthorization();
        androidNotifications.RegisterNotificationChannels();
    }

    private void OnApplicationFocus(bool focus)
    {
        if(focus)
        {
            AndroidNotificationCenter.CancelAllNotifications();
        }
       else
        {
           //mission Complete notifications
            foreach (Mission mission in gameState.activeMissions)
            {
                androidNotifications.SendNotification("Mission Complete!", $"A team has completed the mission to {mission.Name}!", mission.EndTime);
            }

            //store restock notifications
            DateTime time = gameState.storeLastStocked + gameState.restockTime;
            androidNotifications.SendNotification("Store Restocked!", "The store has new items!", time);
        }

    }
}

