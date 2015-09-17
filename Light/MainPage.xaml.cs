using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Capture;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Windows;
using Windows.Devices.Enumeration;
using Windows.UI.Core; 


namespace Light
{
    public sealed partial class MainPage : Page
    {
        private MediaCapture mcLight;

        public MainPage()
        {
            try
            {
                this.InitializeComponent();

                CoreWindow.GetForCurrentThread().Activated += CoreWindowOnActivated;
            }
            catch (Exception ex)
            {
                logException(ex);
            }
        }

        private void CoreWindowOnActivated(CoreWindow sender, WindowActivatedEventArgs args)
        {
            try
            {
                if (args.WindowActivationState == CoreWindowActivationState.Deactivated)
                {
                    LightOff();
                }
                else
                {
                    LightOn();
                }
            }
            catch (Exception ex)
            {
                logException(ex);
            }
        }

        private async void LightOn()
        {
            try
            {
                var devices = await Windows.Devices.Enumeration.DeviceInformation.FindAllAsync(DeviceClass.VideoCapture);
                var rearCamera = devices[0];
                
                if (devices.Count > 0)
                {
                    rearCamera = devices.Single(currDev =>
                      currDev.EnclosureLocation.Panel == Windows.Devices.Enumeration.Panel.Back
                    );
                }
                else
                {
                    return; 
                }

                mcLight = new MediaCapture();
                await mcLight.InitializeAsync(new MediaCaptureInitializationSettings() { VideoDeviceId = rearCamera.Id });

                if (mcLight.VideoDeviceController.TorchControl.Supported == true)
                {
                    var torch = mcLight.VideoDeviceController.TorchControl;

                    if (torch.Enabled == false)
                    {
                        torch.Enabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                logException(ex);
            }
        }

        private void LightOff()
        {
            try
            {
                mcLight.VideoDeviceController.TorchControl.Enabled = false;
                mcLight.Dispose();
            }
            catch (Exception ex)
            {
                logException(ex);    
            }
        }

        private void logException(Exception ex)
        {
            //All errors, if any are just ignored.
        }
    }
}
