﻿using HotelsSystem.Pages.Configs;

namespace HotelsSystem.BL;

public class ClS_Reports
{
    private ISqlDataAccess _db { get; set; }

    int SessionValue = 0;

    public ClS_Reports(ISqlDataAccess db, SPResult session)
    {
        SessionValue = session.Result;
        _db = db;
    }
    public async Task<PagedResult<T>> SearchList<T>(int SelectPro = 1, int PageNumber = 1, int PageSize = 10,string FullName="",string Mobile="",string MotherName="", int  DirectorateID=0,int WorkPlaceID=0,int HotelID=0,  int RoomID = 0, int GenderID = 0,  int NationalityID = 0, string FromCheckInDate = "", string ToCheckInDate = "",string FromCheckOutDate="",string ToCheckOutDate="", string SortColumn = "", string SortDirection = "Asc")
    {
        return await _db.GetGridResult<T, dynamic>("Pro_SearchList", new { Select = SelectPro, FullName= FullName, Mobile= Mobile, MotherName= MotherName, DirectorateID = DirectorateID , WorkPlaceID= WorkPlaceID, HotelID= HotelID, RoomID = RoomID, GenderID = GenderID, NationalityID = NationalityID, FromCheckInDate = FromCheckInDate, ToCheckInDate = ToCheckInDate, FromCheckOutDate= FromCheckOutDate, ToCheckOutDate= ToCheckOutDate, PageNumber = PageNumber, PageSize = PageSize, SortColumn = SortColumn, SortDirection = SortDirection, EntryBy = SessionValue }, PageNumber: PageNumber, PageSize: PageSize);
    }


}
