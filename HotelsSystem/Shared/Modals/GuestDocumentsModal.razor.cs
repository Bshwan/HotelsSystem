
using Blazored.Modal.Services;
using static MudBlazor.CategoryTypes;

namespace HotelsSystem.Shared.Modals;

public partial class GuestDocumentsModal 
{
    [Parameter]
    public ClS_Config config { get; set; } = default!;
    [Parameter]
    public int GuestID { get; set; }
    //[Parameter]
    //public ClS_Hotels hotel { get; set; } = default!;
    //[Parameter]
    //public ClS_Guests guest { get; set; } = default!;
    //[Parameter]
    //public HotelRoomsInfo HotelDetail { get; set; } = default!;
    [Inject]
    protected IToaster Toaster { get; set; } = default!;
    [Inject]
    protected IJSRuntime jSRuntime { get; set; } = default!;
    [Inject]
    protected NavigationManager nav { get; set; } = default!;
    [CascadingParameter]
    MudDialogInstance BlazoredModal { get; set; } = default!;
    [Inject]
    protected IDialogService DialogService { get; set; } = default!;


    //GuestDetailsInfo SelectedGuest = new GuestDetailsInfo() { GD_Gender = 1, };
    //GuestDetailsCombos combos = new GuestDetailsCombos();
    //IEnumerable<GuestDetailsInfo> GuestDetails = Enumerable.Empty<GuestDetailsInfo>();

    //public string InputFileMessage = "";
    //byte[]? SelectedDoc = null;
    //int DocType = 0;
    IEnumerable<AttachmentInfo> Attachments = Enumerable.Empty<AttachmentInfo>();

    //int SelectedRoomToChange = 0;
    //string SelectedRoomToChangeText = "";

    //enum Steps { list, edit, doc }
    //Steps CurrentStep = Steps.list;

    private async Task Close() => BlazoredModal.Close();
    private async Task Cancel() => BlazoredModal.Cancel();
    //private DotNetObjectReference<AddGuestModal>? _objRef;

    protected override async Task OnInitializedAsync()
    {
        await GetAllAttachments();
        //InputFileMessage = L["select-file"];
        //await GetRoomGuests();
        //await GetCombos();

        //if (HotelDetail.GM_Room <= 0)
            //HotelDetail.GM_Room = HotelDetail.htr_ID;
        // HotelDetail.GM_ID=HotelDetail.htr_ID;
        //await GetRoomByID();
        //_objRef = DotNetObjectReference.Create(this);
        //await jSRuntime.InvokeAsync<string>("SetDotNetHelper", _objRef);

        // await GetAllAttachments();
        // await GetRooms();
    }
    void ShowDocumentPreviewModal(int ID)
    {
        var options = new DialogOptions
        {
            CloseOnEscapeKey = true,
            CloseButton = true,
            Position = DialogPosition.TopCenter,
            MaxWidth = MaxWidth.Large,
            FullScreen=true,
        };
        var parameters = new DialogParameters();
        parameters.Add("URL", nav.BaseUri + $"main/GetAttachment?id=" + ID);
        DialogService.Show<DocumentViewModal>("", parameters, options);
        //var res = await modal.Result;
        // StateHasChanged();
    }
    //async Task GetRoomByID()
    //{
    //    if (HotelDetail.GM_ID > 0)
    //        HotelDetail = await config.GetOneInfo<HotelRoomsInfo>(SelectPro: 9, ValID: HotelDetail.GM_ID);

    //}
    //// async Task GetRooms()
    //// {
    ////     Rooms = await config.GetAllInfo<HotelRoomsInfo>(SelectPro: 8);
    //// }
    //async Task GetCombos()
    //{
    //    combos = await guest.GuestDetailsCombos(SelectPro: 3);
    //    System.Console.WriteLine(combos.Rooms.Count());
    //}
    //async Task GetComboRooms()
    //{
    //    combos.Rooms = await config.GetCMB<HotelRoomsInfo>(SelectPro: 7);
    //}
    //FluentTextField? FullNameField;
    //async Task InsertUpdateGuest()
    //{
    //    if (config.IsSaveClicked3)
    //        return;
    //    try
    //    {

    //        config.IsSaveClicked3 = true;

    //        if (SelectedGuest.GD_Fullname.IsStringNullOrWhiteSpace() || SelectedGuest.GD_Mobile.IsStringNullOrWhiteSpace())
    //        {
    //            config.IsInValidated3 = true;
    //            return;
    //        }
    //        config.IsInValidated3 = false;
    //        SPResult result = await guest.InsertUpdateGuest<SPResult>(
    //            SelectPro: 3,
    //            ValID: SelectedGuest.GD_ID,
    //            GuestEmail: SelectedGuest.GD_Email.ToEmptyOnNull(),
    //            RoomID: HotelDetail.GM_Room,
    //            GusetMasterID: HotelDetail.GM_ID,
    //            FullName: SelectedGuest.GD_Fullname.ToEmptyOnNull(),
    //            SurName: SelectedGuest.GD_Surname.ToEmptyOnNull(),
    //            MotherName: SelectedGuest.GD_MotherName.ToEmptyOnNull(),
    //            Mobile: SelectedGuest.GD_Mobile.ToEmptyOnNull(),
    //            IDNumber: SelectedGuest.GD_IdNumber.ToEmptyOnNull(),
    //            Gender: SelectedGuest.GD_Gender,
    //            DateOfBirth: SelectedGuest.GD_DOB.ToyyyyMMddElseEmpty(),
    //            NationalityID: SelectedGuest.GD_Nationality,
    //            Note: SelectedGuest.GD_Note.ToEmptyOnNull());

    //        if (result.Result == 1)
    //        {
    //            if (SelectedGuest.GD_ID <= 0)
    //            {
    //                if (int.TryParse(result.LastValue, out int val) && val > 0)
    //                {
    //                    await GetGuestByID(val);
    //                    CurrentStep = Steps.doc;
    //                    StateHasChanged();
    //                    await jSRuntime.InvokeVoidAsync("DetectOnFileChange");
    //                }
    //                else
    //                {
    //                    SelectedGuest = new() { GD_Gender = 1 };
    //                    CurrentStep = Steps.list;
    //                }
    //            }
    //            else
    //            {
    //                CurrentStep = Steps.doc;
    //                StateHasChanged();
    //                await jSRuntime.InvokeVoidAsync("DetectOnFileChange");
    //            }


    //            await GetRoomGuests();
    //            Toaster.Success(".", result.MSG);
    //            return;
    //        }
    //        Toaster.Error(".", result.MSG);
    //    }
    //    finally
    //    {
    //        config.IsSaveClicked3 = false;
    //    }
    //}
    //async Task GetRoomGuests()
    //{
    //    GuestDetails = await config.GetGrid<GuestDetailsInfo>(SelectPro: 1, ValID: HotelDetail.GM_ID);
    //}
    //async Task GetGuestByID(int id)
    //{
    //    SelectedGuest = await config.GetOneInfo<GuestDetailsInfo>(SelectPro: 7, ValID: id);
    //    await GetAllAttachments();
    //    if (HotelDetail.GM_ID <= 0)
    //    {
    //        HotelDetail.GM_ID = SelectedGuest.GD_GM;
    //        await GetRoomByID();
    //        // await GetRoomGuests();
    //    }
    //    StateHasChanged();
    //    await jSRuntime.InvokeVoidAsync("DetectOnFileChange");
    //}
    //async Task CheckoutGuest(int id)
    //{
    //    SPResult result = await guest.InsertUpdateGuest<SPResult>(SelectPro: 4, ValID: id);
    //    if (result.Result == 1)
    //    {
    //        if (GuestDetails.Count(x => !x.GD_ChechOut.HasValue) == 1)
    //        {
    //            await BlazoredModal.CloseAsync();
    //            return;
    //        }
    //        await GetRoomGuests();
    //        Toaster.Success(".", result.MSG);
    //        return;
    //    }
    //    Toaster.Error(".", result.MSG);

    //}

    //private async Task LoadFiles(Microsoft.AspNetCore.Components.Forms.InputFileChangeEventArgs e)
    //{
    //    InputFileMessage = e.File.Name;
    //    try
    //    {
    //        MemoryStream ms = new MemoryStream();
    //        await e.File.OpenReadStream(maxAllowedSize: 10 * 1024 * 1024).CopyToAsync(ms);
    //        // SelectedDoc=await Util.CompressImage(ms);
    //        // System.Console.WriteLine(SelectedDoc.Length);
    //        // System.Console.WriteLine(ms.Length);
    //        SelectedDoc = ms.ToArray();
    //        await InsertAttachment();
    //        InputFileMessage = e.File.Name;
    //    }
    //    catch (Exception ex)
    //    {
    //        System.Console.WriteLine(ex.Message);
    //        Toaster.Error(".", L["choose-image-below-1mb"]);
    //    }
    //}
    async Task GetAllAttachments()
    {
        Attachments = await config.HotelGetAllInfo<AttachmentInfo>(SelectPro: 4, ValID: GuestID);
    }

    //async Task InsertAttachment()
    //{
    //    if (config.IsSaveClicked2)
    //        return;
    //    try
    //    {
    //        config.IsSaveClicked2 = true;

    //        if (ValidateAddDoc())
    //        {
    //            config.IsInValidated2 = true;
    //            return;
    //        }
    //        config.IsInValidated2 = false;
    //        SPResult result = await guest.InsertUpdateGuest<SPResult>(
    //            SelectPro: 5,
    //            GuestAttachments: SelectedDoc,
    //            RoomID: DocType,
    //            GusetMasterID: SelectedGuest.GD_ID);

    //        if (result.Result == 1)
    //        {
    //            InputFileMessage = L["select-file"];
    //            SelectedDoc = null;
    //            await GetAllAttachments();
    //            Toaster.Success(".", result.MSG);
    //            return;
    //        }
    //        Toaster.Error(".", result.MSG);
    //    }
    //    finally
    //    {
    //        config.IsSaveClicked2 = false;
    //    }
    //}
    //async Task DeleteAttachment(int id)
    //{
    //    SPResult result = await guest.InsertUpdateGuest<SPResult>(
    //        SelectPro: 6,
    //        ValID: id);

    //    if (result.Result == 1)
    //    {
    //        await GetAllAttachments();
    //        Toaster.Success(".", result.MSG);
    //        return;
    //    }
    //    Toaster.Error(".", result.MSG);
    //}
    //Task<IEnumerable<HotelRoomsInfo>> SearchHotelRoom(string e)
    //{
    //    var s = combos.Rooms.Where(x => x.htr_Detail.ToEmptyOnNull().ContainsIgnoreCase(e.ToEmptyOnNull()));
    //    return Task.FromResult(s);
    //}
    //void OnSelectedRoomChange(HotelRoomsInfo e)
    //{
    //    if (e == null)
    //    {
    //        SelectedRoomToChangeText = "";
    //        SelectedRoomToChange = 0;
    //        return;
    //    }
    //    SelectedRoomToChange = e.htr_ID;
    //    SelectedRoomToChangeText = e.htr_Detail.ToEmptyOnNull();

    //}
    //async Task ChangeRoom()
    //{
    //    var options = new ModalOptions()
    //    {
    //        HideCloseButton = true,
    //        Class = " blazored-modal custom-modal-sm",
    //    };
    //    var parameters = new ModalParameters();
    //    parameters.Add("Message", (L["change-room-confirm"]).Value.Replace("{1}", HotelDetail.GM_Room.ToString()).Replace("{2}", SelectedRoomToChange.ToString()));
    //    var modal = Modal.Show<CheckOutConfirm>("", parameters, options);

    //    var res = await modal.Result;
    //    if (res.Cancelled)
    //        return;

    //    SPResult result = await guest.InsertUpdateGuest<SPResult>(SelectPro: 10, ValID: HotelDetail.GM_ID, RoomID: SelectedRoomToChange);
    //    if (result.Result == 1)
    //    {
    //        SelectedRoomToChange = 0;
    //        SelectedRoomToChangeText = "";
    //        await GetComboRooms();
    //        await GetRoomByID();
    //        Toaster.Success(".", result.MSG);
    //        return;
    //    }
    //    Toaster.Error(".", result.MSG);
    //}
    //async Task showCheckOutConfirmModal(int id,string name)
    //{

    //    var options = new ModalOptions()
    //    {
    //        HideCloseButton = true,
    //        Class = " blazored-modal custom-modal-sm",
    //    };
    //    var parameters = new ModalParameters();
    //    parameters.Add("Message", L["are-you-sure-from-checkout"].Value.ToEmptyOnNull());
    //    parameters.Add("Title", L["checkout"].Value.ToString()+" - "+name);

    //    var modal = Modal.Show<CheckOutConfirm>("", parameters, options);

    //    var res = await modal.Result;
    //    if (!res.Cancelled)
    //        await CheckoutGuest(id);
    //}

    //[JSInvokable]
    //public async Task OnFileUploadChange(string value)
    //{
    //    SelectedDoc = Convert.FromBase64String(value);
    //    await InsertAttachment();
    //    StateHasChanged();
    //}
    //async Task CheckAddGuest()
    //{
    //    //SPResult result = await guest.InsertUpdateGuest<SPResult>(SelectPro: 11, RoomID: HotelDetail.GM_Room);
    //    //if (result.Result == 1)
    //    //{
    //        SelectedGuest = new() { GD_Gender = 1 };
    //        CurrentStep = Steps.edit;
    //        return;
    //   // }
    //  //  Toaster.Error(".", result.MSG);
    //}

    //bool ValidateAddDoc()
    //{
    //    return ((SelectedDoc == null || SelectedDoc.Length <= 0));
    //}

    //public void Dispose()
    //{
    //    if (_objRef != null)
    //        _objRef.Dispose();
    //}
}