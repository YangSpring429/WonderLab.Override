using Avalonia.Controls.Notifications;
using System;

namespace WonderLab.Classes.Models.Messaging;

public record NotificationMessage(string Text, NotificationType NotificationType, Action OnClick = default, Action OnClose = default);