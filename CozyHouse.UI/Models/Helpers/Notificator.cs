using Microsoft.AspNetCore.Mvc;

namespace CozyHouse.UI.Models.Helpers
{
    public static class Notificator
    {
        public static void CreateNotification(Controller controller, string message, string type)
        {
            controller.TempData["NotificationMessage"] = message;
            controller.TempData["NotificationValue"] = type;
        }
    }
}
