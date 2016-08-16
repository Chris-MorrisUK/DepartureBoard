using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DarwinFeed.Darwin;

namespace DarwinFeed
{
    public class DarwinClient
    {
        public static DarwinDepartureBoard GetGetEntireBoard(string crs, int offset, int window)
        {
            Darwin.LDBServiceSoapClient client = new LDBServiceSoapClient();
            StationBoardWithDetails board = client.GetDepBoardWithDetails(DarwinToken.TheAccessToken, 0, crs, string.Empty, FilterType.from, offset, window);
            return DarwinDepartureBoardFactory.CreateDepartureBoard(board);
        }
    }
}
