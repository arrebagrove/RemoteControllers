using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.WindowsAzure.MobileServices;



namespace RemoteControls
{
    public sealed partial class MainPage : Page
    {
        string CurrentUnit = "TV";
        string CurrentKey = "1";
        int CurrentX = 0;
        int CurrentY = 0;

        private async void Button_Click(object sender, RoutedEventArgs e)
        {

            if (UseSerial)
            { 
                if (Globals.SerialTerminalPage == null)
                {
                    this.Frame.Navigate(typeof(SerialTerminalPage), null);
                    return;
                }
            }
            else
            {
                if (Globals.BluetoothSerialTerminalPage2 == null)
                {
                    this.Frame.Navigate(typeof(BluetoothSerialTerminalPage2), null);
                    return;
                }
            }

            Button button = (Button)sender;
            if (button != null)
            {
                ShowProgress(true);
                Point coords = (Point)button.Tag;
                //System.Diagnostics.Debug.WriteLine(command);
                //Paste your button switch statement here from Debug output
                IMobileServiceTable<RemoteControl> telemetryTable = App.MobileService.GetTable<RemoteControl>();
                string sCoords = coords.ToPt();
                string cmd = ((string)button.Content).Replace(" ", "_");
                //string val;
                //string lcdMsg;

                iCol = coords.Col;
                iRow = coords.Row;
                CurrentX = iCol;
                CurrentY = iRow;

                    

                //double value;
                try
                {
                    switch (sCoords)
                    {
                        // Mode and Power
                        case "(1,0)":
                            CurrentUnit = "TV";
                            ShowProgress(false);
                            return;
                            break;
                        case "(2,0)":
                            CurrentUnit = "DVD";
                            ShowProgress(false);
                            return;
                            break;
                        case "(3,0)":
                            CurrentUnit = "PVR";
                            ShowProgress(false);
                            return;
                            break;
                            //await Globals.RemoteControlAPI.Insert("DVD", "REC", 2, 3, 20, "RAW", 137, 1, 3);
                            //System.Diagnostics.Debug.WriteLine("Mode/Pwr");
                            //return;
                        case "(3,1)":
                            //await Globals.RemoteControlAPI.Insert("DVD", "STOP", 2, 3, 20, "RAW", 137, 1,3);
                            //System.Diagnostics.Debug.WriteLine("Mode/Pwr");
                            //return;
                            break;
                        // Number pad
                        case "(1,1)":
                        case "(2,1)":
                        case "(0,1)":
                        case "(1,2)":
                        case "(2,2)":
                        case "(0,2)":
                        case "(1,3)":
                        case "(2,3)":
                        case "(0,3)":
                        case "(1,4)":
                            //navButton_Click(sender, e);
                            System.Diagnostics.Debug.WriteLine("Num");
                            //return;
                            break;
                        //Channel up and down
                        case "(3,2)":
                        case "(3,3)":
                            System.Diagnostics.Debug.WriteLine("Ch");
                            //return;
                            break;
                        //Mute Vol up and down
                        case "(3,4)":
                        case "(3,5)":
                        case "(3,6)":
                            System.Diagnostics.Debug.WriteLine("Vol");
                            //return;
                            break;
                        //Nav buttons incl OK/Enter
                        case "(1,5)":
                        case "(0,6)":
                        case "(1,6)":
                        case "(2,6)":
                        case "(1,7)":
                            System.Diagnostics.Debug.WriteLine("Nav");
                            //return;
                            break;
                        //Misc Nav buttons 
                        case "(2,5)":
                        case "(0,7)":
                        case "(2,7)":
                            System.Diagnostics.Debug.WriteLine("Misc Nav");
                            //return;
                            break;                        // Player
                        case "(0,8)":
                        case "(1,8)":
                        case "(2,8)":
                        case "(0,9)":
                        case "(1,9)":
                        case "(2,9)":
                        case "(3,9)":
                        case "(1,10)":
                        case "(2,10)":
                            //navButton_Click(sender, e);
                            System.Diagnostics.Debug.WriteLine("Player");
                            //return;
                            break;
                        //Misc
                        case "(0,4)":
                        case "(0,5)":
                        case "(0,10)":
                        case "(2,4)":
                            System.Diagnostics.Debug.WriteLine("Misc");
                            //return;
                            break;
                        //Spare
                        case "(3,7)":
                        case "(3,8)":
                            System.Diagnostics.Debug.WriteLine("Spare");
                            //return;
                            break;
                        case "(3,10)":
                            navButton_Click(sender, e);
                            break;
                        //navButton_Click(sender, e);;
                        /*   //case "(4,0)":
                           //    try
                           //    {
                           //        Globals.SerialTerminalPage.Send("~C");
                           //        //Globals.STP.Send("0123456789a123456789b123456789c123456789");
                           //        string str = "~" + ArduinoLCDDisplay.LCD.CMD_HOME_1_CH.ToString();
                           //        str += "Read: A B C D   ";

                           //        str += "~" + ArduinoLCDDisplay.LCD.CMD_HOME_2_CH.ToString();


                           //        str+= "DT BT HUMID PRES";

                           //        str += "~" + ArduinoLCDDisplay.LCD.CMD_HOME_1_CH;
                           //        str += "~" + ArduinoLCDDisplay.LCD.CMD_BLINK_CH;
                           //        for (int i = 0; i < 9; i++)
                           //        {
                           //            str += "~" + ArduinoLCDDisplay.LCD.CMD_CURSORRIGHT_CH;
                           //        }
                           //        Globals.SerialTerminalPage.Send(str);
                           //    }
                           //    catch (Exception ex)
                           //    {
                           //    }
                           //    break;
                           // Read sensors
                           case "(1,1)":
                           case "(2,1)":
                           case "(3,1)":
                           case "(4,1)":
                               System.Diagnostics.Debug.WriteLine(sCoords);
                               if (UseSerial)
                                   Globals.SerialTerminalPage.Send(coords.Col - 1);
                               else
                                   Globals.BluetoothSerialTerminalPage.Send(coords.Col - 1);
                               break;
                           // Post to Azure
                           case "(1,3)":
                           case "(2,3)":
                           case "(3,3)":
                           case "(4,3)":
                               val = TextBoxes["textBox" + cmd].Text;
                               if (!double.TryParse(val, out value))
                                   return;
                               await Telemetry.Post(cmd, value);
                               lcdMsg = "~C" + Commands.Sensors[iCol - 1];
                               lcdMsg += "~" + ArduinoLCDDisplay.LCD.CMD_DISPLAY_LINE_2_CH + Commands.CommandActions[iRow] + " Done" + Globals.EndBlanks;
                               if (UseSerial)
                                   Globals.SerialTerminalPage.Send(lcdMsg); 
                               else
                                   Globals.BluetoothSerialTerminalPage.Send(lcdMsg);
                               break;
                           // Get from Azure
                           case "(1,4)":
                           case "(2,4)":
                           case "(3,4)":
                           case "(4,4)":
                               value = await Telemetry.RefreshTelemetryItemValue(cmd);
                               string valueStr = value.ToString();
                               //The database returns -1 if the record isn't found
                               if (value == -1)
                                   valueStr = "Failed";
                               TextBoxes["textBox" + cmd].Text = valueStr;
                               lcdMsg = "~C" + Commands.Sensors[iCol - 1];
                               lcdMsg += "~" + ArduinoLCDDisplay.LCD.CMD_DISPLAY_LINE_2_CH + Commands.CommandActions[iRow] + " " + valueStr + Globals.EndBlanks;
                               if (UseSerial)
                                   Globals.SerialTerminalPage.Send(lcdMsg);
                               else
                                   Globals.BluetoothSerialTerminalPage.Send(lcdMsg);
                               break;
                           // Get full history for sensor
                           case "(1,5)":
                           case "(2,5)":
                           case "(3,5)":
                           case "(4,5)":
                               this.Frame.Navigate(typeof(ListTelemetryPage), coords.Col - 1);
                               break;
                           // Update current sensor value in Azure
                           case "(1,6)":
                           case "(2,6)":
                           case "(3,6)":
                           case "(4,6)":
                               val = TextBoxes["textBox" + cmd].Text;
                               if (!double.TryParse(val, out value))
                                   return;
                               lcdMsg = "~C" + Commands.Sensors[iCol - 1];
                               lcdMsg += "~" + ArduinoLCDDisplay.LCD.CMD_DISPLAY_LINE_2_CH + Commands.CommandActions[iRow];
                               if (await Telemetry.UpdateTelemetryItem(cmd, value))
                               {
                                   lcdMsg += " Done"+Globals.EndBlanks;
                                   if (UseSerial)
                                       Globals.SerialTerminalPage.Send(lcdMsg);
                                   else
                                       Globals.BluetoothSerialTerminalPage.Send(lcdMsg);
                               }
                               else
                               {
                                   lcdMsg += " Failed"+Globals.EndBlanks;
                                   if (UseSerial)
                                       Globals.SerialTerminalPage.Send(lcdMsg);
                                   else
                                       Globals.BluetoothSerialTerminalPage.Send(lcdMsg);
                               }
                               break;
                           // Clear current sensor value from Azure
                           case "(1,7)":
                           case "(2,7)":
                           case "(3,7)":
                           case "(4,7)":
                               lcdMsg = "~C" + Commands.Sensors[iCol - 1];
                               lcdMsg += "~" + ArduinoLCDDisplay.LCD.CMD_DISPLAY_LINE_2_CH + Commands.CommandActions[iRow];
                               if (  await Telemetry.DeleteTelemetryItem(cmd))
                               {
                                   lcdMsg += " Done" + Globals.EndBlanks;
                                   if (UseSerial)
                                       Globals.SerialTerminalPage.Send(lcdMsg);
                                   else
                                       Globals.BluetoothSerialTerminalPage.Send(lcdMsg);
                               }
                               else
                               {
                                   lcdMsg += " Failed" + Globals.EndBlanks;
                                   if (UseSerial)
                                       Globals.SerialTerminalPage.Send(lcdMsg);
                                   else
                                       Globals.BluetoothSerialTerminalPage.Send(lcdMsg);
                               }

                               break;
                           // Clear all sensor values from Azure
                           case "(1,8)":
                           case "(2,8)":
                           case "(3,8)":
                           case "(4,8)":
                               TextBoxes["textBox" + cmd].Text = "Todo";
                               //this.Frame.Navigate(typeof(ListTelemetryPage), coords.Col - 1);
                               break;*/


                        default:
                            System.Diagnostics.Debug.WriteLine("Command: {0} not found", coords);
                            break;
                    }
                    CurrentKey = (string)button.Content;
                    //ShowProgress(false);
                }
                catch (Exception ex)
                {
                    ShowProgress(false);
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
        }
        private void navButton_Click(object sender, RoutedEventArgs e)
        {
            //Frame.GoBack();
            Button button = (Button)sender;
            string buttonContent = (string)button.Content;
            if (buttonContent == "Setup Serial")
                this.Frame.Navigate(typeof(SerialTerminalPage), null);
            else if (buttonContent == "Setup Serial")
                this.Frame.Navigate(typeof(SerialTerminalPage), null);
            else if (buttonContent == "Setup BT")
                this.Frame.Navigate(typeof(BluetoothSerialTerminalPage), null);
            else if (buttonContent == "Show sensor list")
            {
                this.Frame.Navigate(typeof(ListRemoteControlsPage), "Current");
            }
            else if (buttonContent == "Back to sensor list")
            {
                this.Frame.Navigate(typeof(ListRemoteControlsPage), "");
            }
        }
    }
}
