using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using Avalonia.Xaml.Interactivity;
using MinecraftLaunch.Classes.Enums;
using MinecraftLaunch.Classes.Models.Auth;
using MinecraftLaunch.Components.Authenticator;
using MinecraftLaunch.Skin;
using MinecraftLaunch.Skin.Class.Fetchers;
using System;
using System.Threading.Tasks;
using WonderLab.Extensions;
using WonderLab.Infrastructure.Enums;

namespace WonderLab.Controls.Media.Behaviors;

public sealed class AccountSkinLoadBehavior : Behavior<Border> {
    public static readonly StyledProperty<SkinArea> SkinAreaProperty =
        AvaloniaProperty.Register<AccountSkinLoadBehavior, SkinArea>(nameof(SkinArea), SkinArea.Head);

    public static readonly StyledProperty<Account> AccountProperty =
        AvaloniaProperty.Register<AccountSkinLoadBehavior, Account>(nameof(Account));

    public SkinArea SkinArea {
        get => GetValue(SkinAreaProperty);
        set => SetValue(SkinAreaProperty, value);
    }

    public Account Account {
        get => GetValue(AccountProperty);
        set => SetValue(AccountProperty, value);
    }

    private async Task<byte[]> GetSkinAsync(Account account) {
        var skin = await Task.Run(async () => {
            return account.Type switch {
                AccountType.Microsoft => await new MicrosoftSkinFetcher(account.Uuid.ToString()).GetSkinAsync(),
                AccountType.Yggdrasil => await new YggdrasilSkinFetcher((account as YggdrasilAccount).YggdrasilServerUrl, account.Uuid.ToString()).GetSkinAsync(),
                AccountType.Offline => "resm:WonderLab.Assets.Image.steve.png".ToBytes(),
                _ => "resm:WonderLab.Assets.Image.steve.png".ToBytes()
            };
        });

        return skin;
    }

    private async void OnLoaded(object sender, RoutedEventArgs e) {
        if (Account is null) {
            return;
        }

        var skin = new SkinResolver(await GetSkinAsync(Account));
        await Dispatcher.UIThread.InvokeAsync(() => {
            AssociatedObject.Background = new ImageBrush(SkinArea switch {
                SkinArea.Head => skin.CropSkinHeadBitmap().ToBitmap(),
                SkinArea.Body => skin.CropSkinBodyBitmap().ToBitmap(),
                SkinArea.LeftHand => skin.CropLeftHandBitmap().ToBitmap(),
                SkinArea.RightHand => skin.CropRightHandBitmap().ToBitmap(),
                SkinArea.LeftLeg => skin.CropLeftLegBitmap().ToBitmap(),
                SkinArea.RightLeg => skin.CropRightLegBitmap().ToBitmap(),
                _ => skin.CropSkinHeadBitmap().ToBitmap()
            });
        }, DispatcherPriority.Background);
    }

    private void OnUnloaded(object sender, RoutedEventArgs e) {
    }

    protected override void OnAttached() {
        AssociatedObject.Loaded += OnLoaded;
        AssociatedObject.Unloaded += OnUnloaded;
    }
}