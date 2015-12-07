using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Linq;

// To add offline sync support, add the NuGet package Microsoft.WindowsAzure.MobileServices.SQLiteStore
// to your project. Then, uncomment the lines marked // offline sync
// For more information, see: http://aka.ms/addofflinesync
//using Microsoft.WindowsAzure.MobileServices.SQLiteStore;  // offline sync
//using Microsoft.WindowsAzure.MobileServices.Sync;         // offline sync

namespace RemoteControls
{

        public class RemoteControlAPI
        {
            //Note: To change the table from the Azure Mobile Service (ie Same service and therefore same app key) ..
            // Just change the class name in the file using Refactoring
            private MobileServiceCollection<RemoteControl, RemoteControl> remoteControlItems;
            private IMobileServiceTable<RemoteControl> remoteControlTable = App.MobileService.GetTable<RemoteControl>();

            // This page needs to update the sensor lists, or at least, update Item Sources for lists there
            private static ListRemoteControlsPage listRemoteControlsPage;
            public RemoteControlAPI()
            {
                Globals.RemoteControlAPI = this;
            }

            public static void SetListTelemetryPage(ListRemoteControlsPage ListTelemetryPage)
            {
                listRemoteControlsPage = ListTelemetryPage;
            }


        #region INSERT
        ////Clean up of Sensor names.
        //private string Normalise(string strn)
        //{
        //    string strnTemp = "";
        //    if (strn != "")
        //    {
        //        strnTemp = strn.Substring(1).ToLower();
        //        strnTemp = strn.ToUpper()[0].ToString() + strnTemp;
        //    }
        //    // Replace spaces (should be any at this level though)
        //    strnTemp.Replace(" ", "_");
        //    return strnTemp;
        //}

        public async Task InsertRemoteControlItem(RemoteControl remoteControlItem)
        {
            await remoteControlTable.InsertAsync(remoteControlItem);
            //await SyncAsync(); // offline sync
        }
        public async Task Post(string unit, string Text, int indexY, int indexX, int bits, string raw, UInt16 valueHi, UInt16 valueLo, int decode_type )
        {
            var remoteControlItem = new RemoteControl {
                Unit = unit,
                Decode_Type = decode_type,
                IndexX = indexX,
                IndexY = indexY,
                Bits = bits,
                Raw = raw,
                Text=Text,
                ValueHi = valueHi,
                ValueLo = valueLo,
                Repeat = false
            };
            await Post(remoteControlItem);
        }

        public async Task Post(RemoteControl item)
        {
           await InsertRemoteControlItem(item);
        }
        #endregion

        #region GET_RECORD/S

        /// <summary>
        /// Get current entry in table for sensor
        /// </summary>
        /// <param name="sensor">Name of the the sensor</param>
        /// <returns></returns>
        public async Task<RemoteControl> GetTelemetryItem(string unit, int  indexX, int indexY)
        {
            var remoteControlItem = await remoteControlTable
            .Where(
                (s => ( (s.Unit== unit) &&(s.IndexX == indexX) && (s.IndexY == indexY)))
                )
            .ToCollectionAsync();

            if (remoteControlItem.Count() != 1)
                return null;
            RemoteControl item = (RemoteControl)remoteControlItem.First();
                return item;
        }

        public async Task<RemoteControl> GetTelemetryItem(string unit, string text)
        {
            var remoteControlItem = await remoteControlTable
            .Where(
                (s =>( (s.Unit== unit) && (s.Text == text)))
                )
            .ToCollectionAsync();

            if (remoteControlItem.Count() != 1)
                return null;
            RemoteControl item = (RemoteControl)remoteControlItem.First();
            return item;
        }
        #endregion

        void RemoteControlCopy(RemoteControl inRC, ref RemoteControl outRC)
        {
            outRC.Id = outRC.Id; //Unchanged
            outRC.Unit = inRC.Unit;
            outRC.IndexX = inRC.IndexX;
            outRC.IndexY = inRC.IndexY;
            outRC.Overflow = inRC.Overflow;
            outRC.Raw = inRC.Raw;
            outRC.Repeat = inRC.Repeat;
            outRC.Text = outRC.Text;
            outRC.ValueHi = inRC.ValueHi;
            outRC.ValueLo = inRC.ValueLo;
        }

        void RemoteControlCopy2( ref RemoteControl outRC, string unit, string text, int indexY, int indexX, int bits, string raw, UInt16 valueHi, UInt16 valueLo, int decode_type)
        {
            outRC.Id = outRC.Id; //Unchanged
            outRC.Unit = unit;
            outRC.IndexX = indexX;
            outRC.IndexY = indexY;
            //outRC.Overflow = overflow;
            outRC.Raw = raw;
            //outRC.Repeat = repeat;
            outRC.Text = text;
            outRC.ValueHi = valueHi;
            outRC.ValueLo = valueLo;
        }

        #region UPDATE
        //public async Task<bool> InsertTelemetryItem(RemoteControl item)
        public async Task<bool> Insert(string unit, string text, int indexY, int indexX, int bits, string raw, UInt16 valueHi, UInt16 valueLo, int decode_type)
        {
            // This code takes a freshly completed TodoItem and updates the database. When the MobileService 
            // responds, the item is removed from the list 
            RemoteControl item = await GetTelemetryItem(unit, indexX, indexY);
            if (item == null)
            {
                await Post(unit, text, indexY, indexX, bits, raw, valueHi, valueLo, decode_type);
                return true;
            }
            RemoteControlCopy2( ref item, unit, text, indexY, indexX, bits, raw, valueHi, valueLo, decode_type);
            await UpdateTelemetryItem(item);
            return true;
        }

        public async Task UpdateTelemetryItem(RemoteControl item)
        {
            if (item != null)
            {
                RemoteControl _item = await GetTelemetryItem(item.Unit, item.IndexX,item.IndexY);
                if (_item == null)
                    return;
                RemoteControlCopy(_item, ref item);
                await remoteControlTable.UpdateAsync(item);
            }
        }

        #endregion

        #region DELETE
        public  async Task<bool> DeleteTelemetryItem(string unit, string text)
        {
            RemoteControl item = await GetTelemetryItem(unit,text);
            if (item == null)
                return false;
            await DeleteTelemetryItem(item);
            return true;
        }

        public async Task<bool> DeleteTelemetryItem(string unit, int indexX, int indexY)
        {
            RemoteControl item = await GetTelemetryItem(unit, indexX, indexY);
            if (item == null)
                return false;
            await DeleteTelemetryItem(item);
            return true;
        }

        public async Task DeleteTelemetryItem(RemoteControl item)
        {
            await remoteControlTable.DeleteAsync(item);
        }

        public async Task ClearTelemetryItems(string unit)
        {
            var telemetryItems = await remoteControlTable
           .Where(
               (s => (s.Unit == unit))
               )
           .ToCollectionAsync();
            foreach (RemoteControl item in telemetryItems)
            {
                await DeleteTelemetryItem(item);
            }
        }
        #endregion

        #region REFRESH_DISPLAY_LIST

        public async Task<string> RefreshRemoteControlItemValue(string unit)
        {
            string value = "";
            MobileServiceInvalidOperationException exception = null;
            try
            {
                // This code refreshes the entries in the list view by querying the telemetry2 table.
                remoteControlItems = await remoteControlTable
                .Where(
                    (s => (s.Unit == unit))  )                  
                .ToCollectionAsync();
            }
            catch (MobileServiceInvalidOperationException e)
            {
                exception = e;
            }

            if (exception != null)
            {
                await new MessageDialog(exception.Message, "Error loading items").ShowAsync();
            }
            else
            {
                //Sort in descending order = Most recent first .. Ww want the latest value of each item
                if (remoteControlItems != null)
                {
                    if (remoteControlItems.Count() != 0)
                        value = (string)remoteControlItems.First().Text;
                }
            }
            return value;
        }

        public async Task RefreshTelemetryHistory(string unit)
        {
            var remoteControlItems = await remoteControlTable
               .Where(
                   (s => (s.Unit == unit))
                   )
               .ToCollectionAsync();

            if (listRemoteControlsPage != null)
                //Set as displayed list
                listRemoteControlsPage.ListItems.ItemsSource = remoteControlItems.OrderBy(s => (s.IndexX + s.IndexY*4)).Reverse();

        }

        #endregion

        #region REFRESH_TABLE_SO_ONLY_LAST_ENTRIES_ARE_CURRENT
        public async Task RefreshTelemetryItems()
        {
            MobileServiceInvalidOperationException exception = null;

            try
            {
                // This code refreshes the entries in the list view by querying the telemetry2 table.
                remoteControlItems = await remoteControlTable
                //.Where
                //(s => (s.Complete == false))
                .ToCollectionAsync();
            }
            catch (MobileServiceInvalidOperationException e)
            {
                exception = e;
            }

            if (exception != null)
            {
                await new MessageDialog(exception.Message, "Error loading items").ShowAsync();
            }
            else
            {
                //Sort in descending order = Most recent first .. Ww want the latest value of each sensor
                var sortedTelemetryItems = remoteControlItems
                    .OrderByDescending(telemetryItem => telemetryItem.Id);

                //Group by unit name
                var orderGroups =
                    from p in sortedTelemetryItems
                    group p by p.Unit into g
                    select new { Category = g.Key, Units = g };

                //Get only the first item in each group = Most Recent
                List<RemoteControl> remoteControlsListMostRecentInGroup = new List<RemoteControl>();
                foreach (var key in orderGroups)
                {

                    remoteControlsListMostRecentInGroup.Add(key.Units.First<RemoteControl>());
                    //Mark other vakues as complete. Embedded devices then can just get the "incomplete values".
                    for (int i = 1; i < key.Units.Count(); i++)
                    {
                        RemoteControl t2i = key.Units.ElementAt<RemoteControl>(i);
                        //t2i.Complete = true;
                        await remoteControlTable.UpdateAsync(t2i);
                    }
                }

                if (listRemoteControlsPage != null)
                    //Set as displayed list
                    listRemoteControlsPage.ListItems.ItemsSource = remoteControlsListMostRecentInGroup.OrderBy(s => s.Id);//.Reverse(); // telemetryItems;
                                                                                                                  //this.ButtonSave.IsEnabled = true;
            }
        }
        public async Task Refresh()
            {
                //await SyncAsync(); // offline sync
                await RefreshTelemetryItems();
            }
        #endregion

    }
}

