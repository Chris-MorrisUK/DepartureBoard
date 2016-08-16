using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DarwinFeed.Darwin;
using System.Diagnostics;

namespace DarwinFeed
{
    public class DarwinClient
    {
        public static DarwinDepartureBoard GetGetEntireBoard(string crs, int offset, int window)
        {
            try
            {
                Darwin.LDBServiceSoapClient client = new LDBServiceSoapClient();
                StationBoardWithDetails board = client.GetDepBoardWithDetails(DarwinToken.TheAccessToken, 0, crs, string.Empty, FilterType.from, offset, window);
                return DarwinDepartureBoardFactory.CreateDepartureBoard(board);
            }
            catch (Exception ex)
            {
                Logging.LogError(ex, "Whilst getting Departure Board", 1);
                return null;
            }
            
        }
    }
}
